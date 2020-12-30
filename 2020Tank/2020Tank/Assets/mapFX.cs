using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapFX : MonoBehaviour
{
    public GameObject fx;

    public void startPlay()
    {
        GameObject.Instantiate(fx, this.transform.position, Quaternion.identity);
    }
}
