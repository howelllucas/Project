using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnderLine : MonoBehaviour
{
    public Text target;
    public RectTransform line;
    public bool checkInUpdate;
    private string lastText;

    void Start()
    {
        lastText = target.text;
        SetLine();
    }

    void Update()
    {
        if (!checkInUpdate || lastText.Equals(target.text))
            return;
        SetLine();
        lastText = target.text;
    }

    private void SetLine()
    {
        var font = target.font;
        int fontSize = target.fontSize;
        string content = target.text;
        var fontStyle = target.fontStyle;
        font.RequestCharactersInTexture(content, fontSize, fontStyle);
        float width = 0;
        CharacterInfo info;
        for (int i = 0; i < content.Length; i++)
        {
            if (font.GetCharacterInfo(content[i], out info, fontSize, fontStyle))
            {
                width += info.advance;
            }
        }

        var lineSize = line.sizeDelta;
        lineSize.x = width;
        line.sizeDelta = lineSize;
    }
}
