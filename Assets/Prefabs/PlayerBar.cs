using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class PlayerBar : MonoBehaviour
{
    [SerializeField] private float barSpeed = 0.5f;
    [SerializeField] private float barMinX, barMaxX;

    [SerializeField] private Transform bar;
    
    public void Reset()
    {
        bar = GetComponent<Transform>();
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
