using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class ElecPopBullet : BaseBullet
    {
        [SerializeField] private float SpeedBuffTime = 1;
        [SerializeField] private float SpeedBuffVal = -0.5f;
        [SerializeField] private int LinkTimes = 1;
        [SerializeField] private float LinkRange = 4;
        [SerializeField] private float Damping = 1;
        private float m_LinkRangeSqr = 10;
        [SerializeField] private GameObject LinkEffect;
        private float m_BaseLength = 5.75f;
        private GameObject m_StartObj;
        private WaveMgr m_WaveMgr;
        private List<Monster> LinkBullet;
        public void Init(GameObject hitMonster, double damage)
        {
            LinkBullet = new List<Monster>(LinkTimes + 1);
            m_LinkRangeSqr = LinkRange * LinkRange;
            m_StartObj = hitMonster;
            m_WaveMgr = Global.gApp.CurScene.GetWaveMgr();
            m_Damage = damage * Damping;
            FindNextLink(LinkTimes);
        }
        private void FindNextLink(int linkTimes)
        {
            if (m_WaveMgr != null)
            {
                // 死亡的情况下会连接到同一个多次
                Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
                Vector3 startPos = m_StartObj.transform.position;
                foreach (KeyValuePair<int, Wave> kv in waves)
                {
                    List<Monster> monsters = kv.Value.GetMonsters();
                    int monsterCount = monsters.Count;
                    int startIndex = Random.Range(0, monsterCount);
                    for (int i = 0; i < monsterCount; i++)
                    {
                        int newIndex = startIndex % monsterCount;
                        Monster monster = monsters[newIndex];
                        if (monster.gameObject != m_StartObj)
                        {
                            Vector3 postion = monster.transform.position;
                            float sqrMagnitude = (startPos - postion).sqrMagnitude;
                            if (sqrMagnitude <= m_LinkRangeSqr)
                            {
                                linkTimes--;
                                //OnHitted(linkObj.transform, monster.transform,Mathf.Sqrt(sqrMagnitude));
                                LinkBullet.Add(monster);
                                if (linkTimes <= 0)
                                {
                                    break;
                                }
                            }
                        }
                        startIndex++;
                    }
                }
                Monster startMonster = m_StartObj.GetComponent<Monster>();
                for (int i = 0; i < LinkBullet.Count; i++)
                {
                    Monster linkObj = LinkBullet[i];
                    OnHitted(startMonster, linkObj);
                    startMonster = linkObj;
                }
                LinkBullet.Clear();
            }
            Destroy(gameObject);
        }
        private void OnHitted(Monster startMonster, Monster endMonster)
        {
            if (m_Damage > 0.1d)
            {
                GameObject effect = GetHittedEnemyEffect();
                effect.transform.localPosition = Vector3.zero;
                effect.transform.SetParent(endMonster.GetBodyNode(), false);

                endMonster.OnHit_Vec(m_Damage, transform);
                endMonster.AddBuff(AiBuffType.MoveSpeed, SpeedBuffTime, SpeedBuffVal);
                GameObject mlinkEffect = Instantiate(LinkEffect);
                mlinkEffect.GetComponent<LinkTools>().Init(startMonster, endMonster);
            }
            m_Damage = m_Damage * Damping;
        }
    }
}
