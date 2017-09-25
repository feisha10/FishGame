using System;
using UnityEngine;
using System.Collections;


//所有的panel继承这个类统一方便管理
public interface PanelBase
{
    //需要提前加载的资源
    string[] BeforLoadInfos();
    string[] BeforLoadTextures();
}
