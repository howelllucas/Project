using EZ.Data;
using EZ.DataMgr;
using System;
using UnityEngine;
namespace EZ
{
    public partial class DialogNode
    {
        FollowNode m_FollowNode;

        private void Awake()
        {
            m_FollowNode = GetComponent<FollowNode>();
            CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
            transform.SetParent(campsiteUI.transform, false);
        }
        public void Init(NpcQuestItemDTO questItem, Transform followNode,Action action)
        {
            m_FollowNode.SetFloowNode(followNode);
            gameObject.AddComponent<DelayCallBack>().SetAction(action, 2, true);
            int state = questItem.state;
            CampNpcItem npcItem = Global.gApp.gGameData.CampNpcConfig.Get(questItem.npcId);
            if(state == NpcState.None)
            {
                int plotLength = npcItem.NoTask.Length;
                int plotIndex = UnityEngine.Random.Range(0, plotLength);
                TaskText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(npcItem.NoTask[plotIndex]);
            }
            else if(state == NpcState.Received)
            {
                int plotLength = npcItem.TaskFinished.Length;
                int plotIndex = UnityEngine.Random.Range(0, plotLength);
                TaskText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(npcItem.NoTask[plotIndex]);
            }
            string lgg = Global.gApp.gSystemMgr.GetMiscMgr().Language;
            if (lgg == null || lgg.Equals(GameConstVal.EmepyStr))
            {
                lgg = UiTools.GetLanguage();
            }
            TaskText.text.font = Global.gApp.gGameData.GetFont(lgg);
        }
        public void SetPlotId(int plotId)
        {
            TaskText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(plotId);
            string lgg = Global.gApp.gSystemMgr.GetMiscMgr().Language;
            if (lgg == null || lgg.Equals(GameConstVal.EmepyStr))
            {
                lgg = UiTools.GetLanguage();
            }
            TaskText.text.font = Global.gApp.gGameData.GetFont(lgg);
        }
    }
}
