using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;

/// <summary>
/// 加载资源结束
/// </summary>
public delegate void LoadAssetFinish(Object obj,string pahth);

public class ResourceManager : SingletonSpawningMonoBehaviour<ResourceManager>
{
    private Action InitInfoBackAction;
    private Dictionary<string, Object> haveLoadDic = new Dictionary<string, Object>();//<key为resource下路径可能为大写,加载好的资源>
    private Dictionary<string, LoadAssetFinish> LoadWaitingDic = new Dictionary<string, LoadAssetFinish>();  //key为resource下路径可能为大写
    private Dictionary<string, LoadAssetFinish> loadingDic = new Dictionary<string, LoadAssetFinish>();  //key为resource下路径可能为大
    private Dictionary<string,LoadResourceInfo> ResourceInfos=new Dictionary<string,LoadResourceInfo>();
    private Dictionary<string,AssetBundle>  UnLoadAbLists=new Dictionary<string, AssetBundle>();   //未被卸载的所有Ab
    private Dictionary<string,int> ManulUnloadDic = new Dictionary<string, int>(); //手动卸载列表 
    private Dictionary<string, int> CompleteAtlasDic = new Dictionary<string, int>();

    private AssetBundleManifest abDepenceInfos;
    public bool IsInit = false;
    private const string ABExtension = ".assetbundle";
    private const string AtlasPrefabName = "_atlasprefab";
    private const string AtlasTexturebName = "_atlas";
    private const int MaxLoadWwwNum = 6;
    public static int currentLoadNum=0;

    private static Dictionary<string, string> appFileDic;   //游戏包内StreamingAssets目录下filelist.txt读出的文件列表
    private static Dictionary<string, string> sdFileDic = new Dictionary<string, string>(); //游戏包外out目录下filelist.txt读出的文件列表
    private static Dictionary<string, int> fileSizeDic;
    private static Dictionary<string, int> versionDic; 

    private readonly bool IsReadFromAB = false;

    public void InitFileDic(Dictionary<string,string> appDic, Dictionary<string,string> sdDic, Action callBack )
    {
        appFileDic = new Dictionary<string, string>(appDic.Count);
        foreach (var pair in appDic)
        {
            string path = pair.Key.Substring(pair.Key.IndexOf('/') + 1);
            //path = path.ToLower();
            _AddFilDic<string>(appFileDic, path, pair.Value);
        }

        foreach (var pair in sdDic)
        {
            string path = pair.Key.Substring(pair.Key.IndexOf('/') + 1);
            //path = path.ToLower();
            string appVer;
            if (appFileDic.TryGetValue(path, out appVer) && appVer == pair.Value)
                continue;
            _AddFilDic<string>(sdFileDic, path, pair.Value);
        }
        InitInfoBackAction = callBack;
    }

    void _AddFilDic<T>(Dictionary<string, T> fileDic, string path, T ver)
    {
        fileDic[path] = ver;
    }

    //只用在一些不确定是否有资源的预先判定
    public bool ContainAsset(string path)
    {
        string key = path.ToLower() + ABExtension;
        if (appFileDic.ContainsKey(key))
            return true;
        if (sdFileDic.ContainsKey(key))
            return true;
        return false;
    }

    public void MualAddAsset(string path, Object o, int sourceType)
    {
        if (ResourceInfos.ContainsKey(path)==false)
        {
            LoadResourceInfo infoOr = new LoadResourceInfo(path, sourceType);
            ResourceInfos.Add(path, infoOr);
        }

        haveLoadDic[path] = o;
    }

    public T LoadExistsAsset<T>(string path, bool showError = true) where T : Object
    {
        if (string.IsNullOrEmpty(path))
            return null;
        if (!ResourceInfos.ContainsKey(path))
        {
            if (showError)
            {
                string msg = "找不到路径：" + path;
                Log.Error(msg);
            }
            return null;
        }
        LoadResourceInfo info = ResourceInfos[path];
        if (haveLoadDic.ContainsKey(info.orialPath))
        {
            return haveLoadDic[info.orialPath] as T;
        }
        if (showError)
        {
            string msg = "找不到原始路径：" + info.orialPath;
            Log.Error(msg);
        }

        return null;
    }

    public AssetBundle GetAssetBundle(string path)
    {
        return haveLoadDic[path] as AssetBundle;
    }

    //首选先先去加载的资源
    public void CommonFirstLoad(Action back)
    {
        Log.Debug("CommonFirstLoad");
        string[] paths = new string[] {"Prefab/UI/Common/FontPrefab", "resources/shader.assetbundle",
             "Prefab/UI/TipAlter/AlertBox", 
            "Prefab/Effect/UIEffect/UI_loading_1"};

        LoadMutileAssets(paths, o =>
        {
            if (back != null)
                back();
        }, false, 3);
    }

    public void SecondLoad(Action back)
    {
        string[] paths = new string[] { "Prefab/UI/Fightloading/FightloadingView", "Prefab/UI/Guide/UI_GuideTip", "Prefab/UI/Common/ConnectingUI", 
            "Prefab/UI/Task/ItemAchTipscr", "Prefab/UI/TipAlter/TipsIconLabel","Prefab/UI/TipAlter/TipsLabel",};

        LoadMutileAssets(paths, o =>
        {
            if (back != null)
                back();
        }, false, 3);

        //VipView界面需要使用包含headbg图集的MotionText,需要提前预加载图集。而考虑到头像框图集的用途广泛，所以和表情一起在初始预加载
        string[] atlaspaths = new string[] { "lib/atlas/chatface/chatface_atlasprefab.prefab.assetbundle", "lib/atlas/headbg/headbg_atlasprefab.prefab.assetbundle" };

        LoadMutileAssets(atlaspaths, o =>
        {

        }, false, 2);
    }

    private string GetPath(string name)
    {
        string path = Path.Combine(Util.OutDir, name);
        if (!File.Exists(path))
        {
            path = Util.GetStreamingAssetsPathNoTarget() + name;
        }
        else
        {
            path = "file:///" + path;
        }
        return path;
    }

    /// <summary>
    /// 多个资源一起加载，    注意当要返回加载进度的时候，只能同时调用一次这个函数
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="backAction"></param>
    /// <param name="returnBackPer">是否返回加载进度</param>
    public void LoadMutileAssets(string[] paths, Action<float> backAction, bool returnBackPer = false, int sourceType = 0)
    {
        int allLenth = 0;
        int curr = 0;
        int all = 0;
        List<LoadResourceInfo> infos = new List<LoadResourceInfo>();
        for (int i = 0; i < paths.Length; i++)
        {
            string tempPath = paths[i];
            LoadResourceInfo infoOr;
            if (ResourceInfos.ContainsKey(tempPath))
                infoOr = ResourceInfos[tempPath];
            else
            {
                infoOr = new LoadResourceInfo(tempPath, sourceType);
                ResourceInfos.Add(tempPath, infoOr);
            }
            
            string[] dependences = abDepenceInfos.GetDirectDependencies(infoOr.unityDepedencePath);
            

            if (!infos.Contains(infoOr))
            {
                for (int j = 0; j < dependences.Length; j++)
                {
                    string dependPath = dependences[j];
                    if (fileSizeDic.ContainsKey(dependPath))
                        allLenth += fileSizeDic[dependPath];
                }
                all += 1;

#if UNITY_EDITOR
                if(IsReadFromAB)
                    all += dependences.Length;
#else
                all += dependences.Length;
#endif

                infos.Add(infoOr);
                if (fileSizeDic.ContainsKey(infoOr.unityDepedencePath))
                    allLenth += fileSizeDic[infoOr.unityDepedencePath];
            }
        }

        LoadAssetFinish innerCallBack = (o, p) =>
        {
            curr++;
            if (returnBackPer)
            {
                LoadResourceInfo info = ResourceInfos[p];
                if (fileSizeDic.ContainsKey(info.unityDepedencePath))
                {
                    float per = 100 * (1.0f * fileSizeDic[info.unityDepedencePath] / allLenth);
                    backAction(per);
                }
            }
            if (curr >= all)
            {
                if (backAction!=null)
                    backAction(100);
            }
        };

        for (int i = 0; i < infos.Count; i++)
        {
            DellDepenceAssent(infos[i], innerCallBack,true);
        }
    }

    //路径分为两类，一类原来的resouce下的路径存在大写，为name做准备；、、
    //一类是stream下面的只有小写为加载资源做准备（依赖的资源的路径都是此类）,还有一类是加载资源路径，需要加头路径的
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="backAction"></param>
    /// <param name="type">0表示普通的资源加载，1表示图集prefab的加载</param>
    public void LoadAsset(string path, LoadAssetFinish backAction, int type = 0)
    {
        LoadResourceInfo info;
        if (ResourceInfos.ContainsKey(path))
            info = ResourceInfos[path];
        else
        {
            switch (type)
            {
                case 1:
                    info = new LoadResourceInfo(path, 2);
                    break;
                case 4:
                    info = new LoadResourceInfo(path, 4);
                    break;
                case 3:
                    info = new LoadResourceInfo(path, 3);
                    break;
                case 5:
                    info = new LoadResourceInfo(path, 5, true);
                    break;
                default:
                    info = new LoadResourceInfo(path, 0);
                    break;
            }
            ResourceInfos.Add(path, info);
        }

        if (haveLoadDic.ContainsKey(info.orialPath))
        {
            if (backAction!=null)
                backAction(haveLoadDic[info.orialPath], info.orialPath);
            return;
        }

        DellDepenceAssent(info, backAction);
    }

    void DellDepenceAssent(LoadResourceInfo path, LoadAssetFinish callbaAction,bool isReturnDepence = false,int LoadNum=0)
    {
        if (string.IsNullOrEmpty(path.orialPath))
        {
            callbaAction(null,path.orialPath);
        }

#if UNITY_EDITOR
        if (IsReadFromAB==false)
        {
            if (haveLoadDic.ContainsKey(path.orialPath))
            {
                callbaAction(haveLoadDic[path.orialPath], path.orialPath);
                return;
            }

            if (path.type == 5)
                AddPathToLoadingList(path, callbaAction);
            else
                StartCoroutine(LoadLocalAsset(path, callbaAction));
            return;
        }
#endif
   
        if (haveLoadDic.ContainsKey(path.orialPath))
        {
            if (callbaAction != null&&isReturnDepence)
            {
                callbaAction(haveLoadDic[path.orialPath], path.orialPath);
                string depencePath1 = path.unityDepedencePath;
                string[] dependences1 = abDepenceInfos.GetDirectDependencies(depencePath1);
                for(int i=0;i<dependences1.Length;i++)
                {
                    callbaAction(haveLoadDic[path.orialPath], dependences1[i]);
                }
            }
            else if (callbaAction != null)
                callbaAction(haveLoadDic[path.orialPath], path.orialPath);
            return;
        }
        string depencePath = path.unityDepedencePath;
        string[] dependences = abDepenceInfos.GetDirectDependencies(depencePath);
        int length = dependences.Length;
        if (length == 0)
        {
            AddPathToLoadingList(path, callbaAction);
        }
        else
        {
            int all = length;
            int curr = 0;

            LoadAssetFinish innerCallBack = (o,p) =>
            {
                curr++;
                if (isReturnDepence)
                    callbaAction(o, p);

                if (curr >= all)
                {
                    AddPathToLoadingList(path, callbaAction);
                }
            };
            LoadNum = LoadNum + 1;

            for (int i = 0; i < dependences.Length; i++)
            {
                string dependPath = dependences[i];
                if (string.IsNullOrEmpty(dependPath))
                {
                    innerCallBack(null,dependPath);
                    continue;
                }
                string[] itemdependences = abDepenceInfos.GetDirectDependencies(dependPath);
                int itemlength = itemdependences.Length;
                if (itemlength > 0)
                    continue;
                LoadResourceInfo info;
                if (ResourceInfos.ContainsKey(dependPath))
                {
                    info = ResourceInfos[dependPath];
                    if (LoadNum <= 1 && info.type == 1)
                    {
                        info.type = 3;
                    }
                }
                else
                {
                    int type = 3;
                    if (dependPath.Contains(AtlasPrefabName))
                        type = 2;
                    else if (dependPath.Contains(AtlasTexturebName))
                        type = 1;
                    else if (LoadNum > 1 && !dependPath.Contains("shader.assetbundle")&&!dependPath.StartsWith("lib/effect"))
                        type = 1;
                    info = new LoadResourceInfo(dependPath, type, true);
                    ResourceInfos.Add(dependPath, info);
                }
                AddPathToLoadingList(info, innerCallBack);
            }

            for (int i = 0; i < dependences.Length; i++)
            {
                string dependPath = dependences[i];
                string[] itemdependences = abDepenceInfos.GetDirectDependencies(dependPath);
                int itemlength = itemdependences.Length;
                if (itemlength == 0)
                    continue;
                LoadResourceInfo info;
                if (ResourceInfos.ContainsKey(dependPath))
                {
                    info = ResourceInfos[dependPath];
                    if(LoadNum<=1&&info.type==1)
                    {
                        info.type = 3;
                    }
                } 
                else
                {
                    int type = 3;
                    if (dependPath.Contains(AtlasPrefabName))
                        type = 2;
                    else if (dependPath.Contains(AtlasTexturebName))
                        type = 1;
                    else if (LoadNum > 1 && !dependPath.Contains("shader.assetbundle") && !dependPath.StartsWith("lib/effect"))
                        type = 1;
                    info = new LoadResourceInfo(dependPath, type, true);
                    ResourceInfos.Add(dependPath, info);
                }
                

                DellDepenceAssent(info, innerCallBack, false, LoadNum);
                //AddPathToLoadingList(info, innerCallBack);
            }          
        }
    }

#if UNITY_EDITOR
    IEnumerator LoadLocalAsset(LoadResourceInfo path, LoadAssetFinish callbaAction)
    {
        Object oo = null;

        string temp = path.orialPath.Replace(ABExtension, "");
        ResourceRequest request = Resources.LoadAsync(temp);
        yield return request;
        oo = request.asset;

        if (!haveLoadDic.ContainsKey(path.orialPath))
        {
            haveLoadDic.Add(path.orialPath, oo);
        }
        callbaAction(oo, path.orialPath);
        yield break;
    }
#endif

    //由于多次实验发现，资源依赖的所有资源包括第二次以及更多层的资源都包含在dependence中所以类型1可以直接加入loading链表中，有错误再修改，需要同时修改依赖加载完成的函数
    private void AddPathToLoadingList(LoadResourceInfo path, LoadAssetFinish obj)
    {
        if(string.IsNullOrEmpty(path.orialPath))
             Log.Debug("path.orialPathpath.orialPathpath.orialPathpath.orialPathpath.orialPathpath.orialPathpath.orialPath");
        if (haveLoadDic.ContainsKey(path.orialPath))
        {
            if (obj!=null)
                obj(haveLoadDic[path.orialPath], path.orialPath);
        }
        else if (LoadWaitingDic.ContainsKey(path.orialPath))
        {
            if (obj != null)
            {
                LoadAssetFinish call = LoadWaitingDic[path.orialPath];
                if (call == null)
                {
                    LoadWaitingDic[path.orialPath] = obj;
                }
                else
                {
                    LoadWaitingDic[path.orialPath] += obj;
                }
            }
        }
        else if (loadingDic.ContainsKey(path.orialPath))
        {
            if (obj != null)
            {
                LoadAssetFinish call = loadingDic[path.orialPath];
                if (call == null)
                {
                    loadingDic[path.orialPath] = obj;
                }
                else
                {
                    loadingDic[path.orialPath] += obj;
                }
            }
        }
        else
        {
            if (currentLoadNum < MaxLoadWwwNum)
            {
                currentLoadNum++;
                loadingDic.Add(path.orialPath, obj);

                LoadAbFix(path.orialPath);
            }
            else
            {
                LoadWaitingDic.Add(path.orialPath, obj);
            }
        }
    }

    void LoadAbFix(string path)
    {
        string _path = path;
        LoadResourceInfo info = ResourceInfos[_path];

#if UNITY_IOS
        StartCoroutine(LoadAB(info, _path));
#else
        StartCoroutine(LoadAB(info, _path));
//        if (info.type == 4 || info.type == 5)
//            StartCoroutine(LoadAB(info, _path));
//        else
//        {
//            StartCoroutine(LoadABRequest(info, _path));
//        }
#endif

    }

    IEnumerator LoadABRequest(LoadResourceInfo info, string path)
    {
        string _path = path;
        AssetBundleCreateRequest _www = AssetBundle.LoadFromFileAsync(info.loadPath);
        yield return _www;
        UnityEngine.Object obj = null;
        if (_www.isDone && _www.assetBundle!=null)
        {
            AssetBundle assetBundle = _www.assetBundle;

            if (assetBundle != null)
            {
                obj = assetBundle.LoadAsset(info.name);
            }

            if (obj == null)
            {
                obj = assetBundle;
            }

            if (info.type == 0)
            {
                UnloadDependce(info);
            }
            else if (info.type == 5)
            {
                //Do Nothing
            }
            else
            {
                UnLoadAbLists.Add(info.orialPath, assetBundle);
                if (info.type == 2)
                {
                    SetAtlasPrefab(info);
                }
            }

            if (info.type != 4 && !haveLoadDic.ContainsKey(info.orialPath))
            {
                haveLoadDic.Add(info.orialPath, obj);
            }

            if (info.type == 0)
            {
                if (assetBundle != null)
                    assetBundle.Unload(false);
            }
        }
        else
        {
            string msg = string.Format("Error Path:{0}||Error Msg:{1}", path, _www.ToString());
            Log.Error(msg);
        }

        //_www.Dispose();
        _www = null;
        currentLoadNum--;

        if (loadingDic.ContainsKey(_path))
        {
            LoadAssetFinish actiona = loadingDic[_path];
            if (actiona != null)
                actiona(obj, _path);
        }

        loadingDic.Remove(_path);

        LoadWaiting();
    }

    IEnumerator LoadAB(LoadResourceInfo info,string path)
    { 
        string _path = path;
        if (info.type!=5 && versionDic.ContainsKey(info.unityDepedencePath) == false)
        {         
            LoadFinishHandler(_path);

            string msg = "找不到路径：" + path;
            Log.Error(msg);

            yield break;
        }
        WWW _www;
        if (info.type == 4 || info.type == 5)
        {
            _www = new WWW(info.loadPath);
        }
        else
        {
            //_www = WWW.LoadFromCacheOrDownload(info.loadPath, versionDic[info.unityDepedencePath]);
            _www = new WWW(info.loadPath);
        }

        yield return _www;

        UnityEngine.Object obj = null;
        if (_www.isDone && string.IsNullOrEmpty(_www.error))
        {
            AssetBundle assetBundle = null;
            if(info.type==4)
            {
                byte[] bytes = _www.bytes;
                Util._MaskData(bytes);
                assetBundle = AssetBundle.LoadFromMemory(bytes);
            }
            else
            {
                assetBundle = _www.assetBundle;
            }

            if (assetBundle != null)
            {
                obj = assetBundle.LoadAsset(info.name);
            }
            else if (info.type == 5)
            {
                obj = _www.texture;
            }

            if (obj == null)
            {
                obj = assetBundle;
            }

            if (info.type == 0)
            {
                UnloadDependce(info);
            }
            else if (info.type == 5 || info.type == 4)
            {
                //Do Nothing
            }
            else
            {
                UnLoadAbLists.Add(info.orialPath, assetBundle);
                if (info.type == 2)
                {
                    SetAtlasPrefab(info);
                }
            }

            if (info.type!=4 && !haveLoadDic.ContainsKey(info.orialPath))
            {
                haveLoadDic.Add(info.orialPath, obj);
            }

            if (info.type == 0)
            {
                 if (assetBundle!=null)
                     assetBundle.Unload(false);
            }
        }
        else
        {
            string msg = string.Format("Error Path:{0}||Error Msg:{1}", path, _www.error);
            Log.Error(msg);
        }

        _www.Dispose();
        _www = null;

        LoadFinishHandler(_path, obj);
    }

    void LoadFinishHandler(string _path, UnityEngine.Object obj = null)
    {
        currentLoadNum--;

        if (loadingDic.ContainsKey(_path))
        {
            LoadAssetFinish actiona = loadingDic[_path];
            if (actiona != null)
                actiona(obj, _path);
        }

        loadingDic.Remove(_path);

        LoadWaiting();
    }

    void LoadWaiting()
    {
        if (LoadWaitingDic.Count > 0 && currentLoadNum < MaxLoadWwwNum)
        {
            string next = GetNextWaitingAsset();
            if (!string.IsNullOrEmpty(next))
            {
                LoadAssetFinish back = LoadWaitingDic[next];
                LoadWaitingDic.Remove(next);

                AddPathToLoadingList(ResourceInfos[next], back);
            }
        }
    }

    string GetNextWaitingAsset()
    {
        var iter = LoadWaitingDic.GetEnumerator();

        if (iter.MoveNext())
        {
            if (string.IsNullOrEmpty(iter.Current.Key))
            {
                LoadWaitingDic.Remove(iter.Current.Key);
                return GetNextWaitingAsset();
            }
            else
            {
                return iter.Current.Key;
            }
        }
        return null;
    }

    void UnloadDependce(LoadResourceInfo path)
    {
        string[] dependences = abDepenceInfos.GetAllDependencies(path.unityDepedencePath);
        for (int i = 0; i < dependences.Length; i++)
        {
            if (string.IsNullOrEmpty(dependences[i]))
                continue;
            LoadResourceInfo info = ResourceInfos[dependences[i]];
            if (info.type == 1)
            {
                if (UnLoadAbLists.ContainsKey(info.orialPath))
                {
                    UnLoadAbLists[info.orialPath].Unload(false);
                    UnLoadAbLists.Remove(info.orialPath);
                    haveLoadDic.Remove(info.orialPath);
                }
            }
            if (info.type == 2)
            {
                SetAtlasPrefab(info);
            }
        }
    }

    public void UnloadManulDependce(string path)
    {
#if UNITY_EDITOR
        if (IsReadFromAB==false)
            return;
#endif
        if(ResourceInfos.ContainsKey(path)==false)
            return;
        LoadResourceInfo info = ResourceInfos[path];
        if(ManulUnloadDic.ContainsKey(info.orialPath))
            return;

        ManulUnloadDic.Add(info.orialPath,1);

        string[] dependences = abDepenceInfos.GetAllDependencies(info.unityDepedencePath);

        for (int i = 0; i < dependences.Length; i++)
        {
            if (string.IsNullOrEmpty(dependences[i]))
                continue;
            LoadResourceInfo temp = ResourceInfos[dependences[i]];
            if (UnLoadAbLists.ContainsKey(temp.orialPath))
            {
                UnLoadAbLists[temp.orialPath].Unload(false);
                UnLoadAbLists.Remove(temp.orialPath);
                haveLoadDic.Remove(temp.orialPath);
            }
        }
    }

    public void RemoveAsset(string path)
    {
#if UNITY_EDITOR
        if (IsReadFromAB == false)
            return;
#endif

        if (ResourceInfos.ContainsKey(path)==false)
            return;

        LoadResourceInfo info = ResourceInfos[path];

        if (UnLoadAbLists.ContainsKey(info.orialPath))
            UnLoadAbLists.Remove(info.orialPath);
        if(haveLoadDic.ContainsKey(info.orialPath))
        {
            haveLoadDic.Remove(info.orialPath);
        }
        if (ManulUnloadDic.ContainsKey(info.orialPath))
            ManulUnloadDic.Remove(info.orialPath);
    }

    private void SetAtlasPrefab(LoadResourceInfo info)
    {
        if (UnLoadAbLists.ContainsKey(info.orialPath))
        {
            if(CompleteAtlasDic.ContainsKey(info.orialPath)==false)
            {

            }
        } 
    }

    public class LoadResourceInfo
    {
        public string unityDepedencePath; //小写的资源路径，非网络图片时是StreamingAssets下的相对路径(也是依赖表的key)
        public string loadPath; //加载资源时的路径，非网络图片时、可以认为是unityDepedencePath加上资源目录绝对路径前缀的路径
        public string orialPath; //原路径Resource下路径为大写
        public string name; //AssetName;无文件名后缀的资源名字。
        public int type; //0表示外部加载资源，1、图集贴图资源 2、图集prefab资源  3、其他资源（暂时不会删除此类的AB  待技能完善后继续处理） 4、加密资源 5、网络图片

        /// <summary>
        /// 0表示外部加载资源，1、图集贴图资源 2、图集prefab资源  3、其他资源（暂时不会删除此类的AB  待技能完善后继续处理） 4、加密资源 5、网络图片
        /// </summary>
        /// <param name="_orialPath"></param>
        /// <param name="_type"></param>
        /// <param name="fromDepence"></param>
        public LoadResourceInfo(string _orialPath, int _type = 3, bool fromDepence = false)
        {
            type = _type;

            if (type == 5)
            {
                unityDepedencePath = loadPath = orialPath = name = _orialPath;
            }
            else
            {
                if (fromDepence)
                {
                    orialPath = unityDepedencePath = _orialPath;
                }
                else
                {
                    orialPath = _orialPath;
                    unityDepedencePath = _orialPath.ToLower();
                    string extension = Path.GetExtension(unityDepedencePath);
                    unityDepedencePath = string.IsNullOrEmpty(extension)
                                             ? (unityDepedencePath + ABExtension)
                                             : unityDepedencePath.Replace(extension, ABExtension);
                }

                string[] arr = _orialPath.Split('/');
                name = arr[arr.Length - 1];
                name = Path.GetFileNameWithoutExtension(name);
                
#if UNITY_EDITOR
                loadPath = Util.GetStreamingAssetsPath() + unityDepedencePath;
#elif UNITY_IOS ||UNITY_ANDROID
                if (sdFileDic.ContainsKey(unityDepedencePath))
                {
                    loadPath = string.Format("file:///{0}{1}/{2}", Util.OutDir, Util.GetTargetPath(), unityDepedencePath);
                }
                else if (appFileDic.ContainsKey(unityDepedencePath))
                    loadPath = Util.GetStreamingAssetsPath() + unityDepedencePath;
                else
                {
                    Log.Error("不包含" + unityDepedencePath);
                    loadPath = "";
                }
#else 
                if (sdFileDic.ContainsKey(unityDepedencePath))
                {
                    if (type==4 || type==5)
                    {
                        loadPath = string.Format("file:///{0}{1}/{2}", Util.OutDir, Util.GetTargetPath(), unityDepedencePath);
                    }
                    else
                    {
                        loadPath = string.Format("{0}{1}/{2}", Util.OutDir, Util.GetTargetPath(), unityDepedencePath);
                    }
                }
                else if (appFileDic.ContainsKey(unityDepedencePath))
                {
                    if (type==4 || type==5)
                    {
                        loadPath = Util.GetStreamingAssetsPath() + unityDepedencePath;
                    }
                    else
                    {
                        loadPath = Util.GetStreamingAssetsPathABLoad() + unityDepedencePath;
                    }
                }
                else
                {
                    Log.Error("不包含" + unityDepedencePath);
                    loadPath = "";
                }
#endif
            }
        }
    }
}


