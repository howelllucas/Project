using UnityEngine;

namespace EZ
{
    public class AIDunLandAct : AiBase
    {
        public GameObject BulletPrefab;
        [SerializeField] private float DtTime = 2;
        float m_DelayTime = 2.5f;
        float m_AnimTime = 100000f;

        private bool m_InAnimAct = false;

        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
            m_InAnimAct = false;
            Vector3 speedVec = m_Player.transform.position - transform.position;
            speedVec = speedVec.normalized;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
        }

        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            m_CurTime = m_CurTime + dtTime;

            if (m_StartAct)
            {
                if (dtTime == 0)
                {
                    m_Monster.SetSpeed(Vector2.zero);
                    return;
                }
                FireToMainPlayer();
            }
            else if(!m_InAnimAct)
            {
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerSecondAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill02,-1,1);
                        m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, -1.0f);
                        m_StartAct = true;
                        m_InAnimAct = true;
                        m_AnimTime = 100000;
                    }
                }
            }
            else if(m_InAnimAct)
            {
                if(m_CurTime >= m_AnimTime)
                {
                    EndDunLandAct();
                }
            }
        }
        public void EndDunLandAct()
        {
            m_Monster.PlayAnim(GameConstVal.Idle, -1, 0);
            m_Monster.EndSecondAct();
            m_CurTime = 0;
            m_InAnimAct = false;
        }
        private void FireToMainPlayer()
        {
            if (m_CurTime >= m_DelayTime)
            {
                m_StartAct = false;
                GameObject dunDiBullet = Instantiate(BulletPrefab);
                dunDiBullet.GetComponent<DunDiBullet>().Init(1, this,transform.position,m_Player.transform.position);
                transform.position = new Vector3(1000, 0, 0);
            }
        }
        public void ArriveAtDst(Vector3 position)
        {
            m_CurTime = 0;
            transform.position = position;
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Skill01_2011);
            effect.transform.position = position;
            Vector3 speedVec = m_Player.transform.position - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
            m_Monster.PlayAnim(GameConstVal.Skill02, -1, 0f);
            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
            m_AnimTime = m_DelayTime;
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
        }
    }
}
