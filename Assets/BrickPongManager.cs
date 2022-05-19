using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    [Header("Level")]
    [SerializeField] private LevelMaker levelMaker;

    [SerializeField] private int _level = 0;
    public int level { get { return _level; } set { _level = value; onLevelChange(value); } }
    public Action<int> onLevelChange;

    private List<Brick> spawnedBricks = new List<Brick>();
    public int breakableBrickCount { get; private set; } = 0;


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





    //--------------------- UI Components Messages ---------------------
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

        life = 3;
        score = 0;

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
        life = 3;
        score = 0;

        currentState = GameState.home;
    }





    //--------------------- Game Components Messages ---------------------
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

        //remove GO
        spawnedBricks.Remove(brick);
        Destroy(brick.gameObject);
    }
    public void BarTouched()
    {
        combo = 0;
    }
}
