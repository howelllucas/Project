using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveGroup : MonoBehaviour {

    #region Release Param
    private Curve m_FirstCurve = null;
    private Curve m_EndCurve = null;
    #endregion
    public void InitOnce(Vector3 startPos,float radio,float cellSize,float offset,float dtAngle)
    {
        Curve[] curveUtilssss = GetComponentsInChildren<Curve>();
        Curve preCurve = null;
        foreach (Curve curve in curveUtilssss)
        {
            if(preCurve != null)
            {
                preCurve.NextCurve = curve;
                preCurve = curve;
            }
            else
            {
                preCurve = curve;
                m_FirstCurve = curve;
            }
            curve.InitOnce(radio,cellSize,offset,dtAngle);
        }
        m_EndCurve = curveUtilssss[curveUtilssss.Length - 1];
        UpdateCell(startPos);
    }
    public Transform GetForward(float posZ)
    {
        Curve EffectCurve = m_FirstCurve;
        while(EffectCurve!= null)
        {
            if(EffectCurve.transform.position.z > posZ)
            {
                return EffectCurve.transform;
            }
            else
            {
                EffectCurve = EffectCurve.NextCurve;
            }
        }
        return transform;
    }
    public float CalcNewPos()
    {
        m_EndCurve.NextCurve = m_FirstCurve;
        m_EndCurve = m_FirstCurve;
        m_FirstCurve = m_FirstCurve.NextCurve;
        m_EndCurve.NextCurve = null;
        return m_FirstCurve.transform.position.z;
    }
    public void UpdateCell(Vector3 prePos)
    {
        m_FirstCurve.UpdateCell(prePos);
    }
}
