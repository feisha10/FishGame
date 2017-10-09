using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemeEquipOutSidecr :BaseItemRender
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Image image_Equipcr;
	private Image Image_skill1cr;
	private ScrollRect Scr_EquipStarcr;
	private GridLayoutGroup glayout_EquipStarcr;
	private Image image_Selectedcr;
	private Text text_EquipNamecr;
	private Text text_EquipRricecr;
	private Image image_EquipLineRightcr;
	private Image image_EquipLineLeftcr;
	private Image image_EquipLineLinecr;
	private Image image_EquipLineLine1cr;
	//结束UI申明;
	private bool isUIinit = false;

    private ConfigEquip _equip;
    UIDataGrid star;

	void Awake ()
	{
		cachedTransform=transform;
		//开始UI获取;
		image_Equipcr = cachedTransform.FindChild("image_Equipcr").GetComponent<Image>();
		Image_skill1cr = cachedTransform.FindChild("image_Equipcr/Image_skill1cr").GetComponent<Image>();
		Scr_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr").GetComponent<ScrollRect>();
		glayout_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr/glayout_EquipStarcr").GetComponent<GridLayoutGroup>();
		image_Selectedcr = cachedTransform.FindChild("image_Equipcr/image_Selectedcr").GetComponent<Image>();
		text_EquipNamecr = cachedTransform.FindChild("text_EquipNamecr").GetComponent<Text>();
		text_EquipRricecr = cachedTransform.FindChild("text_EquipRricecr").GetComponent<Text>();
		image_EquipLineRightcr = cachedTransform.FindChild("image_EquipLineRightcr").GetComponent<Image>();
		image_EquipLineLeftcr = cachedTransform.FindChild("image_EquipLineLeftcr").GetComponent<Image>();
		image_EquipLineLinecr = cachedTransform.FindChild("image_EquipLineLinecr").GetComponent<Image>();
		image_EquipLineLine1cr = cachedTransform.FindChild("image_EquipLineLine1cr").GetComponent<Image>();
		//结束UI获取;
		isUIinit = true;

        star = glayout_EquipStarcr.gameObject.AddMissingComponent<UIDataGrid>();
        star.InitDataGrid("prefab/UI/Equip/ItemeEquipStarcr", "ItemeEquipStarcr", star.gameObject, null, CommonEnum.Direction.none);

	    UGUIClickHandler.Get(Image_skill1cr.gameObject).onPointerClick = OnClickJO;
	}

    void OnClickJO(GameObject go)
    {
        EquipsOutsideView.Instance.ClickItem(this);
    }

    public override void SetData(object data)
    {
        base.SetData(data);
        _equip = data as ConfigEquip;
        Image_skill1cr.SetSprite(_equip.AtalsName, _equip.picName);
        text_EquipRricecr.text = EquipManager.Instance.GetBuyMoney(_equip).ToString();
        text_EquipNamecr.text = _equip.name;

        string[] arr = new string[_equip.leve];
        star.DataProvider = arr;

        SetLineInfo(0, 0, 0);
    }

    /// <summary>
    /// 设置连接线的情况
    /// </summary>
    /// <param name="type">0，都不显示，1显示right param1表示长度 param2表示y坐标值 2，显示left和line param1表示长度 param2表示y坐标值  参数为0表示不用显示</param>
    /// <param name="param"></param>
    public void SetLineInfo(int type, float param1, float param2)
    {
        switch (type)
        {
            case 0:
                image_EquipLineRightcr.gameObject.SetActive(false);
                image_EquipLineLeftcr.gameObject.SetActive(false);
                image_EquipLineLinecr.gameObject.SetActive(false);
                image_EquipLineLine1cr.gameObject.SetActive(false);
                break;
            case 1:
                image_EquipLineRightcr.gameObject.SetActive(true);
                if (param1 == 0)
                    break;
                image_EquipLineLine1cr.gameObject.SetActive(true);
                image_EquipLineLine1cr.transform.localPosition = new UnityEngine.Vector3(image_EquipLineLine1cr.transform.localPosition.x, param2);
                image_EquipLineLine1cr.rectTransform.sizeDelta = new UnityEngine.Vector2(image_EquipLineLine1cr.rectTransform.sizeDelta.x, param1);
                break;
            case 2:
                image_EquipLineLeftcr.gameObject.SetActive(true);
                if (param1 == 0)
                    break;
                image_EquipLineLinecr.gameObject.SetActive(true);
                image_EquipLineLinecr.transform.localPosition = new UnityEngine.Vector3(image_EquipLineLinecr.transform.localPosition.x, param2);
                image_EquipLineLinecr.rectTransform.sizeDelta = new UnityEngine.Vector2(image_EquipLineLine1cr.rectTransform.sizeDelta.x, param1);
                break;
        }
    }

    public void ClickItem(bool click)
    {
        image_Selectedcr.gameObject.SetActive(click);
    }

    public ConfigEquip GetData()
    {
        return _equip;
    }
}

