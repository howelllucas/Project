using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class CampNewGuidOne:CampNewGuidBase
    {
        public override void StartCampStep(int step)
        {
            base.StartCampStep(step);
            AddPlot(0,PlotCallBack);
        }
        private void PlotCallBack()
        {
            EndCampStep();
            EndCampRender();
        }
        protected override void EndCampStep()
        {
            Global.gApp.gSystemMgr.GetCampGuidMgr().StepGuidEnd();
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.FreshOldWomanTaskState();
        }
    }
}
