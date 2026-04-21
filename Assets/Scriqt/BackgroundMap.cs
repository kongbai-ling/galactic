using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMap : MonoBehaviour
{
    public GameObject CameraMap;
    private float mapWidth;
    public float mapNum;
    private float totalWidth;
    void Start()
    {
        mapWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        totalWidth = mapWidth * mapNum;
    }


    void Update()
    {
        Vector3 temp = transform.position;
        if(CameraMap.transform.position.x > transform.position.x + totalWidth / 2)
        {
            temp.x += totalWidth;
            transform.position = temp;
        }
        if(CameraMap.transform.position.x < transform.position.x - totalWidth / 2)
        {
            temp.x -= totalWidth;
            transform.position = temp;
        }
    }
}
