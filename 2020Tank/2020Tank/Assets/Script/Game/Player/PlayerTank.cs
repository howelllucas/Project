using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    Vector2 moveDir;
    private Rigidbody rid;
    public float moveSpeed = 8.0f;
    void Awake()
    {
        rid = this.transform.GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetAxis("Horizontal")!=0||Input.GetAxis("Vertical")!=0)
        {
            moveDir.x = Input.GetAxis("Horizontal");
            moveDir.y = Input.GetAxis("Vertical");
            Move(moveDir);
        }
        
    }

    void Move(Vector2 dir)
    {
        //旋转
        Vector3 rot = new Vector3(moveDir.x, 0, moveDir.y);
        this.transform.rotation = Quaternion.LookRotation(rot);
        //移动
        Vector3 Dir = this.transform.forward * moveSpeed * Time.deltaTime;
        rid.MovePosition(this.transform.position+ Dir);
    }
}
