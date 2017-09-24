using System;
using System.Collections.Generic;
using UnityEngine;

namespace UpdateWork
{
    public class FirstRecordMsg
    {
        public static string Record_Url;
        public static string device = "";   //设备端：android| ios | web | pc
        public static string did = "";
        public static string game_version = "";
        public static string os = "";
        public static string os_version = "";
        public static string device_name = "";
        public static string Nm = "";

        private static Dictionary<string, int> recordDic = new Dictionary<string, int>();

        public static void Init()
        {
#if UNITY_EDITOR
        device = "PC Editor";
#elif UNITY_ANDROID
        device = "android";
        Record_Url = "http://apiapm.gz4399.com/api/apm/8a7059ebc37bac711";
#elif UNITY_IOS
        device = "ios";
        Record_Url = "http://apiapm.gz4399.com/api/apm/32d85f9ecaa1ce711";
#elif UNITY_STANDALONE
            device = "pc";
#elif UNITY_WEBPLAYER
        device = "web";        
#endif

            did = SystemInfo.deviceUniqueIdentifier;
            //FirstUtil.WriteLog("did=" + did);
            //FirstUtil.WriteLog("Record_Url=" + Record_Url);
            SetDeviceInfo();
        }

        static void SetDeviceInfo()
        {
            os = SystemInfo.operatingSystem;
            os_version = SystemInfo.operatingSystem;
            device_name = SystemInfo.deviceModel;

#if UNITY_ANDROID ||UNITY_IOS
            string[] strs = os.Split(' ');
            if (strs.Length > 2)
            {
                os = strs[0];
                os = os.Replace("iPhone", "iOS");
                os_version = strs[2];
            }
#endif

            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Nm = "WIFI";
            }
            else
            {
                Nm = "CarrierData";
            }
        }


        private static void Send(string recordMsg, int msgType)
        {
            if (string.IsNullOrEmpty(Record_Url))
                return;

            AppPerMonitoring.Inst.apiUrl = Record_Url;
            AppPerMonitoring.Inst.code = msgType;
            AppPerMonitoring.Inst.server_id = 0;
            AppPerMonitoring.Inst.uid = "0";
            AppPerMonitoring.Inst.code_msg = recordMsg;
            AppPerMonitoring.Inst.did = did;
            AppPerMonitoring.Inst.device = device;
            AppPerMonitoring.Inst.device_name = device_name;
            AppPerMonitoring.Inst.game_version = game_version;
            AppPerMonitoring.Inst.os = os;
            AppPerMonitoring.Inst.os_version = os_version;
            AppPerMonitoring.Inst.Mno = "";
            AppPerMonitoring.Inst.Nm = Nm;
            AppPerMonitoring.Inst.f_code = 7001;
            AppPerMonitoring.Inst.Run();

            FirstUtil.WriteLog(recordMsg);
        }

        public static void Error(string recordMsg, int msgType)
        {
            if (string.IsNullOrEmpty(recordMsg) == false && recordDic.ContainsKey(recordMsg) == false)
            {
                recordDic.Add(recordMsg, 1);
                Send(recordMsg, msgType);
            }
        }
    }

    //添加新类型，需要填写 client/ErrorCode模板.xls
    public static class MsgType
    {
        public static int DownloadError = 1001; //下载资源错误
        public static int CrcError = 1002;      //文件crc验证错误
        public static int WriteFileError = 1003; //写文件时错误
        public static int HandleFileException = 1004; //操作文件异常
        public static int ReflectionDll = 1005; //反射dll异常
    }

}
