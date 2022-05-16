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

    [SerializeField] private Type type = Type.clay;



    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
