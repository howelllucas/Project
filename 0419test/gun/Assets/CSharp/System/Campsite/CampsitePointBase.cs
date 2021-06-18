using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public abstract class CampsitePointBase : MonoBehaviour
    {
        protected static float m_DtTime = 0.25f;
        protected static int m_MaxCount = 5;
        protected static int m_CreateCount = 1;

        private float m_CurTime;
        protected abstract Transform MonsterBornNode { get; }
        protected abstract Vector3 GainDropPos { get; }

        private Dictionary<string, GameObject> m_MonstersPrefabMap = new Dictionary<string, GameObject>();
        protected List<Monster> m_Monsters = new List<Monster>();

        protected void UpdateMonster(float dt)
        {
            m_CurTime += dt;

            foreach (Monster monster in m_Monsters)
            {
                if (monster == null || monster.InDeath)
                {
                    m_Monsters.Remove(monster);
                    break;
                }
            }
            if (m_CurTime > m_DtTime)
            {
                m_CurTime = 0;
                CreateMonsters();
            }
        }

        protected abstract string GetMonsterPath();

        protected virtual void CreateMonsters()
        {
            if (m_Monsters.Count < m_MaxCount)
            {
                for (int i = 0; i < m_CreateCount; i++)
                {
                    GameObject monsterGoPrefab;
                    GameObject monsterGo;
                    string path = GetMonsterPath();
                    if (!m_MonstersPrefabMap.TryGetValue(path, out monsterGoPrefab))
                    {
                        monsterGoPrefab = Resources.Load(path) as GameObject;
                        m_MonstersPrefabMap.Add(path, monsterGoPrefab);
                    }
                    monsterGo = GameObject.Instantiate<GameObject>(monsterGoPrefab);
                    CampsiteMonster monsterComp = monsterGo.GetComponent<CampsiteMonster>();

                    Vector3 localScale = MonsterBornNode.localScale;
                    float x = Random.Range(-0.5f, 0.5f);
                    float y = Random.Range(-0.5f, 0.5f);
                    Vector3 posOffset = new Vector3(localScale.x * x, localScale.y * y, 0);
                    posOffset = MonsterBornNode.rotation * posOffset;
                    Vector3 position = MonsterBornNode.transform.position + posOffset;
                    position.z = 0;
                    monsterGo.transform.position = position;

                    monsterComp.Init(GainDropPos);
                    m_Monsters.Add(monsterComp);
                }
            }
        }

        protected void DestroyMonster()
        {
            foreach (CampsiteMonster monster in m_Monsters)
            {
                if (monster != null)
                    GameObject.Destroy(monster.gameObject);
            }
            m_Monsters.Clear();
        }
    }
}