using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class BouncePopBullet : BaseBullet
    {
        [SerializeField] private int BounceTimes = 1;
        [SerializeField] private float BounceSpeed = 10;
        [SerializeField] private float BounceRange = 4;
        [SerializeField] private float Damping = 1;
        private float m_BounceTime = 1;
        private int m_BounceTimes;
        private float m_BounceRangeSqr = 10;
        private GameObject m_StartObj;
        private WaveMgr m_WaveMgr;
        private Vector3 m_LockPos;
        private Monster m_NextMonster;
        private List<GameObject> m_BouncedGo = new List<GameObject>();
        public void Init(GameObject hitMonster, double damage)
        {
            m_BouncedGo.Add(hitMonster);
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            m_LockPos = hitMonster.transform.position;
            m_BounceTimes = BounceTimes;
            m_BounceRangeSqr = BounceRange * BounceRange;
            m_StartObj = hitMonster;
            m_WaveMgr = Global.gApp.CurScene.GetWaveMgr();
            m_Damage = damage * Damping;
            InitBulletEffect(hitMonster.transform);
            FindNextLink();
        }
        private void Update()
        {
            if (m_NextMonster != null)
            {
                if (!m_NextMonster.InDeath)
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    float rate = 1;
                    if (m_BounceTime != 0)
                    {
                        rate = m_CurTime / m_BounceTime;
                        Vector3 pos = m_LockPos * (1 - rate) + m_NextMonster.transform.position * rate;
                        transform.position = pos;
                    }
                    if (rate >= 1)
                    {
                        AddHittedEffect(m_NextMonster);
                        OnHitted(m_NextMonster.transform);
                        m_CurTime = 0;
                        FindNextLink();
                    }
                }
                else
                {
                    m_CurTime = 0;
                    FindNextLink();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void FindNextLink()
        {
            if (m_BounceTimes <= 0)
            {
                Destroy(gameObject);
                return;
            }
            m_BounceTimes--;
            GameObject linkObj = m_StartObj;
            if (m_WaveMgr != null)
            {
                Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
                Vector3 startPos = linkObj.transform.position;
                foreach (KeyValuePair<int, Wave> kv in waves)
                {
                    List<Monster> monsters = kv.Value.GetMonsters();
                    int monsterCount = monsters.Count;
                    int startIndex = Random.Range(0, monsterCount);
                    for (int i = 0; i < monsterCount; i++)
                    {
                        int newIndex = startIndex % monsterCount;
                        Monster monster = monsters[newIndex];
                        if (!m_BouncedGo.Contains(monster.gameObject))
                        {
                            Vector3 postion = monster.transform.position;
                            float sqrMagnitude = (startPos - postion).sqrMagnitude;
                            if (sqrMagnitude <= m_BounceRangeSqr)
                            {
                                m_LockPos = m_StartObj.transform.position;
                                transform.position = m_LockPos;

                                m_StartObj = monster.gameObject;
                                m_NextMonster = m_StartObj.GetComponent<Monster>();
                                m_BouncedGo.Add(m_StartObj);
                                m_BounceTime = Mathf.Sqrt(sqrMagnitude) / BounceSpeed;
                                return;
                            }
                        }
                        startIndex++;
                    }
                }
                foreach (GameObject go in m_BouncedGo)
                {
                    if (go != null && go != m_StartObj && !go.GetComponent<Monster>().InDeath)
                    {
                        Vector3 postion = go.transform.position;
                        float sqrMagnitude = (startPos - postion).sqrMagnitude;
                        if (sqrMagnitude <= m_BounceRangeSqr)
                        {
                            m_LockPos = m_StartObj.transform.position;
                            transform.position = m_LockPos;
                            m_StartObj = go;
                            m_NextMonster = m_StartObj.GetComponent<Monster>();
                            return;
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
        private void OnHitted(Transform hittedMonster)
        {
            hittedMonster.GetComponent<Monster>().OnHit_Vec(m_Damage, transform);
            m_Damage = m_Damage * Damping;
        }
    }
}
