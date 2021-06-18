using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class AIAddHpAct : AiBase
    {

        [SerializeField] private float DtTime = 3.0f;
        [Range(1, 100)]
        [SerializeField] private float HpPercentage = 3.0f;
        [SerializeField] private float TriggerRange = 3.0f;
        private float m_SqrTriggerRange = 25.0f;
        private float m_EndTime = 2.2f;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_SqrTriggerRange = TriggerRange * TriggerRange;
        }
        void Update()
        {
            if (m_StartAct)
            {
                CheckEnded();
            }
            else
            {
                m_CurTime += GetActDtTime();
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerFirstAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        AddRangeHp();
                    }
                }
            }
        }
        private void AddRangeHp()
        {
            WaveMgr mgr = m_Player.GetComponent<Player>().GetWaveMgr();
            Dictionary<int, Wave> waves = mgr.GetWaves();
            Vector3 curPos = transform.position;
            foreach (KeyValuePair<int, Wave> kv in waves)
            {
                List<Monster> monsters = kv.Value.GetMonsters();
                foreach (Monster monster in monsters)
                {
                    if((monster.transform.position - curPos).sqrMagnitude < m_SqrTriggerRange)
                    {
                        monster.AddHpByPercent(HpPercentage / 100);
                        GameObject hpEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Skill01_2018_2);
                        hpEffect.transform.SetParent(monster.GetBodyNode(), false);
                    }
                }
            }
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Skill01_2018_1);
            effect.transform.position = transform.position;
        }
        private void CheckEnded()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if(BaseScene.GetDtTime() <= 0)
            {
                m_Monster.SetSpeed(Vector2.zero);
                return;
            }
            m_Monster.SetSpeed(Vector2.zero);
            if (m_CurTime >= m_EndTime)
            {
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Monster.EndFirstAct();
            }
        }
    }
}

