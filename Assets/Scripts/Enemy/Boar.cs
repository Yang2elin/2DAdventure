using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy   //野猪继承Enemy
{
    protected override void Awake() //重写enemy.awake
    {
        base.Awake();   //在执行enemy。awake的基础上
        patrolState = new BoarPatrolState();    //创建野猪巡逻逻辑给到enemy中的patrolState
        chaseState = new BoarChaseState();
    }
}
