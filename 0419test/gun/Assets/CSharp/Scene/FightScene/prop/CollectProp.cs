using UnityEngine;

namespace EZ
{
    public class CollectProp : BaseProp
    {

        [SerializeField] private GameObject Appear;
        [SerializeField] private GameObject Disappear;

        [SerializeField] private GameObject ParticleEffect;

        [SerializeField] private float KeepTime = 1;

        private float m_CurLiveTime = 0;
        private float m_CurTime = 0;
        private bool m_IsDeath = false;
        private float m_EndedTime = 1;
        private GameObject m_Appear;
        private GameObject m_Disappear;


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
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
            {
                Player player = collision.gameObject.GetComponentInParent<Player>();
                if (player != null)
                {
                    m_IsDeath = true;
                    gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    m_Appear.SetActive(false);
                    m_Disappear.SetActive(true);
                    m_CurTime = 0;
                    BroadGainProp();
                }
            }
        }
    }
}

