using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMove : MonoBehaviour
{
    public int speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z<-20)
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
