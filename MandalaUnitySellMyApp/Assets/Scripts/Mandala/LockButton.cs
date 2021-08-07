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


public class LockButton : MonoBehaviour
{

    public void UnlockShade()
    {
        if (AdsManager.unlockingInProgress)
            return;
        GlobalVariables.unlockShade = true;
        GlobalVariables.shadeToUnlock = transform.parent.GetSiblingIndex();
        StartUnlockForVideo();
    }

    public void UnlockSticker()
    {
        if (AdsManager.unlockingInProgress)
            return;
        GlobalVariables.unlockShade = false;
        GlobalVariables.stickerToUnlock = transform.parent.GetSiblingIndex();
        StartUnlockForVideo();
    }

    void StartUnlockForVideo()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AdsManager.Instance.ShowVideoReward(GlobalVariables.videoRewardID);
#elif UNITY_EDITOR
        Debug.Log("Unlocked from editor");
        AdsManager.Instance.VideoRewarded();
#endif
    }
}
