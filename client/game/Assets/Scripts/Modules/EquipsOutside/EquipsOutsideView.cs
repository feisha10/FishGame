using System.Collections.Generic;
using System;
using Client;
using UnityEngine;
using UnityEngine.UI;

public class EquipsOutsideViewBeforeLoad : PanelBase
{

	public string[] BeforLoadInfos()
	{
        return new[] { "prefab/UI/EquipsOutside/ItemeEquipOutSideChoose2cr", "prefab/UI/Equip/ItemeEquipStarcr", "Prefab/UI/EquipsOutside/ItemeEquipOutSidecr",
            "Prefab/UI/EquipsOutside/ItemeEquipOutSide3cr" };
	}
	public string[] BeforLoadTextures()
	{
		 string[] backs = new string[] {"textures/etcimage/bg_battle_reward.assetbundle"};
		return backs;
	}
	 //endTexture
}

public class EquipsOutsideView : SingletonMonoBehaviour<EquipsOutsideView>
{

	private Transform cachedTransform ;
	//开始UI申明;
	private RawImage rawI_EquipBottomcr;
	private GameObject go_EquipListcr;
	private ScrollRect Scr_EquipListcr;
	private GameObject go_contentcr;
	private GridLayoutGroup glayout_EquipList1cr;
	private GridLayoutGroup glayout_EquipList2cr;
	private GridLayoutGroup glayout_EquipList3cr;
	private Scrollbar scrollbar_Applycr;
	private Button btn_Homecr;
	private Button btn_Replycr;
	private GameObject go_MyEquipBarcr;
	private ScrollRect Scr_MyEquipBarcr;
	private GridLayoutGroup glayout_EquipListcr;
	private Button btn_Modifycr;
	private Button btn_Renamedecr;
	private Image image_Herocr;
	private Button btn_ChangeHerocr;
	private Button btn_Cancelcr;
	private Button btn_Surecr;
	private Button btn_EquipSchemecr;
	private ToggleGroup togGroup_EquipBtnListcr;
	private Text text_AttackTogcr;
	private Text text_LiveTogcr;
	private Text text_MoveTogcr;
	private GameObject go_rightcentercr;
	private ScrollRect Scr_ContentcrTextcr;
	private Text Text_contcr;
	private Text text_Namecr;
	private Button btn_Synthesiscr;
	private GameObject go_EquipSynthesiscr;
	private Button btn_Closedcr;
	private ScrollRect Scr_ContentcrText2cr;
	private Text Text_cont2cr;
	private Text text_Name2cr;
	private Text text_Rricecr;
	private ScrollRect Scr_CanSynthesisEquipcr;
	private GridLayoutGroup glayout_CanSynthesisEquipcr;
	private Button btn_leftcr;
	private Button btn_rightcr;
	//结束UI申明;
	private bool isUIinit = false;

    public int ChooseHeroType = 0;
    //private ConfigCharactor _chooseCharactor;
    private UIDataGrid _heroChooseEquipGrid;
    private p_equip_info _currInfo;

    TabBar tabBar;
    UIDataGrid dataEquipt1;
    UIDataGrid dataEquipt2;
    UIDataGrid dataEquipt3;
    RectTransform equip1;
    RectTransform equip2;
    RectTransform equip3;
    RectTransform equipSelf;
    RectTransform equipScr;
    private int _currType;

    UIDataGrid canCombineGrid;

	protected override void Awake ()
	{
		base.Awake();
		cachedTransform=transform;
		//开始UI获取;
		rawI_EquipBottomcr = cachedTransform.FindChild("rawI_EquipBottomcr").GetComponent<RawImage>();
		go_EquipListcr = cachedTransform.FindChild("go_EquipListcr").gameObject;
		Scr_EquipListcr = cachedTransform.FindChild("go_EquipListcr/Scr_EquipListcr").GetComponent<ScrollRect>();
		go_contentcr = cachedTransform.FindChild("go_EquipListcr/Scr_EquipListcr/go_contentcr").gameObject;
		glayout_EquipList1cr = cachedTransform.FindChild("go_EquipListcr/Scr_EquipListcr/go_contentcr/glayout_EquipList1cr").GetComponent<GridLayoutGroup>();
		glayout_EquipList2cr = cachedTransform.FindChild("go_EquipListcr/Scr_EquipListcr/go_contentcr/glayout_EquipList2cr").GetComponent<GridLayoutGroup>();
		glayout_EquipList3cr = cachedTransform.FindChild("go_EquipListcr/Scr_EquipListcr/go_contentcr/glayout_EquipList3cr").GetComponent<GridLayoutGroup>();
		scrollbar_Applycr = cachedTransform.FindChild("go_EquipListcr/Scr_EquipListcr/scrollbar_Applycr").GetComponent<Scrollbar>();
		btn_Homecr = cachedTransform.FindChild("go_topright/go_TopImformation/btn_Homecr").GetComponent<Button>();
		btn_Replycr = cachedTransform.FindChild("go_bottomright/btn_Replycr").GetComponent<Button>();
		go_MyEquipBarcr = cachedTransform.FindChild("go_bottomright/go_MyEquipBarcr").gameObject;
		Scr_MyEquipBarcr = cachedTransform.FindChild("go_bottomright/go_MyEquipBarcr/Scr_MyEquipBarcr").GetComponent<ScrollRect>();
		glayout_EquipListcr = cachedTransform.FindChild("go_bottomright/go_MyEquipBarcr/Scr_MyEquipBarcr/glayout_EquipListcr").GetComponent<GridLayoutGroup>();
		btn_Modifycr = cachedTransform.FindChild("go_bottomright/btn_Modifycr").GetComponent<Button>();
		btn_Renamedecr = cachedTransform.FindChild("go_bottomright/btn_Renamedecr").GetComponent<Button>();
		image_Herocr = cachedTransform.FindChild("go_bottomright/go_ChangeHero/image_Herocr").GetComponent<Image>();
		btn_ChangeHerocr = cachedTransform.FindChild("go_bottomright/go_ChangeHero/btn_ChangeHerocr").GetComponent<Button>();
		btn_Cancelcr = cachedTransform.FindChild("go_bottomright/btn_Cancelcr").GetComponent<Button>();
		btn_Surecr = cachedTransform.FindChild("go_bottomright/btn_Surecr").GetComponent<Button>();
		btn_EquipSchemecr = cachedTransform.FindChild("go_bottomright/btn_EquipSchemecr").GetComponent<Button>();
		togGroup_EquipBtnListcr = cachedTransform.FindChild("go_leftcenter/togGroup_EquipBtnListcr").GetComponent<ToggleGroup>();
		text_AttackTogcr = cachedTransform.FindChild("go_leftcenter/togGroup_EquipBtnListcr/Attack/text_AttackTogcr").GetComponent<Text>();
		text_LiveTogcr = cachedTransform.FindChild("go_leftcenter/togGroup_EquipBtnListcr/Live/text_LiveTogcr").GetComponent<Text>();
		text_MoveTogcr = cachedTransform.FindChild("go_leftcenter/togGroup_EquipBtnListcr/Move/text_MoveTogcr").GetComponent<Text>();
		go_rightcentercr = cachedTransform.FindChild("go_rightcentercr").gameObject;
		Scr_ContentcrTextcr = cachedTransform.FindChild("go_rightcentercr/Scr_ContentcrTextcr").GetComponent<ScrollRect>();
		Text_contcr = cachedTransform.FindChild("go_rightcentercr/Scr_ContentcrTextcr/Content/Text_contcr").GetComponent<Text>();
		text_Namecr = cachedTransform.FindChild("go_rightcentercr/Scr_ContentcrTextcr/text_Namecr").GetComponent<Text>();
		btn_Synthesiscr = cachedTransform.FindChild("go_rightcentercr/btn_Synthesiscr").GetComponent<Button>();
		go_EquipSynthesiscr = cachedTransform.FindChild("go_EquipSynthesiscr").gameObject;
		btn_Closedcr = cachedTransform.FindChild("go_EquipSynthesiscr/btn_Closedcr").GetComponent<Button>();
		Scr_ContentcrText2cr = cachedTransform.FindChild("go_EquipSynthesiscr/go_rightcenter2/Scr_ContentcrText2cr").GetComponent<ScrollRect>();
		Text_cont2cr = cachedTransform.FindChild("go_EquipSynthesiscr/go_rightcenter2/Scr_ContentcrText2cr/Content/Text_cont2cr").GetComponent<Text>();
		text_Name2cr = cachedTransform.FindChild("go_EquipSynthesiscr/go_rightcenter2/Scr_ContentcrText2cr/Content/text_Name2cr").GetComponent<Text>();
		text_Rricecr = cachedTransform.FindChild("go_EquipSynthesiscr/go_rightcenter2/text_Rricecr").GetComponent<Text>();
		Scr_CanSynthesisEquipcr = cachedTransform.FindChild("go_EquipSynthesiscr/go_topleft2/image_CanSybottom/Scr_CanSynthesisEquipcr").GetComponent<ScrollRect>();
		glayout_CanSynthesisEquipcr = cachedTransform.FindChild("go_EquipSynthesiscr/go_topleft2/image_CanSybottom/Scr_CanSynthesisEquipcr/glayout_CanSynthesisEquipcr").GetComponent<GridLayoutGroup>();
		btn_leftcr = cachedTransform.FindChild("go_EquipSynthesiscr/go_topleft2/image_CanSybottom/btn_leftcr").GetComponent<Button>();
		btn_rightcr = cachedTransform.FindChild("go_EquipSynthesiscr/go_topleft2/image_CanSybottom/btn_rightcr").GetComponent<Button>();
		//结束UI获取;
		//开始texture设置;
		TextureManager.Instance.SetTexure(rawI_EquipBottomcr,"Textures/ETCImage/bg_battle_reward.assetbundle");
		//结束texture设置;
		isUIinit = true;

        UIEventListener.Get(btn_Homecr.gameObject, AudioConst.CommonReturn).onClick = OnBtnExitClick;
        UIEventListener.Get(btn_ChangeHerocr.gameObject).onClick = OnChangeHero;
        UIEventListener.Get(image_Herocr.gameObject).onClick = OnChangeHero;

        UIEventListener.Get(btn_EquipSchemecr.gameObject).onClick = OnSchemeClick;
        UIEventListener.Get(btn_Replycr.gameObject).onClick = OnDefaultClick;
        UIEventListener.Get(btn_Renamedecr.gameObject).onClick = OnRenameClick;
        UIEventListener.Get(btn_Modifycr.gameObject).onClick = OnModifyClick;
        UIEventListener.Get(btn_Cancelcr.gameObject).onClick = OnCancelClick;
        UIEventListener.Get(btn_Surecr.gameObject).onClick = OnSureClick;
        UIEventListener.Get(btn_Synthesiscr.gameObject).onClick = OnSynthesisClick;
        UIEventListener.Get(btn_Closedcr.gameObject).onClick = OnSynthesisCloseClick;

        _heroChooseEquipGrid = glayout_EquipListcr.gameObject.AddComponent<UIDataGrid>();
        _heroChooseEquipGrid.InitDataGrid("prefab/UI/EquipsOutside/ItemeEquipOutSideChoose2cr", "ItemeEquipOutSideChoose2cr", glayout_EquipListcr.gameObject, Scr_MyEquipBarcr, CommonEnum.Direction.none);

        equip1 = glayout_EquipList1cr.gameObject.GetRectTransform();
        equip2 = glayout_EquipList2cr.gameObject.GetRectTransform();
        equip3 = glayout_EquipList3cr.gameObject.GetRectTransform();
        equipSelf = go_contentcr.GetRectTransform();
        equipScr = Scr_EquipListcr.gameObject.GetRectTransform();

        dataEquipt1 = glayout_EquipList1cr.gameObject.AddMissingComponent<UIDataGrid>();
        dataEquipt1.InitDataGrid("Prefab/UI/EquipsOutside/ItemeEquipOutSidecr", "ItemeEquipOutSidecr", dataEquipt1.gameObject, Scr_EquipListcr, CommonEnum.Direction.Vertical);
        dataEquipt2 = glayout_EquipList2cr.gameObject.AddMissingComponent<UIDataGrid>();
        dataEquipt2.InitDataGrid("Prefab/UI/EquipsOutside/ItemeEquipOutSidecr", "ItemeEquipOutSidecr", dataEquipt2.gameObject, Scr_EquipListcr, CommonEnum.Direction.Vertical);
        dataEquipt3 = glayout_EquipList3cr.gameObject.AddMissingComponent<UIDataGrid>();
        dataEquipt3.InitDataGrid("Prefab/UI/EquipsOutside/ItemeEquipOutSidecr", "ItemeEquipOutSidecr", dataEquipt3.gameObject, Scr_EquipListcr, CommonEnum.Direction.Vertical);

        tabBar = togGroup_EquipBtnListcr.gameObject.AddComponent<TabBar>();
        tabBar.ManulInit(togGroup_EquipBtnListcr.GetComponentsInChildren<Toggle>(), togGroup_EquipBtnListcr);
        tabBar.InitColorAndSize(Color.white, Color.yellow,20,24);
        tabBar.OnChange = SelectTab;
        tabBar.SelectTab(0);

	}

    private void OnSynthesisClick(GameObject go)
    {
        go_EquipSynthesiscr.gameObject.SetActive(true);

        if (canCombineGrid==null)
        {
            canCombineGrid = glayout_CanSynthesisEquipcr.gameObject.AddComponent<UIDataGrid>();
            canCombineGrid.InitDataGrid("Prefab/UI/EquipsOutside/ItemeEquipOutSide3cr", "ItemeEquipOutSide3cr", glayout_CanSynthesisEquipcr.gameObject, Scr_CanSynthesisEquipcr);
            canCombineGrid.ArrowImage1 = btn_leftcr.gameObject;
            canCombineGrid.ArrowImage2 = btn_rightcr.gameObject;
        }

        if(lastClickItem!=null)
        {
            ConfigEquip equip = lastClickItem.GetData();
            ChooseItem(equip);
        }
    }

    private ItemeEquipOutSide3cr _chooseCombineItem;
    void ShowChooseItemInfo(ConfigEquip equipConfig)
    {
        text_Name2cr.text = equipConfig.name;
        Text_cont2cr.text = EquipManager.Instance.GetEquipDec(equipConfig);
        text_Rricecr.text = EquipManager.Instance.GetBuyMoney(equipConfig).ToString();
        SetCanCombine(equipConfig);
    }
    public void ClickCombineItme(ItemeEquipOutSide3cr item)
    {
        if (_chooseCombineItem != null)
        {
            _chooseCombineItem.UnSelect();
            _chooseCombineItem = item;
            _chooseCombineItem.Select();
            ShowChooseItemInfo(_chooseCombineItem.Data as ConfigEquip);
        }
    }
    public void ChooseItem(ConfigEquip equipConfig)
    {
        DrawSynthesis(equipConfig);
        ShowChooseItemInfo(equipConfig);
    }

    private void SetCanCombine(ConfigEquip equipConfig)
    {
        List<ConfigEquip> list = new List<ConfigEquip>();
        for (int i = 0; i < equipConfig.cancombine.Length; i++)
        {
            int equipid = equipConfig.cancombine[i];
            ConfigEquip temp = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);
            list.Add(temp);

            for (int j = 0; j < temp.cancombine.Length; j++)
            {
                equipid = temp.cancombine[j];
                ConfigEquip temp1 = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);
                list.Add(temp1);
            }
        }
        canCombineGrid.DataProvider = list.ToArray();
    }

    List<GameObject>  synthesisItemList = new List<GameObject>();
    private void DrawSynthesis(ConfigEquip equipConfig)
    {
        for (int i = 0; i < synthesisItemList.Count; i++)
            Destroy(synthesisItemList[i]);
        synthesisItemList.Clear();

        float rootX = 410f;
        float rooty = 320f;
        int secondW = 240;
        int threew = 120;
        int height = 100;

        ItemeEquipOutSide3cr itemScript = CreateSynthesisItem(equipConfig, rootX, rooty, true);
        _chooseCombineItem = itemScript;

        int[] combine = equipConfig.combine;
        if (combine.Length > 0)
        {
            if (combine.Length == 1)
                itemScript.SetLineInfo(3, 0, 0);
            else
                itemScript.SetLineInfo(2, secondW * (combine.Length - 1), 0);

            bool fx = combine.Length % 2 == 0;
            for (int i = 0; i < combine.Length; i++)
            {
                int equipid = combine[i];
                equipConfig = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);
                float x = rootX + i * secondW;
                x = x - ((secondW / 2) * combine.Length) / 2f;
                if (fx == false)
                    x += (secondW / 4);

                float y = rooty - height;
                itemScript = CreateSynthesisItem(equipConfig, x, y);

                int[] three = equipConfig.combine;

                if (three.Length > 0)
                {
                    if (three.Length == 1)
                        itemScript.SetLineInfo(3, 0, 0);
                    else
                        itemScript.SetLineInfo(2, threew * (three.Length - 1), 0);
                    itemScript.SetTopLine();

                    bool fx1 = three.Length % 2 == 0;
                    for (int j = 0; j < three.Length; j++)
                    {
                        equipid = three[j];
                        equipConfig = ConfigManager.Instance.GetConfig<ConfigEquip>(equipid);
                        float threex = x + (j * threew);
                        threex = threex - ((threew / 2) * three.Length) / 2f;
                        if (fx1 == false)
                            threex += (threew / 4);

                        y = y - height;
                        itemScript = CreateSynthesisItem(equipConfig, threex, y);
                        itemScript.SetLineInfo(1, 0, 0);
                    }
                }
                else
                {
                    itemScript.SetLineInfo(1, 0, 0);
                }
            }
        }
    }

    ItemeEquipOutSide3cr CreateSynthesisItem(ConfigEquip equip,float x, float y,bool isRoot=false)
    {
        GameObject renderPrefab = ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/EquipsOutside/ItemeEquipOutSide3cr");
        GameObject item = UGUITools.AddChild(go_EquipSynthesiscr, renderPrefab);
        synthesisItemList.Add(item);
        ItemeEquipOutSide3cr itemScript = item.AddMissingComponent<ItemeEquipOutSide3cr>();
        itemScript.type = 1;
        itemScript.SetData(equip);
        if (isRoot)
            itemScript.Select();
        item.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        return itemScript;
    }

    private void OnSynthesisCloseClick(GameObject go)
    {
        go_EquipSynthesiscr.gameObject.SetActive(false);
    }

    private void SetSynthesisViewVisble(bool b)
    {
        if(b)
        {
            go_rightcentercr.SetActive(true);
            btn_Synthesiscr.gameObject.SetActive(true);
            text_Namecr.gameObject.SetActive(true);
            Text_contcr.gameObject.SetActive(true);
        }
        else
        {
            go_rightcentercr.SetActive(false);
            btn_Synthesiscr.gameObject.SetActive(false);
            text_Namecr.gameObject.SetActive(false);
            Text_contcr.gameObject.SetActive(false);
        }
    }

    Dictionary<int, List<ConfigEquip>> backReferrals;
    List<ConfigEquip> list1;
    List<ConfigEquip> list2;
    List<ConfigEquip> list3;

    private void SelectTab(string barName, GameObject go)
    {
        SetSynthesisViewVisble(false);
        if (lastClickItem != null)
        {
            lastClickItem.ClickItem(false);
            lastClickItem = null;
        }

        switch (barName)
        {
            case "Attack":
                _currType = 1;
                break;
            case "Live":
                _currType = 2;
                break;
            case "Move":
                _currType = 3;
                break;
        }
        backReferrals = EquipManager.Instance.GetEquiptsFromType(_currType);

        if (backReferrals.Count > 0)
            list1 = backReferrals[1];
        else
            list1 = new List<ConfigEquip>();
        if (backReferrals.Count > 1)
            list2 = backReferrals[2];
        else
            list2 = new List<ConfigEquip>();
        if (backReferrals.Count > 2)
            list3 = backReferrals[3];
        else
            list3 = new List<ConfigEquip>();

        dataEquipt1.DataProvider = list1.ToArray(); //如果第一列最长会有问题！
        if (list2.Count>list3.Count)
        {
            dataEquipt3.DataProvider = list3.ToArray();
            dataEquipt2.DataProvider = list2.ToArray();
        }
        else
        {
            dataEquipt2.DataProvider = list2.ToArray();
            dataEquipt3.DataProvider = list3.ToArray();
        }

        float length = equip1.sizeDelta.y;
        length = equip2.sizeDelta.y > length ? equip2.sizeDelta.y : length;
        length = equip3.sizeDelta.y > length ? equip3.sizeDelta.y : length;
        equipSelf.sizeDelta = new Vector2(600, length);
        equipSelf.localPosition = new Vector2(0, (-length + equipScr.sizeDelta.y) / 2);
        TransformUtil.MoveY(equip1.transform, (length - equipScr.sizeDelta.y) / 2);
        TransformUtil.MoveY(equip2.transform, (length - equipScr.sizeDelta.y) / 2);
        TransformUtil.MoveY(equip3.transform, (length - equipScr.sizeDelta.y) / 2);
    }

    ItemeEquipOutSidecr lastClickItem;
    List<ItemeEquipOutSidecr> lastClickList = new List<ItemeEquipOutSidecr>();
    public void ClickItem(ItemeEquipOutSidecr itemEquip)
    {
        SetSynthesisViewVisble(true);
        if (lastClickItem != null)
            lastClickItem.ClickItem(false);
        lastClickItem = itemEquip;
        lastClickItem.ClickItem(true);
        ConfigEquip item = lastClickItem.GetData();
        text_Namecr.text = item.name;
        Text_contcr.text = EquipManager.Instance.GetEquipDec(item);
        for (int i = 0; i < lastClickList.Count; i++)
        {
            lastClickList[i].SetLineInfo(0, 0, 0);
        }
        lastClickList.Clear();
        lastClickList.Add(lastClickItem);
        SetLineParentInfoHor(lastClickItem, 0);
        SetLineChildInfoHor(lastClickItem, 0);
    }

    public ItemeEquipOutSidecr GetSelectClickItem()
    {
        return lastClickItem;
    }

    private void SetLineParentInfoHor(ItemeEquipOutSidecr setparent, int num)
    {
        ConfigEquip item = setparent.GetData();
        if (num > 3)
        {
            Log.Error("装备合成路径存在环策划查看配置" + item.id);
            return;
        }
        if (item.cancombine.Length > 0)
        {
            Vector3 parent = setparent.gameObject.transform.localPosition;
            Vector3 Tops = Vector3.zero;
            Vector3 Bottoms = Vector3.zero;
            num++;
            int selfCen = 1;
            ItemeEquipOutSidecr par = dataEquipt1.GetItem<ConfigEquip>("id", item.id) as ItemeEquipOutSidecr;
            if (par == null)
            {
                par = dataEquipt2.GetItem<ConfigEquip>("id", item.id) as ItemeEquipOutSidecr;
                selfCen = 2;
                if (par == null)
                    selfCen = 3;
            }

            for (int i = 0; i < item.cancombine.Length; i++)
            {
                int parentCen = 2;
                ItemeEquipOutSidecr par1 = dataEquipt2.GetItem<ConfigEquip>("id", item.cancombine[i]) as ItemeEquipOutSidecr;
                if (par1 == null)
                {
                    parentCen = 3;
                    par1 = dataEquipt3.GetItem<ConfigEquip>("id", item.cancombine[i]) as ItemeEquipOutSidecr;
                }
                if (par1 != null && parentCen - selfCen == 1)
                {
                    par1.SetLineInfo(2, 0, 0);
                    Vector3 pos = setparent.gameObject.transform.InverseTransformPoint(par1.gameObject.transform.position) + parent;
                    if (Tops != Vector3.zero)
                    {
                        if (pos.y > Tops.y)
                        {
                            Tops = pos;
                        }
                        if (pos.y < Bottoms.y)
                        {
                            Bottoms = pos;
                        }
                    }
                    else
                    {
                        Tops = pos;
                        Bottoms = pos;
                    }
                    lastClickList.Add(par1);
                    SetLineParentInfoHor(par1, num);
                }
            }
            float length;
            float postion;
            if (Tops.y - Bottoms.y > 1)
            {
                if (parent.y > Tops.y)
                    length = parent.y - Bottoms.y;
                else if (parent.y < Bottoms.y)
                    length = Tops.y - parent.y;
                else
                    length = Tops.y - Bottoms.y;
                postion = ((Tops.y - parent.y) > 0 ? (Tops.y - parent.y) : 0) - length / 2;
            }
            else
            {
                length = parent.y - Tops.y;
                postion = -length / 2;
                length = Math.Abs(length);
            }
            if (Tops == Bottoms && Tops == Vector3.zero)
                return;
            setparent.SetLineInfo(1, length, postion);
        }
    }

    //设置line信息    三列的情况
    private void SetLineChildInfoHor(ItemeEquipOutSidecr setparent, int num)
    {

        ConfigEquip item = setparent.GetData();
        if (num > 3)
        {
            Log.Error("装备合成路径存在环策划查看配置" + item.id);
            return;
        }
        if (item.combine.Length > 0)
        {
            Vector3 parent = setparent.gameObject.transform.localPosition;
            Vector3 Tops = Vector3.zero;
            Vector3 Bottoms = Vector3.zero;
            num++;
            int selfCen = 2;
            ItemeEquipOutSidecr par = dataEquipt2.GetItem<ConfigEquip>("id", item.id) as ItemeEquipOutSidecr;
            if (par == null)
            {
                par = dataEquipt3.GetItem<ConfigEquip>("id", item.id) as ItemeEquipOutSidecr;
                selfCen = 3;
                if (par == null)
                    selfCen = 1;
            }
            for (int i = 0; i < item.combine.Length; i++)
            {
                int childCen = 1;
                ItemeEquipOutSidecr ch1 = dataEquipt1.GetItem<ConfigEquip>("id", item.combine[i]) as ItemeEquipOutSidecr;
                if (ch1 == null)
                {
                    childCen = 2;
                    ch1 = dataEquipt2.GetItem<ConfigEquip>("id", item.combine[i]) as ItemeEquipOutSidecr;
                }
                if (ch1 != null && selfCen - childCen == 1)
                {
                    ch1.SetLineInfo(1, 0, 0);
                    Vector3 pos = setparent.gameObject.transform.InverseTransformPoint(ch1.gameObject.transform.position) + parent;
                    if (Tops != Vector3.zero)
                    {
                        if (pos.y > Tops.y)
                        {
                            Tops = pos;
                        }
                        if (pos.y < Bottoms.y)
                        {
                            Bottoms = pos;
                        }
                    }
                    else
                    {
                        Tops = pos;
                        Bottoms = pos;
                    }
                    lastClickList.Add(ch1);
                    SetLineChildInfoHor(ch1, num);
                }
            }
            float length;
            float postion;
            if (Tops.y - Bottoms.y > 1)
            {
                if (parent.y > Tops.y)
                    length = parent.y - Bottoms.y;
                else if (parent.y < Bottoms.y)
                    length = Tops.y - parent.y;
                else
                    length = Tops.y - Bottoms.y;
                postion = ((Tops.y - parent.y) > 0 ? (Tops.y - parent.y) : 0) - length / 2;
            }
            else
            {
                length = parent.y - Tops.y;
                postion = -length / 2;
                length = Math.Abs(length);
            }
            if (Tops == Bottoms && Tops == Vector3.zero)
                return;
            setparent.SetLineInfo(2, length, postion);
        }

    }

    private void OnBtnExitClick(GameObject go)
    {
        UILayerManager.Instance.ReturnPanel();
    }

    private void OnModifyClick(GameObject go)
    {
        btn_Modifycr.gameObject.SetActive(false);
        btn_Cancelcr.gameObject.SetActive(true);
        btn_Surecr.gameObject.SetActive(true);

        Dictionary<int, BaseItemRender> renders = _heroChooseEquipGrid.GetRenders();
        foreach (var baseItemRender in renders.Values)
        {
            (baseItemRender as ItemeEquipOutSideChoose2cr).ModefyState();
        }
    }

    private void OnCancelClick(GameObject go)
    {
        CancelModifyState(false);
    }

    private void OnSureClick(GameObject go)
    {
        CancelModifyState(true);
    }

    void CancelModifyState(bool isSure)
    {
        btn_Modifycr.gameObject.SetActive(true);
        btn_Cancelcr.gameObject.SetActive(false);
        btn_Surecr.gameObject.SetActive(false);

        Dictionary<int, BaseItemRender> renders = _heroChooseEquipGrid.GetRenders();
        List<int> list = new List<int>();
        foreach (var baseItemRender in renders.Values)
        {
            (baseItemRender as ItemeEquipOutSideChoose2cr).CancelModefyState(isSure);
            if(isSure)
            {
                list.Add((baseItemRender as ItemeEquipOutSideChoose2cr).GetEquipid());
            }
            else
            {
                (baseItemRender as ItemeEquipOutSideChoose2cr).ResetData();
            }
        }

        if(isSure)
        {
            _currInfo.equip_list = list.ToArray();
            RoleModel.Instance.TosChangeEquip(ChooseHeroType, _currInfo,2);
            TipManager.Instance.ShowTip(string.Format("{0}修改成功", _currInfo.plan_name),Color.white);
        }
    }

    private void OnDefaultClick(GameObject go)
    {
        if (Global.Instance.HeroDic.ContainsKey(ChooseHeroType))
        {
            SetDefault(_chooseCharactor, Global.Instance.HeroDic[ChooseHeroType].equip_id);
        }
    }

    private void OnRenameClick(GameObject go)
    {
        string[] chatPrefabs = new string[] { "Prefab/UI/EquipsOutside/ItemRenamedcr" };

        ResourceManager.Instance.LoadMutileAssets(chatPrefabs, o =>
        {
            if (ItemRenamedcr.Exists == false)
            {
                GameObject view = UGUITools.AddChild(gameObject, ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/EquipsOutside/ItemRenamedcr"));
                view.AddComponent<ItemRenamedcr>();
                UGUITools.SetStretch(view);

                ItemRenamedcr.Instance.SetName(_currInfo.plan_name,_currInfo.plan_id);
            }
        }, false, 3);
    }

    private void OnChangeHero(GameObject go)
    {
        string[] chatPrefabs = new string[] { "Prefab/UI/Equip/ItemHeroListcr", "prefab/UI/Equip/ItemHeroChoicebtncr", "prefab/UI/Equip/ItemEquipOutsideHerocr" };

        ResourceManager.Instance.LoadMutileAssets(chatPrefabs, o =>
        {
            if (ItemHeroListcr.Exists==false)
            {
                GameObject view = UGUITools.AddChild(gameObject, ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/Equip/ItemHeroListcr"));
                view.AddComponent<ItemHeroListcr>();
                UGUITools.SetStretch(view);
            }
        }, false, 3);
    }

    public void ChooseHero(int heroType)
    {
        ChooseHeroType = heroType;
        _chooseCharactor = ConfigManager.Instance.GetConfig<ConfigCharactor>(ChooseHeroType);
        if (_chooseCharactor != null)
        {
            image_Herocr.SetSprite(AtlasName.HeroHead, _chooseCharactor.HeadPicture);

            btn_Renamedecr.gameObject.SetActive(true);
            p_hero hero = null;
            Global.Instance.HeroDic.TryGetValue(ChooseHeroType, out hero);
            if (hero == null || hero.equip_list.Length == 0)
            {
                SetDefault(_chooseCharactor);
            }
            else
            {
                if (hero.equip_list.Length == 3)
                    SortUtil.Sort(hero.equip_list, new string[1] { "plan_id" });

                for (int i = 0; i < hero.equip_list.Length; i++)
                {
                    if (hero.equip_id == hero.equip_list[i].plan_id)
                    {
                        _currInfo = hero.equip_list[i];
                        SetPlanData();

                    }
                }
            }
        }
    }

    public void UpdatePlanData(p_equip_info equip_info)
    {
        _currInfo = equip_info;
        SetPlanData();
    }

    void SetPlanData()
    {
        btn_EquipSchemecr.gameObject.GetComponentInChildren<Text>().text = _currInfo.plan_name;

        string[] arr = new string[_currInfo.equip_list.Length];
        for (int i = 0; i < _currInfo.equip_list.Length; i++)
        {
            arr[i] = _currInfo.equip_list[i].ToString();
        }

        _heroChooseEquipGrid.DataProvider = arr;
    }

    void SetDefault(ConfigCharactor charactor,int planid = 0)
    {
        string[] arr = charactor.EquipPlans.Split(';');
        for (int i = arr.Length-1; i >= 0; i--)
        {
            string temp = arr[i];
            string[] tempArr = temp.Split(':');
            p_equip_info equip_info = new p_equip_info();
            equip_info.plan_id = i+1;
            equip_info.plan_name = tempArr[0];

            string[] equips = tempArr[1].Split(',');
            List<int> list = new List<int>();
            for (int j = 0; j < equips.Length; j++)
            {
                list.Add(int.Parse(equips[j]));
            }
            equip_info.equip_list = list.ToArray();

            if (planid == 0 || planid == equip_info.plan_id)
            {
                RoleModel.Instance.TosChangeEquip(ChooseHeroType, equip_info, 4);
            }
        }
    }

    private void OnSchemeClick(GameObject go)
    {
        string[] chatPrefabs = new string[] { "Prefab/UI/Equip/ItemEquipmentSchemecr", "prefab/UI/Equip/ItemSingleSchemecr", "prefab/UI/EquipsOutside/ItemeEquipOutSide2cr", "prefab/UI/Equip/ItemeEquipStarcr" };

        ResourceManager.Instance.LoadMutileAssets(chatPrefabs, o =>
        {
            if (ItemEquipmentSchemecr.Exists == false)
            {
                GameObject view = UGUITools.AddChild(gameObject, ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/Equip/ItemEquipmentSchemecr"));
                view.AddComponent<ItemEquipmentSchemecr>();
                UGUITools.SetStretch(view);

                if (Global.Instance.HeroDic.ContainsKey(ChooseHeroType))
                {
                    ItemEquipmentSchemecr.Instance.SetData(Global.Instance.HeroDic[ChooseHeroType], _chooseCharactor);
                }
                else
                {
                    ItemEquipmentSchemecr.Instance.SetData(null, _chooseCharactor);
                }
            }
        }, false, 3);
    }

    private ItemeEquipOutSideChoose2cr _preChooseEquipItem;
    public void ClickChooseEquipItem(ItemeEquipOutSideChoose2cr item)
    {
        if(_preChooseEquipItem!=null)
        {
            _preChooseEquipItem.UnSelect();
        }
        item.Select();
        _preChooseEquipItem = item;

        ConfigEquip equip = item.GetData();
        int type = equip.type;
        if(type!=_currType)
        {
            tabBar.SelectTab(type-1);
        }

        ItemeEquipOutSidecr itemeEquip = dataEquipt3.GetItem<ConfigEquip>("id", equip.id) as ItemeEquipOutSidecr;
        if (itemeEquip==null)
            itemeEquip = dataEquipt2.GetItem<ConfigEquip>("id", equip.id) as ItemeEquipOutSidecr;
        if (itemeEquip == null)
            itemeEquip = dataEquipt1.GetItem<ConfigEquip>("id", equip.id) as ItemeEquipOutSidecr;

        ClickItem(itemeEquip);
    }

}

