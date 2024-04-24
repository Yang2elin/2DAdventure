using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;//因为player所在场景不会被unload所以可以直接拖拽传递不会丢失
    public Vector3 firstPosition;
    public Vector3 menuPosition;


    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;


    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO currentLoadedScene;
    private GameSceneSO locationToLoad;
    private Vector3 posToGo;
    private bool fadeScreen;
    private bool isLoading; //是否正在加载
    public float fadeDuration;  //渐变时间


    private void Start()
    {
        //NewGame();
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
    }

    private void Awake()
    {

        //对象，模式叠加Additive，（单独加载是Single）， 默认加载后直接激活
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

    }

    //注册事件
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }
    //注销事件
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        locationToLoad = firstLoadScene;
        //OnLoadRequestEvent(locationToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(locationToLoad, firstPosition, true);
    }

    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    //                              注意同名的全局变量
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading) return;  //如果正在加载就返回
        isLoading = true;  //正在加载
        //this是全局变量 后面的是传进来的参
        this.locationToLoad = locationToLoad;
        this.posToGo = posToGo;
        this.fadeScreen = fadeScreen;

        //加载场景
        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    //使用协程卸载场景方便判断
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen) //如果有渐变的话
        {
            fadeEvent.FadeIn(fadeDuration);  //渐变
        }
        yield return new WaitForSeconds(fadeDuration);  //等待填写的渐变时间以后

        unloadedSceneEvent.RaiseLoadRequestEvent(locationToLoad, posToGo, true);


        yield return currentLoadedScene.sceneReference.UnLoadScene();  //卸载当前场景

        playerTrans.gameObject.SetActive(false);  //隐藏玩家防止穿帮
        LoadNewScene();  //加载新的场景
    }

    private void LoadNewScene()
    {
        var loadingOption = locationToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);  //加载新的场景 异步加载
        loadingOption.Completed += OnLoadingCompleted;  //注册一个方法 加载完成后调用
    }

    /// <summary>
    /// 加载完成后调用
    /// </summary>
    /// <param name="handle"></param>
    private void OnLoadingCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        //throw new NotImplementedException();
        currentLoadedScene = locationToLoad;  //当前加载的场景是新的场景

        playerTrans.position = posToGo;  //传送玩家到指定位置
        playerTrans.gameObject.SetActive(true);  //显示玩家

        isLoading = false;  //加载完成

        //afterSceneLoadedEvent.RaiseEvent();  //广播加载完成事件

        if (fadeScreen)
        {

            fadeEvent.FadeOut(fadeDuration);  //渐变
        }

        if (currentLoadedScene.sceneType != SceneType.Menu)
        {
            afterSceneLoadedEvent.RaiseEvent();
            //Debug.Log("no menu");
        }
    }
}
