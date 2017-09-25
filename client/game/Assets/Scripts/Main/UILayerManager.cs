using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 使用此类来加载要求prefab的name和代码的名字统一，**View,prefab所在的文件夹为Resources\prefab\UI\**;
/// </summary>
public class UILayerManager : Singleton<UILayerManager>
{
    private Dictionary<string, GameObject> panelGO = new Dictionary<string, GameObject>();
    private List<string> closInfo = new List<string>();
    private string currentPanel="";
    private string lastPanel = "";

    public void Init()
    {
        EventManager.Regist<string>(EventIdx.OnSingletenPanelDestory, OnDestoryPanel);
        EventManager.Regist<string, GameObject>(EventIdx.OnSingletenPanelCreate, OnCreatePanel);
    }

    private void OnCreatePanel(string obj, GameObject go)
    {
        if (!panelGO.ContainsKey(obj))
            panelGO.Add(obj, go);
    }

    private void OnDestoryPanel(string obj)
    {
        if (panelGO.ContainsKey(obj))
            panelGO.Remove(obj);
    }

    public void OpenPanel(string panelName, Action<UnityEngine.Object> OpenBack, bool IsDestoryInChangeScene = true)
    {
        if (IsDestoryInChangeScene && !closInfo.Contains(panelName))
            closInfo.Add(panelName);
        if (panelName == currentPanel)
            return;
        lastPanel = currentPanel;
        currentPanel = panelName;
        if (panelGO.ContainsKey(panelName))
        {
            privateClosePanel(lastPanel);
            panelGO[currentPanel].gameObject.SetActive(true);
            if (OpenBack != null)
                OpenBack(panelGO[currentPanel]);
        }
        else
        {
            string name = panelName;
            System.Type type = Type.GetType(name);
            PanelBase script = null;
            Action<UnityEngine.Object> back = OpenBack;
            if (null != type)
            {
                script = type.Assembly.CreateInstance(type.Name + "BeforeLoad") as PanelBase;
                List<string> beforLoad = new List<string>();
                if (script.BeforLoadInfos() != null)
                    beforLoad.AddRange(script.BeforLoadInfos());
                if (script.BeforLoadTextures() != null)
                    beforLoad.AddRange(script.BeforLoadTextures());
                if (beforLoad.Count > 0)
                {
                    ResourceManager.Instance.LoadMutileAssets(beforLoad.ToArray(), (o) =>
                    {
                        ResourceManager.Instance.LoadAsset(GetPrefabPath(panelName), (o1, p1) =>
                        {
                            if (panelName != currentPanel)
                                return;

                            privateClosePanel(lastPanel);
                            if (!panelGO.ContainsKey(panelName))
                            {
                                GameObject go = o1 as GameObject;
                                go = GameObject.Instantiate(go);
                                go.SetActive(false);
                                go.transform.SetParent(UIManager.UiLayer);
                                go.transform.localScale = Vector3.one;
                                RectTransform trn = go.GetRectTransform();
                                if (trn != null)
                                {
                                    trn.offsetMax = Vector2.zero;
                                    trn.offsetMin = Vector2.zero;
                                }
                                else
                                {
                                    go.transform.localPosition = Vector3.one;
                                }
                                if (null != type)
                                {
                                    go.AddComponent(type);
                                }
                                go.SetActive(true);

                                Util.RefreshPanelShaderInEditor(go);
                                if (OpenBack != null)
                                    OpenBack(go);
                            }
                        });
                    });
                }
                else
                {
                    ResourceManager.Instance.LoadAsset(GetPrefabPath(panelName), (o, p) =>
                    {
                        if (!panelGO.ContainsKey(panelName))
                        {
                            if (panelName != currentPanel)
                                return;
                            privateClosePanel(lastPanel);
                            GameObject go = o as GameObject;
                            go = GameObject.Instantiate(go);
                            go.SetActive(false);
                            go.transform.SetParent(UIManager.UiLayer);
                            go.transform.localScale = Vector3.one;
                            RectTransform trn = go.GetRectTransform();
                            if (trn != null)
                            {
                                trn.offsetMax = Vector2.zero;
                                trn.offsetMin = Vector2.zero;
                            }
                            else
                            {
                                go.transform.localPosition = Vector3.one;
                            }
                            if (null != type)
                            {
                                go.AddComponent(type);
                            }
                            go.SetActive(true);
                            Util.RefreshPanelShaderInEditor(go);
                            if (OpenBack != null)
                                OpenBack(go);
                        }
                    });
                }
            }
        }
    }

    void privateClosePanel(string panelName)
    {
        if (!string.IsNullOrEmpty(panelName) && panelGO.ContainsKey(panelName))
        {
            panelGO[panelName].gameObject.SetActive(false);
        }
    }

    private string GetPrefabPath(string name)
    {
        string path = name.Replace("View", "");
        return string.Format("Prefab/UI/{0}/{1}", path, name);
    }
}
