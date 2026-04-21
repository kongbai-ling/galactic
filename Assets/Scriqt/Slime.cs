using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyBase
{
    private int SlimeTpye =  1;
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    public override void DeathEnter()
    {
        base.DeathEnter();
        GameManager.Instance.KillEnemyeNumber(SlimeTpye);
        GameManager.Instance.EXPAdd(0.5f);
    }
}
