using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using EZ;
using BitBenderGames;
using System.Linq;

namespace Game
{
    public class CampsiteObjectMgr : InnerSingleton<CampsiteObjectMgr>
    {
        private Camera mainCam;
        public MobileTouchCamera camTouchCtrl { get; private set; }
        public TouchInputController camTouchInputCtrl { get; private set; }
        public FocusCameraOnItem camFocusCtrl { get; private set; }
        private Dictionary<int, CampsiteBuildingPoint> buildingDic = new Dictionary<int, CampsiteBuildingPoint>();

        private bool hasCamRecord;
        private Vector3 recordCamPos;
        private Quaternion recordCamRot;

        public void SetFocusBuilding(int dataIndex, Vector2 focusScreenPos, float duration = 0.5f)
        {
            if (camFocusCtrl == null)
                return;

            FocusBuilding(dataIndex, focusScreenPos, duration);
        }

        private void FocusBuilding(int dataIndex, Vector2 focusScreenPos, float duration)
        {
            if (camFocusCtrl == null)
                return;
            CampsiteBuildingPoint building;
            if (buildingDic.TryGetValue(dataIndex, out building))
            {
                camFocusCtrl.FocusScreenPos = focusScreenPos;
                camFocusCtrl.TransitionDuration = duration;
                camFocusCtrl.FocusCameraOnTransform(building.FocusTrans);
            }
        }

        public void CancelFocus(float duration = 0.5f)
        {
            if (camFocusCtrl == null)
                return;
            camFocusCtrl.FocusScreenPos = new Vector2(0.5f, 0.5f);
            camFocusCtrl.TransitionDuration = duration;
            camFocusCtrl.ResetFocus();
        }

        public CampsiteBuildingPoint[] GetAllBuildings()
        {
            return buildingDic.Values.ToArray();
        }

        public void InitCampsiteObjects(GameObject m_FightNode)
        {
            #region Cam
            mainCam = Camera.main;
            Global.gApp.gGameAdapter.AdaptCamera(ref mainCam);
            camTouchCtrl = mainCam.GetComponent<MobileTouchCamera>();
            camTouchInputCtrl = mainCam.GetComponent<TouchInputController>();
            camFocusCtrl = mainCam.GetComponent<FocusCameraOnItem>();
            ApplyCamRecord();
            #endregion

            #region Npc
            Transform npcRoot = m_FightNode.transform.Find("NpcPoints");
            CampsiteNpcPoint[] campsiteNpcs = null;
            if (npcRoot != null && npcRoot.gameObject.activeSelf)
            {
                campsiteNpcs = m_FightNode.GetComponentsInChildren<CampsiteNpcPoint>(true);
                for (int i = 0; i < campsiteNpcs.Length; i++)
                {
                    var point = campsiteNpcs[i];
                    int linkDataIndex = point.PointId - 1;
                    //if (CampsiteMgr.singleton.HasPoint(linkDataIndex))
                    //{
                    point.SetValid(linkDataIndex);
                    //}
                    //else
                    //{
                    //    point.SetInvalid();
                    //}
                }
            }
            #endregion

            int baseSeed = CampsiteMgr.singleton.Id * 10;

            #region Building
            buildingDic.Clear();
            int allocLockObjIndex = 0;
            int[] lockObjPathArr = new int[] { 1, 2, 3, 4, 5, 6 };
            Vector2Int[] lockObjParamArr = new Vector2Int[lockObjPathArr.Length * 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < lockObjPathArr.Length; j++)
                {
                    int index = i * lockObjPathArr.Length + j;
                    lockObjParamArr[index] = new Vector2Int(lockObjPathArr[j], i);
                }
            }

            var campsiteBuildings = m_FightNode.GetComponentsInChildren<CampsiteBuildingPoint>(true);
            for (int i = 0; i < campsiteBuildings.Length; i++)
            {
                var building = campsiteBuildings[i];
                int linkDataIndex = building.PointId - 1;

                MyRandom random = new MyRandom(new MyRandom.State(baseSeed + linkDataIndex, 0));
                #region Alloc LockObj to Building
                {
                    int j = random.Range(allocLockObjIndex, lockObjParamArr.Length);
                    var lockObjParam = lockObjParamArr[j];
                    lockObjParamArr[j] = lockObjParamArr[allocLockObjIndex];
                    lockObjParamArr[allocLockObjIndex] = lockObjParam;
                    building.SetLockObjParam(string.Format("Prefabs/Campsite/Building/LockObj/Lock{0}", lockObjParam.x), new Vector3(0, 0, lockObjParam.y * 90f));
                    allocLockObjIndex++;
                    if (allocLockObjIndex >= lockObjParamArr.Length)
                    {
                        allocLockObjIndex = 0;
                    }
                }
                #endregion

                if (CampsiteMgr.singleton.HasPoint(linkDataIndex))
                {
                    building.SetValid(linkDataIndex);
                    buildingDic[linkDataIndex] = building;
                }
                else
                {
                    building.SetInvalid();
                }
            }

            #endregion

            int allocNpcCount = 0;

            foreach (var building in buildingDic.Values)
            {
                MyRandom random = new MyRandom(new MyRandom.State(baseSeed + building.PointDataIndex, 0));

                #region Alloc Npc to Building
                if (campsiteNpcs != null)
                {
                    int lastNpcIndex = campsiteNpcs.Length - 1 - allocNpcCount;
                    if (lastNpcIndex >= 0)
                    {
                        int linkNpcIndex = random.Range(0, buildingDic.Count - building.PointDataIndex);
                        if (linkNpcIndex <= lastNpcIndex)
                        {
                            var npc = campsiteNpcs[linkNpcIndex];
                            building.SetLinkNpc(npc);
                            campsiteNpcs[linkNpcIndex] = campsiteNpcs[lastNpcIndex];
                            campsiteNpcs[lastNpcIndex] = npc;
                            allocNpcCount++;
                        }
                    }
                }
                #endregion

                building.Init();
            }
        }

        public void RecordCamTrans()
        {
            if (mainCam == null)
                return;
            recordCamPos = mainCam.transform.position;
            recordCamRot = mainCam.transform.rotation;
            hasCamRecord = true;
        }

        private void ApplyCamRecord()
        {
            if (!hasCamRecord || mainCam == null)
                return;
            mainCam.transform.position = recordCamPos;
            mainCam.transform.rotation = recordCamRot;
            hasCamRecord = false;
        }

        public void Relese()
        {
            mainCam = null;
            camTouchCtrl = null;
            camTouchInputCtrl = null;
            camFocusCtrl = null;
            buildingDic.Clear();
        }

    }
}