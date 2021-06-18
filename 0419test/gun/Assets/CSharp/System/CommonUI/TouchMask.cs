using EZ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchMask : MonoBehaviour {

    // Use this for initialization
    private int m_Ref = 0;
    private string m_PanelName;
    private BaseUi m_Panel;
    private Action m_Action;
    public void Awake()
    {
        m_PanelName = null;
        m_Panel = null;
        m_Action = null;
    }
    public void AddCloseListener(string name,BaseUi panel,Action act)
    {
        m_PanelName = name;
        m_Panel = panel;
        m_Action = act;
        AddBtnListener();
    }
    public void AddCloseListener(string name,BaseUi panel)
    {
        m_PanelName = name;
        m_Panel = panel;
        AddBtnListener();
    }
    public void AddCloseListener(string name)
    {
        m_PanelName = name;
        AddBtnListener();
    }
    private void ClickCallBack()
    {
        if(m_PanelName != null)
        {
            if (m_Panel)
            {
                m_Panel.TouchClose(); 
            }
            else
            {
                Global.gApp.gUiMgr.ClosePanel(m_PanelName);
            }
        }
        else if(m_Action != null)
        {
            m_Action();
        }
    }

    private void AddBtnListener()
    {
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(ClickCallBack);
    }
    public void AddRef()
    {
        m_Ref = m_Ref + 1;
        if(m_Ref == 1)
        {
            gameObject.SetActive(true);
        }
    }
    public void RemoveRef()
    {
        m_Ref = m_Ref - 1;
        m_Ref = Mathf.Max(m_Ref,0);
        if(m_Ref == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
