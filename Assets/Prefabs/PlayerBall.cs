using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBall : MonoBehaviour
{
    [SerializeField] private PlayerBar bar;

    [SerializeField] private Rigidbody body;
    [SerializeField] private float speed = 20f;


    [SerializeField] private Vector3 servingPos = new Vector3(0, 1, 0);     //relative to bar
    private bool isServing = true;  //the ball is waiting for player to serve


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
        //use the cached velocity, since the currentt velocity is already affected by the collision
        body.velocity = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);

        //TODO: when y velocity is 0, create small y velocity
        //TODO: when y velocity is small & touches the board, create large y velocity
        if(collision.transform.gameObject == bar.gameObject)
        {
            BrickPongManager.instance.BarTouched();
        }
    }




    public void ApplyInitialVelocity()
    {
        body.velocity = speed * (new Vector3(1, 1, 0)).normalized;

        isServing = false;
    }
    public void ResetBall()
    {
        body.velocity = Vector3.zero;
        lastVelocity = Vector3.zero;

        isServing = true;
    }
}
