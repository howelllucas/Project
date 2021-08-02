/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// Copyright (C) 2015 Google, Inc.
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

#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Android
{
    public class AdLoaderClient : AndroidJavaProxy, IAdLoaderClient
    {
        private AndroidJavaObject adLoader;
        private Dictionary<string, Action<CustomNativeTemplateAd, string>> CustomNativeTemplateCallbacks
        {
            get; set;
        }
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;
        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        public AdLoaderClient(AdLoader unityAdLoader) : base(Utils.UnityAdLoaderListenerClassName)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
            AndroidJavaObject activity =
                playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            adLoader = new AndroidJavaObject(Utils.NativeAdLoaderClassName, activity,
                unityAdLoader.AdUnitId, this);

            this.CustomNativeTemplateCallbacks = unityAdLoader.CustomNativeTemplateClickHandlers;

            if (unityAdLoader.AdTypes.Contains(NativeAdType.CustomTemplate))
            {
                foreach (string templateId in unityAdLoader.TemplateIds)
                {
                    adLoader.Call("configureCustomNativeTemplateAd", templateId,
                        this.CustomNativeTemplateCallbacks.ContainsKey(templateId));
                }
            }
            adLoader.Call("create");
        }

        public void LoadAd(AdRequest request)
        {
            adLoader.Call("loadAd", Utils.GetAdRequestJavaObject(request));
        }

        public void onCustomTemplateAdLoaded(AndroidJavaObject ad)
        {
            if (this.OnCustomNativeTemplateAdLoaded != null)
            {
                CustomNativeEventArgs args = new CustomNativeEventArgs()
                {
                    nativeAd = new CustomNativeTemplateAd(new CustomNativeTemplateClient(ad))
                };
                this.OnCustomNativeTemplateAdLoaded(this, args);
            }
        }

        void onAdFailedToLoad(string errorReason)
        {
            AdFailedToLoadEventArgs args = new AdFailedToLoadEventArgs()
            {
                Message = errorReason
            };
            OnAdFailedToLoad(this, args);
        }

        public void onCustomClick(AndroidJavaObject ad, string assetName)
        {
            CustomNativeTemplateAd nativeAd = new CustomNativeTemplateAd(
                    new CustomNativeTemplateClient(ad));
            this.CustomNativeTemplateCallbacks[nativeAd.GetCustomTemplateId()](nativeAd, assetName);
        }
    }
}

#endif
