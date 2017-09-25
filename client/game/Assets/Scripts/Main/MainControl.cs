using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : SingletonMonoBehaviour<MainControl>
{

    protected override void Awake()
    {
        base.Awake();

        GameSetting.Init();
        CameraSetting.InitUICamera();

        InitFirstUtil();
    }

    void InitBuglySDK()
    {

    }

    void InitFirstUtil()
    {
        FirstInit();
    }

    private void FirstInit()
    {
        UILayerManager.Instance.Init();
        UIManager.Init(SceneName.MainScene);

        UILayerManager.Instance.OpenPanel("LoginView", o =>
        {

        });
    }
}
