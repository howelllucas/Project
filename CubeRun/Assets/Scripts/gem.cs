using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gem : MonoBehaviour
{
    Transform box;
    void Start()
    {
        box = this.transform.Find("gem 3").transform;
    }

    // Update is called once per frame
    void Update()
    {
        box.Rotate(Vector3.up);
    }
}
