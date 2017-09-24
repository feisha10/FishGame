using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace UpdateWork
{
    public class FirstUtil
    {
        public static string SDKName = string.Empty;
        public static string ApkUrl = string.Empty;
        public static string ApkId = string.Empty;
        public static int agentID = 0;
        public static string platform;
        public static string newVersion;

        public const string SelectServerKey = "SelectServerKey";
        public static List<int> NeedToUpdateServerList;
        public static bool IsUrlChange = false;
        public static bool IsShowLog = true;
        public static string UpdateUrl;
        public static AssetBundle FontAb;

        private static uint[] CRC32Table = MakeCRC32Table();

        private static uint[] MakeCRC32Table()
        {
            uint[] table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint vCRC = i;
                for (int j = 0; j < 8; j++)
                {
                    if (vCRC % 2 == 0)
                        vCRC = vCRC >> 1;
                    else
                        vCRC = (vCRC >> 1) ^ 0xEDB88320;
                }
                table[i] = vCRC;
            }
            return table;
        }

        public static uint CRC32(byte[] bytes)
        {
            uint result = 0xFFFFFFFF;
            foreach (byte vByte in bytes)
                result = CRC32Table[(result & 0xff) ^ vByte] ^ (result >> 8);
            return ~result;
        }

        public static void WriteLog(string msg, params object[] args)
        {
            if (IsShowLog == false)
                return;

            if (args.Length > 0)
                msg = string.Format(msg, args);
            Debug.Log(msg);
        }

        public static string[] ReadAllLines(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return new string[0];
            return txt.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        }

        private static string _streamingAssetPath;

        public static string StreamingAssetsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_streamingAssetPath))
                {
#if UNITY_EDITOR
                    _streamingAssetPath = "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_IOS
                _streamingAssetPath = "file://" + Application.dataPath + "/Raw/";          
#elif UNITY_ANDROID
                    _streamingAssetPath = "jar:file://" + Application.dataPath + "!/assets/";
#else
                    _streamingAssetPath = "file://" + Application.dataPath + "/StreamingAssets/";
#endif
                    if (string.IsNullOrEmpty(platform) == false)
                    {
                        _streamingAssetPath += platform;
                        _streamingAssetPath += "/";
                    }
                }
                return _streamingAssetPath;
            }
        }

        private static string _streamingAssetPathNoTarget;

        public static string GetStreamingAssetsPathNoTarget()
        {
            if (string.IsNullOrEmpty(_streamingAssetPathNoTarget) == false)
                return _streamingAssetPathNoTarget;
            string s;
#if UNITY_EDITOR
            s = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
#elif UNITY_IOS  
        s = string.Format("file://{0}/Raw/", Application.dataPath);
#elif UNITY_ANDROID
            s = string.Format("jar:file://{0}!/assets/", Application.dataPath);
#else
            s = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
#endif
            _streamingAssetPathNoTarget = s;
            return _streamingAssetPathNoTarget;
        }

        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return;
            CreateDirectory(Directory.GetParent(path).FullName);
            Directory.CreateDirectory(path);

#if UNITY_IOS
        Device.SetNoBackupFlag(path);
#endif
        }

        public static void CreateFileDirectory(string path)
        {
            CreateDirectory(Path.GetDirectoryName(path));
        }

        public static void ExitApp()
        {
#if UNITY_STANDALONE
        Application.Quit();
#elif UNITY_WEBPLAYER
        Application.ExternalEval("document.location.reload(true)");
#else
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception e)
            {
                Application.Quit();
            }
#endif
        }

        // 安装包自身版本
        public static string PackVersion { get; set; }
        // 当前包外版本
        public static string OutVersion { get; set; }

        public static string OutDir
        {
            get
            {
                return Path.Combine(Application.persistentDataPath, "out/");
            }
        }

        public static void RefreshShaderInEditor(GameObject go)
        {
#if UNITY_EDITOR
            if (go != null)
            {
                var renders = go.GetComponentsInChildren<Image>(true);
                for (int i = 0; i < renders.Length; i++)
                {
                    var mat = renders[i].material;
                    if (mat != null)
                    {
                        mat.shader = Shader.Find(mat.shader.name);
                    }
                }
            }
#endif
        }
        public static void RefreshRenderInEditor(GameObject go)
        {
#if UNITY_EDITOR

            if (go != null)
            {
                var renders = go.GetComponentsInChildren<Renderer>(true);

                for (int i = 0; i < renders.Length; i++)
                {
                    var mats = renders[i].sharedMaterials;

                    for (int j = 0; j < mats.Length; j++)
                    {
                        var mat = mats[j];

                        if (mat != null)
                        {
                            mat.shader = Shader.Find(mat.shader.name);
                        }
                    }
                }
            }
#endif
        }
    }


    }
