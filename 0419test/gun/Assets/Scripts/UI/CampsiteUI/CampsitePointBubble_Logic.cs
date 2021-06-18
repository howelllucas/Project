using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using DG.Tweening;

namespace EZ
{
    public partial class CampsitePointBubble
    {
        public Color autoColor = Color.green;
        public Color nonAutoColor = Color.red;

        public int DataIndex
        {
            get
            {
                if (pointDataMgr == null)
                    return -1;
                return pointDataMgr.index;
            }
        }

        private CampsitePointMgr pointDataMgr;
        private FollowNode followNode;
        private DOTweenAnimation nonAutoAnim;

        public void Init(int pointIndex, Transform followNodeTrans)
        {
            pointDataMgr = CampsiteMgr.singleton.GetPointByIndex(pointIndex);
            followNode = GetComponent<FollowNode>();
            followNode.SetFloowNode(followNodeTrans);
            nonAutoAnim = RewardNode.gameObject.GetComponent<DOTweenAnimation>();

            UnlockBtn.button.onClick.AddListener(OnUnlockBtnClick);
            RewardBtn.button.onClick.AddListener(OnRewardBtnClick);
            CardBtn.button.onClick.AddListener(OnCardBtnClick);
            RefreshData();
        }

        private void OnEnable()
        {
            RegisterListeners();
            RefreshData();
        }

        private void OnDisable()
        {
            UnRegisterListeners();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CampsitePointDataChange, OnPointDataChange);
            Global.gApp.gMsgDispatcher.AddListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, OnPointDataChange);
            Global.gApp.gMsgDispatcher.RemoveListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void RefreshData()
        {
            if (pointDataMgr == null)
                return;
            if (pointDataMgr.isFrozen)
            {
                LockRoot.gameObject.SetActive(false);
                UnlockRoot.gameObject.SetActive(false);
            }
            else
            {
                if (!pointDataMgr.isUnlock)
                {
                    LockRoot.gameObject.SetActive(true);
                    UnlockRoot.gameObject.SetActive(false);
                }
                else
                {
                    LockRoot.gameObject.SetActive(false);
                    UnlockRoot.gameObject.SetActive(true);
                    var reward = pointDataMgr.SettlementRewardVal;
                    if (reward > 0)
                    {
                        RewardCount.text.text = UiTools.FormateMoney(reward);
                        RewardNode.gameObject.SetActive(true);
                    }
                    else
                    {
                        RewardNode.gameObject.SetActive(false);
                    }

                    if (pointDataMgr.equipGunId > 0)
                    {
                        var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(pointDataMgr.equipGunId);
                        CardIconImg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(cardRes.icon);
                        CardIconImg.gameObject.SetActive(true);
                    }
                    else
                    {
                        CardIconImg.gameObject.SetActive(false);
                    }

                    if (pointDataMgr.isAuto)
                    {
                        if (nonAutoAnim.tween.IsPlaying())
                        {
                            nonAutoAnim.DORewind();
                        }
                        ProgressImg.image.color = autoColor;
                    }
                    else
                    {
                        ProgressImg.image.color = nonAutoColor;
                    }

                    if (PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.BuildSetGun)
                        && CampsiteMgr.singleton.PointCanSetGun(pointDataMgr.index))
                    {
                        RedPoint.gameObject.SetActive(true);
                    }
                    else
                    {
                        RedPoint.gameObject.SetActive(false);
                    }

                    RewardFlag.gameObject.SetActive(!pointDataMgr.isFight);
                }
            }


        }

        private void Update()
        {
            if (pointDataMgr == null)
                return;
            if (pointDataMgr.isUnlock)
            {
                ProgressImg.image.fillAmount = pointDataMgr.Progress;
                if (!pointDataMgr.isAuto && pointDataMgr.Progress >= 1f && !nonAutoAnim.tween.IsPlaying())
                {
                    nonAutoAnim.DOPlay();
                }
            }
        }

        private void OnPointDataChange(int index)
        {
            if (pointDataMgr != null && pointDataMgr.index == index)
            {
                RefreshData();
            }
        }

        private void OnModuleOpen(GameModuleType module)
        {
            if (module == GameModuleType.BuildSetGun)
                RefreshData();
        }

        private void OnUnlockBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampsiteUnlockUI, pointDataMgr.index.ToString());
        }

        private void OnRewardBtnClick()
        {
            if (pointDataMgr != null)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, GoodsType.CAMPSITE_REWARD, 5, RewardBtn.gameObject.transform.position);

                CampsiteMgr.singleton.ClaimPointReward(pointDataMgr.index);

            }
        }

        private void OnCardBtnClick()
        {
            if (pointDataMgr != null)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampsitePointUI, pointDataMgr.index.ToString());
            }
        }
    }
}