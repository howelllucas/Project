using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampChangeSky : MonoBehaviour {

    enum CurTime
    {
        None = 0,
        DayTime = 1,
        Night = 2,
    }
    MeshRenderer[] m_MeshRenderers;
    CurTime m_CurTimeState = CurTime.None;
    CurTime m_CurTimeRealState = CurTime.None;
    float m_CurTime = 0;
    float m_CheckTime = 0;
    [Tooltip("切换需要的时间 ")]
    public float CheckDayTime = 900;

    [Tooltip("检测间隔")]
    public float CheckDayDtTime = 30;
    public float m_ChangeTime = 4;
	void Start () {
        m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        CheckCurTime();
     }

    private static string KeyWord = "_LightRange";

    private void CheckTime()
    {
        m_CheckTime += Time.deltaTime;
        if (m_CheckTime > CheckDayDtTime)
        {
            m_CheckTime = 0;
            CheckCurTime();
        }
    }
    private void CheckCurTime()
    {
        double curTime = DateTimeUtil.GetMills(DateTime.Now);
        double curDayTime = DateTimeUtil.GetMills(DateTime.Today);
        double curTotalSecond = (curTime - curDayTime) / 1000;
        double curHourSecond = curTotalSecond % 3600;
        int curIndex = Mathf.CeilToInt((float)(curHourSecond / CheckDayTime));
        if (curIndex % 2 == 1)
        {
            if (m_CurTimeRealState == CurTime.Night || m_CurTimeRealState == CurTime.None)
            {
                m_CurTimeState = CurTime.DayTime;
                m_CurTimeRealState = CurTime.DayTime;
                m_CurTime = 0;
            }
        }
        else if (curIndex % 2 == 0)
        {
            if (m_CurTimeRealState == CurTime.DayTime || m_CurTimeRealState == CurTime.None)
            {
                m_CurTimeState = CurTime.Night;
                m_CurTimeRealState = CurTime.Night;
                m_CurTime = 0;
            }
        }

    }
	// Update is called once per frame
	void Update () {

        CheckTime();
        if (m_CurTimeState != CurTime.None)
        {
            if (Time.deltaTime < 0.034f)
            {
                m_CurTime += Time.deltaTime;
            }

            float rate = 0;
            if (m_CurTimeRealState == CurTime.DayTime)
            {
                rate = 1 - m_CurTime / m_ChangeTime;
                if (rate <= 0)
                {
                    rate = 0;
                    m_CurTimeState = CurTime.None;
                    m_CurTime = 0;
                }
            }
            else
            {
                rate = m_CurTime / m_ChangeTime;
                if (rate >= 1)
                {
                    rate = 1;
                    m_CurTimeState = CurTime.None;
                    m_CurTime = 0;
                }
            }
            MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();
            foreach (MeshRenderer meshRenderer in m_MeshRenderers)
            {
                meshRenderer.GetPropertyBlock(matPropertyBlock);
                matPropertyBlock.SetFloat(KeyWord, rate);
                meshRenderer.SetPropertyBlock(matPropertyBlock);
            }
        }
    }
}
