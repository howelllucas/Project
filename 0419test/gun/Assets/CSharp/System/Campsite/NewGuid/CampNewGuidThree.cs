using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class CampNewGuidThree : CampNewGuidBase
    {
        private float m_CurTime = 0;
        private bool m_StartBuild = false;
        private int m_Index = 0;
        Vector3[] m_PosDst;
        public override void StartCampStep(int step)
        {
            base.StartCampStep(step);
            m_PosDst = Global.gApp.gSystemMgr.GetCampGuidMgr().GetBuidWallPoss();
            AddPlot(0, Plot1CallBack);
        }
        private void Plot1CallBack()
        {
            Global.gApp.gAudioSource.PlayOneShot(CampbuildClip,true);
            m_StartBuild = true;
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
        }

        private void Update()
        {
            if (m_StartBuild)
            {
                m_CurTime += Time.deltaTime;
                if (m_Index < m_PosDst.Length)
                {
                    GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Build_1);
                    Vector3 pos = m_PosDst[m_Index];
                    effect.transform.position = pos;
                    m_Index++;
                }
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
            campsiteUI.FreshCampLvState(true);
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

        }
    }
}
