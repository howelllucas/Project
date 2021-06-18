using UnityEngine;

namespace EZ
{
    public class AIGallopAct : AiBase
    {

        [SerializeField] private float DtTime = 2;
        [SerializeField] private float DashTime = 0.3f;
        [SerializeField] private float StartSpeed = 20;
        [SerializeField] private float EndSpeed = 0;
        [SerializeField] float m_DelayTime = 1;
        [SerializeField] float MoveDistance = 4;
        private Vector3 m_SpeedVec;
        private bool m_NeedCalcSpeed = false;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_NeedCalcSpeed = false;
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
                DashToMainPlayer();
            }
            else
            {
                if (m_CurTime >= DtTime)
                {

                    if ( m_Monster.TriggerFirstAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        m_NeedCalcSpeed = true;
                        if (m_NeedCalcSpeed)
                        {
                            m_NeedCalcSpeed = false;
                            m_SpeedVec = m_Player.transform.position - transform.position;
                            m_SpeedVec = m_SpeedVec.normalized;
                            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(m_SpeedVec, Vector3.up));
                        }
                    }
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MapTag) && m_StartAct)
            {
                m_Monster.SetSpeed(Vector2.zero);
                m_SpeedVec = Vector2.zero;
            }
            else if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) && m_StartAct)
            {
                if (!m_SpeedVec.Equals(Vector2.zero))
                {
                    //GameObject BossWarning = Global.gApp.gResMgr.InstantiateObj("Prefabs/Effect/enemy/3001_baodian_1");
                    //BossWarning.transform.position = collision.gameObject.transform.position;
                }
            }
        }

        private void DashToMainPlayer()
        {
            if (m_CurTime >= m_DelayTime)
            {
                float speed = Mathf.Lerp(StartSpeed, EndSpeed, m_CurTime / DashTime);
                m_Monster.SetSpeed(m_SpeedVec * speed * BaseScene.TimeScale);
                if (m_CurTime >= DashTime)
                {
                    m_CurTime = 0;
                    m_StartAct = false;
                    m_NeedCalcSpeed = false;
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndFirstAct();
                }
            }
        }
        public override void Death()
        {
            base.Death();
            m_NeedCalcSpeed = false;
        }
    }
}
