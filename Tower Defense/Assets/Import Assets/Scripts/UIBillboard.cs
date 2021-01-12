using UnityEngine;
using System.Collections;

//ui看向摄像机
public class UIBillboard : MonoBehaviour
{
    /// <summary>
    /// 摄像机变换对象
    /// </summary>
    private Transform camTrans;

    /// <summary>
    /// 跟随对象
    /// </summary>
    private Transform trans;

    void Awake()
    {
        camTrans = Camera.main.transform;
        trans = transform;
    }

    void Update()
    {
        transform.LookAt(trans.position + camTrans.rotation * Vector3.forward, camTrans.rotation * Vector3.up);
    }
}