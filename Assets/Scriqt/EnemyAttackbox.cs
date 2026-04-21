using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackbox : MonoBehaviour
{
    public float buildtime = 0.3f;
    public float damage = 10f;
    void Start()
    {
        Destroy(gameObject, buildtime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Playerscriqt>().GetHit(damage);
        }
    }
}
