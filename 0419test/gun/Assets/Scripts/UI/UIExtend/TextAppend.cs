using System;
using UnityEngine.UI;
using UnityEngine;

namespace PT3Localization
{
    public class TextAppend : MonoBehaviour
    {
        [SerializeField] protected string m_KeyString;
        public string keyString
        {
            get { return this.m_KeyString; }
            set { this.m_KeyString = value; }
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(keyString))
                return;

            var text = GetComponent<Text>();
            if (text == null)
                return;

            text.text = Game.LanguageMgr.GetText(keyString);
        }
    }
}
