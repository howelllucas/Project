using EZ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUi_Scroll : MonoBehaviour
{

    float m_TotalHeight;
    float m_CurHeight;
    float m_Height = 200;
    RectTransform m_RectTsf;
    public GameObject m_Prefab;
    private int CreateCount = 20;
    int m_CurCreateCount = 0;
    float m_CurTime = 0;
    float m_TotalTime = 0;
    float m_AvrSpeed = 1000;
    private void Start()
    {
        int childCount = CreateCount - 1;
        m_TotalHeight = childCount * m_Height;
        m_RectTsf = GetComponent<RectTransform>();
        m_TotalTime = m_TotalHeight / m_AvrSpeed;
        CreateCell(3);
        InitCall();
    }
    void CreateCell(int count)
    {
        if (m_CurCreateCount < CreateCount)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject cell = Instantiate<GameObject>(m_Prefab);
                cell.transform.SetParent(transform, false);
                cell.transform.SetAsLastSibling();
                cell.SetActive(true);
                cell.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, m_CurCreateCount * m_Height);
                m_CurCreateCount++;
            }
        }
    }
    void InitCall()
    {
        DelayCallBack callBack = gameObject.AddComponent<DelayCallBack>();
        float IncSpeed = 1;
        float Sympble = 1;
        float AdSpeed = 1f;
        float RectAdSpeed = 1f;
        bool calcSpeed = true;
        callBack.SetAction(
            () =>
            {
                float dtTime = 0.01667f;
                m_CurTime += IncSpeed * Sympble * dtTime;
                Debug.Log(IncSpeed);
                float rate = m_CurTime / m_TotalTime;
                if (rate >= 1f)
                {
                    rate = 1;
                    enabled = false;
                    callBack.enabled = false;
                }
                m_CurHeight = Mathf.Lerp(0, m_TotalHeight, rate);
                m_RectTsf.anchoredPosition = new Vector2(0, -m_CurHeight);
                CreateCell(1);
                if (rate < 0.4f)
                {
                    IncSpeed = IncSpeed + AdSpeed * dtTime;
                }
                else if (rate > 0.8f)
                {
                    if (calcSpeed)
                    {
                        calcSpeed = false;
                        float newTime = m_TotalTime * (1 - rate);
                        float endSpeed = 0f;
                        float t = 2 * newTime / (IncSpeed + endSpeed);
                        AdSpeed = (IncSpeed - endSpeed) / t;
                        RectAdSpeed = AdSpeed;
                    }
                    if(IncSpeed < 0.35f)
                    {
                        AdSpeed = RectAdSpeed / 2;
                    }
                    IncSpeed = IncSpeed - AdSpeed * dtTime;
                    IncSpeed = Mathf.Max(0.05f, IncSpeed);
                }
            },
            0);
        callBack.SetCallTimes(9999999);
    }
}
