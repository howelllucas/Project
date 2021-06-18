using System.Collections;
using UnityEngine;
namespace EZ
{
    public class Monster3002 : Monster
    {
        [SerializeField] private GameObject StoneBullet;
        [SerializeField] private Transform m_BulletPoint;
        AIHitGroundAct m_HitGroundAct;
        AIThrowStoneAct m_ThrowStoneAct;
        private GameObject m_StoneBullet;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_HitGroundAct = gameObject.GetComponent<AIHitGroundAct>();
            m_ThrowStoneAct = gameObject.GetComponent<AIThrowStoneAct>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave, this);
            m_BeatBackAct.Init(player, wave, this);
            m_HitGroundAct.Init(player, wave, this);
            m_ThrowStoneAct.Init(player, wave, this);
            m_BeatBackAct.SetWeight(Weight);
            if (monster.hasShowAct > 0)
            {
                AIBossShow3001 show = gameObject.GetComponent<AIBossShow3001>();
                show.SetShowCall(InstanceStoneBullet);
            }
            else
            {
                InstanceStoneBullet();
            }
        }

        private void AddAppearWarningEffect()
        {
            GameObject BossWarning = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/BossWarning");
            BossWarning.GetComponent<DelayDestroy>().SetIgnoreSceneTimeScale(true);
            BossWarning.transform.SetParent(Global.gApp.gUiMgr.GetUiCanvasTsf(), false);
            BossWarning.transform.SetAsFirstSibling();
        }
        public override void Update()
        {
            base.Update();
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
            InstanceStoneBullet();
            base.EndSecondAct();
            m_PursureAct.enabled = true;
            m_BeatBackAct.enabled = false;
        }

        public void ThrowStoneBullet(Vector3 lockPos)
        {
            if (m_StoneBullet != null)
            {
                m_StoneBullet.transform.SetParent(Global.gApp.gBulletNode.transform, true);
                m_StoneBullet.GetComponent<StoneBullet>().Run(lockPos);
                m_StoneBullet = null;
            }
        }

        public void InstanceStoneBullet()
        {
            if (m_StoneBullet == null)
            {
                m_StoneBullet = Instantiate(StoneBullet);
                m_StoneBullet.transform.SetParent(m_BulletPoint, false);
            }
        }
    }
}
