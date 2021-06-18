using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class AIPauseAct : MonoBehaviour
    {
        Monster m_Monster;
        private float m_MaxIdleTime = 5;
        private float m_MinIdleTime = 3;
        private float m_CurIdleTime = 4;
        private float m_CurTime = 0;
        private float m_WaitTime = 5 ;
        private bool m_InWaitState = false ;
        private bool m_StartAct = false ;
        private List<AIPauseAct> m_AiPauseAct = new List<AIPauseAct>();

        Rigidbody2D m_Rigidbody2D;
        Collider2D m_Collider2D ;
        public bool TriggerStart
        {
            get;set;
        }
         void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Collider2D = GetComponent<Collider2D>();
            m_Collider2D.enabled = false;
            m_Rigidbody2D.simulated = false;
        }
        public void Init(Monster monster)
        {
            m_Collider2D.enabled = true;
            m_Rigidbody2D.simulated = true; 
            m_CurIdleTime = Random.Range(3, 5);
            m_Monster = monster;
            m_Monster.PlayAnim(GameConstVal.Idle);
            m_Monster.SetActEnable(false);
            gameObject.SetActive(true);
            m_StartAct = true;
            TriggerStart = false;
            m_InWaitState = false;
            m_CurTime = 0;
            gameObject.layer = GameConstVal.MonsterLayer;
            m_AiPauseAct.Clear();
        }
        private void Update()
        {
            if (m_StartAct && !TriggerStart)
            {
                foreach(AIPauseAct aIPauseAct in m_AiPauseAct)
                {
                    if(aIPauseAct == null || aIPauseAct.TriggerStart)
                    {
                        DisableCmp();
                        return;
                    }
                }
                m_CurTime += BaseScene.GetDtTime();
                if (!m_InWaitState)
                {
                    if (m_CurTime >= m_CurIdleTime)
                    {
                      
                        m_Monster.PlayAnim(GameConstVal.Wait);
                        m_InWaitState = true;
                        m_CurTime = 0;
                    }
                }
                else
                {
                    if (m_CurTime >= m_WaitTime)
                    {
                        m_CurIdleTime = Random.Range(3, 5);
                        m_Monster.PlayAnim(GameConstVal.Idle);
                        m_InWaitState = false;
                        m_CurTime = 0;
                    }
                }
                m_Monster.SetSpeed(Vector2.zero);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_StartAct && !TriggerStart)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
                {
                    DisableCmp();
               
                }
                if (!TriggerStart)
                {
                    if (collision.gameObject.CompareTag(GameConstVal.MonsterActiveTag))
                    {
                        if(m_AiPauseAct.Count > 2) {
                            gameObject.layer = GameConstVal.OnlyMainRoleLayer;
                            return;
                        }
                        m_AiPauseAct.Add(collision.GetComponent<AIPauseAct>());
                    }
                }
            }
        }

        public void ResumeAi()
        {
            DisableCmp();
        }
        public void Deadth()
        {
            m_StartAct = false;
        }
        private void DisableCmp()
        {
            if (TriggerStart)
            {
                return;
            }
            m_Monster.PlayAnim(GameConstVal.Run);
            if (!m_Monster.InDeath)
            {
               m_Monster.SetActEnable(true);
            }
            m_Collider2D.enabled = false;
            m_Rigidbody2D.simulated = false;
            gameObject.SetActive(false);
            TriggerStart = true;
            m_StartAct = false;
            m_AiPauseAct.Clear();
        }
    }
}
