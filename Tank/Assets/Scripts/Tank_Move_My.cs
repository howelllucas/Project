using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Move_My : MonoBehaviour
{
    public int Speed = 5;
    public int angularSpeed = 8;
    private Rigidbody rigidbody;
    float V;
    float H;
    // Start is VVcalled before the first frame update
    void Start()
    {
        rigidbody = this.transform.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        V = Input.GetAxis("VerticalUI");
        rigidbody.velocity = this.transform.forward * V * Speed;

        H = Input.GetAxis("HorizontalUI");
        rigidbody.angularVelocity = this.transform.up * H * angularSpeed;



    }
   
}
