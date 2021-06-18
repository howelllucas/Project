using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class CampsiteNpcPoint : CampsitePointBase
    {
        protected string[] m_EnemyPath = new string[]
        {
            "Prefabs/Campsite/Role/Enemy10000001",
            "Prefabs/Campsite/Role/Enemy10000002",
            "Prefabs/Campsite/Role/Enemy10000008",
            "Prefabs/Campsite/Role/Enemy10000009",
            "Prefabs/Campsite/Role/Enemy10000010",
            "Prefabs/Campsite/Role/Enemy10000011",
            "Prefabs/Campsite/Role/Enemy10000012",
            "Prefabs/Campsite/Role/Enemy10000013",
            "Prefabs/Campsite/Role/Enemy10000014",
            "Prefabs/Campsite/Role/Enemy10000015",
        };

        public enum CampsitePointState
        {
            Undefine,
            Invalid,
            Valid_Locked,
            Valid_Opened,
        }

        private int m_PointId;
        public int PointId { get { return m_PointId; } }

        protected override Transform MonsterBornNode { get { return m_BornNode; } }

        protected override Vector3 GainDropPos { get { return m_RoleNode.position; } }

        private Transform m_InvalidNode;
        private Transform m_ValidNode;
        private Transform m_IconNode;
        private Transform m_BornNode;
        private Transform m_RoleNode;

        private CampsitePointState m_state;

        private int m_PointDataIndex;

        private CampsitePerformer m_role;
        private int equipGunId;

        private void Awake()
        {
            m_PointId = int.Parse(gameObject.name.Replace("Point", ""));
            m_InvalidNode = transform.Find("InvalidNode");
            m_ValidNode = transform.Find("ValidNode");
            m_IconNode = m_ValidNode.Find("IconNode");
            m_BornNode = m_ValidNode.Find("BornNode");
            m_RoleNode = m_ValidNode.Find("RoleNode");
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
            DestroyMonster();
        }

        private void RegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CampsitePointDataChange, HandleDataChange);
        }

        private void UnRegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, HandleDataChange);
        }

        public void SetInvalid()
        {
            m_ValidNode.gameObject.SetActive(false);
            m_InvalidNode.gameObject.SetActive(true);
            m_state = CampsitePointState.Invalid;
        }

        public void SetValid(int pointDataIndex)
        {
            m_ValidNode.gameObject.SetActive(true);
            m_InvalidNode.gameObject.SetActive(false);
            m_PointDataIndex = pointDataIndex;
            Refresh();
        }

        public void SetEquipGunId(int equipGunId)
        {
            if (this.equipGunId != equipGunId)
            {
                this.equipGunId = equipGunId;
                Refresh();
            }
        }

        private void Refresh()
        {
            if (m_state == CampsitePointState.Invalid)
                return;
            //var pointData = CampsiteMgr.singleton.GetPointByIndex(m_PointDataIndex);
            //if (pointData == null)
            //    return;

            //if (!pointData.isUnlock)
            //{
            //    m_IconNode.gameObject.SetActive(true);
            //    m_RoleNode.gameObject.SetActive(false);
            //    m_state = CampsitePointState.Valid_Locked;
            //}
            //else
            {
                m_IconNode.gameObject.SetActive(false);
                m_RoleNode.gameObject.SetActive(true);
                if (m_role == null)
                {
                    var roleObj = Global.gApp.gResMgr.InstantiateObj("Prefabs/Campsite/Role/NpcRole1");
                    //var roleObj = Global.gApp.gResMgr.InstantiateObj(pointData.buildingRes.prefab);
                    roleObj.transform.SetParent(m_RoleNode, false);
                    m_role = roleObj.GetComponent<CampsitePerformer>();
                    //m_role = m_RoleNode.Find("MainRole").GetComponent<CampsitePerformer>();
                    m_role.SetMonsterList(m_Monsters);
                }

                var gunData = equipGunId > 0 ? TableMgr.singleton.GunCardTable.GetItemByID(equipGunId) : null;
                //var gunData = pointData.equipGunId > 0 ? TableMgr.singleton.GunCardTable.GetItemByID(pointData.equipGunId) : null;
                //if (pointData.isFight)
                {
                    m_role.ChangeGun(gunData);
                    m_role.SetFight();
                }
                //else
                //{
                //    m_role.ChangeGun(gunData);
                //    m_role.SetIdle();
                //}

                m_state = CampsitePointState.Valid_Opened;
            }
        }

        public void RespondClick()
        {
            //if (m_state == CampsitePointState.Valid_Opened)
            //{
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.NPCSettingUI);
            //    var npcSettingUI = Global.gApp.gUiMgr.GetPanelCompent<NPCSettingUI>(Wndid.NPCSettingUI);
            //    npcSettingUI.Init(m_IconNode, m_PointDataIndex, null);
            //}
            //else if (m_state == CampsitePointState.Valid_Locked)
            //{
            //    //tips
            //}

        }

        //private void HandleDataChange(int index)
        //{
        //    if (index != m_PointDataIndex || m_state == CampsitePointState.Undefine)
        //        return;
        //    Refresh();
        //}

        private void Update()
        {
            if (m_state != CampsitePointState.Valid_Opened)
                return;
            var dt = BaseScene.GetDtTime();
            UpdateMonster(dt);
        }

        protected override string GetMonsterPath()
        {
            int index = UnityEngine.Random.Range(0, m_EnemyPath.Length);
            string path = m_EnemyPath[index];
            return path;
        }
    }
}