using System;
using UnityEngine;
using UnityEngine.UI;
using Client;

public class ItemRenamedcr : SingletonMonoBehaviour<ItemRenamedcr>
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Button btn_Surecr;
	private Image image_Buttom1cr;
	private Text text_Alertcr;
	private InputField input_Applymsgcr;
	private Button btn_Closecr;
	//结束UI申明;
	private bool isUIinit = false;

    private int _planid;

    private int _iosCharacterLimit = 0;

	protected override void Awake ()
	{
        base.Awake();
		cachedTransform=transform;
		//开始UI获取;
		btn_Surecr = cachedTransform.FindChild("image_Buttom2/btn_Surecr").GetComponent<Button>();
		image_Buttom1cr = cachedTransform.FindChild("image_Buttom1cr").GetComponent<Image>();
		text_Alertcr = cachedTransform.FindChild("image_Buttom1cr/text_Alertcr").GetComponent<Text>();
		input_Applymsgcr = cachedTransform.FindChild("image_Buttom1cr/input_Applymsgcr").GetComponent<InputField>();
		btn_Closecr = cachedTransform.FindChild("btn_Closecr").GetComponent<Button>();
		//结束UI获取;
		isUIinit = true;

        UIEventListener.Get(btn_Closecr.gameObject).onClick = OnBtnExitClick;
        UIEventListener.Get(btn_Surecr.gameObject).onClick = OnSureClick;

        _iosCharacterLimit = BundleIdUtil.CheckIosInput(input_Applymsgcr, OnCheckName);
	}

    private void OnCheckName(string name)
    {
        if (input_Applymsgcr.text.Length > _iosCharacterLimit)
        {
            input_Applymsgcr.text = input_Applymsgcr.text.Substring(0, _iosCharacterLimit);
        }
    }

    private void OnBtnExitClick(GameObject go)
    {
        Destroy(gameObject);
    }

    private void OnSureClick(GameObject go)
    {
        if(string.IsNullOrEmpty(input_Applymsgcr.text))
        {
            TipManager.Instance.ShowTip("名字不能为空",ColorEnum.White);
        }
        else
        {
            p_equip_info info = new p_equip_info();
            info.plan_id = _planid;
            info.plan_name = input_Applymsgcr.text;

            RoleModel.Instance.TosChangeEquip(EquipsOutsideView.Instance.ChooseHeroType, info, 3);

            OnBtnExitClick(null);
        }
    }

    public void SetName(string text,int planid)
    {
        _planid = planid;
        input_Applymsgcr.text = text;
    }


}

