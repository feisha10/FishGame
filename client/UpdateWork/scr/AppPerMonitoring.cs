using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UpdateWork
{
    /// <summary>
    /// APM应用性能监控后台接口类(异步调用)
    /// Version: APM v1.2.0
    /// Environment: 兼容.NET 3.5以上 
    /// Developer: GZ1114
    /// ModifiTime: 2016-11-01 14:45
    /// </summary>
    public class AppPerMonitoring
    {
        public int server_id; //服务器ID
        public string uid; //平台用户唯一标识
        public int log_time; //事件发生的时间戳
        public int code; //错误码
        public string code_msg; //具体错误码值或信息
        public string did; //设备id
        public string device; //设备端,可选值为如下:android|ios|web|pc
        public string device_name; //设备名称
        public string game_version; //游戏版本号
        public string os; //操作系统，android、iOS
        public string os_version; //操作系统版本
        public string Mno; //Sim卡网络运营商（中国移动、中国联通）
        public string Nm; //联网方式 3G、4G、WIFI
        public int f_code; //记录启用DNS重连的错误码
        public string apiUrl; //APM接口地址
        public int timeout = 3000; //连接超时5s
        public string channel; //平台或渠道名称
        public string geo; //GPS位置信息（格式："中国,广东,广州市,天河区"）

        // 使用Singleton模式
        private AppPerMonitoring() { }
        public static readonly AppPerMonitoring Inst = new AppPerMonitoring();


        /// <summary>
        /// 向APM接口提交数据
        /// </summary>
        /// <param name="objName">Web代理的实例对象</param>
        private void Commit(WebProxy objName = null)
        {
            string ip = string.Empty;

            Stream dataStream = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.Timeout = this.timeout;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ServicePoint.Expect100Continue = false;
                if (objName != null)
                {
                    request.Proxy = objName;
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(PostUri());
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                dataStream.Dispose();
                dataStream = null;
            }
            catch (WebException ex)
            {
                Debug.LogException(ex);
                if (dataStream != null)
                {
                    dataStream.Close();
                    dataStream.Dispose();
                    dataStream = null;
                }

                //不要把"NameResolutionFailure"设置成"Timeout",会引发死循环
                if (ex.Status.ToString().Equals("NameResolutionFailure"))
                {
                    ip = DnsRetryGet("apiapm.gz4399.com");
                    if (!ip.Equals("0"))
                    {
                        WebProxy proxy = new WebProxy(ip, 80);
                        Commit(proxy);

                        PostApmFail(ip, string.Format("ip:'{0}',status:'{1}',msg:'{2}'", ip, ex.Status, ex.Message));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                if (dataStream != null)
                {
                    dataStream.Close();
                    dataStream.Dispose();
                    dataStream = null;
                }
            }
        }

        /// <summary>
        /// APM接口上报失败(当启用DNS重连机制后,上报该码)
        /// </summary>
        /// <param name="retip">DNS重连IP</param>
        /// <param name="err_msg">错误信息</param>
        private void PostApmFail(string retip, string err_msg)
        {
            Stream dataStream = null;

            if (f_code == 0)
            {
                return;
            }
            try
            {
                string newPostUri = this.PostUri().Replace("&code=" + this.code, "&code=" + this.f_code);
                newPostUri = newPostUri.Replace("&code_msg=" + this.code_msg, "&code_msg=" + err_msg);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.apiUrl);
                request.Proxy = new WebProxy(retip, 80);
                request.Method = "POST";
                request.Timeout = this.timeout;
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = Encoding.UTF8.GetBytes(newPostUri);
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                dataStream.Dispose();
                dataStream = null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                if (dataStream != null)
                {
                    dataStream.Close();
                    dataStream.Dispose();
                    dataStream = null;
                }
            }
        }


        /// <summary>
        /// httpDNS重连请求机制
        /// </summary>
        /// <param name="doMain"></param>
        /// <returns>返回对应IP</returns>
        public string DnsRetryGet(string doMain)
        {
            string ip = String.Empty;
            string[] ipArray = { };
            string dnsUrl = "http://119.29.29.29/d?dn=";
            string pattern = "((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)";
            Regex ipMatch = new Regex(pattern);

            WebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;

            try
            {
                dnsUrl = (doMain.Length > 1) ? (dnsUrl + doMain) : "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(dnsUrl);
                request.Method = "GET";
                request.Timeout = timeout;

                response = request.GetResponse();
                dataStream = response.GetResponseStream();

                if (dataStream != null)
                {
                    reader = new StreamReader(dataStream, Encoding.UTF8);
                    string reqText = reader.ReadToEnd();
                    ipArray = reqText.Split(';');
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                    ip = (ipMatch.IsMatch(ipArray[0])) ? ipArray[0] : "0";

                    dataStream.Close();
                    dataStream.Dispose();
                    dataStream = null;
                }
                else
                {
                    ip = "0";
                }

                response.Close();
                response = null;
            }
            catch (Exception e)
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                    dataStream.Dispose();
                    dataStream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                return "0";
            }
            return ip;
        }


        /// <summary>
        /// 组装POST元素的头部Uri
        /// </summary>
        /// <returns>Uri格式</returns>
        private string PostUri()
        {
            this.log_time = (this.log_time > 0) ? this.log_time : (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            StringBuilder data = new StringBuilder();
            data.Append("&server_id=" + this.server_id);
            data.Append("&uid=" + this.uid);
            data.Append("&log_time=" + this.log_time);
            data.Append("&code=" + this.code);
            data.Append("&code_msg=" + this.code_msg);
            data.Append("&did=" + this.did);
            data.Append("&device=" + this.device);
            data.Append("&device_name=" + this.device_name);
            data.Append("&game_version=" + this.game_version);
            data.Append("&os=" + this.os);
            data.Append("&os_version=" + this.os_version);
            data.Append("&Mno=" + this.Mno);
            data.Append("&Nm=" + this.Nm);
            data.Append("&channel=" + this.channel);
            data.Append("&geo=" + this.geo);
            return data.ToString();
        }


        //定义委托
        private delegate void Asyncdelegate(WebProxy objName);
        //异步完成时执行回调方法
        private void CallbackMethod(IAsyncResult ar)
        {
            Asyncdelegate dlgt = (Asyncdelegate)ar.AsyncState;
            dlgt.EndInvoke(ar);
        }

        //异步调用Commit方法
        public void Run()
        {
            Asyncdelegate isgt = new Asyncdelegate(Commit);
            //需要时才使用回调方法
            isgt.BeginInvoke(null, new AsyncCallback(CallbackMethod), null);

            //IAsyncResult ar = isgt.BeginInvoke(null, null, isgt);
        }

    }
}
