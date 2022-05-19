using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        extendedBar, quickerBar,
        extraBall
    }

    public Type type = Type.extendedBar;


    private const float deletionYPos = -100f;
    private void FixedUpdate()
    {
        if(transform.position.y < deletionYPos) Destroy(gameObject);    //self deletion if dropped away from play field, since trigger cannot be detected by trigger CollectionArea
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            BrickPongManager.instance.PowerUpObtained(this);
        }
    }
}
