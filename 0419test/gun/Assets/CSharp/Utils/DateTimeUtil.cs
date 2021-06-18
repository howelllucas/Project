

using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;
using EZ;
using EZ.Data;
using System.Collections;
using UnityEngine.Networking;

public class DateTimeUtil
{

    //一秒的毫秒数
    public static int m_Sec_Mills = 1000;
    //一分钟的秒数
    public static int m_Minute_Secs = 60;
    //一小时的秒数
    public static int m_Hour_Secs = 60 * m_Minute_Secs;
    //一天的秒数
    public static int m_Day_Secs = 24 * m_Hour_Secs;
    //一天的毫秒数
    public static int m_Day_Mills = m_Day_Secs * m_Sec_Mills;

    //网络时间结果
    public static string m_WebDateTime = string.Empty;
    //1970年1月1日
    public static DateTime initDate = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

    public static string m_WebUnixTimeUrl = "http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2";

    public static double GetMills(DateTime date)
    {
         return date.Subtract(initDate).TotalMilliseconds;
    }

    public static DateTime GetDate(double mills)
    {
        return initDate.AddMilliseconds(mills);
    }

    public static int SubDays(DateTime d1, DateTime d2)
    {
        DateTime date1 = new DateTime(d1.Year, d1.Month, d1.Day);
        DateTime date2 = new DateTime(d2.Year, d2.Month, d2.Day);
        return (date1 - date2).Days;
    }

    public static int[] GetTimeString(double mills)
    {
        double day = mills / m_Day_Mills;
        double dayLeft = mills % m_Day_Mills;
        double hour = dayLeft / (m_Hour_Secs * m_Sec_Mills);
        double hourLeft = dayLeft % (m_Hour_Secs * m_Sec_Mills);
        double minute = hourLeft / (m_Minute_Secs * m_Sec_Mills);
        double minuteLeft = hourLeft % (m_Minute_Secs * m_Sec_Mills);
        double secs = minuteLeft / (m_Sec_Mills);

        int[] result = new int[4];
        result[0] = Convert.ToInt32(Math.Floor(day));
        result[1] = Convert.ToInt32(Math.Floor(hour));
        result[2] = Convert.ToInt32(Math.Floor(minute));
        result[3] = Convert.ToInt32(Math.Floor(secs));
        //Debug.Log(string.Format("mills = {0} day = {1} hour = {2} mimute = {3} secs = {4} now = {5}", mills, day, hour, minute, secs, DateTime.Now.ToString()));
        return result;
    }

    public static string GetWebUnixTime()
    {
        //无网络
        //if (SdkdsNativeUtil.Instance.GetCurrentNetStatue() == 4)
        //{
        //    return string.Empty;
        //}

        HttpWebRequest request = WebRequest.Create(m_WebUnixTimeUrl) as HttpWebRequest;//创建请求
        request.Method = "GET"; //请求方法为GET
        HttpWebResponse res; //定义返回的response
        try
        {
            res = (HttpWebResponse)request.GetResponse(); //此处发送了请求并获得响应
        }
        catch (WebException ex)
        {
            
            Debug.LogError("WebException message = " + ex.Message + ", WebException status = " + ex.Status);
            return GetMills(DateTime.Now).ToString();
        }
        StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
        string content = sr.ReadToEnd(); //响应转化为String字符串
        string[] prms = content.Split('=');
        content = prms[prms.Length - 1];
        return content;
    }

    public static IEnumerator GetWebUnixTimeAsyn()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(m_WebUnixTimeUrl))
        {
            Debug.Log("GetWebUnixTimeAsyn");
            m_WebDateTime = string.Empty;
            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                m_WebDateTime = GetMills(DateTime.Now).ToString();
                Debug.Log(webRequest.error + "\n" + webRequest.downloadHandler.text);
            }
            else
            {
                string[] prms = webRequest.downloadHandler.text.Split('=');

                m_WebDateTime = prms[prms.Length - 1];

                Debug.Log("GetWebUnixTimeAsyn = " + webRequest.downloadHandler.text);
            }
        }
    }

    public static string CheckWebTime()
    {
        string webTimeStr = DateTimeUtil.GetWebUnixTime();
        return CheckWebTime(webTimeStr);
    }

    public static string CheckWebTime(string webTimeStr)
    {
        if (webTimeStr.Equals(string.Empty))
        {
            return CheckNetTypeConstVal.NO_NET;
        }
        double clientTime = DateTimeUtil.GetMills(DateTime.Now);
        GeneralConfigItem errorHoursCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.WEB_TIME_ERROR_HOURS);
        double errorMillss = double.Parse(errorHoursCfg.content) * DateTimeUtil.m_Hour_Secs * DateTimeUtil.m_Sec_Mills;
        double webTimeMills;
        if (!double.TryParse(webTimeStr, out webTimeMills))
        {
            webTimeMills = clientTime;
        }
        double error = clientTime - webTimeMills;
        Debug.Log(error);
        if (error > errorMillss || error < -errorMillss)
        {
            return CheckNetTypeConstVal.CHANGE_TIME;
        }
        return CheckNetTypeConstVal.RIGHT;
    }
}