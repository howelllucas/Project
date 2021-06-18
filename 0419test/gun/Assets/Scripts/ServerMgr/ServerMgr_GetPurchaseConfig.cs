using LitJson;
using MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public partial class ServerMgr
    {
        private const string PATH_GetPurchaseConfig = "/rg/getPurchaseConfig";

        public delegate void RequestGetPurchaseConfigCallback(RequestCallbackState state, JsonData configJson);

        private bool isRequesting_GetPurchaseConfig;
        private RequestGetPurchaseConfigCallback callbacks_GetPurchaseConfig;

        public void RequestGetPurchaseConfig(RequestGetPurchaseConfigCallback callback)
        {
            callbacks_GetPurchaseConfig -= callback;
            callbacks_GetPurchaseConfig += callback;

            if (!isRequesting_GetPurchaseConfig)
            {
                isRequesting_GetPurchaseConfig = true;
                RequestGetServerData(PATH_GetPurchaseConfig, HandleGetPurchaseConfigResponse);
            }
        }

        private void HandleGetPurchaseConfigResponse(bool connectOK, UnityWebRequest request)
        {
            if (!connectOK)
            {
                InvokeGetPurchaseConfigCallbacks(RequestCallbackState.NetFail, null);
                return;
            }

            JsonData data;
            if (!ExtractionJsonData(request.downloadHandler.text, out data))
            {
                InvokeGetPurchaseConfigCallbacks(RequestCallbackState.Undefine, null);
                return;
            }

            InvokeGetPurchaseConfigCallbacks(RequestCallbackState.Success, data);
        }

        private void InvokeGetPurchaseConfigCallbacks(RequestCallbackState state, JsonData configJson)
        {
            if (callbacks_GetPurchaseConfig != null)
            {
                callbacks_GetPurchaseConfig.Invoke(state, configJson);
                callbacks_GetPurchaseConfig -= callbacks_GetPurchaseConfig;
            }
            isRequesting_GetPurchaseConfig = false;
        }
    }
}