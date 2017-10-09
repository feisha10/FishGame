using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemeEquipOutSide3cr :BaseItemRender
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Image image_Equipcr;
	private Image Image_skill1cr;
	private ScrollRect Scr_EquipStarcr;
	private GridLayoutGroup glayout_EquipStarcr;
	private Image image_Selectedcr;
	private Image image_EquipLineLinecr;
	private Image image_EquipLineTopcr;
	private Image image_EquipLineBottomcr;
	//结束UI申明;
	private bool isUIinit = false;

    private ConfigEquip _equip;
    UIDataGrid star;
    public int type = 0;

	void Awake ()
	{
		cachedTransform=transform;
		//开始UI获取;
		image_Equipcr = cachedTransform.FindChild("image_Equipcr").GetComponent<Image>();
		Image_skill1cr = cachedTransform.FindChild("image_Equipcr/Image_skill1cr").GetComponent<Image>();
		Scr_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr").GetComponent<ScrollRect>();
		glayout_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr/glayout_EquipStarcr").GetComponent<GridLayoutGroup>();
		image_Selectedcr = cachedTransform.FindChild("image_Equipcr/image_Selectedcr").GetComponent<Image>();
		image_EquipLineLinecr = cachedTransform.FindChild("image_EquipLineLinecr").GetComponent<Image>();
		image_EquipLineTopcr = cachedTransform.FindChild("image_EquipLineTopcr").GetComponent<Image>();
		image_EquipLineBottomcr = cachedTransform.FindChild("image_EquipLineBottomcr").GetComponent<Image>();
		//结束UI获取;
		isUIinit = true;

        star = glayout_EquipStarcr.gameObject.AddMissingComponent<UIDataGrid>();
        star.InitDataGrid("prefab/UI/Equip/ItemeEquipStarcr", "ItemeEquipStarcr", star.gameObject, null, CommonEnum.Direction.none);

        UGUIClickHandler.Get(Image_skill1cr.gameObject).onPointerClick = OnClickJO;
	}

    void OnClickJO(GameObject go)
    {
        if(type==1)
        {
            EquipsOutsideView.Instance.ClickCombineItme(this);
        }
        else
        {
            EquipsOutsideView.Instance.ChooseItem(_equip);
        }
    }

    public override void SetData(object data)
    {
        base.SetData(data);

        _equip = data as ConfigEquip;

        Image_skill1cr.SetSprite(_equip.AtalsName, _equip.picName);

        string[] arr = new string[_equip.leve];
        star.DataProvider = arr;

        SetLineInfo(0, 0, 0);
    }

    /// <summary>
    /// 设置连接线的情况
    /// </summary>
    /// <param name="type">0，都不显示，1显示right(参数不需要)2，显示left和line param1表示长度 param2表示y坐标值</param>
    /// <param name="param"></param>
    public void SetLineInfo(int type, float param1, float param2)
    {
        switch (type)
        {
            case 0:
                image_EquipLineTopcr.gameObject.SetActive(false);
                image_EquipLineBottomcr.gameObject.SetActive(false);
                image_EquipLineLinecr.gameObject.SetActive(false);
                break;
            case 1:
                image_EquipLineTopcr.gameObject.SetActive(true);
                image_EquipLineBottomcr.gameObject.SetActive(false);
                image_EquipLineLinecr.gameObject.SetActive(false);
                break;
            case 2:
                image_EquipLineTopcr.gameObject.SetActive(false);
                image_EquipLineBottomcr.gameObject.SetActive(true);
                image_EquipLineLinecr.gameObject.SetActive(true);
                image_EquipLineLinecr.rectTransform.sizeDelta = new UnityEngine.Vector2(param1, image_EquipLineLinecr.rectTransform.sizeDelta.y);
                break;
            case 3:
                image_EquipLineTopcr.gameObject.SetActive(false);
                image_EquipLineBottomcr.gameObject.SetActive(true);
                image_EquipLineLinecr.gameObject.SetActive(false);
                break;
        }
    }

    public void SetTopLine()
    {
        image_EquipLineTopcr.gameObject.SetActive(true);
    }

    public void Select()
    {
        image_Selectedcr.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        image_Selectedcr.gameObject.SetActive(false);
    }
}

