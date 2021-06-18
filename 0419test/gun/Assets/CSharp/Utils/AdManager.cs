using System;
using UnityEngine;

namespace EZ
{
    // 看广告统计
    public enum AdShowSceneType
    {
        UNDIFINEND = 0,//未定义
        REVIVE = 1,//复活
        BALANCE = 2,//结算
        SHOP_GET_MDT = 3,//商城得狗牌 
        NO_MDT_GET = 4,//没有狗牌打开界面得狗牌 
        SEVEN_DAY = 5,//七日登录双倍
        GET_REWARD = 6,//降落伞
        GET_ENERGY = 7,//得体力
        SHOP_GET_ENERGY = 8,//商城得体力
        LEVEL_DETAIL_THREE = 9,//关卡奖励3倍
        SHOP_GET_BOX = 10,//商城开箱子
        CAMP_GET_HEART = 11,//营地看广告得红心

    }


    public class AdManager 
    {
        private static AdManager mInstance;
        private Action<bool> mVideoCall = null;
        private int mAdShowSceneType = 0;


        public static AdManager instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new AdManager();
                    mInstance.InitListener();
                }
                return mInstance;
            }
        }

        public void InitListener()
        {
            //SDKDSAdmobManager.Instance.SDKDSAdmobRewardVideoOnAdClosed += AdVideoClosed;
            //SDKAdsManager.SDKAdsRewardVideoOnAdClosed += AdVideoClosed;
        }

        /// <summary>
        /// 展示激励视频
        /// </summary>
        /// <param name="scene1">广告展示的场景，游戏自定义</param>
        /// <param name="cid">关卡id     游戏自定义的关卡，整形，没有则填写默认值0</param>
        /// <param name="completion">1：关卡完成  2：关卡失败，没有则填写默认值0</param>
        /// <param name="retry_show">该展示是否来自重试：1重试，2非重试。  例如广告展示失败后等待几秒再次尝试展示广告的逻辑属于重试展示.   没有则填写默认值0</param>
        public void ShowRewardVedio(Action<bool> fCall, AdShowSceneType scene1, int cid = 0, int completion = 0, int retry_show = 0)
        {
            
            //if (!SDKAdsManager.ShowRewardedVideoAd((int)scene1, cid, completion, retry_show))
            //{
            //    fCall(false);
            //    Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3039);
            //    mVideoCall = null;
            //    mAdShowSceneType = 0;
            //    return;
            //}

            mVideoCall = fCall;
            mAdShowSceneType = (int)scene1;
        }

        private void AdVideoStart(object sender, EventArgs args)
        {

            //Global.gApp.gAudioSource.audioVolumnTmp = Global.gApp.gAudioSource.audioVolumn;
            //Global.gApp.gAudioSource.musicVolumnTmp = Global.gApp.gAudioSource.musicVolumn;

            //Global.gApp.gAudioSource.audioVolumn = 0f;
            //Global.gApp.gAudioSource.musicVolumn = 0f;
            Debug.Log("SDKDSAdmobManager.Instance.ShowRewardedVideoAd = start");
        }

        private void AdVideoClosed(object sender, EventArgs args)
        {
            Debug.Log("AdVideoClosed");
            //if (typeof(SDKBUAdsEventArgs).IsInstanceOfType(args))
            //{
            //    SDKBUAdsEventArgs tempArgs = (SDKBUAdsEventArgs)args;
            //    Debug.Log("admob_log_SDKTest HandleRewardBasedVideoClosed needRewarded: " + tempArgs.needRewarded);
            //    if (mVideoCall != null)
            //    {
            //        mVideoCall(tempArgs.needRewarded);
            //    }
            //    if (!tempArgs.needRewarded)
            //    {
            //        Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3040);
            //    }
            //}
            //else if (typeof(SDKDSAdsCustomEvent).IsInstanceOfType(args))
            //{
            //    SDKDSAdsCustomEvent tempArgs = (SDKDSAdsCustomEvent)args;
            //    Debug.Log("admob_log_SDKTest HandleRewardBasedVideoClosed needRewarded: " + tempArgs.needRewarded);
            //    if (mVideoCall != null)
            //    {
            //        mVideoCall(tempArgs.needRewarded);
            //    }
            //    if (!tempArgs.needRewarded)
            //    {
            //        Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3040);
            //    }
            //} else
            {
                Debug.Log("IsInstanceOfType error");
                mVideoCall(false);
            }
            
            mVideoCall = null;

            //Global.gApp.gAudioSource.audioVolumn = Global.gApp.gAudioSource.audioVolumnTmp;
            //Global.gApp.gAudioSource.musicVolumn = Global.gApp.gAudioSource.musicVolumnTmp;

            //Global.gApp.gAudioSource.audioVolumnTmp = -1f;
            //Global.gApp.gAudioSource.musicVolumnTmp = -1f;
            Global.gApp.gSystemMgr.GetMiscMgr().SumAdTimes = Global.gApp.gSystemMgr.GetMiscMgr().SumAdTimes + 1;
            AdLog();
            mAdShowSceneType = 0;
            Debug.Log("SDKDSAdmobManager.Instance.ShowRewardedVideoAd = close");

        }

        private void AdLog()
        {
            if (mAdShowSceneType != 0)
            {
                switch (mAdShowSceneType)
                {
                    case 1:
                    case 3:
                    case 4:
                        //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_ad_revive);
                        break;
                    case 7:
                    case 8:
                        //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_ad_energy);
                        break;
                    case 2:
                    case 6:
                    case 9:
                        //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_ad_coin);
                        break;
                    case 10:
                        //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_ad_box);
                        break;
                }


            }

            int newV = Global.gApp.gSystemMgr.GetMiscMgr().SumAdTimes;
            //首日
            if (Global.gApp.gSystemMgr.GetMiscMgr().GetCreateDays() == 1)
            {
                if (newV % 5 == 0)
                {
                    //SDKDSAnalyticsManager.trackEvent(string.Format(AFInAppEvents.af_mz_1day_ad, newV));
                }
            }
            //7日
            if (Global.gApp.gSystemMgr.GetMiscMgr().GetCreateDays() <= 6)
            {
                if (newV % 5 == 0)
                {
                    //SDKDSAnalyticsManager.trackEvent(string.Format(AFInAppEvents.af_mz_7day_ad, newV));
                }
            }
        }


        public void ListenAD(int scene1 = 0, int cid = 0, int completion = 0, int retry_show = 0)
        {
            //SDKAdsManager.ShowRewardedVideoAd(scene1, cid, completion, retry_show);
            
        }

        public void DeleteVedioCall()
        {
            mVideoCall = null;
        }
    }
}

