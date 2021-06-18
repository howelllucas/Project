using UnityEngine;

namespace EZ
{
    public class AIStrollAct : AiBase
    {

        // Use this for initialization
        public float Speed;
        private Vector2 m_SpeedVec;
        private float m_ActTime = 5f;
        [SerializeField] private float m_StrollTime = 3.5f;
        [SerializeField] private float m_IdleTime = 1f;
        public override void Init(GameObject player,Wave wave,Monster monster)
        {
            base.Init(player,wave,monster);
            m_Monster.PlayAnim(GameConstVal.Idle);
        }
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (BaseScene.GetDtTime() > 0)
            {
                if(m_CurTime >= m_ActTime)
                {
                    m_CurTime = 0;
                    RandomAct();
                }
                StrollRandom();
            }
            else
            {
                m_Monster.SetSpeed(Vector2.zero);
            }
        }
        private void RandomAct()
        {
            if(!m_StartAct)
            {
                m_StartAct = true;
                m_Monster.PlayAnim(GameConstVal.Run);
                float randomAngle = Mathf.Deg2Rad * Random.Range(0,360);
                m_SpeedVec = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Speed;
                m_ActTime = Random.Range(m_StrollTime - 0.5f, m_StrollTime + 0.5f);
                transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(m_SpeedVec, Vector3.up));
            }
            else
            {
                m_StartAct = false;
                m_Monster.PlayAnim(GameConstVal.Idle);
                m_SpeedVec = Vector2.zero;
                m_ActTime = Random.Range(m_IdleTime - 0.2f, m_IdleTime + 0.1f);
            }
        }
        private void StrollRandom()
        {
            if (m_Player)
            {
                m_Monster.SetSpeed(m_SpeedVec * BaseScene.TimeScale);
            }
        }
    }
}
