using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;


public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    public GameObject signSprite;   //获取组件
    private Animator anim; //用来获取子物体挂载的动画
    public Transform playerTransform;   //用来获取player的transform
    private IInteractable targetItem;  //用来获取IInteractable接口
    private bool canPress;  //是否有按下条件

    private void Awake()
    {
        //anim = GetComponentInChildren<Animation>(); //获取子物体上的组件***因为开始子物体是关闭的所以这里不适用***
        anim = signSprite.GetComponent<Animator>();  //获取子物体身上的组件***在inspector中将子物体传给了signSprite所以这里可以获取到***
        playerInput = new PlayerInputControl();
        playerInput.Enable();
        //canPress = false;
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;  //unity自带方法检测输入设备；
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }

    // private void OnDisable()
    // {
    //     canPress = false;
    // }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case XInputControllerWindows:
                    anim.Play("Xbox");
                    break;
                case Keyboard:
                    anim.Play("Keyboard");
                    break;
            }
        }
    }


    private void Update()
    {
        //signSprite.SetActive(canPress);
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTransform.localScale;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            //targetItem.GetComponent<AudioDefination>()?.PlayAudioClip();
            targetItem.TriggerAction(); //调用接口的方法，再由接口分配不同obj的不同方法
        }
    }
}
