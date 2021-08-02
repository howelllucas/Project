/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// Copyright (C) 2016 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_IOS

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.iOS
{
    [StructLayout(LayoutKind.Sequential),Serializable]
    // The System.Boolean (bool in C#) type is special. A bool within a structure is marshaled in a different format
    // than when passed as an argument to a function (4-byte integer vs 2-byte integer, non zero = true vs -1 = true).
    // Using ints instead for simplicity.
    public struct NativeAdTypes
    {
        public int CustomTemplateAd;
    }

    public class AdLoaderClient : IAdLoaderClient, IDisposable
    {
        private IntPtr adLoaderPtr;
        private IntPtr adLoaderClientPtr;
        private NativeAdTypes adTypes;

        private Dictionary<string, Action<CustomNativeTemplateAd, string>>
            customNativeTemplateCallbacks;

        public AdLoaderClient(AdLoader unityAdLoader)
        {
            this.adLoaderClientPtr = (IntPtr)GCHandle.Alloc(this);

            this.customNativeTemplateCallbacks = unityAdLoader.CustomNativeTemplateClickHandlers;
            string[] templateIdsArray = new string[unityAdLoader.TemplateIds.Count];
            unityAdLoader.TemplateIds.CopyTo(templateIdsArray);

            this.adTypes = new NativeAdTypes();
            if (unityAdLoader.AdTypes.Contains(NativeAdType.CustomTemplate))
            {
                adTypes.CustomTemplateAd = 1;
            }

            this.AdLoaderPtr = Externs.GADUCreateAdLoader(
                this.adLoaderClientPtr,
                unityAdLoader.AdUnitId,
                templateIdsArray,
                templateIdsArray.Length,
                ref adTypes);

            Externs.GADUSetAdLoaderCallbacks(
                this.AdLoaderPtr,
                AdLoaderDidReceiveNativeCustomTemplateAdCallback,
                AdLoaderDidFailToReceiveAdWithErrorCallback);
        }

        internal delegate void GADUAdLoaderDidReceiveNativeCustomTemplateAdCallback(
            IntPtr adLoader, IntPtr nativeCustomTemplateAd, string templateID);

        internal delegate void GADUAdLoaderDidFailToReceiveAdWithErrorCallback(
            IntPtr AdLoader, string error);

        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        // This property should be used when setting the adLoaderPtr.
        private IntPtr AdLoaderPtr
        {
            get
            {
                return this.adLoaderPtr;
            }

            set
            {
                Externs.GADURelease(this.adLoaderPtr);
                this.adLoaderPtr = value;
            }
        }

        public void LoadAd(AdRequest request)
        {
            IntPtr requestPtr = Utils.BuildAdRequest(request);
            Externs.GADURequestNativeAd(this.AdLoaderPtr, requestPtr);
            Externs.GADURelease(requestPtr);
        }

        // Destroys the AdLoader.
        public void DestroyAdLoader()
        {
            this.AdLoaderPtr = IntPtr.Zero;
        }

        public void Dispose()
        {
            this.DestroyAdLoader();
            ((GCHandle)this.adLoaderClientPtr).Free();
        }

        ~AdLoaderClient()
        {
            this.Dispose();
        }

        [MonoPInvokeCallback(typeof(GADUAdLoaderDidReceiveNativeCustomTemplateAdCallback))]
        private static void AdLoaderDidReceiveNativeCustomTemplateAdCallback(
            IntPtr adLoader, IntPtr nativeCustomTemplateAd, string templateID)
        {
            AdLoaderClient client = IntPtrToAdLoaderClient(adLoader);
            Action<CustomNativeTemplateAd, string> clickHandler =
                    client.customNativeTemplateCallbacks.ContainsKey(templateID) ?
                    client.customNativeTemplateCallbacks[templateID] : null;

            if (client.OnCustomNativeTemplateAdLoaded != null)
            {
                CustomNativeEventArgs args = new CustomNativeEventArgs()
                {
                    nativeAd = new CustomNativeTemplateAd(new CustomNativeTemplateClient(
                        nativeCustomTemplateAd, clickHandler))
                };
                client.OnCustomNativeTemplateAdLoaded(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADUAdLoaderDidFailToReceiveAdWithErrorCallback))]
        private static void AdLoaderDidFailToReceiveAdWithErrorCallback(
            IntPtr adLoader, string error)
        {
            AdLoaderClient client = IntPtrToAdLoaderClient(adLoader);
            if (client.OnAdFailedToLoad != null)
            {
                AdFailedToLoadEventArgs args = new AdFailedToLoadEventArgs()
                {
                    Message = error
                };
                client.OnAdFailedToLoad(client, args);
            }
        }

        private static AdLoaderClient IntPtrToAdLoaderClient(IntPtr adLoader)
        {
            GCHandle handle = (GCHandle)adLoader;
            return handle.Target as AdLoaderClient;
        }
    }
}

#endif
