using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPongManager : MonoBehaviour
{
    [SerializeField] private LevelMaker levelMaker;
    [SerializeField] private int level = 0;

    [SerializeField] private PlayerBall ball;
    [SerializeField] private Transform bar;

    [SerializeField] private float ballSpeed = 20f;

    [SerializeField] private float barSpeed = 0.15f;
    [SerializeField] private float barMinX, barMaxX;


    private void Awake()
    {
        levelMaker.InitializeLevel(level);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            ball.ApplyInitialVelocity(ballSpeed);
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            bar.position = new Vector3(Mathf.Max(bar.position.x-barSpeed, barMinX), bar.position.y);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            bar.position = new Vector3(Mathf.Min(bar.position.x+barSpeed, barMaxX), bar.position.y);
        }

    }

}
