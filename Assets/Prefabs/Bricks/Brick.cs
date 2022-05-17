using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum Type
    {
        clay, clayBlue, clayRed,
        hardenedClay,
        metal, rebouncer
    }

    public Type type = Type.clay;
    public int hitCount { get; private set; } = 0;


    [SerializeField] private int requiredHitCount = 2;      //for "hardenedClay" only
    public float rebouncerAcceleration = 10f;               //for "rebouncer" only


    public bool isBreakable() { return type!=Type.metal && type!=Type.rebouncer; }


    private void OnCollisionEnter(Collision collision)
    {
        hitCount++;

        if(isBreakable())
        {
            if(type==Type.hardenedClay && hitCount<requiredHitCount) return;

            BrickPongManager.instance.BrickSmashed(this);
            Destroy(gameObject);
        }
    }
}
