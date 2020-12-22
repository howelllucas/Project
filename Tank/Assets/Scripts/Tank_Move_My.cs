using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Move_My : MonoBehaviour
{
    public int Speed = 5;
    public int angularSpeed = 8;
    private Rigidbody rigidbodyR;

    public AudioClip idleClip;
    public AudioClip drivingClip;

    private AudioSource audioSource;

    float V;
    float H;
    // Start is VVcalled before the first frame update
    void Start()
    {
        rigidbodyR = this.transform.GetComponent<Rigidbody>();
        audioSource = this.transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        V = Input.GetAxis("VerticalUI");
        rigidbodyR.velocity = this.transform.forward * V * Speed;

        H = Input.GetAxis("HorizontalUI");

        rigidbodyR.angularVelocity = this.transform.up * H * angularSpeed;

        if (Math.Abs(V)>0.1||Math.Abs(H)>0.1)
        {
            audioSource.clip = drivingClip;
            if (audioSource.isPlaying == true) return;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = idleClip;
            if (audioSource.isPlaying == true) return;
            audioSource.Play();
        }


    }
   
}
