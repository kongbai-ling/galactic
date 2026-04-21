using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//状态机的制作
//状态机由状态和状态转换构成
public enum EnemyState//State状态机
{
    Idle,//待机
    Patrol,//巡逻
    Pursuit,//追击
    Attack,//攻击
    GetHit,//受击
    Death//死亡
}
public class EnemyBase : MonoBehaviour
{
    [Header("敌人移动")]
    public EnemyState Currentstate = EnemyState.Patrol;//当前状态
    public Transform left;
    public Transform right;
    public bool isright = false;
    public Rigidbody2D Rb;
    public float speed;
    [HideInInspector]public bool Canmove = true;
    public SpriteRenderer sprite;
    public Animator anim;
    [Header("敌人追击")]
    public GameObject player;
    public float AttackDis = 0.65f;
    [Header("敌人Attack")]
    public bool IsAttack = false;
    public float AttackCD = 2f;
    public GameObject AttackPoint;
    public Transform AttackPointPos1;
    public Transform AttackPointPos2;
    public float ATK = 10.0f;
    [Header("敌人HP")]
    public float MaxHP = 100f;
    public float NowHP;
    [Header("敌人gethit")]
    public bool IsGetHit = false;
    public float GetHitAddForce = 300;
    [Header("敌人Death")]
    public GameObject Enemyandposition;
    [Header("敌人UI")]
    public Slider hpslider;
    public GameObject hpTextEff;
    public GameObject hpTextposition;
    public virtual void Start()
    {
        PatrolEnter();
        NowHP = MaxHP;
        speed = UnityEngine.Random.Range(0.5f, 1.5f);

    }

    public virtual void Update()
    {
        switch (Currentstate)
        {
            case EnemyState.Idle: IdleUpdate(); break;
            case EnemyState.Patrol: PatrolUpdate(); break;
            case EnemyState.Pursuit: PursuitUpdate(); break;
            case EnemyState.Attack: AttackUpdate(); break;
            case EnemyState.GetHit: GetHitUpdate(); break;
            case EnemyState.Death: DeathUpdate(); break;
        }
    }
    private void FixedUpdate()
    {
        if (Canmove)
        {
            
            Rb.velocityX = speed * (isright ? 1 : -1);
        }
        else
        {
            Rb.velocityX = 0;
        }
    }
    #region 状态机
    public virtual void IdleEnter()
    {
        Canmove = false;
        anim.SetBool("IsMove", false);
        Invoke(nameof(ChangeDirection), 3f);
    }
    public virtual void IdleUpdate()
    {
        
    }
    public virtual void IdleExit()
    {

    }
    public virtual void PatrolEnter()
    {
        anim.SetBool("IsMove", true);
    }
    public virtual void PatrolUpdate()
    {
        if (Canmove) {
            if (transform.position.x <= left.position.x && !isright)
            {
                ChangecurrentState(EnemyState.Idle);

            }
            else if (transform.position.x >= right.position.x && isright)
            {
                ChangecurrentState(EnemyState.Idle);
            }
        }
    }
    public virtual void PatrolExit()
    {

    }
    public virtual void PursuitEnter()
    {
        Canmove = true;
        anim.SetBool("IsMove", true);
    }
    public virtual void PursuitUpdate()
    {
        if (Canmove) {
            if (player.transform.position.x <= transform.position.x)
            {
                isright = false;
                sprite.flipX = isright;

            }
            else
            {
                isright = true;
                sprite.flipX = isright;
            }
            if (Mathf.Abs(player.transform.position.x - transform.position.x) <= AttackDis&&!IsGetHit)
            {
                ChangecurrentState(EnemyState.Attack);

            }
        }
       
    }
    public virtual void PursuitExit()
    {

    }
    public virtual void AttackEnter()
    {
        Canmove = false;
        anim.SetBool("IsMove", false);
     
    }
    public virtual void AttackUpdate()
    {
        if (!IsAttack&&!IsGetHit&&Currentstate!=EnemyState.Death)
        {
            IsAttack = true;
            anim.SetTrigger("Isattack");
            Invoke(nameof(ChangeDis), AttackCD);
            Invoke(nameof(EnemyAttackbox), 0.45f);
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > AttackDis)
            {
                ChangecurrentState(EnemyState.Pursuit);
            }
            else
            {
                if (player.transform.position.x <= transform.position.x && !isright && Canmove)
                {
                    ChangecurrentState(EnemyState.Pursuit);
                }
                if (player.transform.position.x > transform.position.x && isright && Canmove)
                {
                    ChangecurrentState(EnemyState.Pursuit);
                }
            }

        }
       
    }
    public virtual void AttackExit()
    {
        
    }
    public virtual void GetHitEnter()
    {
        Canmove = false;
        anim.SetBool("IsGetHit", true);
        anim.SetBool("IsMove", false);
        Invoke(nameof(GetHitend), 0.8f);
    }
    public virtual void GetHitUpdate()
    {
        if (!IsGetHit)
        {
            IsGetHit = true;
           if(player.transform.position.x < transform.position.x)
            {
                Rb.AddForce(new Vector2(GetHitAddForce,0));
            }
            else
            {
                Rb.AddForce(new Vector2(-GetHitAddForce, 0));
            }
        }
    }
    public virtual void GetHitExit()
    {

    }
    public virtual void DeathEnter()
    {
        Canmove = false;
        anim.SetBool("IsMove", false);
        anim.SetBool("IsGetHit", false);
        anim.SetTrigger("IsDeath");
        CancelInvoke();
        Destroy(gameObject, 3f);
    }
    public virtual void DeathUpdate()
    {

    }
    public virtual void DeathExit()
    {

    }
#endregion

        public virtual void ChangecurrentState(EnemyState state)
    {
        switch (Currentstate)
        {
            case EnemyState.Idle: IdleExit(); break;
            case EnemyState.Patrol: PatrolExit(); break;
            case EnemyState.Pursuit: PursuitExit(); break;
            case EnemyState.Attack: AttackExit(); break;
            case EnemyState.GetHit: GetHitExit(); break;
            case EnemyState.Death: DeathExit(); break;
        }
        Currentstate = state;
        switch (state) {
            case EnemyState.Idle: IdleEnter(); break;
            case EnemyState.Patrol: PatrolEnter(); break;
            case EnemyState.Pursuit: PursuitEnter(); break;
            case EnemyState.Attack: AttackEnter(); break;
            case EnemyState.GetHit: GetHitEnter(); break;
            case EnemyState.Death: DeathEnter(); break;
        }
    }
    public void ChangeDirection()
    {
        isright = !isright;
        sprite.flipX = isright;
        Canmove = true;
        anim.SetBool("IsMove", true);
        ChangecurrentState(EnemyState.Patrol);
    }
    public void playerEnter(GameObject players)
    {
        if (Currentstate != EnemyState.Death)
        {
            player = players;
            ChangecurrentState(EnemyState.Pursuit);
        }
          
    }
    public void playerOut()
    {
        if(Currentstate != EnemyState.Death)
        {
            Canmove = true;
            ChangecurrentState(EnemyState.Patrol);
        }
    }
    public void ChangeDis()
    {
        IsAttack = false;
    }
    public void GetHit(float damage)
    {
       if(Currentstate != EnemyState.Death)
        {
            ChangecurrentState(EnemyState.GetHit);
            NowHP = NowHP - damage;
            GameObject go = Instantiate(hpTextEff,hpTextposition.transform.position,hpTextposition.transform.rotation,hpTextposition.transform);
            go.transform.localScale = hpTextposition.transform.lossyScale;
            go.GetComponent<Text>().text = damage.ToString();
            Destroy(go, 0.8f);
            hpslider.value = NowHP/MaxHP;
            if (NowHP <= 0)
            {
                ChangecurrentState(EnemyState.Death);
            }
        }
    }
    public void GetHitend()
    {
        if (Currentstate != EnemyState.Death)
        {
            Canmove = true;
            IsGetHit = false;
            anim.SetBool("IsGetHit", false);
            ChangecurrentState(EnemyState.Pursuit);
        }

    }
    public void EnemyAttackbox()
    {
        if (Currentstate != EnemyState.Death&&Currentstate != EnemyState.GetHit)
        {
            GameObject go;
            if (!isright)
            {
                go = Instantiate(AttackPoint, AttackPointPos1.transform.position, AttackPointPos1.transform.rotation);
                go.transform.localScale = AttackPointPos1.lossyScale;
            }
            else {
                go = Instantiate(AttackPoint, AttackPointPos2.transform.position, AttackPointPos2.transform.rotation);
                go.transform.localScale = AttackPointPos2.lossyScale;
            }
            go.GetComponent<EnemyAttackbox>().damage =Mathf.Round(ATK * UnityEngine.Random.Range(0.8f, 1.2f));
        }
    }
}
