using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class CampNewGuidTwo : CampNewGuidBase
    {
   
        private int m_Index = 0;
        private float m_CurTime = 0;
        private bool m_StartBuild = false;
        Vector3[] m_PosDst;
        AudioSource m_CheerAudioSource;
        public override void StartCampStep(int step)
        {
            base.StartCampStep(step);
            m_PosDst = Global.gApp.gSystemMgr.GetCampGuidMgr().GetBuidPoss();
            AddPlot(0, Plot1CallBack);
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
        // 建筑升级 npc 欢呼 每个人掉落一颗红心
        // 实际逻辑为 先掉落红心 + 建筑升级 + 欢呼 
        // 红心数据数值型 的必须 掉落完 就算升级结束
        private void Plot1CallBack()
        {
            m_StartBuild = true;
            Global.gApp.gAudioSource.PlayOneShot(CampbuildClip,true);
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
        }
        private void EffectDelay()
        {
            m_CheerAudioSource = Global.gApp.gAudioSource.PlayLoopSource("camp_guid_cheer");
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.PlayNPCheerAnimAndFreshHeart();
            EndCampStep();
            campsiteUI.FreshBuild(true);
            AddPlot(1, Plot2CallBack);
        }
        // 建筑升级
        private void Plot2CallBack()
        {
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.ResetOriAnim();
            Global.gApp.gAudioSource.DestroyLoopSource(m_CheerAudioSource);
            EndCampRender();
        }

        public override void BrageClose()
        {
            CampsiteUI campsiteUI = gameObject.GetComponent<CampsiteUI>();
            campsiteUI.FreshBadgeByGuid();
            base.BrageClose();
        }

        protected override void EndCampStep()
        {
            Global.gApp.gSystemMgr.GetCampGuidMgr().StepGuidEnd();
            Global.gApp.gSystemMgr.GetNpcMgr().ResetHeartTime();
        }
    }
}
