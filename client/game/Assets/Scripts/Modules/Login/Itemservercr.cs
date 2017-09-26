using System;
using UnityEngine;
using UnityEngine.UI;

public class Itemservercr : MonoBehaviour
{

	private Transform cachedTransform ;
	//开始UI申明;
	private Image image_servercr;
	private Text text_serNamecr;
	//结束UI申明;
	private bool isUIinit = false;

	void Awake ()
	{
		cachedTransform=transform;
		//开始UI获取;
		image_servercr = cachedTransform.Find("image_servercr").GetComponent<Image>();
		text_serNamecr = cachedTransform.Find("image_servercr/text_serNamecr").GetComponent<Text>();
		//结束UI获取;
		isUIinit = true;
	}
}

