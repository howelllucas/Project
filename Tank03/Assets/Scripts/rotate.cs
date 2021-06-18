using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float speed = 5;

    // Update is called once per frame
    void Update()
    {
        
        this.transform.Rotate(this.transform.up, speed * Time.deltaTime);
    }
}
