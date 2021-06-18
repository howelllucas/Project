using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Game.Util;
using LitJson;

namespace Game
{
    public partial class ServerMgr
    {
        private const string PATH_GetResource = "/rg/getResource";

        public delegate void RequestGetResourceCallback(RequestCallbackState state);

        private bool isRequesting_GetResource;
        private RequestGetResourceCallback callbacks_GetResource;

        public void RequestGetResource(RequestGetResourceCallback callback)
        {
            callbacks_GetResource -= callback;
            callbacks_GetResource += callback;

#if SERVER_CHECK
            if (!isRequesting_GetResource)
            {
                isRequesting_GetResource = true;
                RequestGetServerData(PATH_GetResource, HandleGetResourceResponse);
            }
#else
            InvokeGetResourceCallbacks(RequestCallbackState.Success);
#endif
        }

        private void HandleGetResourceResponse(bool connectOK, UnityWebRequest request)
        {
            if (!connectOK)
            {
                InvokeGetResourceCallbacks(RequestCallbackState.NetFail);
                return;
            }

            JsonData data;
            if (!ExtractionJsonData(request.downloadHandler.text, out data))
            {
                InvokeGetResourceCallbacks(RequestCallbackState.Undefine);
                return;
            }

            RefreshServerResource(data);
            InvokeGetResourceCallbacks(RequestCallbackState.Success);
        }

        private void InvokeGetResourceCallbacks(RequestCallbackState state)
        {
            if (callbacks_GetResource != null)
            {
                callbacks_GetResource.Invoke(state);
                callbacks_GetResource -= callbacks_GetResource;
            }
            isRequesting_GetResource = false;
        }

    }
}