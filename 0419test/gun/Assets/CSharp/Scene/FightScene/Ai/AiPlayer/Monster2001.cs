using UnityEngine;
namespace EZ
{
    public class Monster2001 : Monster
    {
        public GameObject m_VenomBullet;
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

        public override void Update()
        {
            base.Update();
        }
     
        public override void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect, hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
            GameObject venomBullet = Instantiate(m_VenomBullet);
            venomBullet.transform.position = transform.position;
            venomBullet.transform.SetParent(Global.gApp.gBulletNode.transform, false);
        }
    }
}
