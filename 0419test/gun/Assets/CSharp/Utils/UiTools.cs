
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EZ
{

    public class UiTools
    {
        private static string[] m_UnitOfMeasurement = new string[]{"k", "m", "b", "t", "aa", "bb", "cc", "dd", "ee", "ff", "gg"};
        public static Vector2 WorldToRectPos(GameObject node,Vector3 worldPos,RectTransform parentRectTsf = null)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
            if(parentRectTsf == null)
            {
                Canvas parentCanvas = node.GetComponentInParent<Canvas>();
                parentRectTsf = parentCanvas.GetComponent<RectTransform>();
            }
            Vector2 areaPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTsf, screenPoint,Global.gApp.gUICameraCmpt, out areaPos);
            return areaPos;
        }
        public static Vector3 ScreenToUiWorld(Vector3 pos)
        {
            Vector3 worldPos = Global.gApp.gUICameraCmpt.ScreenToWorldPoint(pos);
            return worldPos;
        }
        //public static string FormateMoneyUP(double money)
        //{
        //    if (money >= 1000000000000000000000d)
        //    {
        //        return ((money / 1000000000000000000d).ToString("f0") + "ab");
        //    }
        //    if (money >= 1000000000000000000d)
        //    {
        //        return ((money / 1000000000000000d).ToString("f0") + "aa");
        //    }
        //    if (money >= 1000000000000000d)
        //    {
        //        return ((money / 1000000000000d).ToString("f0") + "t");
        //    }
        //    else if (money >= 1000000000000d)
        //    {
        //        return ((money / 1000000000d).ToString("f0") + "b");
        //    }
        //    else if (money >= 1000000000d)
        //    {
        //        return ((money / 1000000d).ToString("f0") + "m");
        //    }
        //    else if (money >= 1000000d)
        //    {
        //        return ((money / 1000d).ToString("f0") + "k");
        //    }
        //    else
        //    {
        //        return money.ToString("f0");
        //    }
        //}
        //public static string FormateMoney(double money)
        //{
        //    if (money >= 100000000000000000000d)
        //    {
        //        return ((money / 1000000000000000000d).ToString("f1") + "ab");
        //    }
        //    if (money >= 10000000000000000000d)
        //    {
        //        return ((money / 1000000000000000000d).ToString("f2") + "ab");
        //    }
        //    if (money >= 1000000000000000000d)
        //    {
        //        return ((money / 1000000000000000000d).ToString("f3") + "ab");
        //    }
        //    if (money >= 100000000000000000d)
        //    {
        //        return ((money / 1000000000000000d).ToString("f1") + "aa");
        //    }
        //    if (money >= 10000000000000000d)
        //    {
        //        return ((money / 1000000000000000d).ToString("f2") + "aa");
        //    }
        //    if (money >= 1000000000000000d)
        //    {
        //        return ((money / 1000000000000000d).ToString("f3") + "aa");
        //    }
        //    if (money >= 100000000000000d)
        //    {
        //        return ((money / 1000000000000d).ToString("f1") + "t");
        //    }
        //    if (money >= 10000000000000d)
        //    {
        //        return ((money / 1000000000000d).ToString("f2") + "t");
        //    }
        //    if (money >= 1000000000000d)
        //    {
        //        return ((money / 1000000000000d).ToString("f3") + "t");
        //    }
        //    if (money >= 100000000000d){
        //        return ((money / 1000000000d).ToString("f1") + "b"); 
        //    }
        //    else if(money >= 10000000000d)
        //    {
        //        return ((money / 1000000000d).ToString("f2") + "b");
        //    }
        //    else if(money >= 1000000000d)
        //    {
        //        return ((money / 1000000000d).ToString("f3") + "b");
        //    }
        //    else if(money >= 100000000d)
        //    {
        //        return ((money / 1000000d).ToString("f1") + "m");
        //    }
        //    else if(money >= 10000000d)
        //    {
        //        return ((money / 1000000d).ToString("f2") + "m");
        //    }
        //    else if(money >= 1000000d)
        //    {
        //        return ((money / 1000000d).ToString("f3") + "m");
        //    }
        //    else if(money >= 100000d)
        //    {
        //        return ((money / 1000d).ToString("f1") + "k");
        //    }
        //    else if(money >= 10000d)
        //    {
        //        return ((money / 1000d).ToString("f2") + "k");
        //    }
        //    else
        //    {
        //        return money.ToString("f0");
        //    }
        //}

        public static string FormateMoney(double money)
        {
            if (money < 10000d)
            {
                return money.ToString("f0");
            }
            double n = Math.Log(money, 10);
            int ni = (int)Math.Floor(n);
            int index = ni / 3;
            int remainder = ni % 3;
            string result;
            if (index > m_UnitOfMeasurement.Length)
            {
                result = ((money / Math.Pow(10, index)).ToString("f0") + m_UnitOfMeasurement[m_UnitOfMeasurement.Length - 1]);
            }
            double tmp = Math.Pow(10, index * 3);
            result = ((money / tmp).ToString("f" + (3 - remainder)) + m_UnitOfMeasurement[index - 1]);
            return result;
        }

        public static string FormateMoneyUP(double money)
        {
            if (money < 1000000d)
            {
                return money.ToString("f0");
            }
            double n = Math.Log(money, 10);
            int ni = (int)Math.Floor(n);
            int index = ni / 3;
            index -= 1;
            int remainder = ni % 3;
            string result;
            if (index > m_UnitOfMeasurement.Length)
            {
                result = ((money / Math.Pow(10, index)).ToString("f0") + m_UnitOfMeasurement[m_UnitOfMeasurement.Length - 1]);
            }
            double tmp = Math.Pow(10, index * 3);
            result = ((money / tmp).ToString("f0") + m_UnitOfMeasurement[index - 1]);
            return result;
        }

        public static GameObject GetEffect(string path, Transform transform)
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(path);
            effect.transform.SetParent(transform, false);
            List<Transform> trans = new List<Transform>();
            effect.GetComponentsInChildren<Transform>(trans);
            int layer = LayerMask.NameToLayer("UI");
            foreach (Transform t in trans)
            {
                t.gameObject.layer = layer;
            }
            return effect;
        }

        public static GameObject GetEffect(string path, Transform transform, int order)
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(path);
            effect.transform.SetParent(transform, false);
            List<Transform> trans = new List<Transform>();
            effect.GetComponentsInChildren<Transform>(trans);
            int layer = LayerMask.NameToLayer("UI");
            foreach (Transform t in trans)
            {
                t.gameObject.layer = layer;
            }
            ParticleSystem[] pss = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in pss)
            {
                ps.GetComponent<Renderer>().sortingOrder = order;
            }
            return effect;
        }

        public static GameObject GetEffect(string path, Transform transform, int order, Vector3 vector3)
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(path);
            effect.transform.SetParent(transform, false);
            effect.transform.localPosition = vector3;
            List<Transform> trans = new List<Transform>();
            effect.GetComponentsInChildren<Transform>(trans);
            int layer = LayerMask.NameToLayer("UI");
            foreach (Transform t in trans)
            {
                t.gameObject.layer = layer;
            }
            ParticleSystem[] pss = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in pss)
            {
                ps.GetComponent<Renderer>().sortingOrder = order;
            }
            return effect;
        }

        public static GameObject GetEffect(string path, Transform transform, Vector3 vector3)
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(path);
            effect.transform.localPosition = vector3;
            effect.transform.localScale = new Vector3(1f, 1f, 1f);
            effect.transform.SetParent(transform);
            List<Transform> trans = new List<Transform>();
            effect.GetComponentsInChildren<Transform>(trans);
            int layer = LayerMask.NameToLayer("UI");
            foreach (Transform t in trans)
            {
                t.gameObject.layer = layer;
            }
            return effect;
        }

        /// <summary>
        /// 判断是否是数字 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static string FormatTime(float time)
        {
            int intVal = (int)time;
            int pointVal = (int)((time - intVal) * 100.0f);
            string pointStr;
            if (pointVal >= 10)
            {
                pointStr = pointVal.ToString();
            }
            else
            {
                pointStr = "0" + pointVal.ToString();
            }
            string intValStr = EZMath.FormateTime(intVal);
            return intValStr + "." + pointStr;
        }
        public static string GetLanguage()
        {
            //string lan = SdkdsNativeUtil.Instance.GetLanguage();
            //Debug.Log("SdkdsNativeUtil.Instance.GetLanguage() = " + lan);
            //string[] pms = lan.Split('-');
            //if (pms[0] != null && !pms[0].Equals(GameConstVal.EmepyStr))
            //{
            //    if (pms[0].Equals("zh"))
            //    {
            //        string country = SdkdsNativeUtil.Instance.GetCountry();
            //        string[] cpms = country.Split('_');
            //        Debug.Log("SdkdsNativeUtil.Instance.GetCountry() = " + cpms[cpms.Length - 1]);
            //        if (cpms[cpms.Length - 1].Equals("TW") || cpms[cpms.Length - 1].Equals("MO") || cpms[cpms.Length - 1].Equals("HK"))
            //        {
            //            return "zh_tw";
            //        }
            //    }
            //    return pms[0];
            //}
            //else
            {
                return "en";
            }
        }
    }
}
