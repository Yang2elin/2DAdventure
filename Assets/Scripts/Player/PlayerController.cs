using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;
using Unity.Collections;
using System;
public class PlayerController : MonoBehaviour   //PlayerController继承于unity自有方法MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO loadEvent;    //监听开始加载场景事件
    public VoidEventSO afterSceneLoadedEvent;    //监听场景加载完成事件
    public PlayerInputControl inputControl; //定义变量 PlayerInputControl是InputSyetem文件夹中生成的
    private Rigidbody2D rb;  //定义变量 类型刚体
    private PhysicsCheck physicsCheck;
    public Vector2 inputDirection;  //定义变量获取人物移动数值获取移动方向
    private CapsuleCollider2D capsuleCollider2D;    //2D胶囊碰撞体
    private PlayerAnimation playerAnimation;    //获取PlayerAnimation脚本
    private Vector2 originalSize;   //原始的碰撞尺寸
    private Vector2 originalOffset; //原始的碰撞位置
    private Collider2D coll;    //碰撞体

    [Header("物理材质")]
    public PhysicsMaterial2D noFriction;    //无摩擦力normal
    public PhysicsMaterial2D fullFriction;  //全摩擦力wall  在unity中可以设置摩擦力

    [Header("基本参数")]
    public bool isCrouch;   //正在下蹲
    public bool isHurt; //正在受伤
    public bool isDead; //死亡
    public bool isAttack;   //正在攻击
    public float speed; //定义变量获取速度 速度可以在unity里输入
    public float jumpForce; //跳跃的力
    public float hurtForce; //受伤被击退的力

    private float runSpeed; //跑步速度
    private float walkSpeed => speed / 3.5f;  //走路速度，lambda表达式每次调用都会执行一下

    //[Header("音效")]
    //public AudioDefination jumpAudio;   //跳跃音效
    //public AudioDefination deadAudio;   //死亡音效
    private AudioDefination audioDefination;//
    public AudioClip jumpAudioClip; //跳跃音效
    public AudioClip deadAudioClip; //死亡音效
    public AudioClip hurtAudioClip; //受伤音效


    #region 周期函数
    private void Awake()
    {  //游戏启动时
        rb = GetComponent<Rigidbody2D>();   //获得Rigidbody2D类型的Component
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();    //不是组件是脚本所以new创建实例实例化
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();  //保存胶囊碰撞体原始大小
        originalOffset = capsuleCollider2D.offset;
        originalSize = capsuleCollider2D.size;
        playerAnimation = GetComponent<PlayerAnimation>();  //获取PlayerAnimation脚本
        coll = GetComponent<Collider2D>();   //获取碰撞体
        audioDefination = GetComponent<AudioDefination>();  //获取AudioDefination脚本

        inputControl.GamePlay.Attack.started += PlayerAttack;

        inputControl.GamePlay.Jump.started += Jump; //+=注册函数 意味着在inputControl.GamePlay.Jump.started按键按下这一刻时执行函数 这一刻可以注册多个函数

        #region 强制走路
        runSpeed = speed;
        inputControl.GamePlay.Walk.performed += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };    //performed按住状态

        inputControl.GamePlay.Walk.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };    //performed按住状态
        #endregion
    }


    private void OnEnable()
    {   //player在unity中被勾选显示出来的话执行
        inputControl.Enable();
        loadEvent.LoadRequestEvent += OnloadEvent; //注册事件
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {  //player在unity中未被勾选的话执行
        inputControl.Disable();
        loadEvent.LoadRequestEvent -= OnloadEvent; //注销事件
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }



    private void Update()
    { //代码周期性函数 游戏中每一帧都会执行
        /*
        将读取的内容传递给inputDirection，
        读取的内容是inputControl中名字为Gameplay的AtcionMap中名字为Move的Action的Vector2的数值（2D Vector）
        Action → （Action Type = value）（Control Type = Vector 2）
        因为inputDirection是public所以在unity中可以显示数字
        */
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();   //检查状态
    }
    private void FixedUpdate()
    {    //跟物理有关的一般都放在fixupdate中执行，他是一个固定时钟频率执行，不受设备影响
        if (!isHurt && !isAttack)    //没有被攻击可以移动
        {
            Move();
        }

    }
    #endregion

    private void OnloadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable();    //禁止游玩操作
        //Debug.Log("PlayerController OnloadEvent");
    }
    private void OnAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();    //允许游玩操作
        //Debug.Log("PlayerController OnAfterSceneLoadedEvent");
    }

    #region 按键绑定
    public void Move()
    { //行动
      //if (!isCrouch) //下蹲状态禁止移动
        rb.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x, rb.velocity.y);//左右移动 Vector2表示二维空间向量 rb.velocity对应unity中Rigidbody2D的velocity前面rb获取了组件 x速度=速度*时间的修正*x方向

        int faceDir = (int)transform.localScale.x;  //定义获取人物x轴的方向，transform组件是每个game object默认都有的无需代码获取
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        if (inputDirection.x < 0)
        {
            faceDir = -1;
        }

        //人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);  //Vector3三维变量 让x旋转yz不变实现左右转身

        //下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)   //胶囊碰撞体修改为下蹲以后
        {
            capsuleCollider2D.offset = new Vector2(-0.07f, 0.75f);
            capsuleCollider2D.size = new Vector2(0.7f, 1.5f);
        }
        else    //还原为正常
        {
            capsuleCollider2D.offset = originalOffset;
            capsuleCollider2D.size = originalSize;
        }

    }

    private void Jump(InputAction.CallbackContext context)  //跳跃
    {
        //Debug.Log("jump");
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);    //添加向上方向的力来跳跃
            //GetComponent<AudioDefination>()?.PlayAudioClip();  //播放跳跃音效
            //jumpAudio?.PlayAudioClip(); //播放跳跃音效
            audioDefination.audioClip = jumpAudioClip; //跳跃音效
            audioDefination.PlayAudioClip(); //播放跳跃音效
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)  //攻击
    {
        playerAnimation.PlayAttack();   //播放攻击动画
        isAttack = true;    //修改人物状态为正在攻击
    }

    #endregion

    #region Unity Event事件
    //传入攻击者的坐标
    public void GetHurt(Transform attacker)
    {
        //修改人物状态为正在受伤
        isHurt = true;
        //受伤反弹前先将人物速度归零
        rb.velocity = Vector2.zero;
        //给x轴攻击者反方向 归一化
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        //方向*反弹力 2D瞬时力
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

        audioDefination.audioClip = hurtAudioClip; //受伤音效
        audioDefination.PlayAudioClip(); //播放受伤音效
    }

    public void PlayerDead()
    {
        // audioDefination.audioClip = deadAudioClip;
        // audioDefination.PlayAudioClip(); //播放死亡音效
        isDead = true;
        inputControl.GamePlay.Disable();    //禁止游玩操作
    }
    #endregion

    #region 其他函数
    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? fullFriction : noFriction;
    }
    #endregion
}
