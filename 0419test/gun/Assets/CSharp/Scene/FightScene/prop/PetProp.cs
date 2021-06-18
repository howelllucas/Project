using UnityEngine;

namespace EZ
{
    public class PetProp : BaseProp
    {

        public GameObject Appear;
        public GameObject Disappear;
        [Tooltip(" 道具持续时间 ")]
        public float KeepTime = 1;
        [Tooltip(" 道具buff 持续时间 ")]
        public float EffectTime = 5;
        [Tooltip(" 宠物 名称 ")]
        [SerializeField] GameWeapon m_PetName = GameWeapon.Maniac;
        private float m_CurLiveTime = 0;
        private float m_CurTime = 0;
        private bool m_IsDeath = false;
        private float m_EndedTime = 1;
        private GameObject m_Appear;
        private GameObject m_Disappear;

        private bool m_AddBlinkEffect = false;

        protected override void Awake()
        {
            base.Awake();
            AdapterName(m_PropName.ToString());
        }
        private void Start()
        {
            m_Appear = Instantiate(Appear);
            m_Disappear = Instantiate(Disappear);
            m_Appear.transform.SetParent(transform, false);
            m_Disappear.transform.SetParent(transform, false);
            m_Appear.SetActive(true);
            m_Disappear.SetActive(false);
            m_Appear.transform.SetParent(transform, false);
            m_Disappear.transform.SetParent(transform, false);
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
                if(player == null)
                {
                    player = collision.gameObject.GetComponentInChildren<Player>();
                }
                if (player != null)
                {
                    m_IsDeath = true;
                    gameObject.GetComponent<CircleCollider2D>().enabled = false;

                    player.ChangePet(m_PetName.ToString(), EffectTime, DamageCoefficient);
                    Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, "Pet", m_PetName.ToString(), EffectTime);

                    m_Appear.SetActive(false);
                    m_Disappear.SetActive(true);
                    m_CurTime = 0;
                    BroadGainProp();
                }
            }
        }
    }
}

