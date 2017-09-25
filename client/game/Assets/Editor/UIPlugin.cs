using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class UIPlugin
{

    [MenuItem("UI Plugin/打开PersistentDataPath目录", false, 3)]
    public static void OpenPersistentDataPath()
    {
        if(Application.platform ==RuntimePlatform.OSXEditor)
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }
        else if(Application.platform ==RuntimePlatform.WindowsEditor)
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }

    [MenuItem("UI Plugin/自动生成图集枚举")]
    public static void CreateAtlasName()
    {
        string classPath = "Assets/Scripts/CommonManager/AtlasName.cs";
        if (File.Exists(classPath))
        {
            string pathPre = Application.dataPath + "/ResourcesLib/Atlas/";
            var files = Directory.GetFiles(pathPre, "*prefab", SearchOption.AllDirectories);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < files.Length; i++)
            {
                string filename = Path.GetFileNameWithoutExtension(files[i]);
                string[] arr = filename.Split('_');
                string atlasName = arr[0].ToLower();
                string s = string.Format("    public static string {0} = \"{1}\";", arr[0], atlasName);
                sb.AppendLine(s);
            }
            InsertStringToClass(classPath, sb.ToString());
        }
        Debug.Log("自动生成图集枚举成功");
    }

   // [MenuItem("UI Plugin/CreateCreatRoleHeadDic")]
    public static void CreateCreatRoleHeadDic()
    {
        string classPath = "Assets/Scripts/CommonManager/RoleHeadDic.cs";
        if (File.Exists(classPath))
        {
            Dictionary<string,string> dic = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                string atlasName = "CreatRoleHead";
                if(i!=0)
                {
                    atlasName = atlasName + i;
                }
                string pathPre = Application.dataPath + "/ResourcesLib/AtlasResource/" + atlasName+"/";
                if(Directory.Exists(pathPre))
                {
                    var files = Directory.GetFiles(pathPre, "*png", SearchOption.AllDirectories);

                    for (int j = 0; j < files.Length; j++)
                    {
                        string filename = Path.GetFileNameWithoutExtension(files[j]);
                        if (string.IsNullOrEmpty(filename)==false)
                        {
                            if (dic.ContainsKey(filename))
                            {
                                Debug.LogError("AtlasName:" + atlasName + " filename:" + filename+" repeat");
                            }
                            else
                            {
                                dic.Add(filename, atlasName);
                            }
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("        Dic = new Dictionary<string, string>({0});", dic.Count));
            foreach (var item in dic)
            {
                sb.AppendLine(string.Format("        Dic[\"{0}\"] = \"{1}\";", item.Key, item.Value.ToLower()));
            }

            InsertStringToClass(classPath, sb.ToString());
        }
        Debug.Log("自动生成图集枚举成功");
    }

    [MenuItem("UI Plugin/ReadCodeLineNum")]
    public static void ReadCodeLineNum()
    {
        List<string> all = new List<string>();
        all.AddRange(Directory.GetFiles("Assets/Scripts/", "*.cs", SearchOption.AllDirectories));
        all.AddRange(Directory.GetFiles("Assets/Plugins/", "*.cs", SearchOption.AllDirectories));
        all.AddRange(Directory.GetFiles("Assets/Editor/", "*.cs", SearchOption.AllDirectories));
        int num = 0;
        for (int i = 0; i < all.Count; i++)
        {
            num+=File.ReadAllLines(all[i]).Length;
        }
        Debug.Log("LineNum:" + num);
    }

   //[MenuItem("UI Plugin/生成特效关联贴图表")]
    public static void MakeEffTextureList()
    {
        List<string> all = new List<string>();
        all.AddRange(Directory.GetFiles("Assets/ResourcesLib/Effect/", "*.mat", SearchOption.AllDirectories));
        StringBuilder sb = new StringBuilder();

        StringBuilder sb1 = new StringBuilder();
        for (int i = 0; i < all.Count; i++)
        {
            string path = all[i];
            string key = path.Replace(Application.dataPath, "");
            string keyColor = path.Replace(Application.dataPath, "");
            string[] allLines = File.ReadAllLines(all[i]);
            for (int j = 0; j < allLines.Length; j++)
            {
                string c = allLines[j];
                if(c.Contains("m_Texture:"))
                {
                    key = key + "|" + c;
                }
                else if(c.Contains("_TintColor"))
                {
                    keyColor = keyColor + "|" + allLines[j + 1];
                }

            }

            sb.AppendLine(key);
            sb1.AppendLine(keyColor);
        }

        string filelistPath = Application.streamingAssetsPath + "/EffTextureList.txt";
        if (!File.Exists(filelistPath))
            File.Create(filelistPath);
        StreamWriter writer = new StreamWriter(filelistPath);
        writer.Write(sb.ToString());
        writer.Close();
        writer.Dispose();

        filelistPath = Application.streamingAssetsPath + "/EffColorList.txt";
        if (!File.Exists(filelistPath))
            File.Create(filelistPath);
        writer = new StreamWriter(filelistPath);
        writer.Write(sb1.ToString());
        writer.Close();
        writer.Dispose();

        Debug.Log("finish");
    }

    public static Dictionary<string, string[]> effcDic = new Dictionary<string, string[]>();
    public static Dictionary<string, string[]> effTDic = new Dictionary<string, string[]>();

    //[MenuItem("UI Plugin/还原特效关联贴图表")]
    public static void ReverseEffTextureList()
    {
        string[] EffTextureList = File.ReadAllLines(Application.streamingAssetsPath + "/EffTextureList.txt");

        effTDic = new Dictionary<string, string[]>();

        for (int i = 0; i < EffTextureList.Length; i++)
        {
            string[] arr = EffTextureList[i].Split('|');

            string key = arr[0].Replace(@"\", "/");
            effTDic.Add(key, arr);
        }

        string[] EffColorList = File.ReadAllLines(Application.streamingAssetsPath + "/EffColorList.txt");

        effcDic = new Dictionary<string, string[]>();

        for (int i = 0; i < EffColorList.Length; i++)
        {
            string[] arr = EffColorList[i].Split('|');

            string key = arr[0].Replace(@"\", "/");
            effcDic.Add(key, arr);
        }

//        AssetDatabase.ImportAsset("Assets/ResourcesLib/Effect/", ImportAssetOptions.ImportRecursive);
//        AssetDatabase.Refresh();
//        AssetDatabase.SaveAssets();
//
//        AssetDatabase.Refresh();
//        AssetDatabase.SaveAssets();
//
//        return;

        List<string> all = new List<string>();
        all.AddRange(Directory.GetFiles("Assets/ResourcesLib/Effect/", "*.mat", SearchOption.AllDirectories));
        for (int i = 0; i < all.Count; i++)
        {
            string path = all[i];
            string key = path.Replace(Application.dataPath, "");
            key = key.Replace(@"\", "/");
            string[] allLines = File.ReadAllLines(all[i],Encoding.UTF8);

            if(effcDic.ContainsKey(key)==false)
                continue;

            string[] tList = effTDic[key];
            string[] clist = effcDic[key];

            if(tList.Length>2)
            {
                Debug.LogError("需要手动：" + key);
                continue;
            }

            if (clist.Length > 2)
            {
                Debug.LogError("需要手动：" + key);
                continue;
            }

            Debug.Log(allLines.Length);

            for (int j = 0; j < allLines.Length; j++)
            {
                string c = allLines[j];
                if (c.Contains("_TintColor"))
                {
                    string temp = clist[1].Replace("      ", "        ");
                    allLines[j + 1] = temp;

                    Debug.Log(clist[1]);
                    break;
                }
            }

            File.WriteAllLines(all[i], allLines);

            Debug.Log(File.ReadAllText(all[i]));

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        Debug.Log("finish134");
    }

    public static void InsertStringToClass(string path, string newString)
    {
#if !UNITY_WEBPLAYER
        if (!File.Exists(path))
        {
            Log.Error("The File not Exists");
            return;
        }

        string allmono = File.ReadAllText(path, Encoding.UTF8);

        string start = "    //insertStart";
        string end = "    //insertEnd";
        int insertStart = allmono.IndexOf(start, System.StringComparison.Ordinal);
        if (insertStart == -1)
        {
            int classEnd = allmono.LastIndexOf("}", System.StringComparison.Ordinal);
            string temp = "\n\n" + start + "\n\n" + end;
            allmono = allmono.Insert(classEnd - 1, temp);
        }

        insertStart = allmono.IndexOf(start, System.StringComparison.Ordinal) + start.Length;
        int insertEnd = allmono.IndexOf(end, System.StringComparison.Ordinal);
        string sub = allmono.Substring(insertStart, insertEnd - insertStart);
        allmono = allmono.Replace(sub, "\r\n");
        insertEnd = allmono.IndexOf(end, System.StringComparison.Ordinal);
        allmono = allmono.Insert(insertEnd, newString);

        File.WriteAllText(path, allmono, Encoding.UTF8);
#endif
    }

    /// <summary>
    /// 使用的组建命名规则  GameObject ,go_**Control  text text_**Control button ，btn_**Control    image  image_**Control   RawImage rawI_**Control  ToggleGroup togGroup_**Control ;
    /// ScrollRect scr_**Control GridLayoutGroup  glayout_**Control  Slider slider_**Control   InputField input_**Control   Canvas canvas_**Control
    /// </summary>
    [MenuItem("Assets/根据prefab生成对应的Script(包含获取需要控制的按钮)/panel")]
    public static void CreatePrefabAddCreateScriptPanel()
    {
        CreatePrefabAddCreateScript(0);
    }
    


   [MenuItem("Assets/根据prefab生成对应的Script(包含获取需要控制的按钮)/普通")]
   public static void CreatePrefabAddCreateScriptNorml()
   {
       CreatePrefabAddCreateScript(1);
      
   }


    static string controlUiDeclaraStr = "";
   static string controlUiGetStr = "";
    static string getRamigeStr = "";  //获取texture
   static Dictionary<string, string> controlUiDeclaraStrDic = new Dictionary<string, string>();
   static Dictionary<string, string> controlUiGetStrDic = new Dictionary<string, string>();
    static Dictionary<string, string> getRamigeStrDic = new Dictionary<string, string>();
    static List<string> beforeLoadTextures = new List<string>();

    public static void CreatePrefabAddCreateScript(int type)
   {
       Object[] selectsObjs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
       for (int i = 0; i < selectsObjs.Length; i++)
       {
            bool fromLib=false;
           string prefabPath = AssetDatabase.GetAssetPath(selectsObjs[i]);
            string libPath="";
           if (prefabPath.Contains("ResourcesLib/Prefab/UI"))
           {
                libPath=Application.dataPath.Replace("Assets", "") + prefabPath;
                string path = prefabPath.Replace("ResourcesLib", "Resources");
                Util.CreateFileDirectory(path);
                path = prefabPath.Replace("ResourcesLib", "Resources");
                string repath = libPath.Replace("ResourcesLib", "Resources");
                if (File.Exists(repath))
                {
                    AssetDatabase.DeleteAsset(path);
                }
                AssetDatabase.CopyAsset(prefabPath,path);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                prefabPath = path;
                fromLib = true;
           }
           if(!prefabPath.Contains("Resources/Prefab/UI"))
            {
                continue;

            }
            if(string.IsNullOrEmpty(libPath))
                libPath = Application.dataPath.Replace("Assets", "") + prefabPath.Replace("Resources", "ResourcesLib");

            if (!fromLib && File.Exists(libPath))
            {
                Log.Error("resourceLib下已经存在此UI请在Lib下面修改生成代码");
                return;
            }


           string name = selectsObjs[i].name;
           string packageName = prefabPath.Substring(0, prefabPath.LastIndexOf('/'));
           packageName = packageName.Substring(packageName.LastIndexOf('/') + 1);
           string scriptPath = Application.dataPath + "/Scripts/Modules/" + packageName + "/" + name + ".cs";
           if (name == "FightUIRootView")
           {
               scriptPath = Application.dataPath + "/Scripts/Modules/Fight/UI/FightUIRootView.cs";
           }
            if (name == "MainUIRootView")
            {
                scriptPath = Application.dataPath + "/Scripts/Modules/MainUI/MainUIManager.cs";
            }
            if (name == "UI_GuideTip")
           {
               scriptPath = Application.dataPath + "/Scripts/CommonManager/Guide/UIGuide.cs";
           }
           if (File.Exists(scriptPath))
           {
               controlUiDeclaraStrDic.Clear();
               controlUiGetStrDic.Clear();
                getRamigeStrDic.Clear();
                beforeLoadTextures.Clear();
                List<string> reedInfos = ReadTextFile(scriptPath);
               string reedText = ReadTextAllFile(scriptPath);
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                bool hase= GetUiPrefabContorlInfo(go.transform, selectsObjs[i].name, "",1,"");
                if (hase&& !fromLib)
                {
                    string path = prefabPath.Replace("Resources", "ResourcesLib");
                    path = Application.dataPath.Replace("Assets","")+path;
                    Util.CreateFileDirectory(path);
                    path = prefabPath.Replace("Resources", "ResourcesLib");
                    AssetDatabase.CopyAsset(prefabPath, path);
                    
                   
                }
                EditorUtility.SetDirty(go);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                StreamWriter sw = new StreamWriter(scriptPath, false, Encoding.UTF8);
                if (beforeLoadTextures.Count>0&& !reedText.Contains("System.Collections.Generic"))
                    sw.WriteLine("using System.Collections.Generic;");
                for (int j = 0; j < reedInfos.Count;j++ )
               {
                   Log.Debug("reedInfos" + reedInfos[j]);
                   sw.WriteLine(reedInfos[j]);
                   if (reedInfos[j].Contains("开始UI申明"))
                   {
                       int k = 0;
                       for ( k = j + 1; k < reedInfos.Count; k++)
                       {
                           if (reedInfos[k].Contains("结束UI申明"))
                           {
                               break;
                           }
                       }
                       foreach (var v in controlUiDeclaraStrDic)
                       {
                           sw.WriteLine(v.Value);
                       }
                       j = k-1;
                   }

                   if (reedInfos[j].Contains("开始UI获取"))
                   {
                        bool getTexture=false;
                       int k = 0;
                       for (k = j + 1; k < reedInfos.Count-1; k++)
                       {
                           if (reedInfos[k].Contains("结束UI获取"))
                           {
                                if (reedInfos[k + 1].Contains("开始texture设置"))
                                {
                                    getTexture = true;
                                }
                               break;
                           }
                       }
                       foreach (var v in controlUiGetStrDic)
                       {
                           sw.WriteLine(v.Value);
                       }
                        sw.WriteLine("\t\t//结束UI获取;");
                        j = k;
                        if (!getTexture&& getRamigeStrDic.Count>0)
                        {
                            sw.WriteLine("\t\t//开始texture设置;");
                            foreach (var v in getRamigeStrDic)
                            {
                                sw.WriteLine(v.Value);
                            }
                            sw.WriteLine("\t\t//结束texture设置;");
                        }
                   }

                    if (reedInfos[j].Contains("开始texture设置"))
                    {
                        
                        int k = 0;
                        List<string> dic = new List<string>();
                        for (k = j + 1; k < reedInfos.Count; k++)
                        {
                            if (reedInfos[k].Contains("结束texture设置"))
                            {
                                break;
                            }
                        }
                        foreach (var v in getRamigeStrDic)
                        {
                                sw.WriteLine(v.Value);
                        }
                        j = k - 1;
                    }
                    if (reedInfos[j].Contains("BeforLoadInfos"))
                    {

                        int k = 0;
                        for (k = j + 1; k < reedInfos.Count; k++)
                        {
                            if (reedInfos[k]==("\t}")|| reedInfos[k] == ("    }"))
                            {
                                if (reedInfos[k + 1].Contains("BeforLoadTextures"))
                                {
                                    for (int l = k + 1; l < reedInfos.Count; l++)
                                    {
                                        if (reedInfos[l].Contains("//endTexture"))
                                        {
                                            k = l+1;
                                        }
                                    }
                                }
                                else
                                    k++;
                                sw.WriteLine("\t}");
                                sw.WriteLine("\tpublic string[] BeforLoadTextures()");
                                sw.WriteLine("\t{");
                                if (beforeLoadTextures.Count == 0)
                                    sw.WriteLine("\t\treturn null;");
                                else
                                {
                                    string write = "\t\t string[] backs = new string[] {";
                                    for (int m = 0; m < beforeLoadTextures.Count; m++)
                                    {
                                        string texure = beforeLoadTextures[m].ToLower();
                                        write = string.Format("{0}\"{1}\",", write, texure);
                                    }
                                    write = write.Substring(0,write.Length-1);
                                    write += "};";
                                    sw.WriteLine(write);
                                    sw.WriteLine("\t\treturn backs;");
                                }
                                sw.WriteLine("\t}");
                                sw.WriteLine("\t //endTexture");
                                break;
                            }
                            sw.WriteLine(reedInfos[k]);
                        }
                        j = k - 1;
                    }

                }
               sw.Close();
           }
           else
           {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (go==null)
                    continue;
                if (!Directory.Exists(Application.dataPath + "/Scripts/Modules/" + packageName))
               {
                   Directory.CreateDirectory(Application.dataPath + "/Scripts/Modules/" + packageName);
               }


               controlUiDeclaraStr = "";
               controlUiGetStr = "";
                getRamigeStr = "";
                beforeLoadTextures.Clear();
                bool hase = GetUiPrefabContorlInfo(go.transform, selectsObjs[i].name, "");
                if (hase && !fromLib)
                {
                    string path = prefabPath.Replace("Resources", "ResourcesLib");
                    path = Application.dataPath.Replace("Assets", "") + path;
                    Util.CreateFileDirectory(path);
                    path = prefabPath.Replace("Resources", "ResourcesLib");
                    AssetDatabase.CopyAsset(prefabPath, path);
                    

                }
                EditorUtility.SetDirty(go);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                StreamWriter sw = new StreamWriter(scriptPath, false, Encoding.UTF8);
                if (beforeLoadTextures.Count > 0)
                    sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System;");
               sw.WriteLine("using UnityEngine;");
               sw.WriteLine("using UnityEngine.UI;");
               sw.WriteLine();
               if (type == 1)
               {
                   sw.WriteLine(string.Format("public class {0} : MonoBehaviour", name));
               }
               else
               {
                   sw.WriteLine(string.Format("public class {0}BeforeLoad : PanelBase", name));
                   sw.WriteLine("{");
                   sw.WriteLine();
                   sw.WriteLine("\tpublic string[] BeforLoadInfos()");
                   sw.WriteLine("\t{");
                   sw.WriteLine("\t\treturn null;");
                   sw.WriteLine("\t}");
                    sw.WriteLine("\tpublic string[] BeforLoadTextures()");
                    sw.WriteLine("\t{");
                    if(beforeLoadTextures.Count==0)
                       sw.WriteLine("\t\treturn null;");
                    else
                    {
                        string write = "\t\t string[] backs = new string[] {";
                        for (int m = 0; m < beforeLoadTextures.Count; m++)
                        {
                            string texure = beforeLoadTextures[m].ToLower();
                            write = string.Format("{0}\"{1}\",", write, texure);
                        }
                        write = write.Substring(0, write.Length - 1);
                        write += "};";
                        sw.WriteLine(write);
                        sw.WriteLine("\t\treturn backs;");
                    }
                    sw.WriteLine("\t}");
                    sw.WriteLine("\t //endTexture");
                    sw.WriteLine("}");
                   sw.WriteLine();
                   sw.WriteLine(string.Format("public class {0} : SingletonMonoBehaviour<{0}>", name));
               }
               sw.WriteLine("{");
               sw.WriteLine();
               sw.WriteLine("\tprivate Transform cachedTransform ;");

               sw.WriteLine("\t//开始UI申明;");
               sw.Write(controlUiDeclaraStr);
               sw.WriteLine("\t//结束UI申明;");
               sw.WriteLine("\tprivate bool isUIinit = false;");
               sw.WriteLine();

               if (type == 1)
               {
                   sw.WriteLine("\tvoid Awake ()");
               }
               else
               {
                   sw.WriteLine("\tprotected override void Awake ()");
               }
               
               sw.WriteLine("\t{");
               if (type != 1)
               {
                   sw.WriteLine("\t\tbase.Awake();");
               }
               sw.WriteLine("\t\tcachedTransform=transform;");
               sw.WriteLine("\t\t//开始UI获取;");
               sw.Write(controlUiGetStr);
               sw.WriteLine("\t\t//结束UI获取;");
                if (!string.IsNullOrEmpty(getRamigeStr))
                {
                    sw.WriteLine("\t\t//开始texture设置;");
                    sw.Write(getRamigeStr);
                    sw.WriteLine("\t\t//结束texture设置;");
                }
                
                sw.WriteLine("\t\tisUIinit = true;");
               sw.WriteLine("\t}");

                

                sw.WriteLine("}");
               sw.WriteLine();
               sw.Close();
           
           }

           

       }
       AssetDatabase.Refresh();
       AssetDatabase.SaveAssets();
      
   }

   public static List<string> ReadTextFile(string file)
   {
       StreamReader sr = null;
       string item = "";
       List<string> text = new List<string>();
       try
       {
           sr = new StreamReader(file, Encoding.UTF8);
           item = sr.ReadLine();
           while (item != null)
           {
               text.Add(item);
               item = sr.ReadLine();
           }
       }
       catch (IOException ex2)
       {
           throw new IOException("读取文件出错：" + file + "\r\n" + ex2.StackTrace);
       }
       finally
       {
           try
           {
               sr.Close();
           }
           catch
           {
           }
       }
       return text;
   }
    public static string ReadTextAllFile(string file)
    {
        StreamReader sr = null;
        string item = "";
        string text ="";
        try
        {
            sr = new StreamReader(file, Encoding.UTF8);
            text = sr.ReadToEnd();
        }
        catch (IOException ex2)
        {
            throw new IOException("读取文件出错：" + file + "\r\n" + ex2.StackTrace);
        }
        finally
        {
            try
            {
                sr.Close();
            }
            catch
            {
            }
        }
        return text;
    }
    /// 使用的组建命名规则  GameObject ,go_**cr  text text_**cr button ，btn_**cr    image  image_**cr   RawImage rawI_**cr Toggle  tog_**cr ToggleGroup togGroup_**cr ;
    /// ScrollRect scr_**cr GridLayoutGroup  glayout_**cr  Slider slider_**cr   InputField input_**cr   Canvas canvas_**cr   Dropdown dropdown_**cr

    //type 0 表示第一次创建 1 表示之前已经创建
    private static bool GetUiPrefabContorlInfo(Transform parent, string orilaName, string parentOrialName, int type = 0, string orialNameShow = "")
   {
       Debug.Log(parent.name);
       Debug.Log(orilaName);
       Debug.Log(parentOrialName);
        bool back=false;
        if(string.IsNullOrEmpty(orialNameShow))
        {
            orialNameShow = orilaName;
        }
       if (parent.name.EndsWith("Control"))
       {
           parent.name = parent.name.Replace("Control", "cr");
           EditorUtility.SetDirty(parent.gameObject);
       
       }
       if(parent.name.EndsWith("cr"))
       {
           string startName=parent.name.Split('_')[0];
           startName = startName.ToLower();

           if (type == 0)
           {
               switch (startName)
               {
                   case "go":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "GameObject", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").gameObject;{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "text":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Text", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Text>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "btn":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Button", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Button>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "image":
                       Image image = parent.GetComponent<Image>();
                       if (image.material.mainTexture == null)
                           Log.Error(orialNameShow+orialNameShow + parent + "贴图没有设置对用的mat存在错误请检查");
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Image", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Image>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "rawi":
                        RawImage raimage = parent.GetComponent<RawImage>();
                        if (raimage != null && raimage.mainTexture != null && raimage.mainTexture.width * raimage.mainTexture.height > 50000)
                        {
                            string texturePath = AssetDatabase.GetAssetPath(raimage.mainTexture);
                            string extension = Path.GetExtension(texturePath);
                            texturePath = string.IsNullOrEmpty(extension)
                            ? (texturePath + ".assetbundle")
                            : texturePath.Replace(extension, ".assetbundle");
                            string abName = texturePath.Replace("Assets/Resources/", "");
                            getRamigeStr += string.Format("{0}{0}TextureManager.Instance.SetTexure({1},\"{2}\");{3}", "\t", parent.name, abName, "\r\n");
                            back = true;
                            raimage.texture = null;
                            if(!beforeLoadTextures.Contains(abName))
                               beforeLoadTextures.Add(abName);
                           // EditorUtility.SetDirty(raimage.gameObject);
                        }
                        controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "RawImage", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<RawImage>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n"); 
                       break;
                   case "tog":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Toggle", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Toggle>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "toggroup":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "ToggleGroup", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<ToggleGroup>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "scr":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "ScrollRect", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<ScrollRect>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "glayout":
                        ToggleGroup group= parent.GetComponent<ToggleGroup>();
                        if(group!=null)
                        {
                            controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "ToggleGroup", parent.name.Replace("glayout", "toggroup"), "\r\n");
                            controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<ToggleGroup>();{3}", "\t", parent.name.Replace("glayout", "toggroup"), string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                        }
                        controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "GridLayoutGroup", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<GridLayoutGroup>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "slider":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Slider", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Slider>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "input":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "InputField", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<InputField>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "canvas":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Canvas", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Canvas>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "dropdown":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Dropdown", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Dropdown>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;
                   case "scrollbar":
                       controlUiDeclaraStr += string.Format("{0}private {1} {2};{3}", "\t", "Scrollbar", parent.name, "\r\n");
                       controlUiGetStr += string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Scrollbar>();{3}", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name)), "\r\n");
                       break;

               }
           }
           else
           {
               try
               {
                    switch (startName)
                    {
                        case "go":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "GameObject", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").gameObject;", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "text":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Text", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Text>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));
                            break;
                        case "btn":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Button", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Button>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "image":
                            Image image = parent.GetComponent<Image>();
                            if (image.material.mainTexture == null)
                                Log.Error(orialNameShow+parentOrialName + parent + "贴图没有设置对用的mat存在错误请检查");
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Image", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Image>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "rawi":
                            RawImage raimage = parent.GetComponent<RawImage>();
                            if (raimage != null && raimage.mainTexture != null && raimage.mainTexture.width * raimage.mainTexture.height > 50000)
                            {
                                string texturePath = AssetDatabase.GetAssetPath(raimage.mainTexture);
                                string extension = Path.GetExtension(texturePath);
                                texturePath = string.IsNullOrEmpty(extension)
                                ? (texturePath + ".assetbundle")
                                : texturePath.Replace(extension, ".assetbundle");
                                string abName = texturePath.Replace("Assets/Resources/", "");
                                getRamigeStrDic.Add(parent.name, string.Format("{0}{0}TextureManager.Instance.SetTexure({1},\"{2}\");", "\t", parent.name, abName));
                                back = true;
                                raimage.texture = null;
                                if (!beforeLoadTextures.Contains(abName))
                                    beforeLoadTextures.Add(abName);
                                //EditorUtility.SetDirty(raimage.gameObject);
                            }
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "RawImage", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<RawImage>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "tog":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Toggle", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Toggle>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "toggroup":
                           
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "ToggleGroup", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<ToggleGroup>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "scr":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "ScrollRect", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<ScrollRect>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "glayout":
                            ToggleGroup group = parent.GetComponent<ToggleGroup>();
                            if (group != null)
                            {
                                controlUiDeclaraStrDic.Add(parent.name.Replace("glayout", "toggroup"), string.Format("{0}private {1} {2};", "\t", "ToggleGroup", parent.name.Replace("glayout", "toggroup")));
                                controlUiGetStrDic.Add(parent.name.Replace("glayout", "toggroup"), string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<ToggleGroup>();", "\t", parent.name.Replace("glayout", "toggroup"), string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));
                            }
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "GridLayoutGroup", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<GridLayoutGroup>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "slider":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Slider", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Slider>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "input":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "InputField", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<InputField>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "canvas":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Canvas", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Canvas>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));
                            break;
                        case "dropdown":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Dropdown", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Dropdown>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));

                            break;
                        case "scrollbar":
                            controlUiDeclaraStrDic.Add(parent.name, string.Format("{0}private {1} {2};", "\t", "Scrollbar", parent.name));
                            controlUiGetStrDic.Add(parent.name, string.Format("{0}{0}{1} = cachedTransform.FindChild(\"{2}\").GetComponent<Scrollbar>();", "\t", parent.name, string.IsNullOrEmpty(parentOrialName) ? parent.name : (string.Format("{0}/{1}", parentOrialName, parent.name))));
                            break;

                    }
                }
               catch (Exception)
               {
                   Log.Error(parent.name + "可能重复了");
                   throw;
               }
               

           }
       }

        bool childBack = false;
       if(parent.childCount!=0)
       {
           for (int i = 0; i < parent.childCount;i++ )
           {
               string _parentOrialName;
               if (parent.name == orilaName)
               {
                   _parentOrialName="";
               }
               else if (string.IsNullOrEmpty(parentOrialName))
               {
                   _parentOrialName = parent.name;
               }
               else
               {
                   _parentOrialName=parentOrialName+"/"+parent.name;
               }

                bool back1 = GetUiPrefabContorlInfo(parent.GetChild(i), orilaName, _parentOrialName, type, orialNameShow);
                childBack = back1|| childBack;
           }
       }
        return back || childBack;
   }

    [MenuItem("UI Plugin/检测AB同名")]
    public static void CheckABSameName()
    {
        string[] paths = Directory.GetFiles("Assets/StreamingAssets/Android", "*.assetbundle", SearchOption.AllDirectories);

        Debug.Log(paths.Length);

        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

        for (int i = 0; i < paths.Length; i++)
        {
            string filename = Path.GetFileNameWithoutExtension(paths[i]);
            if (dic.ContainsKey(filename) == false)
            {
                List<string> list = new List<string>();
                list.Add(paths[i]);
                dic[filename] = list;
            }
            else
            {
                dic[filename].Add(paths[i]);
            }
        }

        foreach (var item in dic.Values)
        {
            if (item.Count > 1)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Debug.Log(item[i]);
                }
            }
        }
    }
}