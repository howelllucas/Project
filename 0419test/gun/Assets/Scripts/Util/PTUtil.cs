/*********************************************************************************
 *Copyright(C) 2016 by wangning
 *All rights reserved.
 *FileName:     Util.cs
 *Author:       wangning
 *Version:      1.0
 *UnityVersion：5.3.5f1
 *Date:         2016-11-07
 *Description:   
 *History:  
**********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

namespace Game.Util
{
    public class DeviceInfo
    {
        public string vendoridentfier;
        public string system_version;
        public string device_model;
        public string device_name;
        public string device_type;
        public string device_uniqueidentifier;
        public string operating_system;
    }

    /// <summary>
    /// 转盘概率信息
    /// </summary>
    public class LuckRateInfo
    {
        public int rate { get; set; }
        public int maxval { get; set; }
        public int minval { get; set; }
        public LuckRateInfo()
        {
            rate = 0;
            maxval = 0;
            minval = 0;
        }
    }

    public class PTUtil
    {
        public static readonly string ENCODE_KEY = "PT@Star2019.pt00";

        /// <summary>
        /// Byte数组 转 十六进制字串
        /// </summary>
        /// <returns>The array tex string.</returns>
        /// <param name="buffer">Buffer.</param>
        public static string ByteArrayToTexString(byte[] buffer)
        {
            string s = BitConverter.ToString(buffer).Replace("-", string.Empty);
            return s;
        }

        // 十六进制字串 转 Byte数组
        ///<summary>Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>  
        ///<param name="s">The string containing the hex digits (with or without spaces).</param>  
        ///<returns>Returns an array of bytes.</returns>  
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace("   ", " ");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 随机排列数组元素
        /// </summary>
        /// <param name="myList"></param>
        /// <returns></returns>
        public static List<int> ListRandom(List<int> myList)
        {

            System.Random ran = new System.Random();
            int index = 0;
            int temp = 0;
            for (int i = 0; i < myList.Count; i++)
            {
                index = ran.Next(0, myList.Count - 1);
                if (index != i)
                {
                    temp = myList[i];
                    myList[i] = myList[index];
                    myList[index] = temp;
                }
            }
            return myList;
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <returns>The time stamp.</returns>
        public static long GetTimeStamp(bool inSec = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            if (inSec)
            {
                return Convert.ToInt64(ts.TotalSeconds);
            }
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// Reads the streaming assets file.
        /// </summary>
        /// <returns>The streaming assets file.</returns>
        /// <param name="filename">Filename.</param>
        public static string ReadStreamingAssetsFile(string filename)
        {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, filename);
            string content = "";
            if (filePath.Contains("://"))
            {
                //以下代码通过JAVA代码来同步读取并且返回给unity
                AndroidJavaClass m_AndroidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject m_AndroidJavaObject = null;
                if (m_AndroidJavaClass != null)
                {
                    m_AndroidJavaObject = m_AndroidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
                }

                content = m_AndroidJavaObject.Call<string>("x_ReadFile", filename);
            }
            else
            {
                StreamReader sr = new StreamReader(filePath);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    content += line;
                }
            }
            return content;
        }

        /// <summary>
        /// 从文件加载json
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static JsonData LoadJsonFromFileWithStreamingAssets(string filePath, bool decode)
        {
            JsonData data = null;
            string dataAsJson = ReadStreamingAssetsFile(filePath);
            if (string.IsNullOrEmpty(dataAsJson) == false)
            {
                if (decode)
                {
                    dataAsJson = AESDecrypt(dataAsJson, PTUtil.ENCODE_KEY);
                }
                data = JsonMapper.ToObject(dataAsJson);
            }

            if (data == null)
            {
                UnityEngine.Debug.LogError("Json File Not found!!  Path: " + filePath);
            }

            return data;
        }

        /// <summary>
        /// 从文件加载json
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static JsonData LoadJsonFromFile(string filePath, bool decode)
        {
            JsonData data = null;
            if (File.Exists(filePath))
            {
                try
                {
                    string dataAsJson = File.ReadAllText(filePath, Encoding.UTF8);
                    if (decode)
                    {
                        dataAsJson = AESDecrypt(dataAsJson, PTUtil.ENCODE_KEY);
                    }
                    data = JsonMapper.ToObject(dataAsJson);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogError("Json Error: " + filePath);
                    UnityEngine.Debug.LogError("Json Error: " + e.ToString());
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Json File Not found!!  Path: " + filePath);
            }
            return data;
        }
        /// <summary>
        /// 从存储字符串中读取json
        /// </summary>
        /// <param name="saveStr"></param>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static JsonData LoadJsonFromSaveStr(string saveStr, bool decode)
        {
            JsonData data = null;
            if (!string.IsNullOrEmpty(saveStr))
            {
                try
                {
                    var dataAsJson = saveStr;
                    if (decode)
                    {
                        dataAsJson = AESDecrypt(dataAsJson, PTUtil.ENCODE_KEY);
                    }
                    data = JsonMapper.ToObject(dataAsJson);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogError("Json Error: " + e.ToString());
                }
            }
            return data;
        }

        /// <summary>
        ///  保存json到文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static bool SaveJsonToFile(JsonData data, string filePath, bool encode)
        {
            if (data == null || string.IsNullOrEmpty(data.ToJson()))
            {
                return false;
            }
            string strJsonData = data.ToJson();
            if (encode)
            {
                strJsonData = AESEncrypt(strJsonData, PTUtil.ENCODE_KEY);
            }

            File.WriteAllText(filePath, strJsonData, Encoding.UTF8);
            return true;
        }

        /// <summary>
        /// 获取json存储字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetJsonSaveStr(JsonData data, bool encode)
        {
            if (data == null || string.IsNullOrEmpty(data.ToJson()))
            {
                return null;
            }
            string strJsonData = data.ToJson();
            if (encode)
            {
                strJsonData = AESEncrypt(strJsonData, PTUtil.ENCODE_KEY);
            }
            return strJsonData;
        }

        /// <summary>  
        /// AES加密
        /// </summary>  
        public static string AESEncrypt(string Data, string Key)
        {
            MemoryStream mStream = new MemoryStream();

            byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
            RijndaelManaged aes = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                Key = Encoding.UTF8.GetBytes(Key),
                IV = Encoding.UTF8.GetBytes(Key)
            };
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }


        /// <summary>  
        /// AES解密 
        /// </summary>  
        public static string AESDecrypt(string Data, string Key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(Data);
            MemoryStream mStream = new MemoryStream(encryptedBytes);
            RijndaelManaged aes = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                Key = Encoding.UTF8.GetBytes(Key),
                IV = Encoding.UTF8.GetBytes(Key)
            };
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        ///  时间戳转DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        public static DateTime Timestamp2DateTime(long timestamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long minStamp = (long)(DateTime.MinValue - startTime).TotalSeconds;
            long maxStamp = (long)(DateTime.MaxValue - startTime).TotalSeconds;
            if (timestamp < minStamp)
                return DateTime.MinValue;
            else if (timestamp > maxStamp)
                return DateTime.MaxValue;
            return startTime.AddSeconds(timestamp);
        }

        /// <summary>
        ///  DateTime转时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        public static long DateTime2Timestamp(DateTime t)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(t - startTime).TotalSeconds;
        }

        /// <summary>
        /// json 数据转时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns>失败返回0</returns>
        public static long JsonData2Timestamp(JsonData data)
        {
            long timestamp = 0;
            if (data.IsInt)
            {
                timestamp = (int)data;
            }
            else if (data.IsLong)
            {
                timestamp = (long)data;
            }
            return timestamp;
        }

        /// <summary>
        /// json 数据转时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns>失败返回0</returns>
        public static DateTime JsonData2DateTime(JsonData data)
        {
            long timestamp = JsonData2Timestamp(data);
            return Timestamp2DateTime(timestamp);
        }

        /// <summary>
        /// Utc时间转时间戳
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static long UtcDateTime2Timestamp(DateTime t)
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1);
            return (long)(t - startTime).TotalSeconds;
        }
        /// <summary>
        /// 时间戳转Utc时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime Timestamp2UtcDateTime(long timestamp)
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1);
            return startTime.AddSeconds(timestamp);
        }

        public static int GetDays(long timestamp)
        {
            int ret = (int)(timestamp / 86400);
            return ret;
        }

        // 两个时间戳是不是同一天, 返回 tsA - tsB 的天数
        public static int IsSameDay(long tsA, long tsB)
        {
            return GetDays(tsA) - GetDays(tsB);
        }

        /// <summary>
        /// json 数据转float
        /// </summary>
        /// <param name="d"></param>
        /// <returns>失败返回0</returns>
        public static float JsonData2Float(JsonData data)
        {
            float val = 0.0f;
            if (!float.TryParse(data.ToString(), out val))
            {
                UnityEngine.Debug.LogError("JsonData2Float Error: " + data.ToString());
            }

            return val;

        }

        /// <summary>  
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9_]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>  
        /// 判断输入的字符串是否只包含数字和英文字母和下划线
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsNumAndEnCh_(string input)
        {
            string pattern = @"^[A-Za-z0-9_]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// Strings the is all number.
        /// </summary>
        /// <returns><c>true</c>, if is all number was strung, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public static bool StringIsAllNumber(string str)
        {
            //  char.IsNumber()是个系统函数，检测字符内的数是否为数字
            foreach (char cc in str)
            {
                if (!char.IsNumber(cc))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Strings the is all letter.
        /// </summary>
        /// <returns><c>true</c>, if is all letter was strung, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public static bool StringIsAllLetter(string str)
        {
            //  char.IsNumber()是个系统函数，检测字符内的数是否为数字
            foreach (char cc in str)
            {
                if (!char.IsLetter(cc))
                {
                    return false;
                }
            }
            return true;
        }

        public static T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        // 获取设备相关信息
        public static DeviceInfo GetDeviceInfo()
        {
            DeviceInfo info = new DeviceInfo();

#if UNITY_IPHONE
			info.vendoridentfier = UnityEngine.iOS.Device.vendorIdentifier;   // 应用厂商id 每个应用唯一
			info.system_version = UnityEngine.iOS.Device.systemVersion; // 系统版本号
#else
            info.vendoridentfier = "";
            info.system_version = "";
#endif
            info.device_model = SystemInfo.deviceModel; // 设备模型
            info.device_name = SystemInfo.deviceName;  // 设备名称
            info.device_type = SystemInfo.deviceType.ToString(); // 设备类型（PC电脑，掌上型)
            info.device_uniqueidentifier = SystemInfo.deviceUniqueIdentifier; // 设备的唯一标识符
            info.operating_system = SystemInfo.operatingSystem; // 操作系统
            return info;
        }

        // 删除目录
        public static void DeleteDirectory(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                DirectoryInfo[] childs = dir.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    child.Delete(true);
                }
                dir.Delete(true);
            }
        }

        // 创建目录
        public static void CreateDirectory(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        //获取百分数
        public static string GetPercentStr(float num, int demical)
        {
            double retNum = Math.Round(num * 100, demical);
            string retStr = retNum.ToString() + "%";
            return retStr;
        }

        //获取时间字符串
        public static string GetTimeStr(int time)
        {
            int sec = time % 60;
            int min = (time / 60) % 60;
            int hour = time / 3600;
            string timeStr = "";
            if (hour > 0)
            {
                timeStr = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
            }
            else
            {
                timeStr = string.Format("{0:00}:{1:00}", min, sec);
            }
            return timeStr;
        }

        //获取金币字符串
        public static string GetGoldStr(int gold)
        {
            int kUit = 1000;
            int mUnit = kUit * kUit;

            float mValue = (gold * 1.0f) / mUnit;
            string str = string.Empty;
            if (mValue >= 1.0f)
            {
                str = mValue.ToString("f1") + "M";
            }
            else
            {
                float kValue = (gold * 1.0f) / kUit;
                if (kValue >= 1.0f)
                {
                    str = kValue.ToString("f1") + "K";
                }
                else
                {
                    str = gold.ToString();
                }
            }
            return str;
        }

        //浮点数取余
        public static double Fmode(double x, double y)
        {
            double fmode = x - (int)(x / y) * y;
            return fmode;
        }

        //获取设计分辨率适配后的宽
        public static float GetDesignAdaptWidth()
        {
            return 720.0f;
        }


        public static float GetDesignAdaptHeight()
        {
            float ratio = (Screen.width * 1.0f) / Screen.height;
            float designHeight = GetDesignAdaptWidth() / ratio;
            return designHeight;
        }

        /// <summary>
        /// 计算字符串在指定text控件中的长度
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int CalculateLengthOfText(string message, Text tex)
        {
            int totalLength = 0;
            Font myFont = tex.font;  //chatText is my Text component
            myFont.RequestCharactersInTexture(message, tex.fontSize, tex.fontStyle);
            CharacterInfo characterInfo = new CharacterInfo();

            char[] arr = message.ToCharArray();

            foreach (char c in arr)
            {
                myFont.GetCharacterInfo(c, out characterInfo, tex.fontSize);

                totalLength += characterInfo.advance;
            }

            return totalLength;
        }

        public static Vector3 ForeachMultiply(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                result[i] = v1[i] * v2[i];
            }

            return result;
        }
    }
}
