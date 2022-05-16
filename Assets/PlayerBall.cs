using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBall : MonoBehaviour
{
    [SerializeField] private PlayerBar bar;


    [SerializeField] private Rigidbody body;

    private float currentSpeed;

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
        lastVelocity = body.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //use the cached velocity, since the currentt velocity is already affected by the collision
        body.velocity = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);
    }




    public void ApplyInitialVelocity(float speed)
    {
        currentSpeed = speed;
        body.velocity = speed * (new Vector3(1, 1, 0)).normalized;
    }
}
