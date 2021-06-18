using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    private Transform targetTrans;
    private Vector3 dirWithTarget;
    //缩放相机视图
    public int scrollSpeed=5;
    private float distance;
    //旋转视图
    public int rotateSpeed=5;
    private bool isRotating=false;
    // Start is called before the first frame update
    void Start()
    {
        targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        this.transform.LookAt(targetTrans.position);
        dirWithTarget = this.transform.position - targetTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = dirWithTarget + targetTrans.position;
        RotateView();
        ScrollView();
        
    }

    private void ScrollView()
    {
        //获取距离的模长
        distance = dirWithTarget.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
        //距离的单位向量*模长
        distance = Mathf.Clamp(distance, 2, 18);
        dirWithTarget = dirWithTarget.normalized * distance;
    }
    private void RotateView()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
        if (isRotating)
        {
            Quaternion angle=transform.rotation;
            Vector3 pos = transform.position;
            this.transform.RotateAround(targetTrans.position, targetTrans.up, Input.GetAxis("Mouse X") * rotateSpeed);
            this.transform.RotateAround(targetTrans.position, transform.right, -Input.GetAxis("Mouse Y") * rotateSpeed);
            if (transform.eulerAngles.x>60|| transform.eulerAngles.x<20)
            {
                transform.rotation = angle;
                transform.position = pos;
            }
        }
        
        dirWithTarget = this.transform.position - targetTrans.position;
    }
}
