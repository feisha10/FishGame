using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ItemeEquipOutSide2cr : BaseItemRender
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Image image_Equipcr;
	private Image Image_skill1cr;
	private ScrollRect Scr_EquipStarcr;
	private GridLayoutGroup glayout_EquipStarcr;
	//结束UI申明;
	private bool isUIinit = false;

    private UIDataGrid _grid;
    private ConfigEquip _equip;

	void Awake ()
	{
		cachedTransform=transform;
		//开始UI获取;
		image_Equipcr = cachedTransform.Find("image_Equipcr").GetComponent<Image>();
		Image_skill1cr = cachedTransform.Find("image_Equipcr/Image_skill1cr").GetComponent<Image>();
		Scr_EquipStarcr = cachedTransform.Find("image_Equipcr/Scr_EquipStarcr").GetComponent<ScrollRect>();
		glayout_EquipStarcr = cachedTransform.Find("image_Equipcr/Scr_EquipStarcr/glayout_EquipStarcr").GetComponent<GridLayoutGroup>();
		//结束UI获取;
		isUIinit = true;

        UIEventListener.Get(Image_skill1cr.gameObject).onPress = OnClickJO;

        _grid = glayout_EquipStarcr.gameObject.AddComponent<UIDataGrid>();
        _grid.InitDataGrid("prefab/UI/Equip/ItemeEquipStarcr", "ItemeEquipStarcr", glayout_EquipStarcr.gameObject, Scr_EquipStarcr, Direction.none);
	}

    private void OnClickJO(GameObject go, bool isPress)
    {

    }

    public override void SetData(object data)
    {
        base.SetData(data);

        int equipid = int.Parse(data as string);

        _equip = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);

        if (_equip!=null)
        {
            Image_skill1cr.gameObject.SetActive(true);
            //Image_skill1cr.SetSprite(_equip.AtalsName, _equip.picName);
            string[] arr = new string[_equip.leve];
            _grid.DataProvider = arr;
        }
        else
        {
            Image_skill1cr.gameObject.SetActive(false);
            _grid.DataProvider = null;
        }
    }
}

