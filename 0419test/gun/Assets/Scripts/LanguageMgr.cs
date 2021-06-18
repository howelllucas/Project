using UnityEngine;
using UnityEngine.UI;
using PT3Localization;
using System;

namespace Game
{
    public class LanguageMgr : Singleton<LanguageMgr>
    {
        public const string LANGUAGE_ENGLISH = "EN"; // 英语
        public const string LANGUAGE_CHINESE = "CN"; // 中文
        public const string LANGUAGE_CHINESETRADITIONAL = "TW"; // 繁体中文
        public const string LANGUAGE_JAPANESE = "JP"; // 日语
        public const string LANGUAGE_FRENCH = "FR"; // 法语
        public const string LANGUAGE_GERMAN = "DE"; // 德语
        public const string LANGUAGE_ITALY = "IT"; // 意语
        public const string LANGUAGE_KOREA = "KR"; // 韩语
        public const string LANGUAGE_RUSSIA = "RU"; // 俄语
        public const string LANGUAGE_SPANISH = "ES"; // 西班牙语
        public const string LANGUAGE_PORTUGUESE = "PO"; // 葡萄牙语 也可以归到英语里面


        private static string languageCode = "CN"; // 多语言文案标识符，通过语言设置获得

        private static string GetLanguageAB(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Afrikaans:
                case SystemLanguage.Arabic:
                case SystemLanguage.Basque:
                case SystemLanguage.Belarusian:
                case SystemLanguage.Bulgarian:
                case SystemLanguage.Catalan:
                    return LANGUAGE_ENGLISH;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    return LANGUAGE_CHINESE;
                case SystemLanguage.ChineseTraditional:
                    return LANGUAGE_CHINESETRADITIONAL;
                case SystemLanguage.Czech:
                case SystemLanguage.Danish:
                case SystemLanguage.Dutch:
                case SystemLanguage.English:
                case SystemLanguage.Estonian:
                case SystemLanguage.Faroese:
                case SystemLanguage.Finnish:
                    return LANGUAGE_ENGLISH;
                case SystemLanguage.French:
                    return LANGUAGE_FRENCH;
                case SystemLanguage.German:
                    return LANGUAGE_GERMAN;
                case SystemLanguage.Greek:
                case SystemLanguage.Hebrew:
                case SystemLanguage.Icelandic:
                case SystemLanguage.Indonesian:
                    return LANGUAGE_ENGLISH;
                case SystemLanguage.Italian:
                    return LANGUAGE_ITALY;
                case SystemLanguage.Japanese:
                    return LANGUAGE_JAPANESE;
                case SystemLanguage.Korean:
                    return LANGUAGE_KOREA;
                case SystemLanguage.Latvian:
                case SystemLanguage.Lithuanian:
                case SystemLanguage.Norwegian:
                case SystemLanguage.Polish:
                case SystemLanguage.Romanian:
                    return LANGUAGE_ENGLISH;
                case SystemLanguage.Portuguese:
                    return LANGUAGE_PORTUGUESE;
                case SystemLanguage.Russian:
                    return LANGUAGE_RUSSIA;
                case SystemLanguage.SerboCroatian:
                case SystemLanguage.Slovak:
                case SystemLanguage.Slovenian:
                    return LANGUAGE_ENGLISH;
                case SystemLanguage.Spanish:
                    return LANGUAGE_SPANISH;
                case SystemLanguage.Swedish:
                case SystemLanguage.Thai:
                case SystemLanguage.Turkish:
                case SystemLanguage.Ukrainian:
                case SystemLanguage.Vietnamese:
                case SystemLanguage.Unknown:
                    return LANGUAGE_ENGLISH;
            }
            return LANGUAGE_CHINESE;
        }

        private void SetLanguage(string languageCode)
        {
            LanguageMgr.languageCode = languageCode;
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <returns>The language.</returns>
        public string GetLanguage()
        {
            return languageCode;
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public void Init()
        {
            //string languageCode = PlayerPrefs.GetString("LanguageCode", "");
            //if ("".Equals(languageCode))
            //{
            SetLanguage(GetLanguageAB(Application.systemLanguage));
            //}
            //else
            //{
            //    SetLanguage(languageCode);
            //}
        }

        /// <summary>
        /// Manuals the set language.
        /// </summary>
        /// <param name="languageCode">Language code.</param>
        public void ManualSetLanguage(string languageCode)
        {
            SetLanguage(languageCode);
            //PlayerPrefs.SetString("LanguageCode", languageCode);
            //PlayerPrefs.Save();
            // todo UI界面刷新
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="key">Key.</param>
        public static string GetText(string key, params object[] args)
        {
            try
            {
                return string.Format(GetText(key), args);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return "[NoDefine]" + key;
        }

        public static string GetText(string key)
        {
            var lUnits = TableMgr.singleton.LanguageTable.GetItemValueByName(key, languageCode);
            if (lUnits != null)
            {
                try
                {
                    return lUnits.ToString().Replace("\\n", "\n");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return "[NoDefine]" + key;
        }
        /// <summary>
        /// Gets the image path.
        /// </summary>
        /// <returns>The image path.</returns>
        /// <param name="key">Key.</param>
        public static string GetImagePath(string key)
        {
            return string.Format("Language/{0}/{1}", languageCode, GetText(key));
        }


        /// <summary>
        /// Sets the UIL anguage.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public void SetUILanguage(GameObject gameObject)
        {
            Text[] texts = gameObject.GetComponentsInChildren<Text>(true);
            foreach (Text text in texts)
            {
                TextAppend ta = text.gameObject.GetComponent<TextAppend>();
                if (ta != null && !ta.keyString.Equals(""))
                {
                    //text.font = languageFont;
                    text.text = GetText(ta.keyString);
                    text.text = text.text.Replace("\\n", "\n");
                }
            }
            Image[] images = gameObject.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                TextAppend ta = image.gameObject.GetComponent<TextAppend>();
                if (ta != null && !ta.keyString.Equals(""))
                {
                    string imgpath = GetImagePath(ta.keyString);
                    var objSpr = Resources.Load(imgpath, typeof(Sprite)) as Sprite;
                    if (objSpr == null)
                        continue;

                    image.sprite = objSpr;
                    image.SetNativeSize();
                }
            }
        }
    }
}
