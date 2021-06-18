using System;
using System.Collections;
using UnityEngine;
namespace EZ
{
    public class AIBossShow3001 : MonoBehaviour
    {
        private GameObject m_PlayerGo;
        private Action m_ShowCallBack;
        public void Init(GameObject playerGo)
        {
            m_PlayerGo = playerGo;
        }
        public void SetShowCall(Action showCall)
        {
            m_ShowCallBack = showCall;
        }
        public void AddApperaEffect()
        {
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //AddAppearWarningEffect();
            GetComponentInChildren<Animator>().Play(GameConstVal.Show);
            transform.localEulerAngles = new Vector3(0, 0, 148.7f);
            Global.gApp.CurScene.Pause();
            MoveToBoss bossNode = Global.gApp.gKeepNode.GetComponentInChildren<MoveToBoss>();
            bossNode.StartAct(transform, ActStartCallBack, 1.0f);
            //bossNode.m
        }
        private void ShowBossCallBack()
        {
            GetComponentInChildren<Animator>().speed = 1;
            //GetComponentInChildren<Animator>().Play("show");
            gameObject.AddComponent<DelayCallBack>().SetAction(MonsterShowAnimEnd, 3.67f,true);
            if (m_ShowCallBack != null)
            {
                m_ShowCallBack();
            }
        }

        private void ShowBossEndCallBack()
        {
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.StartAct(m_PlayerGo.transform, ActEndedllBack, 0.67f);
        }
        private void ActEndedllBack()
        {
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            GetComponentInChildren<Animator>().Play(GameConstVal.Run);
            Global.gApp.CurScene.Resume();
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.Ended();
            Destroy(this);
        }
        private void ActStartCallBack()
        {
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.StartShowBossAnim(ShowBossCallBack);
        }
        void MonsterShowAnimEnd()
        {
            MoveToBoss bossNode = Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>();
            bossNode.StartShowBossEndAnim(ShowBossEndCallBack);
        }

        private void AddAppearWarningEffect()
        {
            GameObject BossWarning = Global.gApp.gResMgr.InstantiateObj(EffectConfig.BossWarning);
            BossWarning.GetComponent<DelayDestroy>().SetIgnoreSceneTimeScale(true);
            BossWarning.transform.SetParent(Global.gApp.gUiMgr.GetUiCanvasTsf(), false);
            BossWarning.transform.SetAsFirstSibling();
        }
    }
}
