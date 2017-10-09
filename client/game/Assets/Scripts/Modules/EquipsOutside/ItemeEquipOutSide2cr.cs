using System;
using UnityEngine;
using UnityEngine.UI;
using Logic;
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
		image_Equipcr = cachedTransform.FindChild("image_Equipcr").GetComponent<Image>();
		Image_skill1cr = cachedTransform.FindChild("image_Equipcr/Image_skill1cr").GetComponent<Image>();
		Scr_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr").GetComponent<ScrollRect>();
		glayout_EquipStarcr = cachedTransform.FindChild("image_Equipcr/Scr_EquipStarcr/glayout_EquipStarcr").GetComponent<GridLayoutGroup>();
		//结束UI获取;
		isUIinit = true;

        UIEventListener.Get(Image_skill1cr.gameObject).onPress = OnClickJO;

        _grid = glayout_EquipStarcr.gameObject.AddComponent<UIDataGrid>();
        _grid.InitDataGrid("prefab/UI/Equip/ItemeEquipStarcr", "ItemeEquipStarcr", glayout_EquipStarcr.gameObject, Scr_EquipStarcr, CommonEnum.Direction.none);
	}

    private void OnClickJO(GameObject go, bool isPress)
    {
        if(isPress)
        {
            string[] chatPrefabs = new string[] { "Prefab/UI/Equip/ItemEquipTipcr" };

            ResourceManager.Instance.LoadMutileAssets(chatPrefabs, o =>
            {
                if (ItemEquipTipcr.Exists == false)
                {
                    GameObject view = UGUITools.AddChild(ItemEquipmentSchemecr.Instance.gameObject, ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/Equip/ItemEquipTipcr"));
                    view.AddComponent<ItemEquipTipcr>();

                    Vector3 pos2 = TransformUtil.MousePositionToLoaclPostion(405, 256,ItemEquipmentSchemecr.Instance.gameObject.transform, CameraSetting.UICamera);

                    if (pos2.x > 300)
                        view.transform.localPosition = new Vector3((pos2.x - 120), pos2.y, 0);
                    else
                    {
                        view.transform.localPosition = new Vector3(pos2.x + (120), pos2.y, 0);
                    }
                    ItemEquipTipcr.Instance.SetData(EquipManager.Instance.GetEquipDec(_equip), _equip.name);

                }
            }, false, 3);
        }
        else
        {
            if(ItemEquipTipcr.Active)
                ItemEquipTipcr.Instance.OnClose();
        }
    }

    public override void SetData(object data)
    {
        base.SetData(data);

        int equipid = int.Parse(data as string);

        _equip = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);

        if (_equip!=null)
        {
            Image_skill1cr.gameObject.SetActive(true);
            Image_skill1cr.SetSprite(_equip.AtalsName, _equip.picName);
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

