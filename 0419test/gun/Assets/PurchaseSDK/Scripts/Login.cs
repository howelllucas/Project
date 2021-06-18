using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class Login : MonoBehaviour
{
    static private Login MainObject = null;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        MainObject = this;
#if UNITY_IOS || UNITY_IPHONE
                    CheckLoginGame("your deviceid", "your gameid", "[\"your_productid1\",\"your_productid2\",\"your_productid3\"]");
#elif UNITY_ANDROID
                    CheckLoginGame("your deviceid", "your gameid", "[\"your_productid1\",\"your_productid2\",\"your_productid3\"]");
#endif
    }

    static public void CheckLoginGame(string deviceid, string gameid, string productid)
    {
        MainObject.StartCoroutine(CheckLogin(deviceid, gameid, productid));
    }

    static public IEnumerator CheckLogin(string m_deviceId, string m_gameId, string m_productid)
    {
        ClientData.m_secenid = System.Guid.NewGuid().ToString().Substring(0, 10);
        UnityWebRequest webrequest = UnityWebRequest.Get("your login url");

        webrequest.SetRequestHeader("device", m_deviceId);

#if UNITY_ANDROID || UNITY_PC_DEBUG || UNITY_EDITOR_WIN
        webrequest.SetRequestHeader("platform", "11");
#elif UNITY_IOS || UNITY_IPHONE
        webrequest.SetRequestHeader("platform", "8");
#endif
        webrequest.SetRequestHeader("timestamp", GetTimeStamp());
        webrequest.SetRequestHeader("scene", ClientData.m_secenid);
        webrequest.SetRequestHeader("protocol", "1.0.00");
        webrequest.SetRequestHeader("gameid", m_gameId);
        webrequest.SetRequestHeader("platformkind", "0");
        webrequest.timeout = 10;
        yield return webrequest.Send();

        if (!webrequest.isNetworkError)
        //if (!webrequest.isError)
        {
            if (webrequest.responseCode == 200)
            {
                Dictionary<string, object> serverResponse = MiniJSON.Json.Deserialize(webrequest.downloadHandler.text) as Dictionary<string, object>;
                if (Convert.ToString(serverResponse["status"]) == "ok")
                {
                    ClientData.m_playerid = Convert.ToString(serverResponse["id"]);
                    ClientData.m_token = Convert.ToString(serverResponse["token"]);

#if UNITY_IOS || UNITY_IPHONE
                    SdkdsPurchaseUtils.InitPurchaseSDK("your pay url", ClientData.m_playerid, "0", m_productid, m_gameId, ClientData.m_token,ClientData.m_secenid);
#elif UNITY_ANDROID
                    SdkdsPurchaseUtils.InitPurchaseSDK("your pay url", ClientData.m_playerid, "0", m_productid, m_gameId, ClientData.m_token,ClientData.m_secenid);
#endif
                    //TODO：登陆成功

                }
                else
                {
                    //TODO：登录失败

                }
            }
        }
        webrequest.Dispose();
    }
    public static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }
}
