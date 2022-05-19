using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPongManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static BrickPongManager _instance;
    public static BrickPongManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BrickPongManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("BrickPongManager");
                    _instance = container.AddComponent<BrickPongManager>();
                }
            }

            return _instance;
        }
    }
    #endregion


    [SerializeField] private PlayerBall ball;
    [SerializeField] private PlayerBar bar;


    [Header("Level")]
    [SerializeField] private LevelMaker levelMaker;

    [SerializeField] private int _level = 0;
    public int level { get { return _level; } set { _level = value; onLevelChange(value); } }
    public Action<int> onLevelChange;

    private List<Brick> spawnedBricks = new List<Brick>();
    public int breakableBrickCount { get; private set; } = 0;


    [Header("Settings")]
    [SerializeField] private int livesPerGame = 3;
    [SerializeField] private float powerUpDuration = 20f;

    [SerializeField] private float fieldWidth = 28;
    [SerializeField] private float playerBarLength = 5f, playerBarLengthExtended = 7.5f;

    [SerializeField] private float playerBarSpeed = 0.5f, playerBarSpeedQuick = 1f;


    [Header("Values")]
    [SerializeField] private int _life = 3;
    [SerializeField] private int _score = 0;
    public int life { get { return _life; } set { _life = value; onLifeChange(value); } }
    public int score { get { return _score; } set { _score = value; onScoreChange(value); } }
    public Action<int> onLifeChange;
    public Action<int> onScoreChange;


    public enum GameState
    {
        home, serving, playing, pausing, finished
    }
    [SerializeField] private GameState _currentState = GameState.home;
    public GameState currentState { get { return _currentState; } set { _currentState = value; onCurrentStateChange(value); } }
    public Action<GameState> onCurrentStateChange;



    private void Awake()
    {
        MakeLevel();
    }
    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space)) ServeBall();
    }





    //==================================== UI Components Messages ====================================
    public void ClearLevel()
    {
        foreach(Brick b in spawnedBricks) Destroy(b.gameObject);
        spawnedBricks.Clear();
    }
    public void MakeLevel()
    {
        spawnedBricks = levelMaker.InitializeLevel(level);
    }

    public void PreviousLevel()
    {
        if(levelMaker.levelCount <= 1) return;  //skip logic if no choice

        ClearLevel();
        if(--level < 0) level = levelMaker.levelCount-1;
        MakeLevel();
    }
    public void NextLevel()
    {
        if(levelMaker.levelCount <= 1) return;  //skip logic if no choice

        ClearLevel();
        if(++level >= levelMaker.levelCount) level = 0;
        MakeLevel();
    }

    public void ServeBall()
    {
        if(currentState==GameState.serving)
        {
            ball.ApplyInitialVelocity();
            currentState = GameState.playing;
        }
    }

    public void StartGame()
    {
        breakableBrickCount = spawnedBricks.FindAll((brick) => brick.isBreakable()).Count;

        life = livesPerGame;
        score = 0;

        ball.ResetBall();

        SetPlayerBarLength(false);
        SetPlayerBarSpeed();

        currentState = GameState.serving;
    }
    private GameState stateWhenPaused;
    public void PauseGame()
    {
        if(currentState!=GameState.serving && currentState!=GameState.playing) return;

        Time.timeScale = 0;
        stateWhenPaused = currentState;

        currentState = GameState.pausing;
    }
    public void ResumeGame()
    {
        if(currentState!=GameState.pausing) return;

        Time.timeScale = 1;

        currentState = stateWhenPaused;
    }
    public void AbandonGame()
    {
        currentState = GameState.finished;

        ball.ResetBall();

        Time.timeScale = 1;
    }
    public void ReturnHome()
    {
        life = livesPerGame;
        score = 0;

        ball.ResetBall();

        currentState = GameState.home;
    }





    //==================================== Game Components Messages ====================================
    //------------------- CollectionArea -------------------
    public void BallOutbound()
    {
        ball.ResetBall();

        if(currentState == GameState.playing)
        {
            currentState = (--life > 0)?
                           GameState.serving:
                           GameState.finished;
        }
    }

    //------------------- Ball -------------------
    public void BarTouched()
    {
        combo = 0;
    }

    //------------------- Brick -------------------
    public static readonly Dictionary<Brick.Type, int> brickScores = new Dictionary<Brick.Type, int>()
    {
        //breakables
        { Brick.Type.clay, 100 },
        { Brick.Type.clayBlue, 200 },
        { Brick.Type.clayRed, 300 },
        { Brick.Type.hardenedClay, 400 },

        //unbreakables
        { Brick.Type.metal, 0 },
        { Brick.Type.rebouncer, 0 }
    };
    private const int comboScore = 10;

    private int combo = 0;
    public void BrickSmashed(Brick brick)
    {
        //count score
        score += brickScores[brick.type] + (combo++ * comboScore);

        //check game finish
        if(brick.isBreakable()) breakableBrickCount--;
        if(breakableBrickCount <= 0)
        {
            currentState = GameState.finished;
        }

        //spawn PowerUp
        foreach(PowerUp.Type powerUp in brick.containingPowerUps)
        {
            GameObject prefab = Resources.Load("PowerUp_" + powerUp) as GameObject;
            if(prefab!=null) Instantiate(prefab, brick.transform.position, Quaternion.identity);
        }

        //remove GO
        spawnedBricks.Remove(brick);
        Destroy(brick.gameObject);
    }

    //------------------- PowerUp -------------------
    public void PowerUpObtained(PowerUp powerUp)
    {
        switch(powerUp.type)
        {
            case PowerUp.Type.extendedBar:
                SetPlayerBarLength(true);
                break;

            case PowerUp.Type.quickerBar:
                SetPlayerBarSpeed(true);
                break;

            case PowerUp.Type.extraBall:
                break;

            default:
                break;
        }

        //remove GO
        Destroy(powerUp.gameObject);
    }
    
    private void SetPlayerBarLength(bool extend = false)
    {
        StopCoroutine(PowerUpEnd_extendedBar());

        bar.SetLength(extend? playerBarLengthExtended: playerBarLength,
                      fieldWidth);

        if(extend) StartCoroutine(PowerUpEnd_extendedBar());
    }
    private IEnumerator PowerUpEnd_extendedBar()
    {
        yield return new WaitForSeconds(powerUpDuration);
        SetPlayerBarLength();
    }

    private void SetPlayerBarSpeed(bool quick = false)
    {
        StopCoroutine(PowerUpEnd_quickerBar());

        bar.SetSpeed(quick? playerBarSpeedQuick: playerBarSpeed);

        if(quick) StartCoroutine(PowerUpEnd_quickerBar());
    }
    private IEnumerator PowerUpEnd_quickerBar()
    {
        yield return new WaitForSeconds(powerUpDuration);
        SetPlayerBarSpeed();
    }
}
