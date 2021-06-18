using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.InteropServices;
using AOT;
using System.Threading;



public enum HttpRequestType
{
    GET = 1,
    POST = 2,
    GETFILE = 3,
    POSTFILE = 4
};

public enum EventCmd
{
    EventHttp = 1,
    EventGameData = 2,
    EventLoginOk = 3,
    EventConflict = 4,
    EventLastAccount = 5,
    EventRemoveAccount = 6,
    EventGetDeviceAndSocialCoreData = 7,
    EventGetGamecenterData = 8,
    EventGetMessageBox = 9,
    EventGetReward = 10,
    EventGetMessageBoxStatus = 11
};

public struct HttpResponse
{
    public int retCode;
    public string resp;
    public string url;
    public string key;

    public void setData(byte[] buffer, int start)
    {
        int pos = start;

        retCode = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        Int32 len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        byte[] dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        resp = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;

        len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        url = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;

        len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        key = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;
    }
};

public struct GameConstData
{
    public string userId;
    public string sceneId;
    public string token;

    public void setData(byte[] buffer, int start)
    {
        int pos = start;

        Int32 len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        byte[] dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        userId = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;

        len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        sceneId = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;

        len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        token = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;


    }
}

public struct GameDataResponse
{
    public int code;
    public string data;
    public string message;
    //public string notify;
    public string gameData;

    public void setData(byte[] buffer, int start)
    {
        int pos = start;
        code = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        Int32 len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        byte[] dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        data = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;

        len = BitConverter.ToInt32(buffer, pos);
        pos += 4;

        dataBuffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            dataBuffer[i] = buffer[i + pos];
        }

        message = System.Text.Encoding.UTF8.GetString(dataBuffer);
        pos += len;

        //len = BitConverter.ToInt32(buffer, pos);
        //pos += 4;

        //dataBuffer = new byte[len];
        //for (int i = 0; i < len; i++)
        //{
        //    dataBuffer[i] = buffer[i + pos];
        //}

        //notify = System.Text.Encoding.UTF8.GetString(dataBuffer);
        //pos += len;
    }
};

public class KeepAccountUtils : MonoBehaviour
{
    private GameObject obj;
    private string methodName;

    const string dllFileName = "serversdk";



#if UNITY_IPHONE && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void addHttpTask(string key, HttpRequestType type, string url, string[] headers, string postData, string filename, Int32 retryCount);

    [DllImport("__Internal")]
    public static extern bool init(Int32 type, byte[] path, Int64 gameId, byte[] device, Int32 platform, bool crossPlatform, Int32 serverType, byte[] url);

    [DllImport("__Internal")]
    public static extern void loginGameBySelf();

    [DllImport("__Internal")]
    public static extern bool bindAccount(string device, Int32 type);

    [DllImport("__Internal")]
    public static extern void doOfflineOperation(Int32 operationCode, byte[] pBuffer, Int32 bufferSize);

    [DllImport("__Internal")]
    public static extern void doOfflineOperationByPurchase(byte[] id, byte[] transactionId, Int32 parameter);

    [DllImport("__Internal")]
    public static extern void doOfflineOperationByList(byte[] key, byte[] val, bool isAdd, byte[] pBuffer);

    [DllImport("__Internal")]
    public static extern void doOfflineOperationByKey(byte[] key, byte[] val, byte[] pBuffer);

    [DllImport("__Internal")]
    public static extern Int32 getEvent(byte[] pBuffer, Int32 bufferSize);

    [DllImport("__Internal")]
    public static extern Int32 storeUserDefineData(byte[] pData, byte[] pDefaultDataDetail);

    [DllImport("__Internal")]
    public static extern Int32 getGameConstData(byte[] pData, Int32 bufferSize);

    [DllImport("__Internal")]
    public static extern void checkLogin();

    [DllImport("__Internal")]
    public static extern bool bindAccountByMulti(byte[] device, Int32 type);

    [DllImport("__Internal")]
    public static extern void getLastAccount(byte[] DeviceId,  Int32 devicePlatform,  Int32 deviceType,  Int32 deviceUserId, byte[] GCId,  Int32 gameId,  Int32 socailType,  Int32 socialPlatform,  Int32 socialUserId);

    [DllImport("__Internal")]
    public static extern void removeAccount(byte[] deviceId,Int32 gameId,Int32 deviceType,Int32 devicePlatform);

    [DllImport("__Internal")]
    public static extern void getDeviceAndSocialCoreData(byte[] deviceId,Int32 platform,Int32 type);

    [DllImport("__Internal")]
    public static extern void getGamecenterData(Int32 gameId,byte[] gamecenterId,Int32 gcPlatform,Int32 gcType);

    [DllImport("__Internal")]
    public static extern void getMessageBox(Int32 versionCode, Int32 platform, Int64 userId,Int32 language);

    [DllImport("__Internal")]
    public static extern void getMessageReward(Int64 userId, Int32 messageBoxId);

    [DllImport("__Internal")]
    public static extern void setMessageStatus(Int64 userId, byte[] messageBoxList,Int32 status);

#else
    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void addHttpTask(string key, HttpRequestType type, string url, string[] headers, string postData, string filename, Int32 retryCount);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool init(Int32 type, byte[] path, Int64 gameId, byte[] device, Int32 platform, bool crossPlatform, Int32 serverType, byte[] url);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void loginGameBySelf();

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool bindAccount(string device, Int32 type);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void doOfflineOperation(Int32 operationCode, byte[] pBuffer, Int32 bufferSize);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void doOfflineOperationByPurchase(byte[] id, byte[] transactionId, Int32 parameter);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void doOfflineOperationByList(byte[] key, byte[] val, bool isAdd, byte[] pBuffer);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void doOfflineOperationByKey(byte[] key, byte[] val, byte[] pBuffer);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Int32 getEvent(byte[] pBuffer, Int32 bufferSize);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Int32 storeUserDefineData(byte[] pData, byte[] pDefaultDataDetail);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Int32 getGameConstData(byte[] pData, Int32 bufferSize);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void checkLogin();

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool bindAccountByMulti(byte[] device, Int32 type);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void getLastAccount(byte[] DeviceId, Int32 devicePlatform, Int32 deviceType, Int32 deviceUserId, byte[] GCId, Int32 gameId, Int32 socailType, Int32 socialPlatform, Int32 socialUserId);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void removeAccount(byte[] deviceId, Int32 gameId, Int32 deviceType, Int32 devicePlatform);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void getDeviceAndSocialCoreData(byte[] deviceId, Int32 platform, Int32 type);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void getGamecenterData(Int32 gameId, byte[] gamecenterId, Int32 gcPlatform, Int32 gcType);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void getMessageBox(Int32 versionCode, Int32 platform, Int64 userId, Int32 language);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void getMessageReward(Int64 userId, Int32 messageBoxId);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void setMessageStatus(Int64 userId, byte[] messageBoxList, Int32 status);


#endif

    public static event Action<GameDataResponse> gameDataAct;
    public static event Action<GameDataResponse> loginDataAct;
    public static event Action<GameDataResponse> conflictDataAct;
    public static event Action<GameDataResponse> EventLastAccountAct;
    public static event Action<GameDataResponse> EventRemoveAccountAct;
    public static event Action<GameDataResponse> EventGetDeviceAndSocialCoreDataAct;
    public static event Action<GameDataResponse> EventGetGamecenterDataAct;
    public static event Action<GameDataResponse> EventGetMessageBoxAct;
    public static event Action<GameDataResponse> EventGetMessageRewardAct;
    public static event Action<GameDataResponse> EventGetMessageBoxStatusAct;




    private byte[] buffer = null;
    int len = 1024 * 2048;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("KeepAccountUpdate");
        if (buffer == null)
        {
            buffer = new byte[len];
        }
        int ret = getEvent(buffer, 1024 * 2048);
        if (ret == 1)
        {
            Int32 cmd = BitConverter.ToInt32(buffer, 0);

            switch (cmd)
            {
                case (Int32)EventCmd.EventHttp:
                    HttpResponse httpResponse = new HttpResponse();
                    httpResponse.setData(buffer, 4);

                    Debug.Log("http return " + httpResponse.key + " resp " + httpResponse.resp);
                    break;

                case (Int32)EventCmd.EventGameData:
                    //GameDataResponse gameDataResponse = new GameDataResponse();
                    //gameDataResponse.setData(buffer, 4);
                    //Debug.Log("gamedata data " + gameDataResponse.data + " message " + gameDataResponse.message);

                    GameDataCallback();
                    break;

                case (Int32)EventCmd.EventLoginOk:
                    //GameConstDataCallback();
                    LoginDataCallback();
                    break;

                case (Int32)EventCmd.EventConflict:
                    ConflictDataCallback();
                    break;
                case (Int32)EventCmd.EventLastAccount:
                    EventLastAccountCallback();
                    break;
                case (Int32)EventCmd.EventRemoveAccount:
                    EventRemoveAccountCallback();
                    break;
                case (Int32)EventCmd.EventGetDeviceAndSocialCoreData:
                    EventGetDeviceAndSocialCoreDataCallback();
                    break;
                case (Int32)EventCmd.EventGetGamecenterData:
                    EventGetGamecenterDataCallback();
                    break;
                case (Int32)EventCmd.EventGetMessageBox:
                    EventGetMessageBoxCallback();
                    break;
                case (Int32)EventCmd.EventGetReward:
                    EventGetMessageRewardCallback();
                    break;
                case (Int32)EventCmd.EventGetMessageBoxStatus:
                    EventGetMessageBoxStatusCallback();
                    break;


            }
        }

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static KeepAccountUtils Instance;
    public static KeepAccountUtils GetInstance()
    {
        if (Instance == null)
        {
            GameObject gameobject = GameObject.Find("KeepAccountUtils");
            if (gameobject == null)
            {
                gameobject = new GameObject("KeepAccountUtils");
            }
            if (gameobject != null)
            {
                gameobject.AddComponent<KeepAccountUtils>();
            }
        }
        return Instance;
    }

    private string CombineJsonMethod(string userId, string sceneId, string token)
    {
        string loginData = "{\"userId\":\"" + userId + "\",\"sceneId\":\"" + sceneId + "\",\"token\":\"" + token + "\"}";
        return loginData;
    }
    /*
     * 
    //初始化函数
     这个账户在不同平台使用的同样的数据
    */

    public bool Init(int type, Int64 gameId, string device, int platform, bool crossPlatform, int serverType, string url)
    {
        string path = Application.persistentDataPath;

        crossPlatform = false;//不支持跨平台数据不一样

        Debug.Log("Server sdk sdk ini path " + path);
        Debug.Log("Server sdk sdk ini 1");
        Debug.Log("Server sdk sdk ini device " + device);
        Debug.Log("Server sdk sdk ini 2");
        Debug.Log(gameId);

        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();

        Debug.Log("Server sdk sdk ini 3");

        byte[] pPathName = e.GetBytes(path);
        byte[] pTempName = new byte[pPathName.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pPathName.Length; jj++)
        {
            pTempName[jj] = pPathName[jj];
        }
        pTempName[jj] = 0;

        Debug.Log("Server sdk sdk ini 4");

        Encoding e1 = Encoding.GetEncoding("UTF-8");
        e1.Clone();

        Debug.Log("Server sdk sdk ini 5");

        byte[] pDevice = e1.GetBytes(device);
        byte[] pTempDevice = new byte[pDevice.Length + 10];
        jj = 0;
        for (jj = 0; jj < pDevice.Length; jj++)
        {
            pTempDevice[jj] = pDevice[jj];
        }
        pTempDevice[jj] = 0;

        Debug.Log("Server sdk sdk ini 6");

        Encoding e2 = Encoding.GetEncoding("UTF-8");
        e2.Clone();

        Debug.Log("Server sdk sdk ini 7");

        byte[] pUrl = e2.GetBytes(url);
        byte[] pTempUrl = new byte[pUrl.Length + 10];
        jj = 0;
        for (jj = 0; jj < pUrl.Length; jj++)
        {
            pTempUrl[jj] = pUrl[jj];
        }
        pTempUrl[jj] = 0;

        Debug.Log("Server sdk sdk ini 8");

        Debug.Log("Server sdk sdk ini pTempName " + System.Text.Encoding.UTF8.GetString(pTempName));
        Debug.Log("Server sdk sdk ini pTempDevice " + System.Text.Encoding.UTF8.GetString(pTempDevice));
        Debug.Log("Server sdk sdk ini pTempDevice " + System.Text.Encoding.UTF8.GetString(pTempUrl));

        bool bResult = init(type, pTempName, gameId, pTempDevice, platform, crossPlatform, serverType, pTempUrl);

        Debug.Log("Server sdk sdk ini return " + bResult);

        return bResult;
    }

    public bool BindData(string device, int type)
    {
        return bindAccount(device, type);
    }

    public enum g_LoginType
    {
        LOGIN_TYPE_DEVICE = 0,
        LOGIN_TYPE_FACEBOOK = 1,
        LOGIN_TYPE_GOOGLE_PLAY = 2,
        LOGIN_TYPE_APPLE = 3,
        LOGIN_TYPE_PHONE = 4


    };


    public int getLastType(int type)
    {
        int lastType = 0;
        switch (type)
        {
            case 1:
                lastType = (int)g_LoginType.LOGIN_TYPE_DEVICE;
                break;
            case 2:
                lastType = (int)g_LoginType.LOGIN_TYPE_FACEBOOK;
                break;
            case 3:
                lastType = (int)g_LoginType.LOGIN_TYPE_GOOGLE_PLAY;
                break;
            case 4:
                lastType = (int)g_LoginType.LOGIN_TYPE_APPLE;
                break;
            case 5:
                lastType = (int)g_LoginType.LOGIN_TYPE_PHONE;
                break;
        }
        return lastType;
    }

    public bool BindDataByMulti(string device, int type)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pDevice = e.GetBytes(device);
        byte[] pTempDevice = new byte[pDevice.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pDevice.Length; jj++)
        {
            pTempDevice[jj] = pDevice[jj];
        }
        pTempDevice[jj] = 0;

        return bindAccountByMulti(pTempDevice, type);
    }

    public void LoginGameBySelf()
    {
        loginGameBySelf();
    }

    public void CheckLogin()
    {
        checkLogin();
    }

    public void GetLastAccount(string deviceId, int devicePlatform, int deviceType, int deviceUserId, string gamecenterId, int gameId, int socailType, int socialPlatform, int socialUserId)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pGCId = e.GetBytes(gamecenterId);
        byte[] pTempGCId = new byte[pGCId.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pGCId.Length; jj++)
        {
            pTempGCId[jj] = pGCId[jj];
        }
        pTempGCId[jj] = 0;

        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pDeviceId = ee.GetBytes(deviceId);
        byte[] pTempDeviceId = new byte[pDeviceId.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pDeviceId.Length; ii++)
        {
            pTempDeviceId[ii] = pDeviceId[ii];
        }
        pTempDeviceId[ii] = 0;

        getLastAccount(pTempDeviceId, devicePlatform, getLastType(deviceType), deviceUserId, pTempGCId, gameId, getLastType(socailType), socialPlatform, socialUserId);
    }

    public void RemoveAccount(string deviceId, Int32 gameId, Int32 deviceType, Int32 devicePlatform)
    {
        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pDeviceId = ee.GetBytes(deviceId);
        byte[] pTempDeviceId = new byte[pDeviceId.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pDeviceId.Length; ii++)
        {
            pTempDeviceId[ii] = pDeviceId[ii];
        }
        pTempDeviceId[ii] = 0;

        removeAccount(pTempDeviceId, gameId, getLastType(deviceType), devicePlatform);
    }

    public void GetDeviceAndSocialCoreData(string deviceId, int platform, int type)
    {
        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pDeviceId = ee.GetBytes(deviceId);
        byte[] pTempDeviceId = new byte[pDeviceId.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pDeviceId.Length; ii++)
        {
            pTempDeviceId[ii] = pDeviceId[ii];
        }
        pTempDeviceId[ii] = 0;

        getDeviceAndSocialCoreData(pTempDeviceId, platform, getLastType(type));
    }

    public void GetGamecenterData(Int32 gameId, string gamecenterId, Int32 gcPlatform, Int32 gcType)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pId = e.GetBytes(gamecenterId);
        byte[] pTempId = new byte[pId.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pId.Length; jj++)
        {
            pTempId[jj] = pId[jj];
        }
        pTempId[jj] = 0;
        getGamecenterData(gameId, pTempId, gcPlatform, getLastType(gcType));
    }

    //支付操作
    public void DoOfflineOperationByPurchase(string id, string transactionId, int parameter)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pId = e.GetBytes(id);
        byte[] pTempId = new byte[pId.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pId.Length; jj++)
        {
            pTempId[jj] = pId[jj];
        }
        pTempId[jj] = 0;

        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pTransactionId = ee.GetBytes(transactionId);
        byte[] pTempTransactionId = new byte[pTransactionId.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pTransactionId.Length; ii++)
        {
            pTempTransactionId[ii] = pTransactionId[ii];
        }
        pTempTransactionId[ii] = 0;

        doOfflineOperationByPurchase(pTempId, pTempTransactionId, parameter);
    }

    public GameDataResponse DoOfflineOperationByList(string key, string val, bool isAdd)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pKey = e.GetBytes(key);
        byte[] pTempKey = new byte[pKey.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pKey.Length; jj++)
        {
            pTempKey[jj] = pKey[jj];
        }
        pTempKey[jj] = 0;

        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pVal = ee.GetBytes(val);
        byte[] pTempVal = new byte[pVal.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pVal.Length; ii++)
        {
            pTempVal[ii] = pVal[ii];
        }
        pTempVal[ii] = 0;

        if (buffer == null)
        {
            buffer = new byte[len];
        }

        //byte[] ys = new byte[2048 * 1024];
        for (int i = 0; i < len; i++)
            buffer[i] = 0;

        GameDataResponse gameDataResponse = new GameDataResponse();
        doOfflineOperationByList(pTempKey, pTempVal, isAdd, buffer);
        gameDataResponse.setData(buffer, 0);
        return gameDataResponse;
    }

    public GameDataResponse DoOfflineOperationByKey(string key, string val)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pKey = e.GetBytes(key);
        byte[] pTempKey = new byte[pKey.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pKey.Length; jj++)
        {
            pTempKey[jj] = pKey[jj];
        }
        pTempKey[jj] = 0;

        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pVal = ee.GetBytes(val);
        byte[] pTempVal = new byte[pVal.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pVal.Length; ii++)
        {
            pTempVal[ii] = pVal[ii];
        }
        pTempVal[ii] = 0;

        if (buffer == null)
        {
            buffer = new byte[len];
        }

        //byte[] ys = new byte[2048 * 1024];
        for (int i = 0; i < len; i++)
            buffer[i] = 0;

        GameDataResponse gameDataResponse = new GameDataResponse();
        doOfflineOperationByKey(pTempKey, pTempVal, buffer);
        gameDataResponse.setData(buffer, 0);
        return gameDataResponse;
    }

    //调用操作id
    public GameDataResponse DoOfflineOperation(int operationCode)
    {
        byte[] ys = new byte[2048];
        doOfflineOperation(operationCode, ys, 2048);
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(ys, 0);
        return gameDataResponse;
    }
    //调用操作id
    public GameDataResponse DoStoreOperation(string jsonData)
    {
        byte[] byteArray = System.Text.Encoding.Default.GetBytes(jsonData);
        //int len = 1024 * 2048;
        //byte[] ys = new byte[len];

        if (buffer == null)
        {
            buffer = new byte[len];
        }

        for (int i = 0; i < len; i++)
            buffer[i] = 0;

        Array.Copy(byteArray, buffer, byteArray.Length);
        buffer[byteArray.Length] = 0;

        doOfflineOperation(32760, buffer, len);

        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 0);
        return gameDataResponse;
    }
    //加载游戏数据，id默认为-1
    public GameDataResponse LoadGameData()
    {
        if (buffer == null)
        {
            buffer = new byte[len];
        }

        for (int i = 0; i < len; i++)
            buffer[i] = 0;
        //byte[] ys = new byte[1024 * 2048];
        doOfflineOperation(-1, buffer, 1024 * 2048);
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 0);
        return gameDataResponse;
    }
    //数据刷新函数，回调接口
    private void GameDataCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);

        //notify game to handle response.
        gameDataAct(gameDataResponse);
    }
    //-999冲突回调接口
    private void ConflictDataCallback()
    {
        GameDataResponse conflictDataResponse = new GameDataResponse();
        conflictDataResponse.setData(buffer, 4);

        //notify game to handle response.
        conflictDataAct(conflictDataResponse);
    }
    //数据上报函数
    public int StoreUserDefineData(string pData, string pDefaultDataDetail)
    {
        Encoding e = Encoding.GetEncoding("UTF-8");
        e.Clone();
        byte[] pDataName = e.GetBytes(pData);
        byte[] pTempDataName = new byte[pDataName.Length + 10];
        int jj = 0;
        for (jj = 0; jj < pDataName.Length; jj++)
        {
            pTempDataName[jj] = pDataName[jj];
        }
        pTempDataName[jj] = 0;
        Encoding ee = Encoding.GetEncoding("UTF-8");
        ee.Clone();
        byte[] pDefaultDataDetailName = ee.GetBytes(pDefaultDataDetail);
        byte[] pTempDefaultDataDetailName = new byte[pDefaultDataDetailName.Length + 10];
        int ii = 0;
        for (ii = 0; ii < pDefaultDataDetailName.Length; ii++)
        {
            pTempDefaultDataDetailName[ii] = pDefaultDataDetailName[ii];
        }
        pTempDefaultDataDetailName[ii] = 0;

        int state = storeUserDefineData(pTempDataName, pTempDefaultDataDetailName);
        return state;
    }
    //登录回调接口
    private void LoginDataCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);

        GameDataResponse loginDataResponse = new GameDataResponse();

        if (gameDataResponse.code != 0)
        {
            loginDataResponse.code = gameDataResponse.code;
            loginDataResponse.data = "";
            loginDataResponse.message = "Login Failed!";
            Debug.Log("login exception : " + gameDataResponse.data);
            Debug.Log("login exception : " + gameDataResponse.message);
        }
        else
        {
            byte[] ys = new byte[2048];
            getGameConstData(ys, 2048);
            GameConstData gameConstData = new GameConstData();
            gameConstData.setData(ys, 0);

            loginDataResponse.code = gameDataResponse.code;
            loginDataResponse.data = CombineJsonMethod(gameConstData.userId, gameConstData.sceneId, gameConstData.token);
            loginDataResponse.message = "Login Successful!";
            loginDataResponse.gameData = gameDataResponse.data;
            //loginDataResponse.notify = gameDataResponse.notify;
        }

        loginDataAct(loginDataResponse);
    }

    private void EventLastAccountCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);
        EventLastAccountAct(gameDataResponse);
    }

    private void EventRemoveAccountCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);
        EventRemoveAccountAct(gameDataResponse);
    }
    private void EventGetDeviceAndSocialCoreDataCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);
        EventGetDeviceAndSocialCoreDataAct(gameDataResponse);
    }
    private void EventGetGamecenterDataCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);
        EventGetGamecenterDataAct(gameDataResponse);
    }
    private void EventGetMessageBoxCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);
        EventGetMessageBoxAct(gameDataResponse);
    }
    private void EventGetMessageRewardCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);
        EventGetMessageRewardAct(gameDataResponse);
    }


    private void EventGetMessageBoxStatusCallback()
    {
        GameDataResponse gameDataResponse = new GameDataResponse();
        gameDataResponse.setData(buffer, 4);

        EventGetMessageBoxStatusAct(gameDataResponse);
    }









}
