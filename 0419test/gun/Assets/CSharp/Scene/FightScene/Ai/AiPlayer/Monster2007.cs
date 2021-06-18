using System.Collections.Generic;
using EZ.Data;
using UnityEngine;
namespace EZ
{
    public class Monster2007 : Monster
    {
        [SerializeField] private int m_FissionMonsterId;
        [SerializeField] private int m_FissionCount;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave, this);
            m_BeatBackAct.Init(player, wave, this);
            m_BeatBackAct.SetWeight(Weight);
        }

        public override void InitForCache(GameObject player, Wave wave, MonsterItem monsterItem)
        {
            base.InitForCache(player, wave, monsterItem);
            CacheDeadthEffect(1);
        }
        public override void OnHittedDeath(double damage, bool ingnoreEffect = false, bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            if (m_FissionCount > 0)
            {
                Vector3 position = transform.position;
				for(int i = 0; i < m_FissionCount; i++)
                {
                    m_Wave.CreateFassionMonster(m_FissionMonsterId, position);
                }
            }

            base.OnHittedDeath(damage, ingnoreEffect,hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
        }

    }
}
