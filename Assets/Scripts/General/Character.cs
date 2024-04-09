using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   //unity事件库

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth; //最大血量
    public float currentHealth; //当前血量

    [Header("受伤无敌")]
    public float invulnerableDuration;  //无敌总时间
    public float invulnerableCounter;   //无敌计时器
    public bool invuluerable;   //无敌状态

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;  //创建事件 传进去transform
    public UnityEvent OnDie;    //死亡事件

    private void Start()
    {
        currentHealth = maxHealth;  //新开始游戏当前血量是最大值
        OnHealthChange?.Invoke(this);
    }

    private void Update()
    {
        if (invuluerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invuluerable = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)  //落水死亡
    {
        if (other.CompareTag("Water"))
        {
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDie?.Invoke();
        }

    }

    public void TakeDamage(Attack attacker) //从攻击者接收伤害
    {
        if (invuluerable)
            return;
        else
        {
            if (currentHealth - attacker.damage > 0)
            {
                //受伤后
                currentHealth -= attacker.damage;   //受伤后当前血量减去伤害
                TriggerInvnlnerable();  //触发无敌
                OnTakeDamage?.Invoke(attacker.transform);   //触发受伤事件》动画》位移
            }
            else
            {
                currentHealth = 0;
                //触发死亡
                OnDie?.Invoke();
            }
        }

        OnHealthChange?.Invoke(this);

    }

    private void TriggerInvnlnerable()  //无敌触发器
    {
        if (!invuluerable)
        {
            invuluerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
