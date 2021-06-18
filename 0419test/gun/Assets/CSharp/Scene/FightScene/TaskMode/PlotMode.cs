using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public sealed class PlotMode : BaseTaskMode
    {
        [Tooltip(" 新剧情剧情id ")]
        public int m_NewPlotId = -1;
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
            base.BeginTask();
            ShowNewPlot(m_NewPlotId, EndTask);
        }
        public override void EndTask()
        {
            Global.gApp.CurScene.Resume();
            base.EndTask();
        }
        public override void Destroy()
        {
        }
    }
}
