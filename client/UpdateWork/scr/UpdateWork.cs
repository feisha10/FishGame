using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using Object = UnityEngine.Object;

namespace UpdateWork
{
    public class UpdateWork : MonoBehaviour
    {
        int _processPercent;
        int ProcessPercent
        {
            set
            {
                if (load != null)
                {
                    load.SetLoadPerCent(value);
                }
                _processPercent = value;
            }
            get
            {
                return _processPercent;
            }
        }
        int _percentAdd = 0;
        WWW _wwwDown = null;

        LoadingView load;
        GameObject LoadingUI;

        private bool _updateOk = false;

        string _updateUrl = null;
        bool _isActiveSDK = false;
        string _sdkName = null;

        private bool _isNewApk = false;
        private int _isPlayMovie = 0;
        private string _serverSdkVersion;

        private string _secondVersion;

        private string localVersion;
        private string outLocalVersion;
        private string localGameDefine;
        private string outLocalGameDefine;
        private string loccalFileList;
        private string outloccalFileList;

        private string _infoTips 
        {
            set 
            {
                if (load != null)
                {
                    load.SetLoadInfo(value);
                }
            }
        }

        void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;//屏幕常

            StartCoroutine(InitInfos());
        }

        //no platform
        private string GetPathNoTarget(string fielName)
        {
            string path = Path.Combine(FirstUtil.OutDir, fielName);
            if (!File.Exists(path))
            {
                path = FirstUtil.GetStreamingAssetsPathNoTarget() + fielName;
            }
            else
            {
                path = "file:///" + path;
            }
            return path;
        }

        //no platform
        private string GetPath(string fielName)
        {
            fielName = fielName + ".assetbundle";
            string path = Path.Combine(FirstUtil.OutDir, FirstUtil.platform + "/" +fielName);
            if (!File.Exists(path))
            {
                path = FirstUtil.StreamingAssetsPath + fielName;
            }
            else
            {
                path = "file:///" + path;
            }
            return path;
        }

        string getWWWTxt(WWW www,bool showLog = true)
        {
            string result = null;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                result = www.text;
                if (showLog)
                    WriteLog("WWW Value:" + result);
            }
            else
            {
                result = "";
                string msg = "loadError:" + www.error;
                Debug.LogError(msg);
                FirstRecordMsg.Error(msg, MsgType.DownloadError);
            }
            www.Dispose();
            return result;
        }

        IEnumerator InitInfos()
        {
            FirstRecordMsg.Init();

            gameObject.AddComponent<FirstPlatform>();

            string path = FirstUtil.GetStreamingAssetsPathNoTarget() + "version.txt";
            WWW www = new WWW(path);
            yield return www;
            localVersion = getWWWTxt(www,false);
            www = null;

            path = FirstUtil.GetStreamingAssetsPathNoTarget() + "gameDefine.txt";
            www = new WWW(path);
            yield return www;
            localGameDefine = getWWWTxt(www,false);
            www = null;

            path = FirstUtil.GetStreamingAssetsPathNoTarget() + "filelist.txt";
            www = new WWW(path);
            yield return www;
            loccalFileList = getWWWTxt(www,false);
            www = null;

            InitLocalInfo();

            InitOutDir();

            path = GetPathNoTarget("version.txt");
            www = new WWW(path);
            yield return www;
            outLocalVersion = getWWWTxt(www);
            www = null;

            path = GetPathNoTarget("gameDefine.txt");
            www = new WWW(path);
            yield return www;
            outLocalGameDefine = getWWWTxt(www);
            www = null;

            path = GetPathNoTarget("filelist.txt");
            www = new WWW(path);
            yield return www;
            outloccalFileList = getWWWTxt(www,false);
            www = null;

            path = GetPathNoTarget("FirstLang.txt");
            www = new WWW(path);
            yield return www;
            InitLang(getWWWTxt(www,false));
            www = null;

            WriteLog("InitFilstInfo");

            InitData();

            StartCoroutine(CheckUpdate());

            FirstResource.InitAppFileList(loccalFileList);
#if UNITY_EDITOR
            FirstResource.InitSdFileList("");
#else
            FirstResource.InitSdFileList(outloccalFileList);
#endif

            localVersion = null;
            outLocalVersion = null;
            localGameDefine =null;
            outLocalGameDefine = null;
            loccalFileList = null;
            outloccalFileList = null;

            StartCoroutine(WorkerUI());     
        }

        private void InitOutDir()
        {
#if UNITY_IOS
            Device.SetNoBackupFlag(Application.persistentDataPath);
#endif
            if (Directory.Exists(FirstUtil.OutDir)) // 存在out目录
            {
                // 检查packversion

                string packVersionPath = FirstUtil.OutDir + "packversion.txt";
                if (!File.Exists(packVersionPath))
                {
                    WriteOutText("packversion.txt", FirstUtil.PackVersion); //兼容老版本没有packversion
                }

                string outPackVersion = File.ReadAllLines(packVersionPath)[0];
                if (outPackVersion == FirstUtil.PackVersion)
                {
                    WriteLog("PackVersion match ok");
                    return;
                }

                DeleteDir(FirstUtil.OutDir);
            }

            // 重建out目录
            try
            {
                if (Directory.Exists(FirstUtil.OutDir)==false)
                {
                    Directory.CreateDirectory(FirstUtil.OutDir);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                FirstRecordMsg.Error(e.StackTrace,MsgType.HandleFileException);

                DeleteDir(FirstUtil.OutDir);

                if (Directory.Exists(FirstUtil.OutDir) == false)
                {
                    Directory.CreateDirectory(FirstUtil.OutDir);
                }
            }

#if UNITY_IOS
            Device.SetNoBackupFlag(Path.Combine(Application.persistentDataPath, "out"));
#endif

            WriteOutText("filelist.txt", loccalFileList);
            WriteOutText("version.txt", localVersion);
            WriteOutText("packversion.txt", FirstUtil.PackVersion);
            WriteLog("create out dir {0}", FirstUtil.PackVersion);

            _isNewApk = true;
        }

        //gamedefine 可能会被热更，但是有些变量值是永远不变，所以取包里的值
        private void InitLocalInfo()
        {
            string[] arr = FirstUtil.ReadAllLines(localGameDefine);
            foreach (string line in arr)
            {
                string[] row = line.Split('=');
                if (row.Length < 2)
                    continue;
                string value = row[1].Trim();
                switch (row[0].Trim())
                {
                    case "SDKName":
                        _sdkName = value;
                        FirstUtil.SDKName = value;
                        break;
                    case "id":
                        FirstUtil.ApkId = value;
                        break;
                    case "platform":
#if UNITY_ANDROID|| UNITY_EDITOR
                        FirstUtil.platform= "Android";
#elif UNITY_IOS
       FirstUtil.platform =  "IOS";
#elif UNITY_STANDALONE
       FirstUtil.platform =  "PC";
#elif UNITY_WEBPLAYER
       FirstUtil.platform =  "WEB";
#endif
                        break;
                    case "update_url":
                        _updateUrl = value;
                        break;
                    case "isShowLog":
                        FirstUtil.IsShowLog = (value == "FYMM");
                        break;
                }
            }

            arr = FirstUtil.ReadAllLines(localVersion);
            foreach (string line in arr)
            {
                string[] row = line.Split('=');
                if (row.Length < 2)
                    continue;
                string value = row[1].Trim();
                switch (row[0].Trim())
                {
                    case "version":
                        FirstUtil.PackVersion = value;
                        break;
                }
            }
        }

        private void InitData()
        {
            string[] arr = FirstUtil.ReadAllLines(outLocalVersion);
            foreach (string line in arr)
            {
                string[] row = line.Split('=');
                if (row.Length < 2)
                    continue;
                string value = row[1].Trim();
                switch (row[0].Trim())
                {
                    case "version":
                        FirstUtil.OutVersion = value;
                        break;
                }
            }

            arr = FirstUtil.ReadAllLines(outLocalGameDefine);
            foreach (string line in arr)
            {
                string[] row = line.Split('=');
                if (row.Length < 2)
                    continue;
                string value = row[1].Trim();
                switch (row[0].Trim())
                {
                    case "update_url":
#if !UNITY_EDITOR
                        _updateUrl = value;
#endif
                        break;
                    case "isAc":
                        _isActiveSDK = value != "FYMM";
                        break;
                    case "isShowLog":
                        FirstUtil.IsShowLog = (value == "FYMM");
                        break;
                    case "isPlayMovie":
                        _isPlayMovie = int.Parse(value);
                        break;
                }
            }

            if (_isNewApk && _isPlayMovie == 1)
                PlayMovie();
        }

        private void PlayMovie()
        {
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER
            Handheld.PlayFullScreenMovie("Movie/Open.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
#endif
        }

        void InitLang(string m_Data)
        {
            if (m_Data == null)
            {
                return;
            }
            FirstLang lang = new FirstLang();
            Type type = lang.GetType();
            FieldInfo[] fields = type.GetFields();
            char[] separator = new char[] { '\r', '\n' };
            char[] chArray2 = new char[] { '=' };
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] arrs = m_Data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int len = arrs.Length;
            for (int i = 0; i < len; i++)
            {
                string[] strArray3 = arrs[i].Split(chArray2, StringSplitOptions.RemoveEmptyEntries);

                if (strArray3.Length == 2)
                {
                    dic.Add(strArray3[0], strArray3[1]);
                }
            }

            len = fields.Length;
            for (int i = 0; i < len; i++)
            {
                FieldInfo fi = fields[i];
                string fName = fi.Name;
                if (dic.ContainsKey(fName))
                    fi.SetValue(this, dic[fName]);

            }
            dic.Clear();
            m_Data = null;
        }

        void AddEventSystom()
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(eventSystem);
        }

        private AssetBundle abLoading;

        private void InitUi()
        {
            if(abLoading!=null)
            {
                GameObject prefab = abLoading.LoadAsset("LoadingView") as GameObject;
                LoadingUI = Instantiate(prefab);
                Scene scene = SceneManager.GetActiveScene();
                GameObject[] gos = scene.GetRootGameObjects();
                Transform parent = null;
                Camera camera = null; ;
                for (int i = 0; i < gos.Length; i++)
                {
                    if (gos[i].name == "LoadingPart")
                    {
                        parent = gos[i].transform;
                    }
                    if (gos[i].name == "MainCamera" || gos[i].name == "Main Camera")
                    {
                        camera = gos[i].GetComponent<Camera>();
                    }
                }
                Canvas can = LoadingUI.GetComponent<Canvas>();
                can.worldCamera = camera;
                LoadingUI.transform.SetParent(parent);
                LoadingUI.transform.localPosition = Vector3.zero;
                LoadingUI.transform.localRotation = Quaternion.Euler(Vector3.zero);
                LoadingUI.transform.localScale = Vector3.one;
                LoadingUI.name = "Load";
                FirstUtil.RefreshShaderInEditor(LoadingUI);
                ChangeLayerAll(LoadingUI, gameObject.layer);
                load = LoadingUI.AddComponent<LoadingView>();
            }    
        }

        void ChangeLayerAll(GameObject rootObj, int layer)
        {
            Transform[] trans = rootObj.GetComponentsInChildren<Transform>(true);
            foreach (Transform tran in trans)
            {
                tran.gameObject.layer = layer;
            }
        }

        private int sign = 0;
        private int sign1 = 0;

        void Update()
        {
            if (_updateOk)
            {
                _updateOk = false;
                StartCoroutine(LoadScenes());
            }

            if (_updateOk == false && loadingDic.Count == 0 && waitingList.Count > 0)
            {
                sign++;
                if (sign > 10)
                {
                    sign = 0;
                    if (currLoadingNumWWW >= LoadMaxNumWWW)
                        currLoadingNumWWW = 0;

                    LoadWaiting();
                }
            }

            if (_updateOk == false && loadingDic.Count > 0)
            {
                sign1++;
                if (sign1 > 5)
                {
                    sign1 = 0;
                    int temp = 0;
                    foreach (var wwwDown in loadingDic)
                    {
                        if (_fielSizeDic.ContainsKey(wwwDown.Key) && wwwDown.Value != null)
                        {
                            int filesize = _fielSizeDic[wwwDown.Key];
                            temp += Mathf.FloorToInt(wwwDown.Value.progress * filesize);
                        }
                    }
                    ProcessPercent = startValue + leftValue * currSize / allSize;
                }
            }
        }

        IEnumerator CheckUpdate()
        {
            IsStopUpdate = false;
            WriteLog("CheckUpdate" + _isActiveSDK);
            if (_isActiveSDK)
                FirstPlatform.CheckUpdate(_sdkName);
            yield break;
        }

        void InitCheck(string txt)
        {
            string[] allLines = FirstUtil.ReadAllLines(txt);
            foreach (string line in allLines)
            {
                string[] row = line.Split(';');
                if (row.Length < 2)
                {
                    if (line.Contains("updateCustomServer"))
                    {
                        string temp = line.Replace("updateCustomServer,", "");
                        var arr = temp.Split(',');
                        if (arr.Length > 1)
                        {
                            FirstUtil.NeedToUpdateServerList = new List<int>();
                            for (int i = 1; i < arr.Length; i++)
                            {
                                FirstUtil.NeedToUpdateServerList.Add(int.Parse(arr[i]));
                            }
                            int selectServerId = PlayerPrefs.GetInt(FirstUtil.SelectServerKey, -1);

                            if (FirstUtil.NeedToUpdateServerList.Contains(selectServerId))
                            {
                                _updateUrl = arr[0]; //替换掉更新url
                                FirstUtil.IsUrlChange = true;
                            }
                        }
                    }
                    continue;
                }

                if (row.Length >= 2)
                    FirstResource.UpdateUrlDic[row[0]] = row[1];
                if (row.Length >= 3)
                    FirstResource.ServerListDic[row[0]] = row[2];
            }
        }

        IEnumerator WorkerUI()
        {
            yield return BeginDownLoad(GetPath("lib/ttffont/gamefont.ttf"));
            FirstUtil.FontAb = EndLoadAssetBundle();

            yield return BeginDownLoad(GetPath("firstui/loadingview"));
            abLoading = EndLoadAssetBundle();

            AddEventSystom();

            InitUi();

            abLoading.Unload(false);
            abLoading = null;

            StartCoroutine(WorkerFunction());
        }

        IEnumerator WorkerFunction()
        {

            bool test = false;
#if UNITY_EDITOR
            if (test)
            {
                if (load != null)
                {
                    load.ShowAlert(123.45, OnConfirm, OnCancel);
                }
                yield break;
            }
            else
            {
                ProcessPercent = 100;
                _updateOk = true;
                Debug.Log("下载完成");
                yield break;
            }
#endif
            if (string.IsNullOrEmpty(_updateUrl))
            {
                _updateOk = true;
                yield break;
            }

            yield return BeginDownLoad(_updateUrl + "check.txt?id=" + (DateTime.Now.ToFileTime() / 10),2);
            string buf2 = EndDownLoadTxt();
            if (string.IsNullOrEmpty(buf2) == false)
            {
                InitCheck(buf2);
                FirstResource.ResourceKey = FirstUtil.ApkId + "_" + FirstUtil.PackVersion;
                if (FirstResource.UpdateUrlDic.ContainsKey(FirstResource.ResourceKey))
                {
                    _updateUrl = FirstResource.UpdateUrlDic[FirstResource.ResourceKey];
                }
            }

            FirstUtil.UpdateUrl = _updateUrl;

            // DateTime.ToFileTime()以100毫秒为单位。此处只取秒数。
            // 用户获取此文件最新版本精确到1秒。cdn服务器理论上每小时最大产生3600个文件缓存。
            yield return BeginDownLoad(_updateUrl + "SecondVersion.txt?id=" + (DateTime.Now.ToFileTime() / 10),2);
            _secondVersion = EndDownLoadTxt();
            yield return BeginDownLoad(_updateUrl + "version.txt?id=" + (DateTime.Now.ToFileTime() / 10),  2);
            string buf = EndDownLoadTxt();
            if (string.IsNullOrEmpty(buf))
            {
                _infoTips = FirstLang.FIRST_MSG_4;
                _updateOk = true;
                yield break;
            }

            if (_sdkName.Contains("IOS"))
            {
                yield return BeginDownLoad(_updateUrl + _sdkName + "_apkversion.txt?id=" + (DateTime.Now.ToFileTime() / 10),2);
                string tempVerStr = EndDownLoadTxt();

                if (string.IsNullOrEmpty(tempVerStr) == false)
                {
                    _serverSdkVersion = FirstUtil.ReadAllLines(tempVerStr)[0];
                    int compareResult = String.Compare(_serverSdkVersion, FirstUtil.PackVersion, StringComparison.Ordinal);  // 直接字符串比较，和运营保持规则一致
                    WriteLog("CheckAPKVersion - _localVersion:{0} _serverVersion:{1} Compare:{2}", FirstUtil.PackVersion, _serverSdkVersion, compareResult);
                    if (compareResult > 0)
                    {
                        yield return BeginDownLoad(_updateUrl + _sdkName + "_apkurl.txt?id=" + (DateTime.Now.ToFileTime() / 10),2);
                        tempVerStr = EndDownLoadTxt();
                        if (string.IsNullOrEmpty(tempVerStr) == false)
                        {
                            FirstUtil.ApkUrl = FirstUtil.ReadAllLines(tempVerStr)[0];
                            FirstPlatform.ShowAlert(FirstLang.FIRST_MSG_15, FirstLang.FIRST_MSG_16, FirstLang.FIRST_MSG_18, FirstLang.FIRST_MSG_17, "1");
                            _infoTips = FirstLang.FIRST_MSG_19;
                            ProcessPercent = 100;
                            //apk需要更新
                            yield break;
                        }
                    }
                }
            }

            string svrVer = FirstUtil.ReadAllLines(buf)[0];
            svrVer = svrVer.Trim();
            FirstRecordMsg.game_version = svrVer;

            string myVer = FirstUtil.OutVersion ?? FirstUtil.PackVersion;
            int cmp = String.Compare(svrVer, myVer, StringComparison.Ordinal);  // 直接字符串比较，和运营保持规则一致
            WriteLog("CheckVer - MyVer:{0} ServerVer:{1} Compare:{2}", myVer, svrVer, cmp);
            if (cmp == 0 || svrVer == "0") //版本号不等需要更新
            {
                ProcessPercent = 100;
                _updateOk = true;
                yield break;
            }

            foreach (object o in DownloadUpdate(svrVer))
            {
                if (o == null)
                    yield break;
                yield return o;
            }
        }

        void RemoveDir(string path)
        {
#if !UNITY_WEBPLAYER
            try
            {
                
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                FirstRecordMsg.Error(e.StackTrace,MsgType.HandleFileException);
            }
#endif
        }

        //删除目录下的所有内容，保留根目录
        bool DeleteDir(string path)
        {
            Debug.Log("delete:" + path);
            try
            {
                if (Directory.Exists(path))
                {
                    string[] dirs = Directory.GetDirectories(path);
                    string[] files = Directory.GetFiles(path);
                    int len;
                    len = files.Length;
                    for (int i = 0; i < len; i++)
                    {
                        File.Delete(files[i]);
                    }
                    len = dirs.Length;
                    for (int i = 0; i < len; i++)
                    {
                        string dir = dirs[i];
                        if (Directory.Exists(dir))
                            Directory.Delete(dir, true);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                FirstRecordMsg.Error(e.StackTrace, MsgType.HandleFileException);
            }

            return false;
        }

        Dictionary<string, int> _fielSizeDic = new Dictionary<string, int>();
        private int allSize = 0;
        private int currSize = 0;
        private int startValue = 0;
        private int leftValue = 0;

        IEnumerable<object> DownloadUpdate(string svrVer)
        {
            loadRootPath = _updateUrl + svrVer + "/";

            FirstUtil.OutVersion = svrVer;
            newVersion = svrVer;
            FirstUtil.newVersion = svrVer;

            yield return BeginDownLoad(loadRootPath + "filelist.txt?id=" + (DateTime.Now.ToFileTime() / 10), 5);

            string txt = EndDownLoadTxt();
            if (string.IsNullOrEmpty(txt))
            {
                _infoTips = FirstLang.FIRST_MSG_8;
                yield return null;
                yield break;
            }

            oldFileDic = FirstResource.SdFileDic;
            FirstResource.InitSdFileList(txt);

            yield return BeginDownLoad(loadRootPath +FirstUtil.platform+ "/filesize.assetbundle",  2);
            txt = EndDownLoadTxt("filesize");
            if (string.IsNullOrEmpty(txt))
            {
                _infoTips = FirstLang.FIRST_MSG_8;
                yield return null;
                yield break;
            }

            parseSizeFile(txt);
            txt = null;
            foreach (var pair in FirstResource.SdFileDic)
            {
                string oldVersion;
                if (!oldFileDic.TryGetValue(pair.Key, out oldVersion) || oldVersion != pair.Value)
                {
                    downList.Add(pair.Key);
                    if (_fielSizeDic.ContainsKey(pair.Key))
                    {
                        allSize += _fielSizeDic[pair.Key];
                    }
                }
            }
            allDown = downList.Count;
            WriteLog("need download: " + allDown);

            if (allDown > 0)
            {
                startValue = ProcessPercent;
                leftValue = 100 - startValue;
                ProcessPercent = startValue + leftValue * currSize / allSize;
            }

            if (allDown == 0)
                LoadComplete();
            else
            {
                if (allSize > 1024 && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    double size = Math.Round(1.0f*allSize/1024, 2);
                    if (load != null)
                    {
                        load.ShowAlert(size, OnConfirm, OnCancel);
                    }
                }
                else
                {
                    LoadMultiAsset(downList);
                }
            }
        }

        private void OnConfirm()
        {
#if UNITY_EDITOR
            ProcessPercent = 100;
            _updateOk = true;
            Debug.Log("下载完成");
#else
            LoadMultiAsset(downList);
#endif
        }

        private void OnCancel()
        {
            FirstUtil.ExitApp();
        }

        void parseSizeFile(string text)
        {
            string[] arr = FirstUtil.ReadAllLines(text);
            foreach (string line in arr)
            {
                string[] row = line.Split('\t');
                if (row.Length == 2)
                {
                    _fielSizeDic.Add(row[0], int.Parse(row[1]));
                }
                else if (row.Length == 3)
                {
                    _fielSizeDic.Add(row[0], int.Parse(row[1]));
                }
            }
        }

#region  handler Update

        private Dictionary<string, string> oldFileDic;
        List<string> downList = new List<string>();
        private int allDown;
        private int currIndex;
        private string loadRootPath;
        private const int LoadMaxNumWWW = 5;
        private int currLoadingNumWWW = 0;
        private List<string> waitingList = new List<string>();
        private Dictionary<string, int> failDic = new Dictionary<string, int>();
        private bool IsLoadFail = false;
        private string newVersion;
        private Dictionary<string, WWW> loadingDic = new Dictionary<string, WWW>();

        void LoadMultiAsset(List<string> paths)
        {
            foreach (var path in paths)
            {
                LoadAsset(path);
            }
        }

        void LoadAsset(string path)
        {
            if (currLoadingNumWWW < LoadMaxNumWWW)
            {
                currLoadingNumWWW++;
                StartCoroutine(LoadAssetReal(path));
            }
            else
            {
                waitingList.Add(path);
            }
        }

        IEnumerator LoadAssetReal(string path)
        {
            if (IsLoadFail)
                yield break;

            if (IsStopUpdate)
            {
                _infoTips = FirstLang.FIRST_MSG_10;
                yield break;
            }

            string fullPath;
            string needCRC = FirstResource.SdFileDic[path];
            string tmpPath = Path.Combine(OutTmpPath, path);
            byte[] buf = null;

#if !UNITY_WEBPLAYER
            if (File.Exists(tmpPath))   // 已经下载过
            {
                buf = File.ReadAllBytes(tmpPath);
                if (FirstUtil.CRC32(buf).ToString("X") == needCRC)   // crc一致
                {
                    //WriteLog("download pass by: {0}", path);

                    HandlerLoadEnd(path);

                    yield break;
                }
            }
#endif

            WriteLog("download file:" + path);
            if (failDic.ContainsKey(path))
            {
                fullPath = loadRootPath + path + "?id=" + (DateTime.Now.ToFileTime() / 10);
            }
            else
            {
                fullPath = loadRootPath + path + "?id=" + needCRC;
            }

            WWW www = new WWW(fullPath);
            loadingDic[path] = www;
            yield return www;

            RemoveLoading(path);

            if (!string.IsNullOrEmpty(www.error) || !www.isDone)
            {
                string temp = string.Format("{0} download error: {1}", www.url, www.error);
                WriteLog(temp);

                FirstRecordMsg.Error(temp,MsgType.DownloadError);
            }
            else
            {
                buf = www.bytes;
                //WriteLog("{0} download ok [{1}]", www.url, buf.Length);
            }

            www.Dispose();
            www = null;

            if (buf == null)
            {
                HandlerLoadEnd(path, 1);

                yield break;
            }

            string crc = FirstUtil.CRC32(buf).ToString("X");
            if (crc == needCRC|| path.Contains("Android/Android"))
            {
                if (WriteTmpFile(path, buf))
                {
                    HandlerLoadEnd(path);
                }
                else
                {
                    HandlerLoadEnd(path,3);
                }
            }
            else
            {
                string temp = string.Format("{0} crc error! down:{1}  need:{2}", path, crc, needCRC);
                WriteLog(temp);
                FirstRecordMsg.Error(temp,MsgType.CrcError);

                HandlerLoadEnd(path, 2);
                yield break;
            }
        }

        void HandlerLoadEnd(string path, int type = 0)
        {
            currLoadingNumWWW--;

            RemoveLoading(path);

            if (type == 0)
            {
                currIndex++;

                if (failDic.ContainsKey(path))
                    failDic.Remove(path);

                if (_fielSizeDic.ContainsKey(path))
                {
                    currSize += _fielSizeDic[path];
                    if (!IsLoadFail)
                    {
                        _infoTips = string.Format("正在更新...{0}MB/{1}MB", Math.Round(1.0f*currSize/1024,2), Math.Round(1.0f * allSize / 1024, 2));
                    }
                }
            }
            else
            {
                if (failDic.ContainsKey(path))
                {
                    failDic[path]++;
                    if (failDic[path] > 3)
                    {
                        if (type == 1)
                            _infoTips = FirstLang.FIRST_MSG_12 + path;
                        else if (type == 2)
                            _infoTips = FirstLang.FIRST_MSG_13 + path;
                        else if (type == 3)
                            _infoTips = FirstLang.FIRST_MSG_20 + path;

                        IsLoadFail = true;
                    }
                }
                else
                {
                    failDic.Add(path, 1);
                }

                if (waitingList.Contains(path) == false)
                    waitingList.Insert(0, path);
            }

            if (currIndex >= allDown)
            {
                LoadComplete();
            }
            else
            {
                LoadWaiting();
            }
        }

        void RemoveLoading(string path)
        {
            if (loadingDic.ContainsKey(path))
                loadingDic.Remove(path);
        }

        void LoadWaiting()
        {
            if (waitingList.Count > 0 && currLoadingNumWWW < LoadMaxNumWWW)
            {
                string item = waitingList[0];
                waitingList.RemoveAt(0);
                LoadAsset(item);
            }
        }

        void LoadComplete()
        {
            // 以下几个函数真正更新out目录（假定io操作期间不会异常、退出，或异常后部分文件更新不会引起下次更新失败）
            CopyOutTmpFile(downList);
            WriteFileList();
            WriteOutText("version.txt", string.Format("version={0}{1}SecondVersion={2}", newVersion, "\r\n", _secondVersion));
            CheckDeleteFile();
            RemoveDir(OutTmpPath);

            downList.Clear();
            _fielSizeDic.Clear();
            WriteLog("download all done.");
            ProcessPercent = 100;
            _updateOk = true;
        }

#endregion

        private IEnumerator LoadScenes()
        {
#if !UNITY_IOS
            string path = GetPathNoTarget("Managed/manager.dll");
            yield return BeginDownLoad(path,0);
            byte[] asset = EndDownLoadDll();
#endif

            Resources.UnloadUnusedAssets();

            try
            {
#if !UNITY_IOS
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(asset);
                System.Type script = assembly.GetType("MainControl");
                WriteLog("Load Game Assembly-CSharp Complete...");
                gameObject.AddComponent(script);
                WriteLog("开始切换场景");
                GameObject.Destroy(load);

#else
               GameObject go  = GameObject.Find("MainController");
	           go.AddComponent<MainControl>();
#endif
            }
            catch (Exception e)
            {
                FirstRecordMsg.Error(e.StackTrace,MsgType.ReflectionDll);
            }
            yield break;
        }

#region  handler File Action

        private static void CopyOutTmpFile(List<string> fileList)
        {
            foreach (string path in fileList)
            {
                FirstUtil.CreateFileDirectory(FirstUtil.OutDir + path);
                File.Copy(OutTmpPath + path, FirstUtil.OutDir + path, true);
            }
        }

        void CheckDeleteFile()
        {
#if !UNITY_WEBPLAYER
            foreach (string path in oldFileDic.Keys)
            {
                if (FirstResource.SdFileDic.ContainsKey(path))
                    continue;
                try
                {
                    WriteLog("delete File :" + path);
                    string tempPath = FirstUtil.OutDir + path;
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
#endif
        }

        void WriteFileList()
        {
#if !UNITY_WEBPLAYER

            try
            {
                var listFile = File.CreateText(FirstUtil.OutDir + "filelist.txt");
                foreach (var pair in FirstResource.SdFileDic)
                    listFile.WriteLine("{0}\t{1}", pair.Key, pair.Value);
                listFile.Close();
            }
            catch (Exception e)
            {
                FirstRecordMsg.Error(e.StackTrace, MsgType.WriteFileError);
            }
#endif
        }

        void WriteOutText(string path, string text)
        {
#if !UNITY_WEBPLAYER
            try
            {
                File.WriteAllBytes(FirstUtil.OutDir + path, Encoding.ASCII.GetBytes(text));
                WriteLog("WriteOutText: {0} ok!", path);
            }
            catch (Exception e)
            {
                FirstRecordMsg.Error(e.StackTrace, MsgType.WriteFileError);
            }
#endif
        }

        bool WriteTmpFile(string path, byte[] buf)
        {
#if !UNITY_WEBPLAYER
            try
            {
                string tmpPath = OutTmpPath + path;
                FirstUtil.CreateFileDirectory(tmpPath);
                File.WriteAllBytes(tmpPath, buf);
                WriteLog("WriteTmpFile: {0} ok!", path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                FirstRecordMsg.Error("Write Error:"+path+":::Stack:"+e.StackTrace,MsgType.WriteFileError);
            }
            return false;
#endif
        }

#endregion

        WWW BeginDownLoad(string path, int percentAdd = 0)
        {
            WriteLog("download file:" + path);
            _percentAdd = percentAdd;
            return _wwwDown = new WWW(path);
        }

        byte[] EndDownLoad()
        {
            byte[] buf = null;
            if (_wwwDown.isDone == false || string.IsNullOrEmpty(_wwwDown.error) == false)
            {
                string temp = string.Format("{0} download error: {1}", _wwwDown.url, _wwwDown.error);
                WriteLog(temp);

                FirstRecordMsg.Error(temp,MsgType.DownloadError);
            }
            else
            {
                buf = _wwwDown.bytes;
                WriteLog("{0} download ok [{1}]", _wwwDown.url, buf.Length);
            }
            ProcessPercent += _percentAdd;
            _percentAdd = 0;

            _wwwDown.Dispose();
            _wwwDown = null;
            return buf;
        }

        string EndDownLoadTxt(string fileName = "")
        {
            string s = "";
            if (_wwwDown.isDone==false || string.IsNullOrEmpty(_wwwDown.error)==false)
            {
                string temp = string.Format("{0} download error: {1}", _wwwDown.url, _wwwDown.error);

                WriteLog(temp, _wwwDown.url, _wwwDown.error);

                FirstRecordMsg.Error(temp,MsgType.DownloadError);
            }
            else
            {
                WriteLog("{0} download ok [{1}]", _wwwDown.url, _wwwDown.bytesDownloaded);

                if (_wwwDown.assetBundle != null)
                {
                    s = string.Copy((_wwwDown.assetBundle.LoadAsset(fileName) as TextAsset).text);
                    _wwwDown.assetBundle.Unload(false);
                }
                else
                    s = _wwwDown.text;
            }
            ProcessPercent += _percentAdd;
            _percentAdd = 0;

            _wwwDown.Dispose();
            _wwwDown = null;
            return s;
        }

        AssetBundle EndLoadAssetBundle()
        {
            AssetBundle ab = null;
            if (!string.IsNullOrEmpty(_wwwDown.error) || _wwwDown.isDone==false)
            {
                string temp = string.Format("WWW Download Error:{0} Error Msg:{1}", _wwwDown.url, _wwwDown.error);

                WriteLog(temp);

                FirstRecordMsg.Error(temp,MsgType.DownloadError);
            }
            else if (_wwwDown.assetBundle == null)
            {
                string temp = string.Format("WWW Download Exception:{0} The File Size:{1}", _wwwDown.url, _wwwDown.bytesDownloaded);
                WriteLog(temp);
                FirstRecordMsg.Error(temp,MsgType.DownloadError);
            }
            else
            {
                WriteLog("WWW Download Ok:{0} The File Size:{1}", _wwwDown.url, _wwwDown.bytesDownloaded);
                ab = _wwwDown.assetBundle;
            }

            _wwwDown.Dispose();
            _wwwDown = null;

            return ab;
        }

        byte[] EndDownLoadDll()
        {
            byte[] buf = null;
            if (_wwwDown.isDone == false || string.IsNullOrEmpty(_wwwDown.error) == false)
            {
                string temp = string.Format("{0} download error: {1}", _wwwDown.url, _wwwDown.error);
                WriteLog(temp);

                FirstRecordMsg.Error(temp,MsgType.DownloadError);
            }
            else
            {
                byte[] bytes = _wwwDown.bytes;
                _MaskData(bytes);
                AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);

                Object o = assetBundle.LoadAsset("manager.dll.bytes");
                buf = (o as TextAsset).bytes;

                assetBundle.Unload(false);
                assetBundle = null;

                WriteLog("{0} download ok [{1}]", _wwwDown.url, buf.Length);
            }

            _wwwDown.Dispose();
            _wwwDown = null;
            return buf;
        }

        void _MaskData(byte[] data)
        {
            uint mask = BitConverter.ToUInt32(data, 0);
            for (int i = 4; i < data.Length; i++)
            {
                int m = i % 4;
                if (m == 0)
                    mask = mask * 1103515245 + 12345;
                data[i] ^= (byte)(mask >> (m * 8));
            }
        }

        void WriteLog(string msg, params object[] args)
        {
            FirstUtil.WriteLog(msg, args);
        }

        static string OutTmpPath { get { return Path.Combine(Application.persistentDataPath, "out_tmp/"); } }
        public static bool IsStopUpdate { get; set; }

    }
}