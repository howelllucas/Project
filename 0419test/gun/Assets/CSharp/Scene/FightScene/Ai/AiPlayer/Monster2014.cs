using UnityEngine;
namespace EZ
{
    public class Monster2014 : Monster
    {
        [SerializeField] private GameObject m_ExplodeBullet;
        GameObject m_RoleNode;
        public ParticleSystem CtlParticle;
        private void Awake()
        {
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_RoleNode = transform.Find("Enemy2014").gameObject;
        }
        protected override void RecycleSelf()
        {
            base.RecycleSelf();
            if(CtlParticle != null)
            {
                CtlParticle.Stop();
            }
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            m_RoleNode.SetActive(true);
            base.Init(player, wave, monster);
            m_PursureAct.Init(player, wave, this);
            m_BeatBackAct.Init(player, wave, this);
            m_BeatBackAct.SetWeight(Weight);
            if (CtlParticle != null)
            {
                CtlParticle.Play();
            }
        }

        public override void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect,hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
            m_DeadTime = 1.0f;
            m_RoleNode.SetActive(false);
            gameObject.AddComponent<DelayCallBack>().SetAction(() => {
                GameObject venomBullet = Instantiate(m_ExplodeBullet);
                venomBullet.transform.position = transform.position;
                venomBullet.transform.SetParent(Global.gApp.gBulletNode.transform, false);

            }, Random.Range(0,0.3f));
        }
    }
}
