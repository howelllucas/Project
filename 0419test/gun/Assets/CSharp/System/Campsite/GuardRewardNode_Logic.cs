using EZ.Data;
using EZ.DataMgr;
using UnityEngine;
namespace EZ
{
    public partial class GuardRewardNode
    {
        FollowNode m_FollowNode;
        private void Awake()
        {
            m_FollowNode = GetComponent<FollowNode>();
            CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
            transform.SetParent(campsiteUI.transform, false);
        }
        public void Init(Transform followNode)
        {
            RewardStateBg.button.onClick.AddListener(AddReward);
            m_FollowNode.SetFloowNode(followNode);
        }
         public void AddReward()
        {
            double addCount = Global.gApp.gSystemMgr.GetNpcMgr().AddGuardReward(GetTextPos());
            if (addCount > 0)
            {
                Destroy(gameObject);
            }
        }
        public Vector3 GetTextPos()
        {
            return RewardCount.rectTransform.position;
        }
        public void Fresh(double val)
        {
            RewardCount.text.text = UiTools.FormateMoney(val);
        }
    }
}
