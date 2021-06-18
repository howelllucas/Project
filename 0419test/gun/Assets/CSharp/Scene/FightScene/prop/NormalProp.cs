using UnityEngine;

namespace EZ
{
    public class NormalProp : BaseProp
    {

        [SerializeField] protected float m_ActiveTime = 2;
        [SerializeField] private GameObject m_ProgressNode;
        [SerializeField] private TextMesh m_TextMesh;
        [SerializeField] private SpriteRenderer m_PercentSprite;
        private float m_CurTime = 0;
        private bool m_Active = false;
        private int m_Symbole = 0;
        private Vector2 m_PercentOriSize;

        void Start()
        {
            m_PercentOriSize = m_PercentSprite.size;
        }
        public override void Update()
        {
            base.Update();
            m_CurTime = m_CurTime + BaseScene.GetDtTime() * m_Symbole;
            m_CurTime = Mathf.Max(m_CurTime, 0);
            if (!m_Active)
            {
                if (m_CurTime >= m_ActiveTime)
                {
                    GetComponent<Collider2D>().enabled = false;
                    enabled = false;
                    m_Active = true;
                    m_ProgressNode.SetActive(false);
                    BroadGainProp();
                    SetActiveFalse();
                    return;
                }
                if (m_CurTime > 0)
                {
                    if (!m_ProgressNode.activeSelf)
                    {
                        BroadCollectingProp();
                    }
                    m_ProgressNode.SetActive(true);
                    float percent = m_CurTime / m_ActiveTime;
                    m_TextMesh.text = GameConstVal.EmepyStr + (int)(percent * 100) + "%";
                    m_PercentSprite.size = new Vector2(percent * m_PercentOriSize.x, m_PercentOriSize.y);
                }
                else if (m_CurTime == 0)
                {
                    m_ProgressNode.SetActive(false);
                }
            }
        }
        private void SetActiveFalse()
        {
            int childCountCount = transform.childCount;
            for(int i = 0;i< childCountCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GetComponent<Collider2D>().enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
            {
                m_Symbole = 1;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
            {
                m_Symbole = -1;
            }
        }
    }
}

