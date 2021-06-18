using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class CampNewGuidFive : CampNewGuidBase
    {
        public override void StartCampStep(int step)
        {
            base.StartCampStep(step);
            AddPlot(0, Plot1CallBack);
        }
        private void Plot1CallBack()
        {
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.FreshTaskIconState(true);
            AddPlot(1, Plot2CallBack);
        }
        private void Plot2CallBack()
        {
            EndCampStep();
            EndCampRender();
        }
        protected override void EndCampStep()
        {
            Global.gApp.gSystemMgr.GetCampGuidMgr().StepGuidEnd();
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.FreshAllNpcTaskState();

        }
    }
}
