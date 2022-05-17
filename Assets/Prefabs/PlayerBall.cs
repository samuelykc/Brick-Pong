using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBall : MonoBehaviour
{
    [SerializeField] private PlayerBar bar;

    [SerializeField] private Rigidbody body;
    [SerializeField] private float baseSpeed = 20f;
    float speed { get { return baseSpeed + appliedRebouncAcceleration; } }

    private float appliedRebouncAcceleration = 0;


    [Header("Serving")]
    [SerializeField] private Vector3 servingPos = new Vector3(0, 1, 0);     //relative to bar
    private bool isServing = true;  //the ball is waiting for player to serve

    [Header("Stuck prevention")]
    [SerializeField] private float yVeclocityCriticalValue = 0.05f;   //if the normalized Y-axis velocity is under this critical value, it will be adjusted by the system for the game to go smoothly
    [SerializeField] [Range(-1f,1f)] private float yVeclocityUponAdjustment = -0.1f;  //the new normalized Y-axis velocity if adjustment is made


    public void Reset()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
    }


    private Vector3 lastVelocity;
    private void FixedUpdate()
    {
        if(isServing)
        {
            body.position = bar.transform.position + servingPos;
        }
        else
        {
            lastVelocity = body.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //handling acceleration from "rebouncer" brick
        appliedRebouncAcceleration = 0;
        Brick brick = collision.transform.gameObject.GetComponent<Brick>();
        if(brick?.type == Brick.Type.rebouncer)
        {
            appliedRebouncAcceleration = brick.rebouncerAcceleration;
        }



        //use the cached velocity for calculation, since the current velocity is already affected by the collision
        Vector3 newVeclocity = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);

        //make sure the speed did not drop, because in some cases the physical handling may go unexpected
        newVeclocity = speed * newVeclocity.normalized;

        //when y velocity is 0 or close to 0, create small y velocity
        if(Mathf.Abs(newVeclocity.normalized.y) < yVeclocityCriticalValue)
            newVeclocity = speed * (new Vector3(newVeclocity.normalized.x, yVeclocityUponAdjustment, 0)).normalized;

        //TODO: when y velocity is small & touches the board, create larger y velocity

        body.velocity = newVeclocity;



        //handling for touching the player bar
        if(collision.transform.gameObject == bar.gameObject)
        {
            BrickPongManager.instance.BarTouched();
        }
    }




    public void ApplyInitialVelocity()  //serve the ball
    {
        body.velocity = speed * (new Vector3(1, 1, 0)).normalized;

        isServing = false;
    }
    public void ResetBall()             //stop the ball for serving again
    {
        body.velocity = Vector3.zero;
        lastVelocity = Vector3.zero;

        isServing = true;
    }
}
