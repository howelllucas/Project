using System.Collections;
using UnityEngine;
namespace EZ
{
    public class Monster3006 : Monster
    {
        AIHurlAct m_HurlAct;
        AIJumpOnlAct m_JumpOnAct;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_HurlAct = gameObject.GetComponent<AIHurlAct>();
            m_JumpOnAct = gameObject.GetComponent<AIJumpOnlAct>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave, this);
            m_BeatBackAct.Init(player, wave, this);
            m_HurlAct.Init(player, wave, this);
            m_JumpOnAct.Init(player, wave, this);
            m_BeatBackAct.SetWeight(Weight);
        }

        public override void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect,hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
        }
        public override bool TriggerFirstAct()
        {
            if (base.TriggerFirstAct())
            {
                m_PursureAct.enabled = false;
                m_BeatBackAct.enabled = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void EndFirstAct()
        {
            base.EndFirstAct();
            m_PursureAct.enabled = true;
            m_BeatBackAct.enabled = false;

        }

        public override bool TriggerSecondAct()
        {
            if (base.TriggerSecondAct())
            {
                m_PursureAct.enabled = false;
                m_BeatBackAct.enabled = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void EndSecondAct()
        {
            base.EndSecondAct();
            m_PursureAct.enabled = true;
            m_BeatBackAct.enabled = false;

        }
    }
}
