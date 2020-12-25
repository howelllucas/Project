using UnityEngine;
using System.Collections;

/// <summary>
/// 告示UI类
/// describe: 告示UI显示及显示设置(跟随摄像机)
/// 
/// company:广州粤嵌通信科技股份有限公司
/// author:Jatn
/// e-mail:jatn@163.com
/// createdDate:2019-2-14
/// modifiedDate:2019-2-14
/// </summary>
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