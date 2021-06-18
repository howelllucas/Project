using EZ.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class PlayerMgr
    {
        protected Dictionary<int, GameObject> m_Players;
        private int m_GuidRef = 0;
        private MonsterCache m_MonsterCache;
        private GPUSkinningPlayerMonoManager m_PlayerManager = new GPUSkinningPlayerMonoManager();

        public PlayerMgr()
        {
            m_MonsterCache = new MonsterCache();
            m_Players = new Dictionary<int, GameObject>();
        }
        // 队友或者 帮助
        public void RemovePlayer(GameObject player)
        {
        }
        public void AddPlayer(GameObject player,int guid)
        {
            m_Players.Add(guid,player);
        }
        public GameObject CreatePlayer()
        {
            m_GuidRef++;
            GameObject player = Global.gApp.gResMgr.InstantiateObj("Prefabs/Role/MainRole1002");
            player.transform.SetParent(Global.gApp.gRoleNode.transform);
            Global.gApp.gCamCompt.enabled = true;
            Global.gApp.gCamCompt.SetTarget(player.transform);
            AddPlayer(player, m_GuidRef);
            return player;
        }

        public Monster CreateMonster(MonsterItem monsterItem)
        {
            m_GuidRef++;
            Monster monster = m_MonsterCache.GetMonster(monsterItem);
            monster.SetGuid(m_GuidRef);
            return monster;
        }
        public Monster CreateMonsterForceCreate(MonsterItem monsterItem)
        {
            m_GuidRef++;
            Monster monster = m_MonsterCache.GetMonsterForceCreate(monsterItem);
            monster.SetGuid(m_GuidRef);
            return monster;
        }
        public void Recycle(MonsterItem monsterItem,Monster monster)
        {
            m_MonsterCache.Recycle(monsterItem, monster);
        }
        public GPUSkinningPlayerMonoManager GetGpuSkineMgr()
        {
            return m_PlayerManager;
        }
        public void Destroy()
        {
            Global.gApp.gCamCompt.enabled = false;
            Global.gApp.gCamCompt.SetTarget(null);
            foreach (GameObject player in m_Players.Values)
            {
                Object.DestroyImmediate(player);
            }
            m_MonsterCache.ClearMonsters();
        }
    }
}
