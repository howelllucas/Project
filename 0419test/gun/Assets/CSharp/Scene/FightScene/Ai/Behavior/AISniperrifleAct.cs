using System;
using UnityEngine;

namespace EZ
{
    public class AISniperrifleAct : AiBase
    {
        public GameObject m_BulletPrefab;
        public GameObject m_FireEffectPrefab;
        public Transform FirePoint;
        [SerializeField] private float DtTime = 2;
        [SerializeField] float m_DelayTime = 2f;
        [SerializeField] float m_AnimTime = 1f;
        [SerializeField] float m_FireDelay = 1.5f;

        private bool m_InAnimAct = false;
        private Action m_Action;
        private GameObject m_Bullet;
        private void Awake()
        {
            m_Action = new Action(FireImp);
        }
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_InAnimAct = false;
            Vector3 speedVec = m_Player.transform.position - transform.position;
            speedVec = speedVec.normalized;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
        }

        void Update()
        {

            float dtTime = BaseScene.GetDtTime();
            if (dtTime == 0)
            {
                m_Monster.SetSpeed(Vector2.zero);
                return;
            }
            if (m_StartAct)
            {
                m_CurTime = m_CurTime + dtTime;
                m_Monster.SetSpeed(Vector2.zero);
                AddFireEffect();
            }
            else if(!m_InAnimAct)
            {
                m_CurTime += GetActDtTime();
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerFirstAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        m_InAnimAct = true;
                        Vector3 speedVec = m_Player.transform.position - transform.position;
                        transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
                        FireToMainPlayer();
                    }
                }
            }
            else
            {
                m_CurTime = m_CurTime + dtTime;
                m_Monster.SetSpeed(Vector2.zero);
                if (dtTime == 0)
                {
                    return;
                }
                if (m_CurTime >= m_AnimTime)
                {
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndFirstAct();
                    m_CurTime = 0;
                    m_InAnimAct = false;
                }
            }
        }
        private void FireToMainPlayer()
        {
            DelayCallBack delayCallBack =  gameObject.AddComponent<DelayCallBack>(); ;
            delayCallBack.SetAction(m_Action, m_FireDelay);
        }
        private void FireImp()
        {
            if (!m_Monster.InDeath)
            {
                m_Bullet = Instantiate(m_BulletPrefab);
                m_Bullet.GetComponent<SniperrifleBullet>().Init(m_Player.transform.position);
                m_Bullet.transform.right = transform.up;
            }
        }
        private void AddFireEffect()
        {
            if (m_CurTime >= m_DelayTime)
            {
                m_StartAct = false;
                GameObject fireEffect = Instantiate(m_FireEffectPrefab);
                fireEffect.transform.SetParent(FirePoint, false);
                DelayDestroy delayDestroy = fireEffect.AddComponent<DelayDestroy>();
                delayDestroy.SetLiveTime(0.2f);
            }
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
            if(m_Bullet != null)
            {
                Destroy(m_Bullet);
                m_Bullet = null;
            }
        }
    }
}
