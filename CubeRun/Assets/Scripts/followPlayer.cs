using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    private Transform c_trans;
    private Transform p_trans;

    

    
    private void Awake()
    {
        c_trans = this.transform;
        p_trans = GameObject.FindGameObjectWithTag("Player").transform;

        
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        followP();
    }

    private void followP()
    {
        if (p_trans.GetComponent<player>().isFollow == true)
        {

            Vector3 nextVec = new Vector3(c_trans.position.x, p_trans.position.y + 1.3f, p_trans.position.z );
            this.transform.position = Vector3.Lerp(this.transform.position, nextVec, Time.deltaTime);
        }
    }
}
