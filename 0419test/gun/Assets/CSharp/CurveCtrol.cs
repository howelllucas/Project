using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class CurveCtrol : MonoBehaviour {

  
    public int Speed = 5; 
    public int Offset = 0; 
    public int Radio = 10;
    public float CellSize = 1;
    public float StartZ = 1;
    public float Border = -12;
    public float m_MaxLength;
    public float m_DtAngle;
    CurveGroup m_CurveGroups;
	// Use this for initialization
    private void Start()
    {
        m_CurveGroups = GetComponentInChildren<CurveGroup>();
        float halfCell = CellSize / 2;
        float DtAngle = Mathf.Asin(halfCell / Radio) * 2 * Mathf.Rad2Deg;
        m_DtAngle = DtAngle;
        Vector3 startPos = new Vector3(0, 0, StartZ);
        m_CurveGroups.InitOnce(startPos, Radio, CellSize, Offset, DtAngle);
    }
    public Transform GetForward(float posZ)
    {
        CurveGroup curveGroups = GetComponentInChildren<CurveGroup>();
        return curveGroups.GetForward(posZ);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            CurveGroup curveGroups = GetComponentInChildren<CurveGroup>();
            float halfCell = CellSize / 2;
            float DtAngle = Mathf.Asin(halfCell / Radio) * 2 * Mathf.Rad2Deg;
            m_DtAngle = DtAngle;
            Vector3 startPos = new Vector3(0, 0, StartZ);
            curveGroups.InitOnce(startPos, Radio, CellSize, Offset, DtAngle);
        }
    }
#endif

    private void Update()
    {
        float dtTime = Time.deltaTime;
        StartZ -= Speed * dtTime;
        Vector3 startPos = new Vector3(0, 0, StartZ);
        m_CurveGroups.UpdateCell(startPos);
        if(StartZ < Border)
        {
            StartZ = m_CurveGroups.CalcNewPos() - CellSize;
        }
    }
}
