using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UpdateWork;

public class LoadingView : MonoBehaviour
{
	private Transform cachedTransform ;
	//开始UI申明;
    private Slider slider_updatecr;
    private Text Text_loadingcr;
    private GameObject go_alertcr;
    private Text text_allsizecr;
    private Button btn_confirmcr;
    private Button btn_cancelcr;
    //结束UI申明;

    void Awake()
	{
		cachedTransform=transform;
		//开始UI获取;
        Text_loadingcr = cachedTransform.FindChild("GameObject/Text_loadingcr").GetComponent<Text>();
        slider_updatecr = cachedTransform.FindChild("go_bottomcr/Slider_updatecr").GetComponent<Slider>();
        Transform trans = cachedTransform.FindChild("go_alertcr");
        if (trans!=null)
        {
            go_alertcr = cachedTransform.FindChild("go_alertcr").gameObject;
            text_allsizecr = cachedTransform.FindChild("go_alertcr/text_allsizecr").GetComponent<Text>();
            btn_confirmcr = cachedTransform.FindChild("go_alertcr/btn_confirmcr").GetComponent<Button>();
            btn_cancelcr = cachedTransform.FindChild("go_alertcr/btn_cancelcr").GetComponent<Button>();
        }

        //结束UI获取;
    }

    public void SetLoadInfo(string loadingInof)
    {
        Text_loadingcr.text = loadingInof;
    }

    public void SetLoadPerCent(int per)
    {
        slider_updatecr.value = per/100f;
    }

    public void ShowAlert(double size, UnityAction onConfirm, UnityAction onCancel)
    {
        if (go_alertcr==null)
        {
            onConfirm.Invoke();
            return;
        }
        go_alertcr.SetActive(true);
        text_allsizecr.text = string.Format("资源大小：{0}MB", size.ToString("F1"));
        btn_confirmcr.onClick.RemoveAllListeners();
        btn_confirmcr.onClick.AddListener(() => 
        {
            go_alertcr.SetActive(false);
            text_allsizecr.text = "点击了更新";
            onConfirm.Invoke();
        });
        btn_cancelcr.onClick.RemoveAllListeners();
        btn_cancelcr.onClick.AddListener(onCancel);
    }
}

