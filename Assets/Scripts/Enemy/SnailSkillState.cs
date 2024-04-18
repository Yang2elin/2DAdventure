using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class SnailSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //currentEnemy.currentSpeed = currentEnemy.chaseSpeed;  //蜗牛的chasespeed是0
        currentEnemy.currentSpeed = 0;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("hide", true);
        currentEnemy.anim.SetTrigger("skill");
        currentEnemy.lostTimeCointer = currentEnemy.lostTime;

        currentEnemy.GetComponent<Character>().invuluerable = true;
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCointer;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCointer <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCointer;

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        //currentEnemy.anim.ResetTrigger("skill");
        currentEnemy.GetComponent<Character>().invuluerable = false;
    }

}
