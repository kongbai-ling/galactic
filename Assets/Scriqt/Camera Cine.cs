using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraCine : MonoBehaviour
{
    public Transform backgroungd;
    private Vector3 lastpos;
    public Vector2 offspeed;
    void Start()
    {
        lastpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newpos = transform.position - lastpos;
        Vector3 newpos2 = new Vector3(newpos.x * offspeed.x, newpos.y * offspeed.y, 0);
        backgroungd.position += newpos2;
        lastpos = transform.position;
    }

    
}


