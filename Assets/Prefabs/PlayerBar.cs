using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class PlayerBar : MonoBehaviour
{
    [SerializeField] private float barMinX, barMaxX;
    [SerializeField] private float barSpeed = 0.5f;

    [SerializeField] private Transform bar;
    
    public void Reset()
    {
        bar = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftArrow) || moveLeft)
        {
            bar.position = new Vector3(Mathf.Max(bar.position.x-barSpeed, barMinX), bar.position.y);
        }
        else if(Input.GetKey(KeyCode.RightArrow) || moveRight)
        {
            bar.position = new Vector3(Mathf.Min(bar.position.x+barSpeed, barMaxX), bar.position.y);
        }
    }

    public bool moveLeft { get; set; } = false;
    public bool moveRight { get; set; } = false;



    public void SetLength(float width, float fieldWidth)
    {
        bar.localScale = new Vector3(width, 1, 1);

        //update position limitations
        barMaxX = (fieldWidth-width)/2f;
        barMinX = -barMaxX;

        //clamp position
        bar.position = new Vector3(Mathf.Clamp(bar.position.x, barMinX, barMaxX), bar.position.y);
    }
    public void SetSpeed(float speed)
    {
        barSpeed = speed;
    }
}
