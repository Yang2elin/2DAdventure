using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGround : MonoBehaviour
{
    [SerializeField] private Vector2 movingSpeed;
    private Vector2 offset;
    private Material material;

    private Vector2 lastPosition;
    private float speedDir;
    private void Start()
    {
        lastPosition = transform.position;    //获取初始位置
    }

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }
    private void Update()
    {
        speedDir = transform.position.x - lastPosition.x;
        //Debug.Log("Current speedDir: " + speedDir);
        // if (speedDir < 0)
        // {
        //     speedDir = -1;
        // }
        // else if (speedDir > 0)
        // {
        //     speedDir = 1;
        // }

        offset = speedDir * Time.deltaTime * movingSpeed;
        material.mainTextureOffset += offset; //=new Vector(offset,0);


        lastPosition = transform.position;    //获取初始位置
    }

}
