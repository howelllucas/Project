using EZ;
using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class LanguageTip : MonoBehaviour {

    public int m_TipId;

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
