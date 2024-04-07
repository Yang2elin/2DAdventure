using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{


    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("switch to chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;    //切换追逐速度
        currentEnemy.anim.SetBool("run", true); //切换动画
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCointer <= 0)    //达到丢失目标时间
        {
            currentEnemy.SwitchState(NPCState.Patro);   //修改回巡逻状态
        }
        //判断不在地面上||撞左墙||撞右墙 不做等待直接追击
        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0)
        {
            // currentEnemy.wait = true;    //不满足巡逻条件开始计时等待
            // currentEnemy.anim.SetBool("walk", false);    //停止巡逻动画
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);  //撞墙 悬崖 马上转身
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.lostTimeCointer = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("run", false);
    }
}
