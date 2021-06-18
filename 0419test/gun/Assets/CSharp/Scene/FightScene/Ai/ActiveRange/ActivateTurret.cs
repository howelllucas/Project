using UnityEngine;
namespace EZ
{

    public class ActivateTurret : BaseActiveRange
    {
        private float m_CurTime = 0;
        private int m_Symbole = 0;
        public GameObject m_OpenRange;
        public GameObject m_GreenRange;
        public GameObject m_ProgressNode;
        public TextMesh m_TextMesh;
        public SpriteRenderer m_PercentSprite;
        private Vector2 m_PercentOriSize;

        protected override void Awake()
        {
            base.Awake();
            m_PercentOriSize = m_PercentSprite.size;
        }
        private void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime() * m_Symbole;
            m_CurTime = Mathf.Max(m_CurTime, 0);
      
            if (m_CurTime >= ActiveTime && !m_Active)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                GetComponentInParent<S_FireTurret>().StartBorn();
                enabled = false;
                SetActiveTrue();
                m_ProgressNode.SetActive(false);
                m_OpenRange.SetActive(false);
                m_GreenRange.SetActive(false);
       
            }
            if (!m_Active)
            {

                if (m_CurTime > 0)
                {
                    m_ProgressNode.SetActive(true);
                    float percent = m_CurTime / ActiveTime;
                    m_TextMesh.text = GameConstVal.EmepyStr + (int)(percent * 100) + "%";
                    m_PercentSprite.size = new Vector2(percent * m_PercentOriSize.x, m_PercentOriSize.y);
                }
                else if (m_CurTime == 0)
                {
                    m_ProgressNode.SetActive(false);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_Active)
            {
                if (collision.gameObject.CompareTag(GameConstVal.MainRoleTag))
                {
                    m_Symbole = 1;
                    m_OpenRange.SetActive(false);
                    m_GreenRange.SetActive(true);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!m_Active)
            {
                if (collision.gameObject.CompareTag(GameConstVal.MainRoleTag))
                {
                    m_OpenRange.SetActive(true);
                    m_GreenRange.SetActive(false);
                    m_Symbole = -1;
                }
            }
        }
    }
}
