﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellenemy : MonoBehaviour
{

    public GameObject ShellExplosion;
    public AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Instantiate(ShellExplosion, this.transform.position, this.transform.rotation);
        AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
        GameObject.Destroy(this.gameObject);
        
        if (other.tag == "Player")
        {
            other.SendMessage("sendMessage");
        }
    }
}