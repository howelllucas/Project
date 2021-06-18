
using UnityEngine;

namespace EZ
{
    public class AIBeatBackAct : AiBase {


        enum BeatBackType {
            None = 0,
            OnPos = 1,
            OnVec = 2,
            OnUp = 3,
        }
        private float m_BackStartSpeed = 1;
        private float m_BackEndSpeed = 0.5f;
        private float m_BackTime = 0.2f;
        private float m_Weight = 1;
        private Vector2 m_SpeedVec2;
        private BeatBackType m_BeatBackType = BeatBackType.None;
        private void Start()
        {
            m_CurTime = 0;
        }
        public void SetWeight(float weight)
        {
            m_Weight = Mathf.Max(weight, 0.01f);
        }
        public void OnHit_Pos(Transform bulletTsf)
        {
            m_BeatBackType = BeatBackType.OnPos;
            Vector3 targetPos = bulletTsf.position;
            Vector2 speedV = new Vector2(transform.position.x - targetPos.x, transform.position.y - targetPos.y);
            OnHitedImp(speedV.normalized, bulletTsf);
        }
        public void OnHit_Vec(Transform bulletTsf)
        {
            m_BeatBackType = BeatBackType.OnVec;
            Vector3 right = bulletTsf.right;
            OnHitedImp(right,bulletTsf);
        }
        public void OnHit_Up(Transform bulletTsf )
        {
            m_BeatBackType = BeatBackType.OnUp;
            Vector3 up = bulletTsf.up;
            OnHitedImp(up, bulletTsf);
        }
        private void OnHitedImp(Vector2 speedV,Transform bulletTsf)
        {
            BaseBullet bullet = bulletTsf.GetComponent<BaseBullet>();
            m_SpeedVec2 = speedV;
            m_CurTime = 0;
            m_BackStartSpeed = bullet.GetBackStartSpeed();
            m_BackEndSpeed = bullet.GetBackEndSpeed();
            //if(m_BackStartSpeed == 0 && m_BackEndSpeed == 0)
            //{
            //    m_Monster.SetAbsAnimSpeed(0);
            //    m_Monster.SetStaticByRigidProp();
            //}
            m_BackTime = Mathf.Max(bullet.GetBackTime(),0.01f);
            m_StartAct = true;
        }
        private void OnDisable()
        {
            if (m_StartAct)
            {
                m_Monster.ResetSpeed();
                m_Monster.ResetStaticByRigidProp();
            }
            m_BeatBackType = BeatBackType.None;
            if (m_Monster != null)
            {
                Vector3 pos = m_Monster.transform.position;
                pos.z = 0;
                m_Monster.transform.position = pos;
            }
        }
        void Update() {
            if (m_StartAct)
            {
                float dtTime = BaseScene.GetDtTime();
                if(dtTime == 0)
                {
                    m_Monster.SetSpeed(Vector2.zero);
                    return;
                }
                m_CurTime = m_CurTime + dtTime;
                if (m_CurTime < m_BackTime)
                {
                    if (m_BeatBackType == BeatBackType.OnPos || m_BeatBackType == BeatBackType.OnVec)
                    {
                        float speed = Mathf.Lerp(m_BackStartSpeed, m_BackEndSpeed, m_CurTime / m_BackTime);
                        m_Monster.SetSpeed(BaseScene.TimeScale * m_SpeedVec2 * speed / m_Weight);
                    }
                    else if (m_BeatBackType == BeatBackType.OnUp)
                    {
                        float G = m_BackStartSpeed * 2 / m_BackTime;
                        float posZ = m_BackStartSpeed * m_CurTime - G * m_CurTime * m_CurTime / 2;
                        Vector3 pos = m_Monster.transform.position;
                        pos.z = -posZ;
                        m_Monster.transform.position = pos;
                        m_Monster.SetSpeed(Vector3.zero); 
                    }
                }
                else
                {
                    m_CurTime = 0;
                    m_StartAct = false;

                    //if (m_BackStartSpeed == 0 && m_BackEndSpeed == 0)
                    //{
                    //    m_Monster.ResetSpeed();
                    //    m_Monster.ResetStaticByRigidProp();
                    //}
                    m_Monster.EndBackAct();
                }
            }
        }
    }
}
