using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EZ;

[System.Serializable]
public class NewbieGuideButton : MonoBehaviour
{
    private Button m_NewbieButton;
    public int[] m_NewbieGuideIds;
    private string m_Param = GameConstVal.EmepyStr;

    

    public int[] NewbieGuideIds
    {
        get
        {
            return m_NewbieGuideIds;
        }

        set
        {
            m_NewbieGuideIds = value;
        }
    }

    public string Param
    {
        get
        {
            return m_Param;
        }

        set
        {
            m_Param = value;
        }
    }

    public Button NewbieButton
    {
        get
        {
            return m_NewbieButton;
        }

        set
        {
            m_NewbieButton = value;
        }
    }

    private void Awake()
    {
        NewbieButton = this.GetComponent<Button>();
        Binding();
    }

    private void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        Global.gApp.gSystemMgr.GetNewbieGuideMgr().OnStart(this);
    }
    
    private void OnClick()
    {
        Global.gApp.gSystemMgr.GetNewbieGuideMgr().OnClick(this);
    }
    private void Binding()
    {
        NewbieButton.onClick.AddListener(OnClick);
    }

}
