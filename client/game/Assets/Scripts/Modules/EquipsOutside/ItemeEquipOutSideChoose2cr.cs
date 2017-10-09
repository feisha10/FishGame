using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemeEquipOutSideChoose2cr :BaseItemRender
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Image image_Equipcr;
	private ScrollRect Scr_EquipStarcr;
	private GridLayoutGroup glayout_EquipStarcr;
	private Image image_Selectedcr;
	private Button btn_Fillcr;
	private Image Image_skill1cr;
	private Button btn_Closedcr;
	//结束UI申明;
	private bool isUIinit = false;

    private UIDataGrid _grid;
    private ConfigEquip _equip;
    private bool _isClose = false;
    private bool _isFill = false;

	void Awake ()
	{
		cachedTransform=transform;
		//开始UI获取;
		image_Equipcr = cachedTransform.FindChild("image_Equipcr").GetComponent<Image>();
		Scr_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr").GetComponent<ScrollRect>();
		glayout_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr/glayout_EquipStarcr").GetComponent<GridLayoutGroup>();
		image_Selectedcr = cachedTransform.FindChild("image_Equipcr/image_Selectedcr").GetComponent<Image>();
		btn_Fillcr = cachedTransform.FindChild("image_Equipcr/btn_Fillcr").GetComponent<Button>();
		Image_skill1cr = cachedTransform.FindChild("image_Equipcr/Image_skill1cr").GetComponent<Image>();
		btn_Closedcr = cachedTransform.FindChild("btn_Closedcr").GetComponent<Button>();
		//结束UI获取;
		isUIinit = true;

        image_Selectedcr.gameObject.SetActive(false);

        UIEventListener.Get(Image_skill1cr.gameObject).onClick = OnClickJO;
        UIEventListener.Get(btn_Closedcr.gameObject).onClick = OnCloseClick;
        UIEventListener.Get(btn_Fillcr.gameObject).onClick = OnFillClick;

        _grid = glayout_EquipStarcr.gameObject.AddComponent<UIDataGrid>();
        _grid.InitDataGrid("prefab/UI/Equip/ItemeEquipStarcr", "ItemeEquipStarcr", glayout_EquipStarcr.gameObject, Scr_EquipStarcr, CommonEnum.Direction.none);
	}

    void OnClickJO(GameObject go)
    {
        EquipsOutsideView.Instance.ClickChooseEquipItem(this);
    }

    void OnCloseClick(GameObject go)
    {
        btn_Closedcr.gameObject.SetActive(false);
        btn_Fillcr.gameObject.SetActive(true);
        Image_skill1cr.gameObject.SetActive(false);
        _isClose = true;
    }

    void OnFillClick(GameObject go)
    {
        ItemeEquipOutSidecr item = EquipsOutsideView.Instance.GetSelectClickItem();
        if(item==null)
        {
            TipManager.Instance.ShowTip("请选择要放进的装备",ColorEnum.White);
        }
        else
        {
            _isClose = false;
            btn_Closedcr.gameObject.SetActive(true);
            btn_Fillcr.gameObject.SetActive(false);
            _equip = item.GetData();
            SetData();
        }
    }

    public void UnSelect()
    {
        image_Selectedcr.gameObject.SetActive(false);
    }

    public void Select()
    {
        image_Selectedcr.gameObject.SetActive(true);
    }

    public void ModefyState()
    {
        if(_equip==null)
        {
            btn_Fillcr.gameObject.SetActive(true);
            btn_Closedcr.gameObject.SetActive(false);
        }
        else
        {
            btn_Fillcr.gameObject.SetActive(false);
            btn_Closedcr.gameObject.SetActive(true);
        }
    }

    public void CancelModefyState(bool isSure)
    {
        btn_Closedcr.gameObject.SetActive(false);
        btn_Fillcr.gameObject.SetActive(false);

        if (isSure==false)
            SetData(Data);
    }

    public void ResetData()
    {
        SetData();
    }

    public int GetEquipid()
    {
        if (_isClose || _equip==null)
            return -1;
        else
        {
            return _equip.id;
        }
    }

    public ConfigEquip GetData()
    {
        return _equip;
    }

    public override void SetData(object data)
    {
        base.SetData(data);

        int equipid = int.Parse(data as string);

        _equip = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);

        SetData();
    }

    void SetData()
    {
        _isClose = false;
        btn_Fillcr.gameObject.SetActive(false);
        if (_equip!=null)
        {
            Image_skill1cr.SetSprite(_equip.AtalsName, _equip.picName);
            Image_skill1cr.gameObject.SetActive(true);
            string[] arr = new string[_equip.leve];
            _grid.DataProvider = arr;
        }
        else
        {
            Image_skill1cr.gameObject.SetActive(false);
        }
    }
}

