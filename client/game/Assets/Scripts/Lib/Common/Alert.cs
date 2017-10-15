using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Alert
{
    private static readonly int DefaultMessageBoxUIDepth = 301;
    public static Dictionary<string,string> dicCheckBoxAlert = new Dictionary<string, string>(); 

    private static void SetImageBtnLabel(GameObject btn, string labelStr)
    {
        if ((labelStr != null) && btn.gameObject.activeSelf)
        {
            Text componentInChildren = btn.GetComponentInChildren<Text>();
           
            if (componentInChildren != null)
            {
                TextSpacing spce=componentInChildren.gameObject.AddMissingComponent<TextSpacing>();
                if (labelStr.Length>2)
                {
                    spce.TxtSpacing = 0;
                }
                else
                {
                    spce.TxtSpacing = 30;
                }
                componentInChildren.text = labelStr;
            }
        }
    }

    public static GameObject Show(string message, GameObject parentGO)
    {
        return Show(message, parentGO, (OnOK)null, (OnCancel)null);
    }

    public static GameObject Show(string message, GameObject parent, OnOK ok, OnCancel cancel)
    {
        return Show(message, parent, ok, cancel, null, null);
    }

    public static GameObject Show(string message, GameObject parentGO, GameObject cbgo)
    {
        return Show(message, null, parentGO, cbgo, false, null, null, null, null);
    }

    public static GameObject Show(string message, GameObject parent, OnOK ok, OnCancel cancel, string okStr, string cancelStr, string checkStr = "", int bgClickType = 0, float maskAlpha = 0f, DirectionPostion pivot = DirectionPostion.Center)
    {
        return Show(message, null, parent, null, false, okStr, ok, cancelStr, cancel, checkStr,bgClickType, pivot);
    }
    public static GameObject Show(string message, string title, GameObject parent, OnOK ok, OnCancel cancel, string okStr, string cancelStr, string checkStr = "", int bgClickType = 0)
    {
        return Show(message, title, parent, null, false, okStr, ok, cancelStr, cancel, checkStr, bgClickType);
    }


    static AlertBox _alertCtrl; // 目前就show有用到
    public static AlertBox AlertCtrl { get { return _alertCtrl; } }
    private static GameObject Show(string message, string title, GameObject parentGO, GameObject receiverGO, bool isSingle, string okStr, OnOK okFun, string cancelStr, OnCancel cancelFun,string checkStr="" ,int bgClickType = 0, DirectionPostion align = DirectionPostion.Center)
    {
        GameObject prefab = ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/TipAlter/AlertBox");
        
        if (parentGO==null)
        {
            parentGO = UIManager.TipLayer.gameObject;
        }

        GameObject obj3 = UGUITools.AddChild(parentGO, prefab);

        Util.SetLayer(obj3, parentGO.layer);
        AlertBox component = obj3.AddMissingComponent<AlertBox>();
        if (string.IsNullOrEmpty(title))
            title = "提示";
        if (string.IsNullOrEmpty(okStr))
            okStr = "确定";
        if (string.IsNullOrEmpty(cancelStr))
            cancelStr = "取消";
        // 默认都是enable
        component.InitSetCommon(title, message, bgClickType, okStr, cancelStr,isSingle);
        component.mOnOK = okFun;
        component.mOnCancel = cancelFun;

        if (!string.IsNullOrEmpty(checkStr))
        {
            component.AddSetCheckBoxInfo(checkStr);

        }
       
        return obj3;
    }

    public static GameObject ShowSingleBtn(string message)
    {
        return ShowSingleBtn(message, null);
    }

    public static GameObject ShowSingleBtn(string message, GameObject parent)
    {
        return ShowSingleBtn(message, parent, null);
    }

    public static GameObject ShowSingleBtn(string message, GameObject parent, OnOK ok)
    {
        return ShowSingleBtn(message, parent, ok, null);
    }

    public static GameObject ShowSingleBtn(string message, GameObject parent, OnOK ok, string Btntext)
    {
        return ShowSingleBtn(message, null,parent, null, Btntext, ok,DirectionPostion.Center);
    }

    public static GameObject ShowSingleBtn(string message, string title, GameObject parent, DirectionPostion align)
    {
        return ShowSingleBtn(message, title, parent, null, null, null,align);
    }

    //空白处和背景是否可点击 由CfmOnly控制
    public static GameObject ShowSingleBtn(string message, GameObject parent, OnOK ok, string Btntext,bool CfmOnly)
    {
        GameObject alertGo = ShowSingleBtn(message, null, parent, null, Btntext, ok, DirectionPostion.Center);
        AlertBox component = alertGo.GetComponent<AlertBox>();
        return alertGo;
    }

    private static GameObject ShowSingleBtn(string message, string title, GameObject parent, GameObject receiverGO, string btnStr, OnOK btnFun, DirectionPostion align)
    {
        return Show(message, title, parent, receiverGO, true, btnStr, btnFun, null, null,"", 0 ,align);
    }

    public static void CheckBuy(int payValue, int payType)
    {
    }

    /// <summary>
    /// 消耗非绑定钻石的操作提示
    /// </summary>
    /// <param name="needNum">需要消耗的非绑定钻石</param>
    /// <param name="okHandler"></param>
    /// <param name="msg">消耗非绑定钻石的提示语</param>
    /// <param name="cancel"></param>
    /// <param name="OnNotEnough"></param>
    /// <param name="parentGo"></param>
    public static void CheckUnbindDiamond(int needNum, OnOK okHandler, string msg = "", OnCancel cancel = null,Action OnNotEnough=null,GameObject parentGo=null,string StrOk=null,string Strcance=null,bool isCheckBox=false,string key ="")
    {
    }


    /// <summary>
    /// 钻石不足处理
    /// </summary>
    /// <param name="parentGo"></param>
    /// <param name="cancel"></param>
    /// <param name="OnNotEnough">钻石不足时的处理函数，多用来关闭一些多余的UI</param>
    public static void AlertDiamondNotEnought(GameObject parentGo,OnCancel cancel = null, Action OnNotEnough = null)
    {
        if (UIManager.TipLayer != null)
        {
            Show("源石不足啦，去弄点么？", parentGo, () =>
            {
                if (OnNotEnough != null)
                    OnNotEnough();
                GameObject go = null;

                go = UIManager.TipLayer.gameObject;
                if (go != null)
                {
                    foreach (Transform item in go.transform)
                    {
                        if (item.name != "PopMenu")
                        {
                            GameObject.Destroy(item.gameObject);
                        }
                    }
                }
/*                ShopData.Instance.OpenChargePanel();*/
                if (cancel != null)
                    cancel();

            }, cancel);
        }
        else
        {
            if (cancel != null)
                cancel();
        }
    }

    public static void Closed()
    {
        if (_alertCtrl != null && _alertCtrl.gameObject != null)
        {
            GameObject.Destroy(_alertCtrl.gameObject);
        }
    }

    public delegate void OnCancel();

    public delegate void OnOK();
}

public enum CheckDiamondResult
{
    DiamondNotEnough = 1,
    ActionNotMsg = 2,
    AlertNotMsg = 3,
    ActionShowMsg = 4,
    AlertShowMsg = 5

}
