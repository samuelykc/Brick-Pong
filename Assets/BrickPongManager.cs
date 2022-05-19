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
    [SerializeField] private int level = 0;

    private List<Brick> spawnedBricks = new List<Brick>();
    private int breakableBrickCount = 0;


    [Header("Values")]
    [SerializeField] private int _life = 3;
    [SerializeField] private int _score = 0;
    public int life { get { return _life; } set { _life = value; onLifeChange(value); } }
    public int score { get { return _score; } set { _score = value; onScoreChange(value); } }
    public Action<int> onLifeChange;
    public Action<int> onScoreChange;


    public enum GameState
    {
        home, serving, playing, finished
    }
    [SerializeField] private GameState _currentState = GameState.serving;
    public GameState currentState { get { return _currentState; } set { _currentState = value; onCurrentStateChange(value); } }
    public Action<GameState> onCurrentStateChange;


    private void Awake()
    {
        spawnedBricks = levelMaker.InitializeLevel(level);
        breakableBrickCount = spawnedBricks.FindAll((brick) => brick.isBreakable()).Count;

        life = 3;
        score = 0;
    }

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space)) ServeBall();
    }



    public void ServeBall()
    {
        if(currentState==GameState.serving)
        {
            ball.ApplyInitialVelocity();
            currentState = GameState.playing;
        }
    }


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
    }
    public void BarTouched()
    {
        combo = 0;
    }
}
