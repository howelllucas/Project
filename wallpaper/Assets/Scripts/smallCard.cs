using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallCard : MonoBehaviour
{
    public GameObject obj;
    ParticleSystem pas;
    private void Awake()
    {
        pas = this.transform.GetComponent<ParticleSystem>();
    }
    private void Start()
    {

        obj.SetActive(false);
    }

    private void Update()
    {
        if (pas.isStopped && Time.time > 3)
        {
            obj.SetActive(true);
        }
    }
}
