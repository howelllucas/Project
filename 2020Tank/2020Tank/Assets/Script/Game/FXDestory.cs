using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDestory : MonoBehaviour
{
    ParticleSystem particle;

    // Start is called before the first frame update
    
    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particle.IsAlive() == false)
        {
            Destroy(gameObject);
        }
    }
}
