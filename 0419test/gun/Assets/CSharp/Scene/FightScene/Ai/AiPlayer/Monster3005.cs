using UnityEngine;
namespace EZ
{
    public class Monster3005 : Monster
    {
        AIThrowHookAct m_TrowHookAct;
        AIHookPosition m_HookPos;
        AIHook m_HookTool;
        [SerializeField] private float PoisonTime = 1;
        private float m_CurTime;
        [SerializeField] private GameObject PosionBllet;
        [SerializeField] private bool m_HasSpeed = false;
        private void Awake()
        {
            m_TrowHookAct = gameObject.GetComponent<AIThrowHookAct>();
            m_BeatBackAct = gameObject.GetComponent<AIBeatBackAct>();
            m_PursureAct = gameObject.GetComponent<AIPursueAct>();
            m_HookPos = gameObject.GetComponentInChildren<AIHookPosition>();
            m_HookTool = gameObject.GetComponentInChildren<AIHook>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_HookTool.gameObject.SetActive(false);
            m_PursureAct.Init(player, wave, this);
            m_TrowHookAct.Init(player, wave, this);
            m_BeatBackAct.Init(player, wave, this);
            m_BeatBackAct.SetWeight(Weight);
        }

        public override void Update()
        {
            base.Update();
            if (m_HasSpeed)
            {
                m_CurTime += BaseScene.GetDtTime();
                if(m_CurTime > PoisonTime)
                {
                    AddPoisonBullet();
                    m_CurTime = m_CurTime - PoisonTime;
                }
            }
        }
        private void AddPoisonBullet()
        {
            GameObject bullet = Instantiate(PosionBllet);
            bullet.transform.position = transform.position;
        }
   
        public override void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect,hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
        }
        public override bool TriggerFirstAct()
        {
            if (m_UsingSkill) { return false; }
            if (!m_HookPos.CanUseSkill()) { return false; };
            if (base.TriggerFirstAct())
            {
                SetRightBodyType(RigidbodyType2D.Static);
                m_PursureAct.enabled = false;
                m_BeatBackAct.enabled = false;
                m_HookTool.gameObject.SetActive(true);
                m_HasSpeed = false;
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
            m_HasSpeed = true;
            m_PursureAct.enabled = true;
            m_BeatBackAct.enabled = false;
            m_HookTool.gameObject.SetActive(false);
        }
    }
}
