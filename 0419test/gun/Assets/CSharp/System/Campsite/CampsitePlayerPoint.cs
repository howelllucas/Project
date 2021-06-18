using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class CampsitePlayerPoint : CampsitePointBase
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

        protected override Transform MonsterBornNode { get { return m_BornNode; } }

        protected override Vector3 GainDropPos { get { return m_BoxNode.position; } }

        private Transform m_ValidNode;
        private Transform m_BornNode;
        private Transform m_RoleNode;
        private Transform m_BoxNode;

        private CampsitePerformer m_role;

        private void Awake()
        {
            m_ValidNode = transform.Find("ValidNode");
            m_BornNode = m_ValidNode.Find("BornNode");
            m_RoleNode = m_ValidNode.Find("RoleNode");
            m_role = m_RoleNode.Find("PlayerRole").GetComponent<CampsitePerformer>();
            m_BoxNode = m_ValidNode.Find("BoxNode");
        }

        private void Start()
        {
            m_role.SetMonsterList(m_Monsters);

        }

        private void OnEnable()
        {
            var useGunId = PlayerDataMgr.singleton.GetUseWeaponID();
            var useGunData = TableMgr.singleton.GunCardTable.GetItemByID(useGunId);
            m_role.ChangeGun(useGunData);
            m_role.SetFight();
        }

        private void Update()
        {
            var dt = BaseScene.GetDtTime();
            UpdateMonster(dt);
        }

        private void OnDisable()
        {
            DestroyMonster();
        }

        protected override string GetMonsterPath()
        {
            int index = UnityEngine.Random.Range(0, m_EnemyPath.Length);
            string path = m_EnemyPath[index];
            return path;
        }
    }
}