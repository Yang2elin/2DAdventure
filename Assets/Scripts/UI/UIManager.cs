using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar; //获得血条ui组件

    [Header("事件监听")]
    public CharacterEventSO healthEvent;

    private void OnEnable()
    {   //注册事件
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {  //取消事件
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character) //事件注册以后执行
    {
        var persetage = character.currentHealth / character.maxHealth;  //计算血量百分比
        playerStatBar.OnHealthChange(persetage);    //将血量百分比传给血条
    }
}
