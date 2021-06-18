using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
using LitJson;

namespace Game
{
    public static class ExtensionClass
    {
        public static void DebugTime(this object mono, string title, bool bflag = false)
        {
#if GAME_DEBUG
            System.TimeSpan ts = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ret;
            if (bflag)
                ret = System.Convert.ToInt64(ts.TotalSeconds);
            else
                ret = System.Convert.ToInt64(ts.TotalMilliseconds);
            Debug.Log(title + ret);
#endif
        }

        /// <summary>
		/// Gets the time stamp.
		/// </summary>
		/// <returns>The time stamp.</returns>
		public static long GetTimeStampInSecond(this object mono)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 查找所有子物体
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindAllChild(this Transform root, string name)
        {
            if (root.name.Equals(name))
                return root;
            for (int i = 0; i < root.childCount; i++)
            {
                var result = root.GetChild(i).FindAllChild(name);
                if (result != null)
                    return result;
            }
            return null;
        }

        #region float扩展
        /// <summary>
        /// 归一化(将与整数相差小于0.0001f的值归一化为整数)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float Normalize(this float data)
        {
            int nData = Mathf.RoundToInt(data);
            if (data - nData <= 0.0001f)
                return nData;

            return data;
        }
        /// <summary>
        /// 近似相等 误差0.0001f
        /// </summary>
        /// <param name="own"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool ApproximatelyEqual(this float own, float other)
        {
            return Mathf.Abs(own - other) <= 0.0001f;
        }

        #endregion

        #region string扩展
        /// <summary>
        /// 提取string中括号内的内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="left">左括号</param>
        /// <param name="right">右括号</param>
        /// <returns></returns>
        public static string ExtractBracketStr(this string content, string left, string right)
        {
            int leftIndex = content.IndexOf(left);
            int rightIndex = content.IndexOf(right);
            int length = rightIndex - 1 - leftIndex;
            if (leftIndex < 0 || rightIndex < 0 || length <= 0)
            {
                throw new System.Exception(string.Format("括号数据提取失败:{0},{1},{2}", content, left, right));
            }

            return content.Substring(leftIndex + 1, length);
        }
        #endregion

        #region 整数类型扩展
        private static string[] integerSymbols = new string[] { "k", "m", "b", "t", "aa", "bb", "cc", "dd", "ee", "ff", "gg" };
        private static string[] integerFormats = new string[] { "{0:#}{1}", "{0:#0.#}{1}", "{0:#0.##}{1}", "{0:#0.###}{1}" };

        /// <summary>
        /// 转换为符号格式字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits">保留小数位数</param>
        /// <param name="minSymbolValue">value>=minSymbolValue时才转换为格式字符串</param>
        /// <returns></returns>
        public static string ToSymbolString(this int value, int digits = 1, int minSymbolValue = 10000)
        {
            int sign = 1;
            var absValue = value;
            if (value < 0)
            {
                sign = -1;
                absValue = -value;
            }

            minSymbolValue = Mathf.Min(minSymbolValue, 1000000);
            if (absValue < minSymbolValue)
                return value.ToString();
            digits = Mathf.Clamp(digits, 0, integerFormats.Length - 1);
            int index = -1;
            while (absValue >= 1000000)
            {
                index++;
                absValue /= 1000;
            }

            float showValue = (int)absValue;
            while (showValue >= 1000)
            {
                showValue *= 0.001f;
                index++;
            }

            showValue *= sign;

            if (index < integerSymbols.Length)
            {
                return string.Format(integerFormats[digits], showValue, integerSymbols[index]);
            }
            else
            {
                int overflow = index - integerSymbols.Length;
                char first = (char)(65 + overflow / 26);
                char last = (char)(65 + overflow % 26);
                return string.Format(integerFormats[digits], showValue, first.ToString() + last.ToString());
            }
        }
        /// <summary>
        /// 转换为符号格式字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits">保留小数位数</param>
        /// <param name="minSymbolValue">value>=minSymbolValue时才转换为格式字符串</param>
        /// <returns></returns>
        public static string ToSymbolString(this long value, int digits = 1, int minSymbolValue = 10000)
        {
            int sign = 1;
            var absValue = value;
            if (value < 0)
            {
                sign = -1;
                absValue = -value;
            }

            minSymbolValue = Mathf.Min(minSymbolValue, 1000000);
            if (absValue < minSymbolValue)
                return value.ToString();
            digits = Mathf.Clamp(digits, 0, integerFormats.Length - 1);
            int index = -1;
            while (absValue >= 1000000)
            {
                index++;
                absValue /= 1000;
            }

            float showValue = (int)absValue;
            while (showValue >= 1000)
            {
                showValue *= 0.001f;
                index++;
            }

            showValue *= sign;

            if (index < integerSymbols.Length)
            {
                return string.Format(integerFormats[digits], showValue, integerSymbols[index]);
            }
            else
            {
                int overflow = index - integerSymbols.Length;
                char first = (char)(65 + overflow / 26);
                char last = (char)(65 + overflow % 26);
                return string.Format(integerFormats[digits], showValue, first.ToString() + last.ToString());
            }
        }
        /// <summary>
        /// 转换为符号格式字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits">保留小数位数</param>
        /// <param name="minSymbolValue">value>=minSymbolValue时才转换为格式字符串</param>
        /// <returns></returns>
        public static string ToSymbolString(this BigInteger value, int digits = 1, int minSymbolValue = 10000)
        {
            int sign = 1;
            var absValue = value;
            if (value < 0)
            {
                sign = -1;
                absValue = -value;
            }
            minSymbolValue = Mathf.Min(minSymbolValue, 1000000);
            if (absValue < minSymbolValue)
                return value.ToString();
            digits = Mathf.Clamp(digits, 0, integerFormats.Length - 1);
            int index = -1;
            while (absValue >= 1000000)
            {
                index++;
                absValue /= 1000;
            }

            float showValue = (int)absValue;
            while (showValue >= 1000)
            {
                showValue *= 0.001f;
                index++;
            }

            showValue *= sign;

            if (index < integerSymbols.Length)
            {
                return string.Format(integerFormats[digits], showValue, integerSymbols[index]);
            }
            else
            {
                int overflow = index - integerSymbols.Length;
                char first = (char)(65 + overflow / 26);
                char last = (char)(65 + overflow % 26);
                return string.Format(integerFormats[digits], showValue, first.ToString() + last.ToString());
            }
        }
        /// <summary>
        /// 获取掉落展示时的数量
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetPropEffectValue(this BigInteger value)
        {
            return value > 30 ? 30 : (int)value;
        }

        #endregion

        #region LitJson扩展
        public static bool TryGetJsonData(this JsonData target, string key, out JsonData data)
        {
            data = null;
            if (!target.IsObject)
                return false;
            if (!target.Keys.Contains(key))
                return false;
            data = target[key];
            return true;
        }

        public static bool TryGetBooleanVal(this JsonData target, string key, out bool val)
        {
            JsonData json;
            bool result = target.TryGetJsonData(key, out json);
            if (result && json.IsBoolean)
            {
                val = (bool)json;
                return true;
            }
            val = default;
            return false;
        }

        public static bool TryGetDoubleVal(this JsonData target, string key, out double val)
        {
            JsonData json;
            bool result = target.TryGetJsonData(key, out json);
            if (result && json.IsDouble)
            {
                val = (double)json;
                return true;
            }
            val = default;
            return false;
        }

        public static bool TryGetFloatVal(this JsonData target, string key, out float val)
        {
            JsonData json;
            bool result = target.TryGetJsonData(key, out json);
            if (result && json.IsDouble)
            {
                return float.TryParse(json.ToString().Trim(),
                    System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands,
                    System.Globalization.CultureInfo.InvariantCulture, out val);
            }
            val = default;
            return false;
        }

        public static bool TryGetIntVal(this JsonData target, string key, out int val)
        {
            JsonData json;
            bool result = target.TryGetJsonData(key, out json);
            if (result && json.IsInt)
            {
                val = (int)json;
                return true;
            }
            val = default;
            return false;
        }

        public static bool TryGetLongVal(this JsonData target, string key, out long val)
        {
            JsonData json;
            bool result = target.TryGetJsonData(key, out json);
            if (result)
            {
                if (json.IsInt)
                {
                    val = (int)json;
                    return true;
                }
                if (json.IsLong)
                {
                    val = (long)json;
                    return true;
                }
            }
            val = default;
            return false;
        }

        public static bool TryGetStringVal(this JsonData target, string key, out string val)
        {
            JsonData json;
            bool result = target.TryGetJsonData(key, out json);
            if (result)
            {
                val = json.ToString();
                return true;
            }
            val = default;
            return false;
        }

        #endregion
    }

}