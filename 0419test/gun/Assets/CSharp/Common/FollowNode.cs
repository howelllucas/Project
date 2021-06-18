using UnityEngine;
namespace EZ
{
    public class FollowNode : MonoBehaviour
    {
        private Transform m_FollwNode;
        private Transform m_TargetTsf;
        private RectTransform m_FollowRectTsf;
        private RectTransform m_ParentRectTsf;

        public void SetFloowNode(Transform tsf)
        {
            m_FollwNode = tsf;
            m_FollowRectTsf = GetComponent<RectTransform>();
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            m_ParentRectTsf = parentCanvas.GetComponent<RectTransform>();
            FollowGo();
        }
        private void LateUpdate()
        {
            FollowGo();
        }
        private void FollowGo()
        {
            if (m_FollwNode)
            {
                m_FollowRectTsf.anchoredPosition = UiTools.WorldToRectPos(gameObject, m_FollwNode.position, m_ParentRectTsf);
            }
        }
    }
}
