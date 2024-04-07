using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb; // 敌人刚体
    [HideInInspector] public PhysicsCheck physicsCheck;  // 物理检测
    [HideInInspector] public Animator anim;  // 敌人动画 protected只能让子类访问 public所有人都可以访问
    public Transform attacker;  // 攻击范围

    [Header("基本参数")]
    public int normalSpeed; // 普通速度
    public int chaseSpeed;  // 追逐速度
    public float currentSpeed; // 当前速度
    public Vector3 faceDir;  // 移动方向
    public float hurtForce; // 受伤力

    [Header("检测")]
    public Vector2 centerOffset;    //偏移量
    public Vector2 checkSize;   //检测盒尺寸
    public float checkDistance; //检测距离
    public LayerMask attackLayer;   //检测图层

    [Header("计时器")]
    public float witeTime; // 等待时间
    public float waitTimeCounter; // 计时器
    public bool wait;  // 是否等待
    public float lostTime;  //丢失目标时间
    public float lostTimeCointer;

    [Header("状态")]
    public bool isHurt; // 是否受伤
    public bool isDead; // 是否死亡
    private BaseState currentState; //当前状态
    protected BaseState patrolState;    //巡逻状态
    protected BaseState chaseState; //追击状态

    //onenable周期函数 当被激活时
    private void OnEnable()
    {
        currentState = patrolState; //激活时默认巡逻状态
        currentState.OnEnter(this); //执行onenter里面的方法 并把当前的enemy传进去
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
    }

    //逻辑判断周期函数持续执行
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);  //Transform组件的Scale属性 获取当前enemy的面对方向以右为正 因为野猪图片默认是朝左所以取负值

        currentState.LogicUpdate(); //调用状态机中的逻辑判断方法
        TimeCounter();  //自定义计时器
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !wait)
        {
            Move();
        }
        currentState.PhysicsUpdate();    //调用状态机中的物理逻辑判断方法
    }

    //ondisable函数在物体消失时执行
    private void OnDisable()
    {
        currentState.OnExit();  //调用状态机中的退出方法
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    public void TimeCounter()   //计时器
    {
        if (wait)   //停止巡逻等待
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = witeTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);    //转身
            }
        }

        if (!FoundPlayer() && lostTimeCointer > 0)
        { //没找到玩家
            lostTimeCointer -= Time.deltaTime;
        }
        // else
        // {
        //     lostTimeCointer = lostTime;
        // }
    }

    //发现玩家
    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);    //原点 尺寸 角度 方向 检测距离 图层
    }

    //切换状态 从Enums中选择
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patro => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans)
    {
        wait = false;
        waitTimeCounter = witeTime;   //收到攻击后不再等待

        attacker = attackTrans;
        //转身
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        isHurt = true;  //受伤状态
        anim.SetTrigger("hurt");    //受伤动画
        Vector2 dir = new Vector2((transform.position.x - attackTrans.position.x), 0).normalized;  //给x轴攻击者反方向 归一化
        rb.velocity = new Vector2(0, rb.velocity.y);    //受伤时把x轴速度归零避免冲锋速度影响击退
        StartCoroutine(OnHurt(dir)); //开始携程OnHurt 固定写法
    }

    private IEnumerator OnHurt(Vector2 dir)  //携程 迭代器 协同程序返回值为IEnumerator
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);  //受伤反弹
        yield return new WaitForSeconds(0.5f);  //等待0.5秒
        isHurt = false;
    }

    public void OnDie() //死亡
    {
        gameObject.layer = 2;   //在unity edit项目设置中物理2d的碰撞关系取消了player和gnore raycast的碰撞 这里修改可以避免player和播放死亡动画的敌人发生碰撞关系
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestoryAfterAnimation()  //动画结束后销毁 在unity anim里调用
    {
        Destroy(this.gameObject);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
