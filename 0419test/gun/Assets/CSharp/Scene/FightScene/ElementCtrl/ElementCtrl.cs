using UnityEngine;

namespace EZ
{
    public abstract class ElementCtrl : MonoBehaviour
    {
        protected enum EleCtrlState
        {
            None = 0,
            Charging = 1,
            UnCharging = 2,
            Active = 3,
            Close = 4,
        }
        CElement[] cElements;

        [Tooltip("可以激活的次数")]
        [SerializeField] private int ActiveTimes = 1;
        private int m_LeftActiveTimes = 0;
        [Tooltip("消耗物品 id 为 0表示不消耗")]
        [SerializeField]private int ConsumId = 0;
        [Tooltip("消耗物品 数量 如果 为 0 同样表示不消耗 ")]
        [SerializeField]private int ConsumCount = 0;
        [Range(0.01f,99999999)]
        [SerializeField] private float ChargeTime = 0.01f;
        private float m_CurChargeTime = 0;
        [Range(0.01f, 99999999.0f)]
        [SerializeField] private float LiveTime = 10f;
        private float m_CurLiveTime = 0;
        protected EleCtrlState m_EleCtrlerState = EleCtrlState.None;
        protected abstract void PowerIncrease(float percent);
        protected abstract void PowerIncreaseEnd();
        protected abstract void PowerDecrease(float percent);
        protected abstract void PowerDecreaseEnd();
        protected abstract void Init();
        protected abstract void TimeOver();
        protected abstract void Close();
        private void Awake()
        {
            cElements = transform.Find(GameConstVal.ElementNode).GetComponentsInChildren<CElement>();
            m_LeftActiveTimes = ActiveTimes;
            Init();
        }
        protected void EleActive()
        {
            m_LeftActiveTimes--;
            // 消耗材质
            ReduceMat();
            // 状态 反向 
            RevertEleState();
            // 抛出 能力增加结束的事件
            PowerIncreaseEnd();
        }
        protected virtual void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if(dtTime > 0)
            {
                if(m_EleCtrlerState == EleCtrlState.Charging)
                {
                    if (CheckMatEnough())
                    {
                        m_CurChargeTime += dtTime;
                        if (m_CurChargeTime < ChargeTime)
                        {
                            PowerIncrease(m_CurChargeTime / ChargeTime);
                        }
                        else
                        {
                            m_EleCtrlerState = EleCtrlState.Active;
                            EleActive();
                        }
                    }
                }
                else if(m_EleCtrlerState == EleCtrlState.UnCharging)
                {
                    m_CurChargeTime -= dtTime;
                    if (m_CurChargeTime <= 0)
                    {
                        m_CurChargeTime = 0;
                        m_EleCtrlerState = EleCtrlState.None;
                        PowerDecreaseEnd();
                    }
                    else
                    {
                        PowerDecrease(m_CurChargeTime / ChargeTime);
                    }
                }
                else if(m_EleCtrlerState == EleCtrlState.Active)
                {
                    m_CurChargeTime = 0;
                    m_CurLiveTime += dtTime;
                    if(m_CurLiveTime >= LiveTime)
                    {
                        m_EleCtrlerState = EleCtrlState.None;
                        m_CurLiveTime = 0;
                        PowerRunOut();
                    }
                }
            }
        }
        // 消耗材料
        protected void ReduceMat()
        {
            // 不消耗任何材料
            if (ConsumId == 0 || ConsumId == 0)
            {
                return;
            }
        }
        protected bool CheckMatEnough()
        {
       // 不消耗任何材料
            if(ConsumId == 0 || ConsumId == 0)
            {
                return true;
            }
            // 道具检测 
            return false;
        }
        protected void PowerRunOut()
        {
            RevertEleState();
            if (m_LeftActiveTimes > 0)
            {
                TimeOver();
            }
            else
            {
                m_EleCtrlerState = EleCtrlState.Close;
                Close();
            }
        }
        private void RevertEleState()
        {
            if (cElements != null)
            {
                foreach (CElement cElement in cElements)
                {
                    cElement.RevertEleState();
                }
            }
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_EleCtrlerState == EleCtrlState.None || m_EleCtrlerState == EleCtrlState.UnCharging)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
                {
                    m_EleCtrlerState = EleCtrlState.Charging;
                }
            }
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (m_EleCtrlerState == EleCtrlState.Charging)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
                {
                    m_EleCtrlerState = EleCtrlState.UnCharging;
                }
            }
        }
    }
}
