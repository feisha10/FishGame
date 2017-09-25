using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginViewBeforeLoad : PanelBase
{

	public string[] BeforLoadInfos()
	{
		return null;
	}
	public string[] BeforLoadTextures()
	{
		return null;
	}
	 //endTexture
}

public class LoginView : SingletonMonoBehaviour<LoginView>
{

	private Transform cachedTransform ;
	//开始UI申明;
	private RawImage rawI_bgcr;
	private GameObject go_Logincr;
	private ScrollRect scr_serverListcr;
	private GridLayoutGroup glayout_servercr;
	private Button btn_Logincr;
	private InputField input_platRoleNamcr;
	private GameObject go_CreateRolecr;
	private RawImage rawI_npcbgcr;
	private ScrollRect Scr_headcr;
	private GridLayoutGroup glayout_headiconcr;
	private InputField input_createRoleNamcr;
	private Button btn_changenamecr;
	private Button btn_createcr;
	private GameObject go_autoLogincr;
	private Button btn_AutLogincr;
	private Button btn_selectServercr;
	private Text Text_versioncr;
	private Text Text_declarecr;
	private Text Text_GameDeccr;
	private Button btn_noticecr;
	private GameObject go_noticecr;
	private Image image_previewMaskcr;
	private Text text_NoticrTitlecr;
	private ScrollRect Scr_ContentcrTextcr;
	private Text text_Noticrcontentcr;
	private GameObject go_ScrollbarList2cr;
	private Button btn_closecr;
	private Button btn_gmcr;
	//结束UI申明;
	private bool isUIinit = false;

	protected override void Awake ()
	{
		base.Awake();
		cachedTransform=transform;
		//开始UI获取;
		rawI_bgcr = cachedTransform.Find("rawI_bgcr").GetComponent<RawImage>();
		go_Logincr = cachedTransform.Find("center/go_Logincr").gameObject;
		scr_serverListcr = cachedTransform.Find("center/go_Logincr/scr_serverListcr").GetComponent<ScrollRect>();
		glayout_servercr = cachedTransform.Find("center/go_Logincr/scr_serverListcr/glayout_servercr").GetComponent<GridLayoutGroup>();
		btn_Logincr = cachedTransform.Find("center/go_Logincr/btn_Logincr").GetComponent<Button>();
		input_platRoleNamcr = cachedTransform.Find("center/go_Logincr/input_platRoleNamcr").GetComponent<InputField>();
		go_CreateRolecr = cachedTransform.Find("center/go_CreateRolecr").gameObject;
		rawI_npcbgcr = cachedTransform.Find("center/go_CreateRolecr/rawI_npcbgcr").GetComponent<RawImage>();
		Scr_headcr = cachedTransform.Find("center/go_CreateRolecr/Scr_headcr").GetComponent<ScrollRect>();
		glayout_headiconcr = cachedTransform.Find("center/go_CreateRolecr/Scr_headcr/glayout_headiconcr").GetComponent<GridLayoutGroup>();
		input_createRoleNamcr = cachedTransform.Find("center/go_CreateRolecr/input_createRoleNamcr").GetComponent<InputField>();
		btn_changenamecr = cachedTransform.Find("center/go_CreateRolecr/btn_changenamecr").GetComponent<Button>();
		btn_createcr = cachedTransform.Find("center/go_CreateRolecr/btn_createcr").GetComponent<Button>();
		go_autoLogincr = cachedTransform.Find("go_autoLogincr").gameObject;
		btn_AutLogincr = cachedTransform.Find("go_autoLogincr/btn_AutLogincr").GetComponent<Button>();
		btn_selectServercr = cachedTransform.Find("go_autoLogincr/btn_selectServercr").GetComponent<Button>();
		Text_versioncr = cachedTransform.Find("go_autoLogincr/Text_versioncr").GetComponent<Text>();
		Text_declarecr = cachedTransform.Find("go_autoLogincr/Text_declarecr").GetComponent<Text>();
		Text_GameDeccr = cachedTransform.Find("go_autoLogincr/Text_GameDeccr").GetComponent<Text>();
		btn_noticecr = cachedTransform.Find("go_autoLogincr/btn_noticecr").GetComponent<Button>();
		go_noticecr = cachedTransform.Find("go_noticecr").gameObject;
		image_previewMaskcr = cachedTransform.Find("go_noticecr/image_previewMaskcr").GetComponent<Image>();
		text_NoticrTitlecr = cachedTransform.Find("go_noticecr/text_NoticrTitlecr").GetComponent<Text>();
		Scr_ContentcrTextcr = cachedTransform.Find("go_noticecr/Scr_ContentcrTextcr").GetComponent<ScrollRect>();
		text_Noticrcontentcr = cachedTransform.Find("go_noticecr/Scr_ContentcrTextcr/Content/text_Noticrcontentcr").GetComponent<Text>();
		go_ScrollbarList2cr = cachedTransform.Find("go_noticecr/Scr_ContentcrTextcr/go_ScrollbarList2cr").gameObject;
		btn_closecr = cachedTransform.Find("go_noticecr/btn_closecr").GetComponent<Button>();
		btn_gmcr = cachedTransform.Find("go_noticecr/btn_gmcr").GetComponent<Button>();
		//结束UI获取;
		isUIinit = true;
	}
}

