
using EZ;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtil
{

    public static string m_DeaultColor = "#FFFFFF";
    public static string m_HalfColor = "#FFFFFF80";
    public static string m_BlackColor = "#000000A0";
    public static string m_WhiteColor = "#8D8D8DA0";
    public static string m_YellowColor = "#FFE96DFF";

    private static Dictionary<string, Color> m_ColorMap = new Dictionary<string, Color> ();
    public static Color GetColor(string colorStr)
    {
        Color color;
        if (!m_ColorMap.TryGetValue(colorStr, out color))
        {
            ColorUtility.TryParseHtmlString(colorStr, out color);
            m_ColorMap.Add(colorStr, color);
        }
        
        return color;
    }


    public static Color GetTextColor(bool isRed, string dftColor)
    {
        string redColor = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.RED_COLOR).content;
        string defaultColor = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.DEFAULT_COLOR).content;

        if (!isRed)
        {
            if (dftColor == null)
            {
                return GetColor(defaultColor);
            }
            else
            {
                return GetColor(dftColor);
            }
        } else
        {
            return GetColor(redColor);
        }
    }

    public static Color GetSpecialColor(bool isSpecial, string redColor)
    {

        if (!isSpecial)
        {
            return GetColor(m_DeaultColor);
        }
        else
        {
            return GetColor(redColor);
        }
    }

    public static Color GetHalfA(Color color)
    {
        Color newColor = new Color(color.r, color.g, color.b, color.a / 2);
        return newColor;
    }

}