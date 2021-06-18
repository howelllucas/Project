using UnityEngine;

namespace EZ
{
    public class AIDashAct : AiBase
    {

        [SerializeField] private float DtTime = 2;
        [SerializeField] private float DashTime = 0.3f;
        [SerializeField] private float StartSpeed = 20;
        [SerializeField] private float EndSpeed = 0;
        [SerializeField] float m_DelayTime = 1;
        [SerializeField] float MoveDistance = 4;
        float m_OriMass = 100;
        private Vector3 m_SpeedVec;
        private bool m_NeedCalcSpeed = false;
        private Rigidbody2D m_RightBody2d;
        private void Awake()
        {
            m_RightBody2d = GetComponentInChildren<Rigidbody2D>();
            m_OriMass = m_RightBody2d.mass;
        }
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            m_NeedCalcSpeed = false;
            base.Init(player, wave, monster);
        }
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            m_CurTime = m_CurTime + dtTime;
            if (dtTime == 0)
            {
                m_Monster.SetAbsSpeed(Vector2.zero);
                return;
            }
            if (m_StartAct)
            {
                DashToMainPlayer();
            }
            else
            {
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerSecondAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetAbsSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        m_NeedCalcSpeed = true;
                        m_RightBody2d.mass = 999999999;
                        if (m_NeedCalcSpeed)
                        {
                            m_NeedCalcSpeed = false;
                            m_SpeedVec = m_Player.transform.position - transform.position;
                            m_SpeedVec = m_SpeedVec.normalized;
                            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(m_SpeedVec, Vector3.up));
                            InstanceEffect();
                        }
                    }
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MapTag) && m_StartAct)
            {
                m_Monster.SetAbsSpeed(Vector2.zero);
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
        private void InstanceEffect()
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Yujing_3001);
            effect.GetComponent<WarningEffect>().Init(DashTime, MoveDistance, transform);
        }

        private void DashToMainPlayer()
        {
            if (m_CurTime >= m_DelayTime)
            {
                float speed = Mathf.Lerp(StartSpeed, EndSpeed, m_CurTime / DashTime);
                m_Monster.SetAbsSpeed(m_SpeedVec * speed * BaseScene.TimeScale);
                if (m_CurTime >= DashTime)
                {
                    m_CurTime = 0;
                    m_StartAct = false;
                    m_NeedCalcSpeed = false;
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_RightBody2d.mass = m_OriMass;
                    m_Monster.EndSecondAct();
                }
            }
        }
        public override void Death()
        {
            base.Death();
            m_NeedCalcSpeed = false;
            m_RightBody2d.mass = m_OriMass;
        }
    }
}
