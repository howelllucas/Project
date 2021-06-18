using EZ.Data;
using UnityEngine;

namespace EZ
{
    public class WeaponProp : BaseProp
    {

        public GameObject Appear;
        public GameObject Disappear;
        public float KeepTime = 1;
        public float EffectTime = 5;
        public bool SecondWeapon = false;
        [SerializeField] GameWeapon m_WamponName = GameWeapon.weapon_firegun;
        private float m_CurLiveTime = 0;

        private float m_CurTime = 0;
        private bool m_IsDeath = false;
        private float m_EndedTime = 1;
        private GameObject m_Appear;
        private GameObject m_Disappear;

        private Vector3 m_OriPos;
        private Vector3 m_OffsetPos;
        private bool m_AddBlinkEffect = false;

        protected override void Awake()
        {
            base.Awake();
            AdapterName(m_WamponName.ToString());
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
            m_OriPos = m_Appear.transform.localPosition;
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
                            m_Appear.transform.localPosition = m_OriPos;
                        }
                        else
                        {
                            m_Appear.transform.localPosition = m_OffsetPos;
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
                if(player == null)
                {
                    player = collision.gameObject.GetComponentInChildren<Player>();
                }
                if (player != null)
                {
                    m_IsDeath = true;
                    gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    if (!SecondWeapon)
                    {
                        player.ChangeWeapon(m_WamponName.ToString(), EffectTime, DamageCoefficient);
                        Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, "Weapon", m_WamponName.ToString(), EffectTime);
                    }
                    else
                    {
                        player.ChangeThirdWeapon(m_WamponName.ToString(), EffectTime, DamageCoefficient);
                        Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, "SecondWeapon", m_WamponName.ToString(), EffectTime);
                    }
                    m_Appear.SetActive(false);
                    m_Disappear.SetActive(true);
                    m_CurTime = 0;
                    BroadGainProp();
                }
            }
        }
    }
}

