using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellEx : MonoBehaviour
{
    public float deadTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, deadTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
