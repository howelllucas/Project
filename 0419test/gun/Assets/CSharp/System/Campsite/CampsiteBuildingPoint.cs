using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EZ
{
    public class CampsiteBuildingPoint : MonoBehaviour
    {
        public enum CampsitePointState
        {
            Undefine,
            Invalid,
            Valid_Frozen,
            Valid_Locked,
            Valid_Opened,
        }

        private int m_PointId;
        public int PointId { get { return m_PointId; } }

        private CampsitePointState m_state;
        private int m_PointDataIndex;
        public int PointDataIndex { get { return m_PointDataIndex; } }

        private GameObject buildingObj;
        private CampsiteNpcPoint linkNpcPoint;

        public Transform FocusTrans { get { return transform; } }
        public Transform BubbleNode { get { return transform; } }

        private string lockObjPath;
        private Vector3 lockObjEuler;
        private GameObject lockObj;

        private void Awake()
        {
            m_PointId = int.Parse(gameObject.name.Replace("Point", ""));

        }

        private void OnEnable()
        {
            RegisterListeners();
            if (m_state != CampsitePointState.Undefine)
                Refresh();
        }

        private void OnDisable()
        {
            UnRegisterListeners();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CampsitePointDataChange, HandleDataChange);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, HandleDataChange);
        }

        public void SetInvalid()
        {
            m_state = CampsitePointState.Invalid;
            ShowLockObj();
        }

        public void SetValid(int pointDataIndex)
        {
            m_PointDataIndex = pointDataIndex;
        }

        public void SetLinkNpc(CampsiteNpcPoint npcPoint)
        {
            this.linkNpcPoint = npcPoint;
        }

        public void SetLockObjParam(string path,Vector3 euler)
        {
            this.lockObjPath = path;
            this.lockObjEuler = euler;
        }

        public void Init()
        {
            Refresh();
        }

        public void RespondClick()
        {
            if (m_state == CampsitePointState.Valid_Opened)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampsitePointUI, PointDataIndex.ToString());
            }
            else if (m_state == CampsitePointState.Valid_Locked)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampsiteUnlockUI, PointDataIndex.ToString());
            }
            //else if (m_state == CampsitePointState.Valid_Frozen)
            //{
            //    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, "不可解锁");
            //}
        }

        private void Refresh()
        {
            if (m_state == CampsitePointState.Invalid)
                return;
            var pointData = CampsiteMgr.singleton.GetPointByIndex(m_PointDataIndex);
            if (pointData == null)
                return;

            if (pointData.isFrozen)
            {
                m_state = CampsitePointState.Valid_Frozen;
                ShowLockObj();
                return;
            }

            if (!pointData.isUnlock)
            {
                m_state = CampsitePointState.Valid_Locked;
                ShowLockObj();
            }
            else
            {
                if (buildingObj == null)
                {
                    buildingObj = Global.gApp.gResMgr.InstantiateObj(pointData.buildingRes.prefab);
                    buildingObj.transform.SetParent(transform, false);
                }

                if(CampsiteMgr.singleton.NewUnlockPointIndex == PointDataIndex)
                {
                    GameObject unlockEf = Global.gApp.gResMgr.InstantiateObj("Prefabs/Campsite/Building/jianzaoeffect");
                    unlockEf.transform.SetParent(transform, false);
                    unlockEf.transform.localPosition = Vector3.zero;
                    var autoDestroy = unlockEf.AddComponent<DelayDestroy>();
                    autoDestroy.SetLiveTime(5f);

                    buildingObj.transform.localScale = Vector3.zero;
                    buildingObj.transform.DOScale(1.0f, 1f).SetDelay(2f).OnComplete(()=> {
                        buildingObj.transform.localScale = Vector3.one;
                    });

                    CampsiteMgr.singleton.ResetNewUnlockPointIndex();
                }

                if (linkNpcPoint != null)
                {
                    linkNpcPoint.SetEquipGunId(pointData.equipGunId);
                }
                if (pointData.isFight)
                {

                }
                else
                {

                }
                HideLockObj();
                m_state = CampsitePointState.Valid_Opened;
            }
        }

        private void ShowLockObj()
        {
            if (!string.IsNullOrEmpty(lockObjPath) && lockObj == null)
            {
                lockObj = Global.gApp.gResMgr.InstantiateObj(lockObjPath);
                lockObj.transform.SetParent(transform, false);
                lockObj.transform.localEulerAngles = lockObjEuler;
            }
            lockObj?.SetActive(true);
        }

        private void HideLockObj()
        {
            lockObj?.SetActive(false);
        }
        
        private void HandleDataChange(int index)
        {
            if (index != m_PointDataIndex || m_state == CampsitePointState.Undefine)
                return;
            Refresh();
        }
    }
}