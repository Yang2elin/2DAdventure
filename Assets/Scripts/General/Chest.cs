using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closedSprite;
    public bool isDone;
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
        Debug.Log("Chest opened!");
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
    }
}
