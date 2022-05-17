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


    [SerializeField] private LevelMaker levelMaker;
    [SerializeField] private int level = 0;

    [SerializeField] private PlayerBall ball;



    private enum GameState
    {
        preparation, serving, playing, finished
    }
    private GameState currentState = GameState.serving;


    private void Awake()
    {
        levelMaker.InitializeLevel(level);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space) && currentState==GameState.serving)
        {
            ball.ApplyInitialVelocity();
            currentState = GameState.playing;
        }
    }


    public void BallOutbound()
    {
        ball.ResetBall();
        currentState = GameState.serving;
    }
}
