using EZ;
using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class BubbleTip : MonoBehaviour {

    public float m_X;
    public float m_Y;
    public float m_ArrowX;
    public float m_ArrowY;
    public float m_ArrowRotationZ;
    public int m_TipId;
    public Image m_Image;
    private Button button;

    public void AddButton()
    {
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }
        button.targetGraphic = m_Image;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OpenBubble);
    }

    private void OpenBubble()
    {
        Global.gApp.gUiMgr.OpenPanel<string>(Wndid.Bubble, m_TipId.ToString());
        Bubble ui = Global.gApp.gUiMgr.GetPanelCompent<Bubble>(Wndid.Bubble);
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        ui.SetPos(m_X, m_Y, m_ArrowX, m_ArrowY, m_ArrowRotationZ, rt.position);
    }

    public int TipId
    {
        get
        {
            return m_TipId;
        }

        set
        {
            m_TipId = value;
        }
    }

}
