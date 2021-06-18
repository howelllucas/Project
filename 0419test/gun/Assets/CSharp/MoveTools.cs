using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoveTools : MonoBehaviour {

    public float Speed = 6;
    public CurveCtrol m_CurveCtrol;
    public float m_OriX = 0;
	void Start () {
        m_OriX = transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        float oriPos = transform.position.z;
        oriPos -= Time.deltaTime * Speed;
        Transform forward = m_CurveCtrol.GetForward(oriPos);
        if (oriPos <= m_CurveCtrol.Offset)
        {
            transform.position = new Vector3(m_OriX, 0, oriPos);
            transform.localRotation = forward.localRotation;
        }
        else
        {
            float radio = m_CurveCtrol.Radio;
            float posY = Mathf.Sqrt((radio * radio - Mathf.Pow(oriPos - m_CurveCtrol.Offset, 2))) - radio;
            transform.position = new Vector3(m_OriX, posY, oriPos);
            transform.localRotation = forward.localRotation;
        }
        if (oriPos < -30)
        {
            transform.position = new Vector3(0, 0,30);
        }
	}
}
