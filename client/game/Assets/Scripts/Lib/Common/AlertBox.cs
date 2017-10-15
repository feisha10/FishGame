using System;
using UnityEngine;
using UnityEngine.UI;

public class AlertBox : MonoBehaviour
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Image image_Maskcr;
	private Image image_bgcr;
	private Text Text_titlecr;
	private Button btn_leftcr;
	private Text Text_okcr;
	private Button btn_rifhttcr;
	private Text text_cancelcr;
	private Toggle tog_togcr;
	private Text text_togcr;
	private Text Text_msgcr;
	//结束UI申明;


    public Alert.OnCancel mOnCancel;
    public Alert.OnOK mOnOK;
    public Action<object> CheckAction;

    private int _bgClickType = 0;
    private bool _activeCheckBox = false;
    private string _checkBoxVal;

    void Awake()
    {

        cachedTransform = transform;
        //开始UI获取;
		image_Maskcr = cachedTransform.Find("image_Maskcr").GetComponent<Image>();
		image_bgcr = cachedTransform.Find("image_bgcr").GetComponent<Image>();
		Text_titlecr = cachedTransform.Find("title/Text_titlecr").GetComponent<Text>();
		btn_leftcr = cachedTransform.Find("btn_leftcr").GetComponent<Button>();
		Text_okcr = cachedTransform.Find("btn_leftcr/Text_okcr").GetComponent<Text>();
		btn_rifhttcr = cachedTransform.Find("btn_rifhttcr").GetComponent<Button>();
		text_cancelcr = cachedTransform.Find("btn_rifhttcr/text_cancelcr").GetComponent<Text>();
		tog_togcr = cachedTransform.Find("tog_togcr").GetComponent<Toggle>();
		text_togcr = cachedTransform.Find("tog_togcr/text_togcr").GetComponent<Text>();
		Text_msgcr = cachedTransform.Find("Text_msgcr").GetComponent<Text>();
		//结束UI获取;
        UIEventListener.Get(image_bgcr.gameObject).onClick = OnClickBg;
        UIEventListener.Get(btn_leftcr.gameObject).onClick = OnClickOK;
        UIEventListener.Get(btn_rifhttcr.gameObject).onClick = OnClickCancel;
        //UIEventListener.Get(BackGround.gameObject).onClick = OnClickBg;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="msg"></param>
    /// <param name="bgClickType">0,不可点击1，可点击点击关闭2点击取消</param>
    public void InitSetCommon(string title, string msg, int bgClickType = 0, string okStr = "", string cancelStr = "",bool sin=false)
    {
        _activeCheckBox = false;
        tog_togcr.gameObject.SetActive(false);
        Text_titlecr.text = title;
        Text_msgcr.text = msg;
        _bgClickType = bgClickType;
        Text_okcr.text = okStr;
        text_cancelcr.text = cancelStr;
        btn_rifhttcr.gameObject.SetActive(true);
        btn_leftcr.gameObject.GetRectTransform().anchoredPosition = new Vector2(-114, -78);
        if (sin)
        {
            btn_leftcr.gameObject.GetRectTransform().anchoredPosition = new Vector2(-15,-78);
            btn_rifhttcr.gameObject.SetActive(false);
        }
        //MotionText mon=Text_msgcr.gameObject.AddMissingComponent<MotionText>();
       // mon.Create();
    }

    public void AddSetCheckBoxInfo(string checkValue)
    {
        _activeCheckBox = true;
        tog_togcr.gameObject.SetActive(true);
        text_togcr.text = checkValue;
    }


    protected void OnClickCancel(GameObject go)
    {
        if (this.mOnCancel != null)
        {
            this.mOnCancel();
        }
        GameObject.Destroy(gameObject);
    }

    void OnDisable()
    {
        if (_activeCheckBox)
        {
            CheckBoxHandler();
        }
    }

    public String GetCheckBoxVal()
    {
        return _checkBoxVal;
    }

    private void OnClickBg(GameObject go)
    {
        switch (_bgClickType)
        {
            case 0:
                break;
            case 1:
                Destroy(gameObject);
                break;
            case 2:
                OnClickCancel(go);
                break;
        }
    }




    protected virtual void OnClickOK(GameObject go)
    {
        if (_activeCheckBox)
        {
            CheckBoxHandler();
        }
        if (this.mOnOK != null)
        {
            this.mOnOK();
        }
        try
        {
            Destroy(gameObject);
        }
        catch(Exception e)
        {

        }

    }

  

   

    /// <summary>
    /// 复选框处理
    /// </summary>
    private void CheckBoxHandler()
    {

    }
}

