
using EZ.Data;
using UnityEngine.UI;

namespace EZ {
    public partial class LanguageConfigUI_BtnLanguage
    {
        private string m_Language;
        private LanguageConfigUI m_Parent;

        public void Init(string language, string showTxt, LanguageConfigUI parent)
        {
            gameObject.SetActive(true);
            m_Language = language;
            m_Parent = parent;
            Ontxt.text.text = showTxt;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(OnChangeLanguage);
            UIFresh();
        }

        public void UIFresh()
        {
            active.gameObject.SetActive(Global.gApp.gSystemMgr.GetMiscMgr().Language.Equals(m_Language));
            Ontxt.text.font = Global.gApp.gGameData.GetFont(m_Language);

        }

        private void OnChangeLanguage()
        {
            m_Parent.TouchClose();
            if (Global.gApp.gSystemMgr.GetMiscMgr().Language.Equals(m_Language))
            {
                return;
            }
            Global.gApp.gSystemMgr.GetMiscMgr().Language = m_Language;
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.Language);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

    }
}
