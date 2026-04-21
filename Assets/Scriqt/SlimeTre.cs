using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTre : MonoBehaviour
{
    public EnemyBase enemy;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player") 
        {
            enemy.playerEnter(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.playerOut();
        }
    }
}
