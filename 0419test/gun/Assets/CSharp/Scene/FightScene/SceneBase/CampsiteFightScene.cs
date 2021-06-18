using System.Collections;
using System.Collections.Generic;
using EZ.Data;
using UnityEngine;
using Game;

namespace EZ
{
    public class CampsiteFightScene : FightScene
    {
        private int levelId;

        public CampsiteFightScene(int levelId, PassItem passData) : base(passData)
        {
            this.levelId = levelId;
        }

        public override void GameWin()
        {
            if (m_Ended) { return; }
            m_Ended = true;
            // log
            FightResultManager.instance.KillProgress = 100;
            FightResultManager.instance.SetFightState(FightResultManager.FightState.SUCCESS);
            Global.gApp.gUiMgr.ClosePanel(Wndid.FightPanel);
            GetMainPlayerComp().GetPlayerData().ResetProtectTime(-100000);
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightWinPanel);
            FightWin panel = Global.gApp.gUiMgr.GetPanelCompent<FightWin>(Wndid.FightWinPanel);
            panel.OnClose += () =>
            {
                CampsiteMgr.singleton.OnCampsiteLevelWin();
                Global.gApp.gGameCtrl.ChangeToMainScene(3);
            };

            //CampTaskMgr.singleton.AddTaskData(TaskType.Finish_Battle, 1);
        }

        public override void GameLose()
        {
            if (m_Ended) { return; }
            Pause();
            m_Ended = true;
            FightResultManager.instance.SetFightState(FightResultManager.FightState.FAIL);
            Global.gApp.gUiMgr.ClosePanel(Wndid.FightPanel);
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightLosePanel);
            FightLose panel = Global.gApp.gUiMgr.GetPanelCompent<FightLose>(Wndid.FightLosePanel);
            panel.OnClose += () =>
            {
                Global.gApp.gGameCtrl.ChangeToMainScene(3);
            };
        }
    }
}