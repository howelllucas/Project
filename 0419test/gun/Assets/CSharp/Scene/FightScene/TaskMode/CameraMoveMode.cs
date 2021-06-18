using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public sealed class CameraMoveMode : BaseTaskMode
    {
        [Tooltip(" 新剧情剧情id ")]
        public int m_NewPlotId = -1;
        [Tooltip("摄像机移动时间")]
        public float MoveTime = 2;
        public bool m_EndGoState = true;
        public bool m_IsBoyCry = false;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.Empty;
            gameObject.SetActive(false);
            base.Init(mgr, playerGo);
        }
        public override void BeginTask()
        {
            Global.gApp.CurScene.Pause();
            gameObject.SetActive(true);
            // show ploat 
            base.BeginTask();
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.StartAct(transform, ActEndedllBack1, MoveTime);

            FightNpcPlayer[] fightNpcPlayers = GetComponentsInChildren<FightNpcPlayer>();
            if (!m_IsBoyCry)
            {
                foreach (FightNpcPlayer fightNpcPlayer in fightNpcPlayers)
                {
                    fightNpcPlayer.PlayAnim(GameConstVal.Greet);
                }
            }
            else
            {
                GameObject cryEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Npc_BoyCry);
                foreach (FightNpcPlayer fightNpcPlayer in fightNpcPlayers)
                {
                    fightNpcPlayer.PlayAnim(GameConstVal.Cry);
                    cryEffect.transform.SetParent(fightNpcPlayer.transform,false);
                }
            }
        }
        private void PlotCallBack()
        {
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.StartAct(Global.gApp.CurScene.GetMainPlayer().transform, ActEndedllBack2, MoveTime);
        }
        private void ActEndedllBack2()
        {
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.Ended();
            EndTask();
        }
        private void ActEndedllBack1()
        {
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            ShowNewPlot(m_NewPlotId, PlotCallBack); 
        }
        public override void EndTask()
        {
            Global.gApp.CurScene.Resume();
            base.EndTask();
            gameObject.SetActive(m_EndGoState);
        }
        public override void Destroy()
        {
        }
    }
}
