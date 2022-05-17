using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum Type
    {
        clay, clayBlue, clayRed,
        metal
    }

    public Type type = Type.clay;
    public bool isBreakable() { return type != Type.metal; }



    private void OnCollisionEnter(Collision collision)
    {
        if(isBreakable())
        {
            BrickPongManager.instance.BrickSmashed(this);
            Destroy(gameObject);
        }
    }
}
