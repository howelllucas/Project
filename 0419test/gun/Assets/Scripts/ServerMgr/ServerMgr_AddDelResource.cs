using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

namespace Game
{
    public partial class ServerMgr
    {
        private const string PATH_AddDelResource = "/rg/addDelResource";

        public delegate void RequestAddDelResourceCallback(RequestCallbackState state);

        public void RequestAddDelResource(int[] type_list, int[] count_list, int[] param_list, RequestAddDelResourceCallback callback)
        {
            JsonData dataJson = new JsonData();
            dataJson.SetJsonType(JsonType.Array);
            for (int i = 0; i < type_list.Length; i++)
            {
                if (type_list[i] <= 0)
                    continue;
                JsonData resJson = new JsonData()
                {
                    ["type"] = type_list[i],
                    ["values"] = count_list[i],
                    //[""] = param_list[i],暂时不用
                };
                dataJson.Add(resJson);
            }

            JsonData postJson = new JsonData();
            postJson["addDelResource"] = dataJson;
            string postData = postJson.ToJson();
            Debug.Log(postData);
            RequestPostServerData(PATH_AddDelResource, postData, (connectOK, request) =>
            {
                HandleAddDelResourceResponse(connectOK, request, callback);
            });
        }

        private void HandleAddDelResourceResponse(bool connectOK, UnityWebRequest request, RequestAddDelResourceCallback callback)
        {
            if (!connectOK)
            {
                callback?.Invoke(RequestCallbackState.NetFail);
                return;
            }

            JsonData data;
            if (!ExtractionJsonData(request.downloadHandler.text, out data))
            {
                callback?.Invoke(RequestCallbackState.Undefine);
                return;
            }
            int code;
            if (!ExtractionJsonCode(request.downloadHandler.text, out code))
            {
                callback?.Invoke(RequestCallbackState.Undefine);
                return;
            }
            RefreshServerResource(data);
            if (code == 0)
            {
                callback?.Invoke(RequestCallbackState.Success);
            }
            else if (code == 3)
            {
                callback?.Invoke(RequestCallbackState.DataFail);
            }
            else
            {
                callback?.Invoke(RequestCallbackState.Undefine);
            }
        }
    }
}