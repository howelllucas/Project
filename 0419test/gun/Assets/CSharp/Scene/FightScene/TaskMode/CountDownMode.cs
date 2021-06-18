using System.Collections;
using UnityEngine;

namespace EZ
{
    public sealed class CountDownMode : BaseTaskMode
    {
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            base.Init(mgr, playerGo);
            string tips = Global.gApp.gGameData.GetTipsInCurLanguage(m_TargetTipsId[0]);
            m_Tips = tips;
        }
        void Update()
        {
            if (m_StartCountDown)
            {
                float dtTime = BaseScene.GetDtTime();
                if (dtTime > 0)
                {
                    m_CountDownTime = BroadCoolDown(m_CountDownTime, true);
                }
            }
        }
        public override void BeginTask()
        {
            base.BeginTask();
            m_StartCountDown = true;
            BroadTargetTips(1);
        }
        public override void EndTask()
        {
            Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
            base.EndTask();
        }

        public override void Destroy()
        {
        }
    }
}
