using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);
        }
        //判断是否满足巡逻条件 不在地面上||撞左墙||撞右墙
        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0)
        {
            currentEnemy.wait = true;    //不满足巡逻条件开始计时等待
            currentEnemy.anim.SetBool("walk", false);    //停止巡逻动画
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }

    public override void OnExit()
    {
    }

    public override void PhysicsUpdate()
    {
    }
}
