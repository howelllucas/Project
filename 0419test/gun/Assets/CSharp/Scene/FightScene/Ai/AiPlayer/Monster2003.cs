using System.Collections;
using UnityEngine;
namespace EZ
{
    public class Monster2003 : Monster
    {
        AIThrowStone2Act m_TrhowStone2Act;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_TrhowStone2Act = gameObject.GetComponent<AIThrowStone2Act>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave, this);
            m_BeatBackAct.Init(player, wave, this);
            m_TrhowStone2Act.Init(player, wave, this);
            m_BeatBackAct.SetWeight(Weight);
        }

        public override void OnHittedDeath(double damage, bool ingnoreEffect = false, bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect, hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
        }

        public override void EndFirstAct()
        {
            base.EndFirstAct();
            m_PursureAct.enabled = true;
            m_BeatBackAct.enabled = false;

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

        public override void SetSpeed(Vector2 speed)
        {
            base.SetSpeed(speed);
            if (speed.x == 0 && speed.y == 0)
            {
                m_Rigidbody2D.drag = 10000;
            }
            else
            {
                m_Rigidbody2D.drag = 0;
            }
        }

    }
}
