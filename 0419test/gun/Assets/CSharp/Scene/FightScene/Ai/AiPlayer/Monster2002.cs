using UnityEngine;
namespace EZ
{
    public class Monster2002 : Monster 
    {
        AIShieldAct m_ShieldAct;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_ShieldAct = gameObject.GetComponent<AIShieldAct>();
        }

        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave,this);
            m_BeatBackAct.Init(player, wave,this);
            m_ShieldAct.Init(player, wave,this);
            m_BeatBackAct.SetWeight(Weight);
        }

        public override void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect,hitByCarrier);
            m_PursureAct.Death();
            m_PursureAct.enabled = false;
            m_BeatBackAct.enabled = false;
            m_ShieldAct.enabled = false;
            Global.gApp.gShakeCompt.StartShake();
        }

        public override bool TriggerFirstAct()
        {
            if (base.TriggerFirstAct())
            {
                m_BeatBackAct.enabled = false;
                m_PursureAct.SetHittedSpeedScaleEnable();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
