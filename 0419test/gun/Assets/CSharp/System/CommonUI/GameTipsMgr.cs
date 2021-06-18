using UnityEngine;

namespace EZ
{
    public class GameTipsMgr
    {

        public GameTipsMgr()
        {
            RegisterListener();
        }
        void ShowGameTips(string text)
        {
            GameObject tips = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/GameTipsUi");
            tips.transform.SetParent(Global.gApp.gUiMgr.GetTopCanvasTsf(), false);
            tips.GetComponent<GameTipsUi>().ShowText(text);
        }
       
        void ShowGameTips(int id)
        {
            GameObject tips = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/GameTipsUi");
            tips.transform.SetParent(Global.gApp.gUiMgr.GetTopCanvasTsf(), false);
            tips.GetComponent<GameTipsUi>().ShowText(id);
        }

        void ShowGameTips(int id, string param)
        {
            GameObject tips = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/GameTipsUi");
            tips.transform.SetParent(Global.gApp.gUiMgr.GetTopCanvasTsf(), false);
            tips.GetComponent<GameTipsUi>().ShowText(id, param);
        }

        void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ShowGameTipsByID, ShowGameTips);
            Global.gApp.gMsgDispatcher.AddListener<int, string>(MsgIds.ShowGameTipsByIDAndParam, ShowGameTips);
            Global.gApp.gMsgDispatcher.AddListener<string>(MsgIds.ShowGameTipsByStr, ShowGameTips);

            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.ShowGameTipsByID);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.ShowGameTipsByIDAndParam);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.ShowGameTipsByStr);
        }
    }
}
