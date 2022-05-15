using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPongManager : MonoBehaviour
{
    [SerializeField] private PlayerBall ball;
    [SerializeField] private Transform bar;

    [SerializeField] private float ballSpeed = 5f;
    [SerializeField] private float barMinX, barMaxX;


    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            bar.position = new Vector3(Mathf.Max(bar.position.x-0.1f, barMinX), bar.position.y);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            bar.position = new Vector3(Mathf.Min(bar.position.x+0.1f, barMaxX), bar.position.y);
        }

        
        if(Input.GetKey(KeyCode.Space))
        {
            ball.ApplyInitialVelocity(ballSpeed);
        }
    }
}
