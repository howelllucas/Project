using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class DunDiBullet : BaseBullet
    {
        float m_ActTime = 1;
        Vector3 m_StartPos;
        Vector3 m_LockPos;
        AIDunLandAct m_dunDiAct;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                float rate = m_CurTime / m_ActTime;
                transform.position = m_LockPos * rate + m_StartPos * (1 - rate);
                if (rate > 1)
                {
                    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                    DelayDestroySelf();
                }
            }
        }

        public void Init(float damage, AIDunLandAct dunDiAct,Vector3 startPos,Vector3 lockPos)
        {
            m_ActTime = (startPos - lockPos).magnitude / m_Speed;
            m_ActTime = Mathf.Max(0.1f, m_ActTime);
            m_StartPos = startPos;
            m_LockPos = lockPos;
            m_dunDiAct = dunDiAct;
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, dunDiAct.transform, 0, 0);
            InitBulletEffect(dunDiAct.transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponentInParent<Player>().StartBackActOnPos(transform);
                DestroySelf();
            }
        }
        private void DelayDestroySelf()
        {
            m_IsInDelayDestroy = true;
            m_dunDiAct.ArriveAtDst(transform.position);
            Destroy(gameObject,1.2f);
        }
        private void DestroySelf()
        {
            m_IsInDelayDestroy = true;
            m_dunDiAct.ArriveAtDst(transform.position);
            Destroy(gameObject);
        }
    }
}
