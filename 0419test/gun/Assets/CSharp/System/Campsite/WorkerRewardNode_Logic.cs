using EZ.Data;
using EZ.DataMgr;
using UnityEngine;
namespace EZ
{
    public partial class WorkerRewardNode
    {
        FollowNode m_FollowNode;
        NpcBehavior m_NpcBehavior;
        private void Awake()
        {
            m_FollowNode = GetComponent<FollowNode>();
            CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
            transform.SetParent(campsiteUI.GetTaskStateNodeTsf(), false);
        }
        public void Init(NpcBehavior npcBehavior, Transform followNode,int dropDiamondCount)
        {
            m_NpcBehavior = npcBehavior;
            m_FollowNode.SetFloowNode(followNode);
            m_Count.text.text = "X" + dropDiamondCount.ToString();
            RewardStateBg.button.onClick.AddListener(OpenTaskDetails);
        }
        private void OpenTaskDetails()
        {
            m_NpcBehavior.RespondClickWorkerDiamond(RewardStateBg.rectTransform.position);
        }
    }
}
