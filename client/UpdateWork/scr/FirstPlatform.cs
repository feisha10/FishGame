using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace UpdateWork
{
    public class FirstPlatform:MonoBehaviour
    {
        static string _sdkName = null;

#if UNITY_EDITOR

#elif UNITY_ANDROID
    
    public static AndroidJavaObject platformAndroidObject = null;

    public static void AndroidCall(string className, string param = "")
    {
        try
        {
            if (null == param)
                platformAndroidObject.Call(className);
            else
                platformAndroidObject.Call(className, param);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
#endif

        static void InitSdk()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            WriteLog("启动Android SDK: {0}", _sdkName);
            if (platformAndroidObject == null)
            {
                AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                platformAndroidObject = ajc.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidCall("InintLang", FirstLang.FIRST_MSG_14);
            }
#elif UNITY_IOS
      //OC_CallIOSFirst("FirstInit");
#endif
        }

        public static void CheckUpdate(string sdkName)
        {
            _sdkName = sdkName;

            InitSdk();

            WriteLog("start CheckUpdate");
#if UNITY_EDITOR
        OnVersionOk();
#elif UNITY_ANDROID
        AndroidCall("platformVersionUpdate");
#elif UNITY_IOS
        switch (_sdkName)
        {
            case "IOSSDK4399":
                OnVersionOk();
                break;
         case "IOSSDKTW":
                OnVersionOk();
                break;
            default:
                break;
        }
#endif
        }

        //android端显示dialog
        public static void ShowAlert(string title, string message, string ok, string chancel, string id)
        {
            WriteLog("ShowAlert");
            string par = string.Format("{0}#{1}#{2}#{3}#{4}", title, message, chancel, ok, id);//安卓的写反了
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidCall("CallCommonFunction", string.Format("ShowAlert;{0}", par));
#elif UNITY_IOS
        OC_CallIOSFirst("OCAlert",new string[] { title,message,ok,chancel,id });
#endif
        }

        /// <summary>
        /// //Android alter端返回 0#0   第一个表示传入的alter id  第二个表示 1确定 2，取消
        /// </summary>
        public void IOS_versionCfm(string info)
        {
            WriteLog("Android_Showalter" + info);
            string[] infos = info.Split('#');

            if (infos.Length > 1)
            {
                if (infos[0] == "1")
                {
                    if (infos[1] == "1")
                    {
                        WriteLog("返回确认");
                        Application.OpenURL(FirstUtil.ApkUrl);
                        FirstUtil.ExitApp();
                    }
                    else if (infos[1] == "2")
                    {
                        WriteLog("返回取消");
                        FirstUtil.ExitApp();
                    }
                }
            }
        }

        //Android alter端返回 0#0   第一个表示传入的alter id  第二个表示 1确定 2，取消
        public void Android_Showalter(string info)
        {
            WriteLog("Android_Showalter" + info);
            string[] infos = info.Split('#');
            if (infos.Length > 1)
            {
                if (infos[0] == "1")
                {
                    if (infos[1] == "1")
                    {
                        WriteLog("返回确认");
                        FirstUtil.ExitApp();
                    }
                    else if (infos[1] == "2")
                    {
                        WriteLog("返回取消");
                        Debug.Log(FirstUtil.ApkUrl);
                        Application.OpenURL(FirstUtil.ApkUrl);
                    }
                }
            }
        }

        /// <summary>
        /// 版本更新回调
        /// </summary>
        /// <param name="msg">
        /// 1 已经是最新版,进入正常游戏
        /// 2 没有检测到SD卡
        /// 3 取消强制更新
        /// 4 取消更新
        /// 5 版本检测失败
        /// 6 强制更新
        /// 7 普通更新正在下载时会调用此方法
        /// 8 网络链接错误时调用
        /// 9 异常
        /// </param>
        public void Andoid_platformVersionUpdateCallback(string msg)
        {
            WriteLog("版本检测：{0}", msg);
            switch (msg)
            {
                case "1":   //无新版本
                    OnVersionOk();
                    break;
                case "2":   //强制更新,停止当前热更新
                    UpdateWork.IsStopUpdate = true;
                    break;
                default:
                    //平台层处理（Alert信息尚未初始化）
                    OnVersionError();
                    break;
            }
        }

        /// <summary>
        /// 版本更新回调
        /// </summary>
        /// <param name="msg">
        /// 1 已经是最新版,进入正常游戏
        /// 2 没有检测到SD卡
        /// 3 取消强制更新
        /// 4 取消更新
        /// 5 版本检测失败
        /// 6 强制更新
        /// 7 普通更新正在下载时会调用此方法
        /// 8 网络链接错误时调用
        /// 9 异常
        /// </param>
        public void IOS_platformVersionUpdateCallback(string msg)
        {
            WriteLog("版本检测：{0}", msg);
            switch (msg)
            {
                case "1":   //无新版本
                    OnVersionOk();
                    break;
                case "2":   //强制更新,停止当前热更新
                    OnVersionError();
                    break;
                default:
                    //平台层处理（Alert信息尚未初始化）
                    OnVersionError();
                    break;
            }
        }

        /// <summary>
        /// 蜂鸟SDK初始化完成
        /// </summary>
        /// <param name="msg"></param>
        public void Android_initFNSdkCallback(string id)
        {
            FirstUtil.agentID = int.Parse(id);
        }

        //调用IOS函数
        public static void IosCall(string funName, string param = null)
        {
            try
            {
                switch (funName)
                {
                    case "OCinit4399SDK":
#if IOS4399
                    //OCinit4399SDK(IosSdkParam.theAppKey4399, IosSdkParam.appSecret4399, IosSdkParam.chanelID4399, IosSdkParam.CallBackReceiver);
#endif
                        break;
                    case "OCAlert": //IOS提示
                        //                    #if IOS4399
                        //                    OC_CallIOS("OCAlert",new string[] { title,msg,ok,cancel,id });
                        //                    #endif
                        break;
                    case "OCinitTWSDK":
#if IOSTW
                    //OCinit4399SDK(IosSdkParam.theAppKey4399, IosSdkParam.appSecret4399, IosSdkParam.chanelID4399, IosSdkParam.CallBackReceiver);
#endif
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

        }


        public void ExitCallback()
        {
            WriteLog("ExitCallback()");

            FirstUtil.ExitApp();
        }

        static void OnVersionOk()
        {

        }

        static void OnVersionError()
        {
            // ...
        }

        static void WriteLog(string msg, params object[] args)
        {
            FirstUtil.WriteLog("[sdk]" + msg, args);
        }

#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern void OC_CallIOSFirst(string funName, string[] param = null);
#endif
    }
}
