using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System.Collections;
using UnityEngine;

namespace EZ
{
    public class LogoUi_Logic : BaseUi
    {
        private float m_CurTime = 0;
        private float m_ChangeFightTime = 2.15f;
        private float m_ChangeTime = 2.2f;
        private bool m_ShowLogo = false;
        private float m_LogoTime = 2.5f;
		private bool m_InitSDK = false;
        private int m_OpenNet = 0;
        private bool m_End = false;
		private int m_WebCheckSwitch;
        private bool m_HaveWeb = true;
        Animator m_Animator;
        [SerializeField]private GameObject m_Logo;
        [SerializeField]private GameObject m_Bg;
        [SerializeField]private GameObject m_Bg1;
        private void Awake()
        {
            Debug.Log("LogoUi Awake");
            m_Animator = GetComponent<Animator>();
            m_Bg.SetActive(false);
            m_Bg1.SetActive(false);
            m_Animator.Play("LogoAnim", -1, 0);
            //gameObject.AddComponent<DelayCallBack>().SetAction(() => { m_Animator.enabled = true;
            //    m_Logo.SetActive(true);
            //}, 0.1f, true);
            m_WebCheckSwitch = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.WEB_TIME_CHECK_SWITCH).content);
            if (!Global.m_SDK)
            {
                m_InitSDK = true;
                StartCoroutine(DelayInit());
                StartCoroutine(DelayInitBugly());
            }
            

			if (m_WebCheckSwitch == 1)
            {
                //无网络
	            //if (SdkdsNativeUtil.Instance.GetCurrentNetStatue() != 4)
	            //{
             //       m_HaveWeb = true;
             //       gameObject.AddComponent<DelayCallBack>().SetAction(() =>
	            //    {
	            //        StartCoroutine(DateTimeUtil.GetWebUnixTimeAsyn());
	            //    }, 2.2f, true);
	            //}
            }
        }

		private IEnumerator DelayInit()
        {
            yield return new WaitForSeconds(2.2f);
            //Global.InitSDK();
        }

        private IEnumerator DelayInitBugly()
        {
            yield return new WaitForSeconds(4.2f);
            //Global.InitBugly();
        }

        private void Update()
        {
            m_CurTime += Time.deltaTime;
            if (m_ShowLogo)
            {
                if (m_CurTime > m_LogoTime)
                {
                    m_ShowLogo = false;
                    m_CurTime = 0;
                    //bool visible = UnityEngine.Random.Range(0, 100) > 50;
                    //m_Bg.SetActive(visible);
                    //m_Bg1.SetActive(!visible);
                    m_Bg.SetActive(true);
                    m_Logo.SetActive(false);
                    Global.gApp.gUiMgr.CachePanel(Wndid.GunUI);
                    Global.gApp.gUiMgr.CachePanel(Wndid.SkillUI);
                    Global.gApp.gUiMgr.CachePanel(Wndid.CampsiteUI);

                    m_Animator.Play("BgAnim1", -1, 0);

                    
                }
            }
            else
            {
                if (m_CurTime > m_ChangeFightTime && !m_End)
                {
                    if (m_OpenNet == 1)
                    {
                        return;
                    }
                    
                    
                    if (m_OpenNet == 0 && m_WebCheckSwitch == 1)
                    {
                        string checkWebTime = DateTimeUtil.CheckWebTime(DateTimeUtil.m_WebDateTime);
                        //如果有网，但是网卡，还是让玩的
                        if (m_HaveWeb && checkWebTime.Equals(CheckNetTypeConstVal.NO_NET))
                        {
                            checkWebTime = CheckNetTypeConstVal.RIGHT;
                        }
                        if (!checkWebTime.Equals(CheckNetTypeConstVal.RIGHT))
                        {
                            m_OpenNet = 1;
                            Global.gApp.gUiMgr.OpenPanel(Wndid.CheckNetUI, checkWebTime);
                            CheckNetUI checkNetUI = Global.gApp.gUiMgr.GetPanelCompent<CheckNetUI>(Wndid.CheckNetUI);
                            checkNetUI.m_RightAction = () =>
                            {
                                m_OpenNet = 2;
                            };
                            return;
                        }
                    }
                    //处理时间相关逻辑  在验证时间后进行
                    Global.gApp.gSystemMgr.AfterInit();

                    m_Animator.Play("BgAnim2", -1, 0);
                    m_End = true;

                    gameObject.AddComponent<DelayCallBack>().SetAction(()=> { EndAction(); }, 0.67f, true);
                }
            }
        }

        private void EndAction()
        {
            if (m_CurTime > m_ChangeFightTime)
            {
                if (Global.gApp.gSystemMgr.GetMiscMgr().FirstMain == 0 && Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() == 1)
                {
                    Global.gApp.gUiMgr.OpenPanel(Wndid.RewardEffectUi);
                    Global.gApp.gUiMgr.OpenPanel(Wndid.CommonPanel);
                    Global.gApp.gUiMgr.OpenPanel(Wndid.TokenUI);
                    Global.gApp.gUiMgr.ClosePanel(Wndid.LogoPanel);
                    //string[] consumeItemStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GAME_CONSUME_ITEMS).contents;
                    //bool result = GameItemFactory.GetInstance().ReduceItem(consumeItemStr, BehaviorTypeConstVal.OPT_GAME_CONSUME);
                    PassItem passItem = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassItem();
                    if (passItem != null)
                    {
                        Global.gApp.gSystemMgr.GetPassMgr().EnterPass();
                        Global.gApp.gUiMgr.ClossAllPanel();
                        Global.gApp.gGameCtrl.ChangeToFightScene(passItem.id);
                    }
                    return;
                }
            }
            if (m_CurTime > m_ChangeTime)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.RewardEffectUi);
                Global.gApp.gUiMgr.ClosePanel(Wndid.LogoPanel);
                Global.gApp.gUiMgr.CachePanel(Wndid.ShopPanel);
                Global.gApp.gUiMgr.OpenPanel(Wndid.HomeUI);
                Global.gApp.gUiMgr.OpenPanel(Wndid.CommonPanel);
                Global.gApp.gUiMgr.OpenPanel(Wndid.TokenUI);

                //InfoCLogUtil.instance.SendClickLog(ClickEnum.BOTTOM_MAIN);
            }
        }
    }
}
