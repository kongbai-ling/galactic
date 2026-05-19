using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerscriqt : MonoBehaviour
{
    [Header("角色移动")]
    public GameObject Player;
    public Rigidbody2D Rg;
    private float xInput;
    public bool isright;
    public bool IsMove =true;
    [Header("动画")]
    public Animator ani;
    public SpriteRenderer Playerone;
    [Header("角色跳跃")]
    public GameObject PlayeGo;
    public LayerMask ground;
    public float jumpHeight;
    [HideInInspector]public bool isGround;
    [HideInInspector] private bool isJump;
    [HideInInspector] public bool isAir = false;
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferTimer = 0f;    // 剩余缓冲时间
    private bool hasJumpBuffered = false;   // 是否有待执行的跳跃
    [Header("角色冲刺")]
    public float Dashspeed;
    public float DashTime;
    public float DashCD = 1f;
    public GameObject DashEffect;
    private float DashOjsetTimer = 0f;
    private float DashOjsetTimers = 0.2f;
    [HideInInspector]public bool isDash = false;
    [HideInInspector]public bool CanDash;
    [Header("角色攻击")]
    public float AttackCD;
    private int combo = 1;
    public bool isAttack = false;
    public Transform AttackPoint1;
    public Transform AttackPoint2;
    public Transform AttackPoint3;
    public Transform AttackPoint4;
    public Transform AttackPoint5;
    public Transform AttackPoint6;
    public GameObject Attackbox;
    public float AttackAddForce = 300f;
    public float[] Attacksoundtime;
    [Header("PlayerAtt")]
    public float ATK = 10f;
    private float MF = 1.5F;
    public float Defense = 1;
    private float speed = 5;
    [Header("PlayerHP")]
    public float MaxHP = 100f;
    public float NowHP;
    public Slider playerHp;
    public Text PlayerHpText;
    [Header("PlayerGetHitandDeath")]
    public bool IsDead = false;
    public bool IsGetHit = false;
    public GameObject GetHitEffect;
    public GameObject GetHitEffectpositi;

    void Start()
    {
        NowHP = MaxHP;
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
        UpdateJumpBuffer();
        playerdash();
        playerAttack();     
        // 检查是否处于冲刺状态且当前时间大于冲刺偏移计时器
        if (isDash && Time.time > DashOjsetTimer)
        {
            
            // 判断冲刺方向是否为右
        if (isright)
        {
                Instantiate(DashEffect, transform.position - new Vector3(1, 0, 0), transform.rotation, transform);
        }
        else
        {
                GameObject go = Instantiate(DashEffect, transform.position + new Vector3(1, 0, 0), transform.rotation, transform);
        // 将特效对象的X轴翻转，使其朝向正确
          go.GetComponent<SpriteRenderer>().flipX = true;
        }
        // 更新冲刺偏移计时器，为下一次冲刺做准备
         DashOjsetTimer = Time.time + DashOjsetTimers;
        }

    }
    private void FixedUpdate()
    {
        FixPlayerMove();
        CheckGround();
    }
    public void PlayerMove()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0 && !IsDead && !isAttack&&isGround)
        {
            ani.SetBool("IfRun", true);
            if (xInput < 0)
            {
                isright = false;
                Playerone.flipX = true;
                
            }
            else
            {
                isright = true;
                Playerone.flipX = false;
            }

        }
        else {
            ani.SetBool("IfRun", false);
        }
    }
    public void FixPlayerMove()
    {
        if (!IsDead && IsMove)
        {
            if (isDash)
            {
                Rg.velocity = new Vector2((isright ? 1 : -1) * Dashspeed, 0);
            }
            else
            {
                Rg.velocity = new Vector2(xInput * speed, Rg.velocityY);
            }
        }
    }
/// <summary>
/// 处理玩家跳跃相关的逻辑
/// 包括跳跃触发、空中状态检测和落地状态处理
/// </summary>
public void PlayerJump()
{
        if (!isDash && !IsGetHit && !IsDead)
        {

            // 检测玩家是否按下空格键
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 检查玩家是否在地面上
                if (isGround)
                {
                    ExecuteJump();
                }
                else
                {
                    StartJumpBuffer();
                }
            }

            if (hasJumpBuffered && isGround)
            {
                ExecuteJump();
                ClearJumpBuffer(); 
            }
            // 检查玩家是否在空中且正在下落，且未标记为空中状态
            if (!isGround && Rg.velocityY < 0 && !ani.GetBool("IsAir"))
            {
                // 设置动画状态为空中
                ani.SetBool("IsAir", true);
                // 标记玩家为空中状态
                isAir = true;
            }
            // 检查玩家是否落地，并且之前处于跳跃或空中状态
            if (isGround && (isJump || isAir))
            {
                // 触发落地动画
                ani.SetTrigger("Isground");
                // 重置空中状态
                ani.SetBool("IsAir", false);
                // 重置跳跃和空中状态标志
                isJump = false;
                isAir = false;
            }
        }
    
}
    private void StartJumpBuffer()
    {
        hasJumpBuffered = true;
        jumpBufferTimer = jumpBufferTime;
    }
    private void UpdateJumpBuffer()
    {
        if (hasJumpBuffered)
        {
            jumpBufferTimer -= Time.deltaTime;

           
            if (jumpBufferTimer <= 0)
            {
                ClearJumpBuffer();
            
            }
        }
    }

    private void ClearJumpBuffer()
    {
        hasJumpBuffered = false;
        jumpBufferTimer = 0f;
    }
    private void ExecuteJump()
    {
        // 触发跳跃动画
        ani.SetTrigger("Ifjump");
        // 添加向上的力使玩家跳跃
        Rg.AddForce(new Vector2(Rg.velocityX, jumpHeight));
        ClearJumpBuffer();
    }
    /// <summary>
    /// 检测玩家是否在地面上
    /// 通过从玩家位置发射一条向下的射线，检测是否与地面碰撞
    /// </summary>
    public void CheckGround()
    {
        if (!IsDead)
        {
            // 获取玩家当前位置作为射线起点
            Vector2 startPos = PlayeGo.transform.position;
            // 计算射线终点位置（起点正下方）
            Vector2 endPos = (Vector2)PlayeGo.transform.position +  new Vector2(0, -0.6f);
            // 发射射线并检测是否与地面碰撞
            RaycastHit2D Hit = Physics2D.Linecast(startPos, endPos, ground);
            // 如果检测到碰撞
            if (Hit.collider != null)
            {
                // 设置isGround为true，表示玩家在地面
                isGround = true;
            }
            else
            {
                // 设置isGround为false，表示玩家不在地面
                isGround = false;
            }
        }
       
    }
    //玩家冲刺
    public void playerdash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsDead)
        {
            
            if (!isDash&&!CanDash)
            {
                isDash = true;
                CanDash = true;
                ani.SetBool("IfDash", true);
                Invoke("dashend", DashTime);
                Invoke("CanDashtime", DashCD);
                
            }
        }
    }
    public void dashend()
    {
        isDash = false;
        ani.SetBool("IfDash", false);
        
    }
    public void CanDashtime()
    {
        CanDash = false;
    }
/// <summary>
/// 玩家攻击方法
/// 处理玩家的攻击输入和连招系统
/// </summary>
public void playerAttack()
{
        // 检测玩家是否按下J键或鼠标左键，并且当前不在攻击状态
        // 同时角色在地面上且水平速度为0时才能执行攻击
    if (Mathf.Abs(Rg.velocity.x) < 0.01f)
    {
            Rg.velocity = new Vector2(0, Rg.velocity.y);//手动速度归零
    }
    if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0)) &&!isAttack&&Rg.velocityX==0&&isGround&&!IsDead)
    {
        isAttack = true;
        CancelInvoke("Comboreset");
        Invoke("Comboreset", 2.5f);
        // 根据当前连招数执行不同的攻击动作
        switch (combo)
        {
            case 1:
                {
                    // 播放第一段攻击动画
                    ani.SetTrigger("IsAttack1");
                    Invoke("BuildAttackBox",0.4f);
                    // 连招数递增到第二段
                    combo = 2;
                    // 0.6秒后结束攻击状态
                    Invoke(nameof(playattacksound), Attacksoundtime[0]);
                    Invoke(nameof(Attackend), 0.4f);
                        if (isright)
                        {
                            Rg.AddForce(new Vector2(AttackAddForce, 0));
                        }
                        else
                        {
                            Rg.AddForce(new Vector2(-AttackAddForce, 0));
                        }
                        return;
                }
            case 2:
                {
                    // 播放第二段攻击动画
                    ani.SetTrigger("IsAttack2");
                    Invoke("BuildAttackBox", 0.4f);
                    // 连招数递增到第三段
                    combo = 3;
                        // 0.6秒后结束攻击状态
                        Invoke(nameof(playattacksound), Attacksoundtime[1]);
                        Invoke(nameof(Attackend), 0.4f);
                        if (isright)
                        {
                            Rg.AddForce(new Vector2(AttackAddForce, 0));
                        }
                        else
                        {
                            Rg.AddForce(new Vector2(-AttackAddForce, 0));
                        }
                        return;
                }
            case 3:
                {
                    // 播放第三段攻击动画
                    ani.SetTrigger("IsAttack3");
                    Invoke("BuildAttackBox", 0.4f); 
                    // 连招数重置为第一段
                    combo = 1;
                        // 0.6秒后结束攻击状态
                        Invoke(nameof(playattacksound), Attacksoundtime[2]);
                        Invoke(nameof(Attackend), 0.4f);
                        if (isright)
                        {
                            Rg.AddForce(new Vector2(AttackAddForce, 0));
                        }
                        else
                        {
                            Rg.AddForce(new Vector2(-AttackAddForce, 0));
                        }
                        return;
                }
        }
    }
    
}

/// <summary>
/// 攻击结束方法
/// 将攻击状态重置为false
/// </summary>
public void Attackend()
{
    // 重置攻击状态
    isAttack = false;
}

/// <summary>
/// 连招重置方法
/// 将连招数重置为1
/// </summary>
public void Comboreset()
{
    // 重置连招数
    combo = 1;
}
//生成攻击盒
public void BuildAttackBox()
    {
        GameObject go;
        switch (combo) {
            case 1:
                if (isright)
                {
                    go = Instantiate(Attackbox, AttackPoint1.position, AttackPoint1.rotation);
                    go.transform.localScale = AttackPoint1.lossyScale;
                }
                else
                {
                    go = Instantiate(Attackbox,AttackPoint4.position, AttackPoint1.rotation);
                    go.transform.localScale = AttackPoint1.lossyScale;
                }
                go.GetComponent<PlayerAttackbox>().damage = Calculatedamage(ATK, MF);
                break;
            case 2:
              
                if (isright)
                {
                    go = Instantiate(Attackbox, AttackPoint2.position, AttackPoint2.rotation);
                    go.transform.localScale = AttackPoint2.lossyScale;
                }
                else
                {
                    go = Instantiate(Attackbox, AttackPoint5.position, AttackPoint5.rotation);
                    go.transform.localScale = AttackPoint5.lossyScale;
                }
                go.GetComponent<PlayerAttackbox>().damage = Calculatedamage(ATK, MF-0.03F);
                break;
            case 3:
                if (isright)
                {
                    go = Instantiate(Attackbox, AttackPoint3.position, AttackPoint3.rotation);
                    go.transform.localScale = AttackPoint3.lossyScale;
                }
                else
                {
                    go = Instantiate(Attackbox, AttackPoint6.position, AttackPoint6.rotation);
                    go.transform.localScale = AttackPoint6.lossyScale;
                }
                go.GetComponent<PlayerAttackbox>().damage = Calculatedamage(ATK,MF);
                break;
                
        }
        
    }
    public float Calculatedamage(float ATK, float MF)
    {
        float damage = ATK * MF;
        return damage;
    }
    //受到攻击伤害
    public void GetHit(float damage,float Force)
    {
        if (!IsDead&&!IsGetHit)
        {
            ani.SetBool("IsGetHit",true);
            GetHitDamage(damage);
            Invoke(nameof(GetHitEnd), 0.6f);
            playerHp.value = NowHP / MaxHP;
            PlayerHpText.text = NowHP.ToString();
            if (isright){Rg.AddForce(new Vector2(-Force, 0));}
            else{ Rg.AddForce(new Vector2(Force, 0));}
            GameObject go = Instantiate(GetHitEffect, GetHitEffectpositi.transform.position, GetHitEffectpositi.transform.rotation, GetHitEffectpositi.transform);
            go.transform.localScale = GetHitEffectpositi.transform.lossyScale;
            go.GetComponent<Text>().text = damage.ToString();
            go.GetComponent<Text>().color = Color.yellow;
            Destroy(go, 0.8f);
            if (NowHP < 0)
            {
                IsDead = true;
            }
            Debug.Log(NowHP);
        }
    }
    public void GetHitDamage(float damage)
    {
        NowHP = NowHP - damage*Defense;
    }
    public void AttributeManager(int AttibuteID)
    {
        if (GameManager.Instance.EXP <1)
        {
            UGUIManager.Instance.PromptBoxShow("技能点不足");
            return;
        }
        switch (AttibuteID)
        {

            case 1:
                if (Defense > 0.2&&GameManager.Instance.EXP>=1)
                {
                    Defense -= 0.05f;
                    Debug.Log("防御力提升");
                    GameManager.Instance.EXP -= 1;
                    GameManager.Instance.Defenselv += 1;
                    UGUIManager.Instance.EXP -= 1;
                    return;
                }
                else
                {
                    UGUIManager.Instance.PromptBoxShow("防御力已达到上限");
                }
                break;
            case 2:
                if (ATK < 100 && GameManager.Instance.EXP >= 1)
                {
                    ATK += 2;
                    GameManager.Instance.EXP -= 1;
                    GameManager.Instance.Powerlv += 1;
                    UGUIManager.Instance.EXP -= 1;
                }else
                {
                    UGUIManager.Instance.PromptBoxShow("攻击力已达到上限");
                }
                break;
            case 3:
                if(speed<8&& GameManager.Instance.EXP >= 1)
                {
                    speed += 0.5f;
                    GameManager.Instance.EXP -= 1;
                    GameManager.Instance.Speedlv += 1;
                    UGUIManager.Instance.EXP -= 1;
                }
                else
                {
                    UGUIManager.Instance.PromptBoxShow("速度已达到上限");
                }
                break;
        }
        UGUIManager.Instance.EXPUGUIManager();
        UGUIManager.Instance.ResetAttLvText(AttibuteID);
    }
    public void GetHitEnd()
    {
        Rg.velocityX = 0;
        ani.SetBool("IsGetHit", false);
        if (IsDead)
        {
            ani.SetTrigger("IsDead");
            Invoke(nameof(GamgOver), 1f);
        }
    }
    public void GamgOver()
    {

    }
    public void playattacksound()
    {
        soundManager.Instance.playshotMuisc(0);
    }
}
