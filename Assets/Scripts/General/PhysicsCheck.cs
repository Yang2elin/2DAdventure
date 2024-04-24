using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll; //碰撞体
    private PlayerController playerController;
    private Rigidbody2D rb;
    [Header("状态")]
    public bool isGround;    //检测是否在地面
    public bool touchLeftWall;  //检测是否碰到左墙
    public bool touchRightWall; //检测是否碰到右墙
    public bool onWall;
    [Header("检测参数")]
    public bool manual; //手动检测
    public bool isPlayer;   //是否是玩家
    public LayerMask grandLayer;    //地面
    public float checkRaduis;   //检测范围

    public Vector2 bottomOffset;    //偏移值
    public Vector2 rightOffset; //偏移值
    public Vector2 leftOffset;  //偏移值

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, coll.size.y / 2);
        }
        if (isPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        Check();
    }

    public void Check()
    {
        //isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, grandLayer); //检测是否在地面(返回true)
        if (onWall)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, grandLayer); //检测是否在地面(返回true)
        }
        else
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, grandLayer); //检测是否在地面(返回true)}
            //isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis, grandLayer); //检测是否在地面(返回true)
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, grandLayer); //检测是否碰到左墙(返回true)
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, grandLayer); //检测是否碰到右墙(返回true)
        }
        if (isPlayer)
        {
            onWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y < 0f; //检测是否在墙上(返回true
        }
    }

    private void OnDrawGizmosSelected()
    {   //绘制脚底检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        //绘制左侧检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        //绘制右侧检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
