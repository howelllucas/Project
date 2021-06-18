using System;
using UnityEngine;

namespace EZ
{
    public class PatriotRocketFly : MonoBehaviour
    {

        [Tooltip(" 转向角速度时间 比例 时间 = rada * liveTime")]
        [Range(0.001f, 1)]
        public float FaceToEnemyTimeRate = 0.85f;
        [Tooltip(" 非锁定时间 = rada * liveTime")]
        [Range(0.001f, 1)]
        public float UnLockTimeRate = 0.85f;

        float UnLockTimeTime;
        float m_LiveTime;
        float m_FaceToTargetTime;
        private Vector3 m_LockPos;
        private bool m_InLockState = false;

        private bool m_CanAutoFollow = false;

        private Monster m_LockMonster;
        float m_CurTime;
        float m_LockSpeed;
        float m_StartSpeed;
        Action m_ReachCall;
        public void Init(Transform firePoint, float angle, float radio, float liveTime, float speed, Action reachCallBack)
        {
            m_ReachCall = reachCallBack;
            m_FaceToTargetTime = liveTime * FaceToEnemyTimeRate;
            UnLockTimeRate = UnLockTimeRate * (UnityEngine.Random.Range(0.5f, 1.2f));
            UnLockTimeRate = Mathf.Min(UnLockTimeRate, (1 - FaceToEnemyTimeRate));
            UnLockTimeTime = liveTime * UnLockTimeRate;
            m_LiveTime = liveTime;
            m_LockSpeed = speed;
            m_StartSpeed = speed;
            transform.eulerAngles = firePoint.eulerAngles;
            transform.Rotate(transform.right, angle + UnityEngine.Random.Range(-10, 10), Space.World);
            Vector3 newPos = firePoint.position + transform.up * radio;
            transform.position = newPos;
            if (UnityEngine.Random.Range(0, 10001) > 5000)
            {
                transform.right = transform.up * UnityEngine.Random.Range(0.5f, 0.75f) + transform.right * UnityEngine.Random.Range(0.5f, 1.0f);
            }
            else
            {
                transform.right = transform.up * UnityEngine.Random.Range(-0.75f, -0.5f) + transform.right * UnityEngine.Random.Range(0.5f, 1.0f);
            }
            GenerateLockMonster();
        }

        public void SetLockMonster(GameObject monster)
        {
            if (monster == null)
                return;
            if (m_LockMonster == null)
            {
                m_LockMonster = monster.GetComponent<Monster>();
                m_CanAutoFollow = true;
            }
        }

        void Update()
        {
            if (!m_InLockState)
            {
                GenerateLockMonster();
                if (m_LockMonster != null && m_LockMonster.InDeath)
                {
                    m_InLockState = true;
                    m_LockPos = m_LockMonster.transform.position;
                }
            }
            if (m_CanAutoFollow)
            {
                float dtTime = BaseScene.GetDtTime();
                m_CurTime += dtTime;
                if (m_CurTime > UnLockTimeTime)
                {
                    float newCurTime = m_CurTime - UnLockTimeTime;
                    float ratio = Mathf.Min(newCurTime / m_FaceToTargetTime, 1);
                    if (!m_InLockState && m_LockMonster != null)
                    {
                        Vector3 newRight = (m_LockMonster.transform.position - transform.position).normalized;
                        newRight = newRight * ratio + transform.right * (1 - ratio);
                        transform.right = newRight;
                        transform.Translate(transform.right * m_LockSpeed * dtTime, Space.World);
                    }
                    else
                    {
                        Vector3 newPos = (m_LockPos - transform.position);
                        if (newPos.sqrMagnitude > 1)
                        {
                            Vector3 newRight = newPos.normalized;
                            newRight = newRight * ratio + transform.right * (1 - ratio);
                            transform.right = newRight;
                            transform.Translate(transform.right * m_LockSpeed * dtTime, Space.World);
                        }
                        else
                        {
                            m_ReachCall();
                        }
                    }
                }
                else
                {
                    transform.Translate(transform.right * m_StartSpeed * dtTime, Space.World);
                }

            }
        }
        void GenerateLockMonster()
        {
            if (m_LockMonster == null)
            {
                if (Global.gApp.CurScene.GetMainPlayerComp() != null)
                {
                    GameObject lockMonsterGo = Global.gApp.CurScene.GetMainPlayerComp().GetFight().GetLockEnemy();
                    if (lockMonsterGo != null)
                    {
                        m_LockMonster = lockMonsterGo.GetComponent<Monster>();
                        m_CanAutoFollow = true;
                    }
                }
            }
        }
    }
}
