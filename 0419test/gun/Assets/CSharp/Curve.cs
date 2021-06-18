using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Curve : MonoBehaviour {
    public Curve NextCurve { get; set; }
    private float m_CellSize = 1;
    private float m_Offset = 0;
    private float m_Radio = 10;
    private float m_DtAngle;
    private Vector3 m_CenterPoint;
    public void InitOnce( float radio, float cellSize, float offset, float dtAngle)
    {
        m_Offset = offset;
        m_CellSize = cellSize;
        m_Radio = radio;
        m_DtAngle = dtAngle;
        float centerX = offset;
        float centerY = -radio;
        m_CenterPoint = new Vector3(0, centerY, centerX);
        transform.Find("Cube").transform.localPosition = new Vector3(Random.Range(-4.0f,4f),0,0);
    }
    public void UpdateCell(Vector3 prePos)
    {
        if (prePos.z + m_CellSize <= m_Offset)
        {
            prePos.z = prePos.z + m_CellSize;
            transform.forward = Vector3.forward;
            transform.position = prePos;
        }
        else
        {
            Vector3 vecFrom = prePos - m_CenterPoint;
            Vector3 vecTo = Quaternion.AngleAxis(m_DtAngle, Vector3.right) * vecFrom;
            transform.forward = vecTo - vecFrom;
            Vector3 newPos = m_CenterPoint + vecTo;
            prePos = newPos;
            transform.position = newPos;
        }
        if (NextCurve != null){
            NextCurve.UpdateCell(transform.position);
        }
    }
}
