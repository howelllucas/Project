/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class GlobalVariables : MonoBehaviour
{

    //ID-s
    public static string applicationID;
    public static int bannerID = 1;
    public static int startInterstitialID = 2;
    public static int homeResetActionID = 3;
    public static int savePictureActionID = 4;
    public static int exitInterstitialID = 5;
    public static int videoRewardID = 6;

    //Flags
    public static bool removeAds = false;
    public static bool unlockAll = false;

    //Helpers
    public static string[] removeAdsString;
    public static string[] unlockAllString;
    public static Texture2D selectedTexture;
    public static string gameMode;
    public static string sceneToLoad;
    public static int mainSceneLoadCount = 0;

    //LockVariables
    public static bool unlockShade;
    // true if we need to unlock shade, false if we need to unlock sticker
    public static int colorToUnlock;
    public static int shadeToUnlock;
    public static int stickerToUnlock;
    public static int[][] shadesLockIndices = new int[12][];
    // Colors X Shades
    public static int[] stickersLockIndices = new int[65];
    public static string tempLockString;
    public static char[] tempCharArray;
    public static int tempLockInt;

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Load();
    }

    void Load()
    {
        if (PlayerPrefs.HasKey("RemoveAds"))
        {
            if (PlayerPrefs.GetString("RemoveAds") == "true")
            {
                removeAds = true;
            }
        }

        if (PlayerPrefs.HasKey("UnlockAll"))
        {
            if (PlayerPrefs.GetString("UnlockAll") == "true")
            {
                unlockAll = true;
            }
        }
            
        for (int i = 0; i < 12; i++)
        {
            // Loads locked shades form PlayerPrefs for each color
            if (PlayerPrefs.HasKey("Shades_" + i.ToString()))
            {
                tempLockString = PlayerPrefs.GetString("Shades_" + i.ToString());     // Loads string
                tempCharArray = tempLockString.ToCharArray();                       // String to CharArray
                shadesLockIndices[i] = new int[tempCharArray.Length];
                
                // Converts every char to int and assign it corresponding place
                for (int j = 0; j < tempCharArray.Length; j++)
                {
                    tempLockInt = int.Parse(tempCharArray[j].ToString());   
                    shadesLockIndices[i][j] = tempLockInt;
                }
                //              Debug.Log("Shades stanje : " + tempLockString);
            }
            else
            {
                shadesLockIndices[i] = new int[] { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0 };     // If there is not PlayerPrefs string, we set default lock key
            }
        }

        if (PlayerPrefs.HasKey("Stickers"))
        {
            tempLockString = PlayerPrefs.GetString("Stickers");
            tempCharArray = tempLockString.ToCharArray();
            stickersLockIndices = new int[tempCharArray.Length];
            for (int j = 0; j < tempCharArray.Length; j++)
            {
                tempLockInt = int.Parse(tempCharArray[j].ToString());   
                stickersLockIndices[j] = tempLockInt;
            }
        }
        else
        {
            for (int i = 0; i < stickersLockIndices.Length; i++)
            {
                if (i % 3 == 0)
                    stickersLockIndices[i] = 1;
                else
                    stickersLockIndices[i] = 0;
            }
        }

        SaveStickerIndices();
        SaveShadesIndices();
    }

    public static void SaveShadesIndices()
    {
        for (int i = 0; i < shadesLockIndices.Length; i++)
        {
            tempLockString = "";
            for (int j = 0; j < shadesLockIndices[i].Length; j++)
            {
                tempLockString += shadesLockIndices[i][j].ToString();
            }
            PlayerPrefs.SetString("Shades_" + i.ToString(), tempLockString);
        }
        PlayerPrefs.Save();
    }

    public static void SaveStickerIndices()
    {
        tempLockString = "";
        for (int i = 0; i < stickersLockIndices.Length; i++)
        {
            tempLockString += stickersLockIndices[i].ToString();
        }
        PlayerPrefs.SetString("Stickers", tempLockString);
        PlayerPrefs.Save();
    }

    public static void UpdateShadeIndices()
    {
        shadesLockIndices[colorToUnlock][shadeToUnlock] = 0;
        SaveShadesIndices();
    }

    public static void UpdateStickerIndices()
    {
        stickersLockIndices[stickerToUnlock] = 0;
        SaveStickerIndices();
    }
}



















