using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerBall)), CanEditMultipleObjects]
public class PlayerBallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerBall controller = (PlayerBall)target;


        if(GUILayout.Button("Move horizontally (right)"))
        {
            if(EditorApplication.isPlaying)
            {
                Rigidbody rg = controller.GetComponent<Rigidbody>();
                rg.velocity = rg.velocity.magnitude * (new Vector3(1, 0, 0));
            }
        }

        if(GUILayout.Button("Move vertically (up)"))
        {
            if(EditorApplication.isPlaying)
            {
                Rigidbody rg = controller.GetComponent<Rigidbody>();
                rg.velocity = rg.velocity.magnitude * (new Vector3(0, 1, 0));
            }
        }
    }
}
