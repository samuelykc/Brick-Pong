using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionArea : MonoBehaviour
{
    [SerializeField] private PlayerBall ball;



    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == ball.gameObject)
        {
            BrickPongManager.instance.BallOutbound();
        }
    }
}
