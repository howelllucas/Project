using System.Collections.Generic;
using UnityEngine;
using EZ.Data;
namespace EZ
{
    public class MonsterCache
    {
        private Dictionary<int, List<Monster>> m_MonsterCache = new Dictionary<int, List<Monster>>();

        public void CacheMonster(MonsterItem monsterItem,int count)
        {
            for (int i = 0; i < count; i++)
            {
                Monster monsterNode = CreateMonster(monsterItem);
                Recycle(monsterItem, monsterNode);
            }
        }
        public Monster GetMonster(MonsterItem monsterItem)
        {
            List<Monster> monsters;
            Monster monster;
            if (m_MonsterCache.TryGetValue(monsterItem.tag, out monsters))
            {
                if (monsters.Count > 0)
                {
                    monster = monsters[0];
                    monsters.RemoveAt(0);
                    return monster;
                }
            }
            monster = CreateMonster(monsterItem);
            return monster;
        }
        public Monster GetMonsterForceCreate(MonsterItem monsterItem)
        {
            return CreateMonster(monsterItem);
        }
        private Monster CreateMonster(MonsterItem monsterItem)
        {
            GameObject monster = Global.gApp.gResMgr.InstantiateObj(monsterItem.path);
            Monster monsterComp = monster.GetComponent<Monster>();
            monsterComp.transform.SetParent(Global.gApp.gRoleNode.transform);
            return monsterComp;
        }
        public void Recycle(MonsterItem monsterItem, Monster monster)
        {
            List<Monster> monsters;
            if (!m_MonsterCache.TryGetValue(monsterItem.tag, out monsters))
            {
                monsters = new List<Monster>();
                m_MonsterCache[monsterItem.tag] = monsters;
            }
            monster.transform.position = new Vector3(1000, 0, 0);
            monsters.Add(monster);
        }
        public void ClearMonsters()
        {
            foreach (KeyValuePair<int, List<Monster>> keyValue in m_MonsterCache)
            {
                foreach(Monster monster in keyValue.Value)
                {
                    monster.DestroySelf();
                }
            }
            m_MonsterCache.Clear();
        }
    }
}
