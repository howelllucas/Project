using UnityEngine;
namespace EZ
{
    public class Monster2016 : Monster 
    {
        AIDesertEagleXAct m_DesertEagleXAct;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_DesertEagleXAct = gameObject.GetComponent<AIDesertEagleXAct>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave,this);
            m_BeatBackAct.Init(player, wave,this);
            m_DesertEagleXAct.Init(player, wave,this);
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
                SetRightBodyType(RigidbodyType2D.Static);
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
            SetRightBodyType(RigidbodyType2D.Dynamic);
            m_PursureAct.enabled = true;
            m_BeatBackAct.enabled = false;
        }
    }
}
