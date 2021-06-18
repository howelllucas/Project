using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TextAutoCentering : MonoBehaviour
    {
        public Text targetTxt;
        public RectTransform rect;
        public float offset;

        private string lastTxtVal;

        private void Awake()
        {
            if (targetTxt != null)
                lastTxtVal = targetTxt.text;
        }

        private void Update()
        {
            if (targetTxt == null || rect == null)
                return;
            if (lastTxtVal != targetTxt.text)
            {
                int length = CalculateLengthOfText(targetTxt.text, targetTxt);
                var pos = rect.anchoredPosition;
                pos.x = -0.5f * (offset + length);
                rect.anchoredPosition = pos;
                lastTxtVal = targetTxt.text;
            }
        }

        /// <summary>
        /// 计算字符串在指定text控件中的长度
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        int CalculateLengthOfText(string message, Text tex)
        {
            int totalLength = 0;
            Font myFont = tex.font;  //chatText is my Text component
            myFont.RequestCharactersInTexture(message, tex.fontSize, tex.fontStyle);
            CharacterInfo characterInfo = new CharacterInfo();

            char[] arr = message.ToCharArray();

            foreach (char c in arr)
            {
                myFont.GetCharacterInfo(c, out characterInfo, tex.fontSize);

                totalLength += characterInfo.advance;
            }

            return totalLength;
        }
    }
}