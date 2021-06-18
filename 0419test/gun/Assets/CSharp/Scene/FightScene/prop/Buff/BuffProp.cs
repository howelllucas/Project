using EZ.Data;
using UnityEngine;

namespace EZ
{
    public class BuffProp : BaseProp
    {

        [SerializeField]private GameObject Appear;
        [SerializeField]private GameObject Disappear;

        [SerializeField]private GameObject ParticleEffect;

        [SerializeField]private float KeepTime = 1;
        [SerializeField]private float EffectTime = 5;
        [SerializeField] private float m_Val = 0;
        [SerializeField] private float m_Val2 = 0;
        [SerializeField] private float m_Val3 = 0;

        [SerializeField] private BuffType m_BuffType = BuffType.ShieldBuff;

        private float m_CurLiveTime = 0;
        private float m_CurTime = 0;
        private bool m_IsDeath = false;
        private float m_EndedTime = 1;
        private GameObject m_Appear;
        private GameObject m_Disappear;
        protected override void Awake()
        {
            base.Awake();
            AdapterName(m_PropName.ToString());
        }

        private void Start()
        {
            m_Appear = Instantiate(Appear);
            m_Disappear = Instantiate(Disappear);

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
                if (player != null)
                {
                    m_IsDeath = true;
                    gameObject.GetComponent<CircleCollider2D>().enabled = false;

                    int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExBuffTime);
                    Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
                    float addPrarm = (skillLevelData == null) ? 1f : skillLevelData.skill_exbufftime[0];

                    player.GetBuffMgr().AddBuff(m_BuffType, EffectTime * addPrarm, m_Val,m_Val2,m_Val3,ParticleEffect);
                    m_Appear.SetActive(false);
                    m_Disappear.SetActive(true);
                    m_CurTime = 0;
                    BroadGainProp();
                    Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, m_PropName.ToString(), m_PropName.ToString(), EffectTime * addPrarm);
                }
            }
        }
    }
}

