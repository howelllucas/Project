
using UnityEngine;

namespace EZ
{
    public partial class PetNameUi
    {
        private Transform m_FollwNode;
        private RectTransform m_FollowRectTsf;
        private RectTransform m_ParentRectTsf;

        public override void SetFloowNode(Transform tsf)
        {
            m_FollwNode = tsf;
            m_FollowRectTsf = GetComponent<RectTransform>();
            m_FollowRectTsf.localScale = tsf.localScale;
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            m_ParentRectTsf = parentCanvas.GetComponent<RectTransform>();
            FollowMonster();
        }
        public void SetName(string name)
        {
            m_NameText.text.text = name;
        }
        private void LateUpdate()
        {
            FollowMonster();
        }
        private void FollowMonster()
        {
            if (m_FollwNode)
            {
                m_FollowRectTsf.anchoredPosition = UiTools.WorldToRectPos(gameObject, m_FollwNode.position, m_ParentRectTsf);
            }
        }
    }
}
