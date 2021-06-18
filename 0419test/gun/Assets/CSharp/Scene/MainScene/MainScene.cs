
using EZ.Data;
using EZ.Util;
using System.Collections;
using UnityEngine;

namespace EZ
{
    public class MainScene : BaseScene
    {
        private TimerMgr m_TimerMgr;
        public MainScene()
        {
            m_TimerMgr = new TimerMgr();
        }
        public override void Init()
        {
            base.Init();
            Global.gApp.gAudioSource.audioVolumn = 0;
            QualitySettings.shadowDistance = 100;
            Global.gApp.gLightCompt.enabled = false;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">1-logo 2-fight 3-campfight</param>
        public void CacheAndOpenUI(int from)
        {
            //if (from != 1)
            {
                Global.gApp.gUiMgr.ClossAllPanel();
                Global.gApp.gUiMgr.CachePanel(Wndid.GunUI);
                //Global.gApp.gUiMgr.CachePanel(Wndid.CampsiteUI);
                //Global.gApp.gUiMgr.CachePanel(Wndid.SkillUI);
                Global.gApp.gUiMgr.CachePanel(Wndid.ShopPanel);
                Global.gApp.gUiMgr.CachePanel(Wndid.HomeUI);
                Global.gApp.gUiMgr.CachePanel(Wndid.ExpeditionUI);
                Global.gApp.gUiMgr.CachePanel(Wndid.TaskUI);
                Global.gApp.gUiMgr.LastingPanel(Wndid.TokenUI);
                Global.gApp.gUiMgr.OpenPanel(Wndid.TokenUI);
                Global.gApp.gUiMgr.LastingPanel(Wndid.CommonPanel);
                Global.gApp.gUiMgr.OpenPanel(Wndid.CommonPanel);
                Global.gApp.gUiMgr.LastingPanel(Wndid.RewardEffectUi);
                Global.gApp.gUiMgr.OpenPanel(Wndid.RewardEffectUi);
                Global.gApp.gUiMgr.OpenPanel(Wndid.HomeUI);

                if (from == 2)
                {
                    Global.gApp.gUiMgr.OpenPanel(Wndid.ExpeditionUI);
                }

                //InfoCLogUtil.instance.SendClickLog(ClickEnum.BOTTOM_MAIN);
            }
            //else
            //{
            //    Global.gApp.gUiMgr.CachePanel(Wndid.HomeUI);
            //    Global.gApp.gUiMgr.LastingPanel(Wndid.CommonPanel);
            //    Global.gApp.gUiMgr.LastingPanel(Wndid.TokenUI);
            //    Global.gApp.gUiMgr.LastingPanel(Wndid.RewardEffectUi);
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.LogoPanel);
            //}
        }
        public override TimerMgr GetTimerMgr()
        {
            return m_TimerMgr;
        }
        public override void Update(float dt)
        {
            dt = dt * BaseScene.TimeScale;
            m_TimerMgr.Update(dt);
        }
        public override void OnDestroy()
        {
            m_TimerMgr.Destroy();
            Global.gApp.gAudioSource.audioVolumn = 1;
        }
    }
}
