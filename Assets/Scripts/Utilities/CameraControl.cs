using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;   //摄像机可移动范围
    public CinemachineImpulseSource impulseSource;  //摄像机振动源
    public VoidEventSO cameraShakeEvent;
    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }
    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();

    }
    private void Start()
    {
        GetNewCameraBounds();
    }
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");   //找到带bounds标签的物体
        if (obj == null)
        {
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();  //绑定新的范围
        confiner2D.InvalidateCache();   //清楚缓存
    }
}
