using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("撞击攻击参数")]
    public int damage;  //攻击伤害
    public float attackRange;   //攻击范围
    public float attackRate;    //攻击频率

    private void OnTriggerStay2D(Collider2D other)    //触发器
    {
        other.GetComponent<Character>()?.TakeDamage(this);   //  调用受伤者,?相当于判断是否有TakeDamage这个函数如果有则执行
    }
}
