
using EZ.Data;

namespace EZ {
    public partial class NGPlotUI
    {
        public void ShowText(int id)
        {
            TipsItem tipsItem = Global.gApp.gGameData.TipsData.Get(id);
            if (tipsItem != null)
            {
                ShowText(Global.gApp.gGameData.GetTipsInCurLanguage(id));
            } else
            {
                ShowText(string.Format(TipsFormatConstValpublic.NO_TIPS_CONFIG_FOMAT, id));
            }
            
        }

        public void ShowText(int id, string param)
        {
            TipsItem tipsItem = Global.gApp.gGameData.TipsData.Get(id);
            if (tipsItem != null)
            {
                ShowText(string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(id), param));
            }
            else
            {
                ShowText(string.Format(TipsFormatConstValpublic.NO_TIPS_CONFIG_FOMAT, id));
            }

        }
        public void ShowText(string text)
        {
            Plottxt.text.text = text;
            string lgg = Global.gApp.gSystemMgr.GetMiscMgr().Language;
            if (lgg == null || lgg.Equals(GameConstVal.EmepyStr))
            {
                lgg = UiTools.GetLanguage();
            }
            Plottxt.text.font = Global.gApp.gGameData.GetFont(lgg);
        }
    }
}
