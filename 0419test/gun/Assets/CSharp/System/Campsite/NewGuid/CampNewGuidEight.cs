using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class CampNewGuidEight : CampNewGuidBase
    {

        private float m_CurTime = 0;
        private bool m_StartBuild = false;
        public override void StartCampStep(int step)
        {
            base.StartCampStep(step);
            AddPlot(0,PlotCallBack1);
        }
        private void Update()
        {
            if (m_StartBuild)
            {
                m_CurTime += Time.deltaTime;
                if (m_CurTime > 3)
                {
                    m_StartBuild = false;
                    Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
                    EffectDelay();
                }
            }
        }

        private void EffectDelay()
        {
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.FreshCakeBuild(true);
            AddPlot(1, PlotCallBack2);
        }
        private void PlotCallBack1()
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Build_1);
            effect.transform.position = new Vector3(5, -26, 0);
            m_StartBuild = true;
            Global.gApp.gAudioSource.PlayOneShot(CampbuildClip,true);
            Global.gApp.gGameCtrl.AddGlobalTouchMask();

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
