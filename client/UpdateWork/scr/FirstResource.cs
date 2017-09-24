using System.Collections.Generic;

namespace UpdateWork
{
    public class FirstResource
    {
        static Dictionary<string, string> _appFileDic = null;
        static Dictionary<string, string> _sdFileDic = null;

        static public Dictionary<string, string> UpdateUrlDic = new Dictionary<string, string>();
        static public Dictionary<string, string> ServerListDic = new Dictionary<string, string>();
        public static string ResourceKey = "ResourceKey";
        static void _ReadFileList(out Dictionary<string, string> dic, string text)
        {
            dic = new Dictionary<string, string>();
            foreach (string line in FirstUtil.ReadAllLines(text))
            {
                string[] row = line.Split('\t');
                if (row.Length == 2)
                    dic.Add(row[0], row[1]);
            }
        }

        public static void InitAppFileList(string text)
        {
            _ReadFileList(out _appFileDic, text);

            WriteLog("_appFileDic" + _appFileDic.Count);
        }

        public static void InitSdFileList(string text)
        {
            _ReadFileList(out _sdFileDic, text);

            WriteLog("_sdFileDic" + _sdFileDic.Count);
        }

        static void WriteLog(string msg, params object[] args)
        {
            FirstUtil.WriteLog("[resource]" + msg, args);
        }

        public static Dictionary<string, string> AppFileDic { get { return _appFileDic; } }
        public static Dictionary<string, string> SdFileDic { get { return _sdFileDic; } }
    }
}
