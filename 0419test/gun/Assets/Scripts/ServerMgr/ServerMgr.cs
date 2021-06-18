using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;

namespace Game
{
    public partial class ServerMgr : Singleton<ServerMgr>
    {
#if TEST_SERVER
        public const string URL = "https://apitest.yuechuan.net";
#else
        public const string URL = "https://gdcasapi.yuechuan.net";
#endif
        public const int PLATFORM = 8;//8-ios 11-version_android 19-version_wp

        private string REQUEST_ROOT_URL { get { return URL + "/checkorder"; } }


        private bool logged = false;

        public Int64 GameID { get; private set; }
        public string UserID
        {
            get; private set;
        }
        public string Token
        {
            get; private set;
        }
        public string SceneID
        {
            get; private set;
        }

        public delegate void ServerResponseCallback(bool connectOK, UnityWebRequest request);

        private Action<bool> loginCallback;

        public void Init(Int64 gameId)
        {
            this.GameID = gameId;
            KeepAccountUtils.loginDataAct += HandleLoginDataAct;
            KeepAccountUtils.conflictDataAct += HandleLoginDataAct;
        }

        public void Login(Action<bool> callback)
        {
            loginCallback = callback;
            KeepAccountUtils.GetInstance().Init(1, GameID, SdkdsNativeUtil.Instance.GetDeviceId(), PLATFORM, false, 1, URL);
        }

        private void HandleLoginDataAct(GameDataResponse data)
        {
            if (data.code == 0)
            {
                var jsonData = JsonMapper.ToObject(data.data.Trim());
                UserID = jsonData["userId"].ToString();
                Token = jsonData["token"].ToString();
                SceneID = jsonData["sceneId"].ToString();
                logged = true;
                Debug.Log("login complete json:" + data.data);
                //同步存档数据 data.gameData
                PlayerDataMgr.singleton.SyncServerData(data.gameData);
                loginCallback?.Invoke(true);
            }
            else
            {
                loginCallback?.Invoke(false);
            }
        }

        private void RequestPostServerData(string relativePath, string postData, ServerResponseCallback onResponse)
        {
            if (logged)
            {
                IEnumeratorHelper.Start(RequestPostServerDataIE(relativePath, postData, onResponse));
            }
        }

        private IEnumerator RequestPostServerDataIE(string relativePath, string postData, ServerResponseCallback onResponse)
        {
            string url = REQUEST_ROOT_URL + relativePath;
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.SetRequestHeader("userId", UserID);
                request.SetRequestHeader("scene", SceneID);
                request.SetRequestHeader("token", Token);
                request.SetRequestHeader("gameid", GameID.ToString());
                request.SetRequestHeader("Content-type", "application/json; charset=utf-8");

                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.LogError(request.error);
                    onResponse?.Invoke(false, request);
                }
                else
                {
                    Debug.Log("server post callback url:" + url + "| json:" + request.downloadHandler.text);
                    onResponse?.Invoke(true, request);
                }
            }
        }

        private void RequestGetServerData(string relativePath, ServerResponseCallback onResponse)
        {
            if (logged)
            {
                IEnumeratorHelper.Start(RequestGetServerDataIE(relativePath, onResponse));
            }
        }

        private IEnumerator RequestGetServerDataIE(string relativePath, ServerResponseCallback onResponse)
        {
            string url = REQUEST_ROOT_URL + relativePath;
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("userId", UserID);
                request.SetRequestHeader("scene", SceneID);
                request.SetRequestHeader("token", Token);
                request.SetRequestHeader("gameid", GameID.ToString());
                request.SetRequestHeader("Content-type", "application/json; charset=utf-8");

                yield return request.SendWebRequest();
                if (request.isNetworkError)
                {
                    Debug.LogError(request.error);
                    onResponse?.Invoke(false, request);
                }
                else
                {
                    Debug.Log("server get callback url:" + url + "| json:" + request.downloadHandler.text);
                    onResponse?.Invoke(true, request);
                }
            }
        }

        private bool ExtractionJsonCode(string jsonStr, out int code)
        {
            code = -1;
            JsonData jsonData = JsonMapper.ToObject(jsonStr.Trim());
            if (jsonData == null)
                return false;
            return jsonData.TryGetIntVal("code", out code);
        }

        private bool ExtractionJsonData(string jsonStr, out JsonData dataJson)
        {
            dataJson = null;
            JsonData jsonData = JsonMapper.ToObject(jsonStr.Trim());
            if (jsonData == null)
                return false;
            return jsonData.TryGetJsonData("data", out dataJson);
        }

        public enum RequestCallbackState
        {
            Undefine,
            Success,
            DataFail,
            NetFail,
        }

        private Dictionary<CurrencyType, string> currencyResDic = new Dictionary<CurrencyType, string>()
        {
            [CurrencyType.DIAMOND] = "diamond"
        };

        private void RefreshServerResource(JsonData jsonData)
        {
            if (jsonData == null)
                return;
            foreach (var currency in currencyResDic)
            {
                int val;
                if (jsonData.TryGetIntVal(currency.Value, out val))
                {
                    PlayerDataMgr.singleton.SetCurrency(currency.Key, val, false);
                }
            }
        }
    }
}