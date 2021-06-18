using UnityEngine;

namespace EZ
{
    public class AIThrowStoneAct : AiBase
    {

        [SerializeField] private float DtTime = 3.0f;
        [SerializeField] private float TriggerRate = 100;
        private float m_DelayTime = 1f;
        private float m_EndTime = 2.7f;
        private bool m_StartEffect = false;
        private Vector3 m_LockPos;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_StartEffect = false;
        }
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime == 0)
            {
                m_Monster.SetSpeed(Vector2.zero);
                return;
            }
            m_CurTime = m_CurTime + dtTime;
            if (m_StartAct)
            {
                ThrowStoneBullet();
            }
            else
            {
                if (m_CurTime >= DtTime)
                {
                    int rate = Random.Range(0, 10001);
                    if (rate <= TriggerRate && m_Monster.TriggerSecondAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill02);
                        m_StartAct = true;
                    }
                }
            }
        }
        private void AddLockEffect()
        {
            GameObject lockEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.HarmRangeEffect);
            lockEffect.transform.localScale = new Vector3(1f, 1f, 1.0f);
            lockEffect.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y, -0.2f);
            m_LockPos = m_Player.transform.position;
            lockEffect.GetComponent<DelayDestroy>().SetLiveTime(1.4f);
        }
        private void ThrowStoneBullet()
        {
            m_Monster.SetSpeed(Vector2.zero);
            if (m_CurTime >= m_DelayTime)
            {
                if (m_StartEffect == false)
                {
                    AddLockEffect();
                    m_StartEffect = true;
                }
                
                GetComponent<Monster3002>().ThrowStoneBullet(m_LockPos);
            }
            if(m_CurTime >= m_EndTime)
            {
                m_CurTime = 0;
                m_StartAct = false;
                m_StartEffect = false;
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Monster.EndSecondAct();
            }
        }

        public override void Death()
        {
            base.Death();
            m_StartEffect = false;
        }
    }
}

