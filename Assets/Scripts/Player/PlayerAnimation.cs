using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();    //获取组件Animator使用权
        rb = GetComponent<Rigidbody2D>();   //获取组件Rigidbody2D使用权
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));  //将Rigidbody2D中横向的速度的绝对值传给velocityX
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isCrouch", playerController.isCrouch);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);    //将isAttack传给animator
    }

    public void PlayHurt()  //受伤动画触发器
    {
        anim.SetTrigger("hurt");
    }

    public void PlayAttack()    //攻击动画触发器
    {
        anim.SetTrigger("attack");
    }
}
