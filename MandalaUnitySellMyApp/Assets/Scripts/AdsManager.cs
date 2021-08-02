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
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

/**
  * Scene:All
  * Object:AdsManager
  * Description: Manager class used for showing ads and rewarding player
  **/
public class AdsManager : MonoBehaviour
{

    #region AdMob

    [Header("Admob")]
    public string adMobAppID = "";
    // this is test ID
    public string interstitalAdMobId = "ca-app-pub-3940256099942544/1033173712";
    // this is test ID
    public string videoAdMobId = "ca-app-pub-3940256099942544/5224354917";
    InterstitialAd interstitialAdMob;
    private RewardBasedVideoAd rewardBasedAdMobVideo;
    AdRequest requestAdMobInterstitial, AdMobVideoRequest;

    #endregion

    [Space(15)]
    #region
    [Header("UnityAds")]
    public string unityAdsGameId = "";
    public string unityAdsVideoPlacementId = "rewardedVideo";

    #endregion

    public static bool unlockingInProgress = false;
    public static bool isEvaluating = false;
    public static GameObject itemToUnlock;

    static bool rewardPlayer = false;

    static AdsManager instance;

    public static AdsManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(AdsManager)) as AdsManager;

            return instance;
        }
    }

    void Awake()
    {
        gameObject.name = this.GetType().Name;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeAds();
    }

    /// <summary>
    /// Shows the interstitial.
    /// </summary>
    /// <param name="ID">ID can be used if you have different ad placements.</param>
    public void ShowInterstitial(int ID)
    {
        ShowAdMob();
    }

    public void IsVideoRewardAvailable(int ID)
    {
        Debug.Log("Is video reward available?");
        if (isVideoAvaiable())
        {
            Debug.Log("Video reward is available!");
            VideoRewardIsReady();
        }
        else
        {
            Debug.Log("Video reward is not available!");
            VideoRewardNotReady();
        }
    }

    public void ShowVideoReward(int ID)
    {
        rewardPlayer = false;

        if (Advertisement.IsReady(unityAdsVideoPlacementId))
        {
            UnityAdsShowVideo();
        }
        else if (rewardBasedAdMobVideo.IsLoaded())
        {
            AdMobShowVideo();
        }
        else
        {
            VideoRewardNotReady();
        }
    }

    private void RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        interstitialAdMob = new InterstitialAd(interstitalAdMobId);

        // Called when an ad request has successfully loaded.
        interstitialAdMob.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitialAdMob.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitialAdMob.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitialAdMob.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitialAdMob.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        requestAdMobInterstitial = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitialAdMob.LoadAd(requestAdMobInterstitial);
    }

    public void ShowAdMob()
    {
        if (interstitialAdMob.IsLoaded())
        {
            interstitialAdMob.Show();
        }
        else
        {
            interstitialAdMob.LoadAd(requestAdMobInterstitial);
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitialAdMob.LoadAd(requestAdMobInterstitial);
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    private void RequestRewardedVideo()
    {
        // Called when an ad request has successfully loaded.
        rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
        // Called when an ad request failed to load.
        rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
        // Called when an ad is shown.
        rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
        // Called when the ad starts to play.
        rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
        // Called when the user should be rewarded for watching a video.
        rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
        // Called when the ad is closed.
        rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
        // Called when the ad click caused the user to leave the application.
        rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
        // Create an empty ad request.
        AdMobVideoRequest = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
    }

    public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");

    }

    public void HandleRewardBasedVideoFailedToLoadAdMob(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
        VideoRewardNotReady();
    }

    public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
        unlockingInProgress = false;
        if (!rewardPlayer)
            VideoRewardCanceled();
    }

    public void HandleRewardBasedVideoRewardedAdMob(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        rewardPlayer = true;
        VideoRewarded();

    }

    public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
        unlockingInProgress = false;
    }

    void InitializeAds()
    {
        MobileAds.Initialize(adMobAppID);
        this.rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
        this.RequestRewardedVideo();
        Advertisement.Initialize(unityAdsGameId);
        RequestInterstitial();
    }


    void AdMobShowVideo()
    {
        rewardBasedAdMobVideo.Show();   
    }

    void UnityAdsShowVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResultUnity;

        Advertisement.Show(unityAdsVideoPlacementId, options);
    }

    void HandleShowResultUnity(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            rewardPlayer = true;
            VideoRewarded();
            Advertisement.Initialize(unityAdsGameId);
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            if (!rewardPlayer)
                VideoRewardCanceled();
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            VideoRewardNotReady();
        }
    }

    bool isVideoAvaiable()
    {
        Debug.Log("isVideoAvailable");
        #if !UNITY_EDITOR
        if(Advertisement.IsReady(unityAdsVideoPlacementId))
        {
        return true;
        }
        else if(rewardBasedAdMobVideo.IsLoaded())
        {
        return true;
        }
//        #else
//        return true; // Test.
        #endif
        return false;
    }

    void VideoRewardIsReady()
    {
        Debug.Log("VIDEO IS READY");
    }

    void VideoRewardNotReady()
    {
        Debug.Log("VIDEO IS NOT READY");
        unlockingInProgress = false;
        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageTitleText("Sorry");
        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageCustomMessageText("Video is currently unavailable, please try again later");
    }

    void VideoRewardCanceled()
    {
        unlockingInProgress = false;
    }

    public void VideoRewarded()
    {
        Debug.Log("REWARDED ");
        unlockingInProgress = false;
        if (GlobalVariables.unlockShade)
        {
            GlobalVariables.UpdateShadeIndices();
            GameObject.Find("Canvas/SubMenus").GetComponent<SubMenusController>().UnlockCurrentShade();
        }
        else
        {
            GlobalVariables.UpdateStickerIndices();
            GameObject.Find("Canvas/SubMenus").GetComponent<SubMenusController>().UnlockCurrentSticker();
        }
        rewardPlayer = false;
    }
}
