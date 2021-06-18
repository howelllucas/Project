using UnityEngine;
namespace EZ
{
    public class CarProp : BaseProp
    {
        public string ItemName = "Carrier";
        public GameObject MCarNode;
        public GameObject Appear;
        public GameObject Disappear;
        public float KeepTime = 1;
        public float MoveSpeed = 10;
        public float EffectTime = 5;

        private float m_CurLiveTime = 0;

        private float m_CurTime = 0;
        private bool m_IsDeath = false;
        private float m_EndedTime = 1;

        private Vector3 m_OriPos;
        private Vector3 m_OffsetPos;
        private bool m_AddBlinkEffect = false;
        protected override void Awake()
        {
            base.Awake();
            AdapterName(m_PropName.ToString());
        }
        private void Start()
        {
            Appear.SetActive(true);
            Disappear.SetActive(false);
            m_OriPos = Appear.transform.localPosition;
            m_OffsetPos = m_OriPos;
            m_OffsetPos.z = -100;
        }
        private void AddBinkEffect()
        {
            if (!m_AddBlinkEffect)
            {
                int callTimes = 0;
                DelayCallBack call = gameObject.AddComponent<DelayCallBack>();
                call.SetAction(() =>
                {
                    if (!m_IsDeath)
                    {
                        callTimes++;
                        if (callTimes % 2 == 0)
                        {
                            Appear.transform.localPosition = m_OriPos;
                        }
                        else
                        {
                            Appear.transform.localPosition = m_OffsetPos;
                        }
                    }
                }
                , 0.5f);
                call.SetCallTimes(9999);
                m_AddBlinkEffect = true;
            }
        }
        public override void Update()
        {
            base.Update();
            if (!m_IsDeath)
            {
                m_CurLiveTime = m_CurLiveTime + BaseScene.GetDtTime();
                if (KeepTime - m_CurLiveTime < 10)
                {
                    //AddBinkEffect();
                }
                if (m_CurLiveTime >= KeepTime)
                {
                    Global.gApp.CurScene.GetPropMgr().RemoveProp(gameObject);
                }
            }
            else
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_EndedTime)
                {
                    Global.gApp.CurScene.GetPropMgr().RemoveProp(gameObject);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                Player player = collision.gameObject.GetComponentInParent<Player>();
                if (player == null)
                {
                    player = collision.gameObject.GetComponentInChildren<Player>();
                }
                if (player != null)
                {
                    m_IsDeath = true;
                    gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    player.SetCarrier(Instantiate(MCarNode), EffectTime, MoveSpeed, DamageCoefficient);
                    Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, "Carrier", ItemName, EffectTime);
                    Appear.SetActive(false);
                    Disappear.SetActive(true);
                    m_CurTime = 0;
                    BroadGainProp();
                }
            }
        }
    }
}
