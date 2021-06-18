using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public sealed class BoyPlotMode : BaseTaskMode
    {
        [Tooltip(" 新剧情剧情id ")]
        public int m_NewPlotId = -1;
        [Tooltip(" 新剧情剧情id ")]
        public int m_NewPlotId2 = -1;
        [Tooltip(" 新剧情剧情id ")]
        public int m_NewPlotId3 = -1;
        [Tooltip("主角移动的时间")]
        public float MoveTime = 0.3f;
        BornNode[] m_BornNodes;
        BornNode m_CurBornNode = null;
        bool m_StartMoveToNpc = false;
        float m_CurMoveTime = 0;
        Transform m_BoyNpcTsf;
        Player m_Player;
        float m_LockX = 0;
        float m_LockY = 0;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.Empty;
            gameObject.SetActive(false);
            m_Player = Global.gApp.CurScene.GetMainPlayerComp();
            base.Init(mgr, playerGo);
        }
        public override void BeginTask()
        {
            m_BornNodes = (Global.gApp.CurScene as FightScene).GetBornNodes();
            gameObject.SetActive(true);
            m_Player.LockMove(9999999);
            // show ploat 
            base.BeginTask();
        }
        private void Update()
        {
            if (m_CurBornNode == null)
            {
                bool hasNewDstNode = GenerateBornNode();
                if (hasNewDstNode)
                {
                    GameObject Npc_boy = Global.gApp.gResMgr.InstantiateObj("Prefabs/Campsite/NpcFight/Npc_boy");
                    Npc_boy.transform.position = m_CurBornNode.transform.position;
                    m_BoyNpcTsf = Npc_boy.transform;
                    FightNormalNpcPlayer npcPlayer = Npc_boy.GetComponent<FightNormalNpcPlayer>();
                    npcPlayer.FightCloseToRoleBehavior.SetPursueCallBack(CloseEndCallBack);
                    npcPlayer.Init();
                    npcPlayer.SetBehavior(FightNpcPlayer.NpcBehaviorType.CloseToRole);
                    Global.gApp.gGameCtrl.AddGlobalTouchMask();
                }
            }
            if (m_StartMoveToNpc)
            {
                m_CurMoveTime += BaseScene.GetDtTime();
                if(m_CurMoveTime < MoveTime)
                {
                    m_Player.GetFight().Move(m_LockX, m_LockY);
                }
                else
                {
                    m_Player.GetFight().Move(0, 0);
                    m_StartMoveToNpc = false;
                    Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
                    ShowNewPlot(m_NewPlotId2, Plot2EndCallBack);
                }
            }
        }
        private void Plot2EndCallBack()
        {
            FightNormalNpcPlayer npcPlayer = m_BoyNpcTsf.GetComponent<FightNormalNpcPlayer>();
            //npcPlayer.FightFarAwayRoleBehavior.SetPursueCallBack(FarAwayEndCallBack);
            npcPlayer.SetBehavior(FightNpcPlayer.NpcBehaviorType.FarAwayRole);
            Destroy(npcPlayer, 5);
            ShowNewPlot(m_NewPlotId3, Plot3EndCallBack);
        }
        private void Plot3EndCallBack()
        {
            ShowRoleTips();
        }
        private void PlotEndCallBack()
        {
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            Vector3 dtVec = m_BoyNpcTsf.position - m_Player.transform.position;
            m_LockX = dtVec.x;
            m_LockY = dtVec.y;
            m_StartMoveToNpc = true;
        }
        private void FarAwayEndCallBack()
        {
            //ShowNewPlot(m_NewPlotId3, Plot3EndCallBack);
        }
        private void ShowRoleTips()
        {
            BroadPlotTips(0);
            m_Player.LockMove(0.01f);
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            EndTask();
        }
        private void CloseEndCallBack()
        {
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            ShowNewPlot(m_NewPlotId, PlotEndCallBack);
        }
        public bool GenerateBornNode()
        {
            if (m_CurBornNode != null && m_CurBornNode.GetIsOutMap())
            {
                return false;
            }
            foreach (BornNode bornNode in m_BornNodes)
            {
                if (bornNode.GetIsOutMap())
                {
                    m_CurBornNode = bornNode;
                    return true;
                }
            }
            return false;
        }

        public override void Destroy()
        {
        }
        public override void EndTask()
        {
            gameObject.SetActive(false);
            base.EndTask();
        }
    }
}
