
using UnityEngine;

namespace EZ
{
    public class BeatBackAct : MonoBehaviour
    {

        public Transform m_HeorNode;
        public float m_V0;
        private float m_G;
        private bool m_InBeatBackState = false;

        private bool m_InLertPoPos = false;
        private float m_LerpTime = 0;
        private Vector3 m_LertLockPos;

        private float m_CurTime;

        private float m_BackStartSpeed = 1;
        private float m_BackEndSpeed = 0.5f;
        private float m_BackTime = 0.2f;

        private float m_Weight = 1;
        private Vector2 m_SpeedVec2;

        private Player m_Player;

        public void Init(Player player,float weight)
        {
            m_Weight = weight;
            m_Player = player;
            m_CurTime = 0;
        }

        public void OnHit_Pos(Transform bulletTsf)
        {
            Vector3 targetPos = bulletTsf.position;
            Vector2 speedV = new Vector2(transform.position.x - targetPos.x, transform.position.y - targetPos.y);
            if (speedV.Equals(Vector2.zero))
            {
                speedV = bulletTsf.right;
            }
            OnHitedImp(speedV.normalized, bulletTsf);
        }
        public void OnHit_Vec(Transform bulletTsf)
        {
            Vector3 right = bulletTsf.right;
            OnHitedImp(right, bulletTsf);
        }
        public void OnHit_RealVec(Transform bulletTsf, Vector3 hitVec)
        {
            OnHitedImp(hitVec, bulletTsf);
        }
        public void StartBackActToPos(Vector3 position, float time)
        {
            m_LertLockPos = position;
            m_LerpTime = time;
            m_InLertPoPos = true;
            m_CurTime = 0;
        }
        private void OnHitedImp(Vector2 speedV, Transform tsf)
        {
            if(m_InLertPoPos || m_InBeatBackState)
            {
                return;
            }
            BaseBullet bullet = tsf.GetComponent<BaseBullet>();
            if (bullet)
            {
                m_BackStartSpeed = bullet.GetBackStartSpeed();
                m_BackEndSpeed = bullet.GetBackEndSpeed();
                m_BackTime = bullet.GetBackTime();
            }
            else
            {
                Monster monster = tsf.GetComponent<Monster>();
                m_BackStartSpeed = monster.GetBackStartSpeed();
                m_BackEndSpeed = monster.GetBackEndSpeed();
                m_BackTime = monster.GetBackTime();
            }
            if (m_BackTime > 0)
            {
                m_CurTime = 0;
                m_SpeedVec2 = speedV;
                m_G = 2 * m_V0 / (m_BackTime * 0.4f);
                m_InBeatBackState = true;
                m_Player.SetSpeed(Vector2.zero);
            }
            else
            {
                m_Player.EndBackAct();
            }
        }

        void Update()
        {
            if (m_InBeatBackState)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime < m_BackTime)
                {
                    float speed = Mathf.Lerp(m_BackStartSpeed, m_BackEndSpeed, m_CurTime / m_BackTime);
                    m_Player.SetSpeed(m_SpeedVec2 * speed * BaseScene.TimeScale / m_Weight);
                    float z = m_V0 * m_CurTime - m_G * m_CurTime * m_CurTime / 2;
                    z = Mathf.Max(z, 0);
                    m_HeorNode.localPosition = new Vector3(0, 0, -z);
                }
                else
                {
                    m_Player.SetSpeed(Vector2.zero);
                    m_CurTime = 0;
                    m_InBeatBackState = false;
                    m_HeorNode.localPosition = Vector2.zero;
                    m_Player.EndBackAct();
                }
            }
            else if(m_InLertPoPos)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime < m_LerpTime)
                {
                    float rata = m_CurTime / m_LerpTime;
                    transform.position = transform.position * (1 - rata) + rata * m_LertLockPos;
                }
                else
                {
                    m_Player.SetSpeed(Vector2.zero);
                    m_CurTime = 0;
                    m_InBeatBackState = false;
                    m_InLertPoPos = false;
                    m_Player.EndBackAct();
                }
            }
        }
    }
}
