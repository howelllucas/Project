
using UnityEngine;

namespace EZ
{

    public class NormalElementCtrl : ElementCtrl
    {
        [SerializeField] private GameObject m_ProgressNode;
        [SerializeField] private TextMesh m_TextMesh;
        [SerializeField] private SpriteRenderer m_PercentSprite;
        private Vector2 m_PercentOriSize;
        private bool m_ChildState = true;

        protected override void Init()
        {
            m_PercentOriSize = m_PercentSprite.size;
            PowerDecreaseEnd();
        }
        protected override void PowerDecrease(float percent)
        {
            m_TextMesh.text = GameConstVal.EmepyStr + (int)(percent * 100) + "%";
            m_PercentSprite.size = new Vector2(percent * m_PercentOriSize.x, m_PercentOriSize.y);
        }
        protected override void PowerDecreaseEnd()
        {
            float percent = 0;
            m_TextMesh.text = GameConstVal.EmepyStr + (int)(percent * 100) + "%";
            m_PercentSprite.size = new Vector2(percent * m_PercentOriSize.x, m_PercentOriSize.y);
            m_ProgressNode.SetActive(false);
        }
        protected override void PowerIncrease(float percent)
        {
            m_ProgressNode.SetActive(true);
            m_TextMesh.text = GameConstVal.EmepyStr + (int)(percent * 100) + "%";
            m_PercentSprite.size = new Vector2(percent * m_PercentOriSize.x, m_PercentOriSize.y);
        }
        protected override void PowerIncreaseEnd()
        {
            SetChildActiveState(false);
        }
        protected override void TimeOver()
        {
            PowerDecreaseEnd();
            SetChildActiveState(true);
        }
        protected override void Close()
        {
            PowerDecreaseEnd();
            SetChildActiveState(false);
        }
        private void SetChildActiveState(bool state)
        {
            if (m_ChildState != state)
            {
                m_ChildState = state;
                transform.Find(GameConstVal.EffectNode).gameObject.SetActive(state);
            }
        }
    }
}
