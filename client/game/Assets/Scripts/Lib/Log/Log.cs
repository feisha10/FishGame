using UnityEngine;
using System;


/// <summary>
/// 日志管理类
/// @LogLevel    用来设置当前可看日志等级
/// 
/// 最常用的是  Debug（string）
/// </summary>
public class Log
{
    private const int TRACE = 5;       //最低级别的调试信息
    private const int DEBUG = 6;       //普通的调试级别，和Debug.log()一样
    private const int INFO = 7;       //Socket信息，主要用做前后端通信日志
    private const int WARN = 8;       //警告级
    private const int RECORD = 9;       //记录级别，用来分析用户信息（如网络登录等）
    private const int EXCEPTION = 10;   //异常级别
    private const int ERROR = 11;       //错误级别

    public static int LogLevel = TRACE;//日志级别，所有级别高于它的信息也会同时报告

    public static bool IsGmShowLog = false;

    /// <summary>
    /// DEBUG（和Debug.log()不太一样）
    /// </summary>
    /// <param name="message"></param>
    public static void Debug(string message, params object[] args)
    {
        //return;
        bool isShowLog = IsShowLog;

        if (!isShowLog && !IsGmShowLog)
            return;

        if (args.Length > 0)
            message = string.Format(message, args);

        message = DateTime.Now.ToString("HH:mm:ss\t#") + message.TrimEnd();

        UnityEngine.Debug.Log(message);

        if (IsGmShowLog && string.IsNullOrEmpty(message) == false && GmView.Active)
                GmView.Instance.ShowLog(message);

    }

    public static void Debug(object obj)
    {
        Debug(obj.ToString());
    }
    /// <summary>
    /// INFO(前后端通信日志)
    /// </summary>
    /// <param name="message"></param>
    public static void InfoVO(int type, object message)
    {
       // return;
        string s = null;

        if (IsShowLog || IsGmShowLog)
        {
            string typeS = message.GetType().ToString();

            //未完成

            if (string.IsNullOrEmpty(s) == false)
                UnityEngine.Debug.Log(DateTime.Now + "\t#" + s);

            if (IsGmShowLog && string.IsNullOrEmpty(s) == false && GmView.Active)
                GmView.Instance.ShowLog(DateTime.Now + "\t#" + s);
        }
    }

    private static bool IsShowLog
    {
        get
        {
            bool isShowLog=false;
#if UNITY_EDITOR
            isShowLog = true;
#else
            isShowLog = GameDefine.isShowLog;
#endif
            return isShowLog;
        }
    }

    /// <summary>
    /// WARN(警告信息)
    /// </summary>
    /// <param name="message"></param>
    public static void Warn(object message)
    {
        bool isShowLog = IsShowLog;

        if (isShowLog || IsGmShowLog)
            UnityEngine.Debug.LogWarning(DateTime.Now + "\t\t\t\t#" + message.ToString());

        if (IsGmShowLog && GmView.Active)
            GmView.Instance.ShowLog(DateTime.Now + "\t#" + message.ToString());

    }

    /// <summary>
    /// ERROR，重要的报错信息
    /// </summary>
    /// <param name="message"></param>
    public static void Error(object message)
    {
        if (ERROR >= LogLevel)
            UnityEngine.Debug.LogError(message.ToString());

        if (IsGmShowLog && GmView.Active)
            GmView.Instance.ShowLog(message.ToString());
    }

    /// <summary>
    /// 一般用在try Catch 中的异常记录
    /// </summary>
    /// <param name="exception">异常</param>
    public static void Exception(Exception exception)
    {
        if (EXCEPTION >= LogLevel)
            UnityEngine.Debug.LogException(exception);

        if (IsGmShowLog && GmView.Active)
            GmView.Instance.ShowLog(exception.StackTrace);
    }

    /// <summary>
    /// 显示堆栈
    /// </summary>
    public static string StackTrace()
    {
        return StackTraceUtility.ExtractStackTrace();
    } 
}
