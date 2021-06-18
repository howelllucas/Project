using UnityEngine;

namespace EZ
{
    public partial class FightNpcPlotUi
    {
        FollowNode m_FollowNode;
        private void Awake()
        {
            m_FollowNode = gameObject.GetComponentInChildren<FollowNode>();
        }
        public void SetFollowNode(Transform followNode)
        {
            m_FollowNode.SetFloowNode(followNode);
        }
    }
}
