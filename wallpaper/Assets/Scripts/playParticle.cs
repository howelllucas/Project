using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playParticle : MonoBehaviour
{
    ParticleSystem ps;
    public GameObject image;
    public GameObject ClipMask;
    // Start is called before the first frame update
    void Start()
    {
        ps=this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ClipMask.GetComponent<Animation>().Play();
            Invoke("startplay", 0.25f);
            
        }
    }

    void startplay()
    {
        image.SetActive(false);
        ps.Play();
    }
}
