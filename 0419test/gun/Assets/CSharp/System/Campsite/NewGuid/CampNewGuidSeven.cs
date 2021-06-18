using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class CampNewGuidSeven : CampNewGuidBase
    {
        public override void StartCampStep(int step)
        {
            base.StartCampStep(step);
            AddPlot(0,PlotCallBack1);
        }
        private void PlotCallBack1()
        {
            AddBrage();
        }
        public override void BrageClose()
        {
            AddPlot(1, PlotCallBack2);
        }
        protected override void EndCampRender()
        {
            Destroy(this);
        }
        private void PlotCallBack2()
        {
            EndCampStep();
            EndCampRender();
        }
        protected override void EndCampStep()
        {
            Global.gApp.gSystemMgr.GetCampGuidMgr().StepGuidEnd();
        }
    }
}
