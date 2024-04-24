using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closedSprite;
    public bool isDone;

    public static UnityEvent OnChestOpened = new UnityEvent();



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closedSprite;
    }
    public void TriggerAction()
    {
        //Debug.Log("Chest opened!");
        if (!isDone)
        {
            //this.gameObject.tag = "Interactable";
            OpenChest();
        }
    }



    private void OpenChest()
    {
        isDone = true;
        spriteRenderer.sprite = openSprite;
        this.gameObject.tag = "Untagged";   //打开后不再是可交互物体
        //GetComponent<AudioDefination>()?.PlayAudioClip();
        GetComponent<AudioSource>()?.Play();

        OnChestOpened?.Invoke();
    }

    // private void OnTriggerStay2D(Collider2D other)    //触发器
    // {
    //     //other.GetComponent<Character>()?.TakeDamage(this);   //  调用受伤者,?相当于判断是否有TakeDamage这个函数如果有则执行
    //     player = other;
    // }
}
