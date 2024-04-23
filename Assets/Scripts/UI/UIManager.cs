using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar; //获得血条ui组件

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {   //注册事件
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }

    private void OnDisable()
    {  //取消事件
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameSceneSO scenToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = scenToLoad.sceneType == SceneType.Menu;  //判断是否为菜单场景
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character) //事件注册以后执行
    {
        var persetage = character.currentHealth / character.maxHealth;  //计算血量百分比
        playerStatBar.OnHealthChange(persetage);    //将血量百分比传给血条
    }
}
