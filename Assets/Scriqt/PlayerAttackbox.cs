using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackbox : MonoBehaviour
{
    public float buildtime = 0.2f;
    public float damage = 10f;
    void Start()
    {
        Destroy(gameObject, buildtime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().GetHit(damage);
        }
    }
}
