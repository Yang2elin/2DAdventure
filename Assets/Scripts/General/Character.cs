using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   //unity事件库

public class Character : MonoBehaviour
{
    [Header("事件监听")]
    //public VoidEventSO newGameEvent;
    public VoidEventSO afterSceneLoadEvent;

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
    //public UnityEvent OnGameOverCheck;   //游戏结束事件

    private void NewGame()
    {
        currentHealth = maxHealth;  //新开始游戏当前血量是最大值
        OnHealthChange?.Invoke(this);
        //Debug.Log("reset health");
    }

    private void OnEnable()
    {
        afterSceneLoadEvent.OnEventRaised += NewGame;
        Chest.OnChestOpened.AddListener(HealPlayer);
    }
    private void OnDisable()
    {
        afterSceneLoadEvent.OnEventRaised -= NewGame;
        Chest.OnChestOpened.RemoveListener(HealPlayer);
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


    private void HealPlayer()
    {
        // 在这里实现加血逻辑，这里加100点生命值
        currentHealth += 100;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChange?.Invoke(this);
        Debug.Log("Player healed! Current health: " + currentHealth);
    }

    public void TakeDamage(Attack attacker) //从攻击者接收伤害
    {
        // if (attacker.CompareTag("GameOverCheck"))//如果标签是GameOverCheck
        // {
        //     OnGameOverCheck?.Invoke();  //触发游戏结束事件
        // }
        if (invuluerable || attacker.damage <= 0)
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
