using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttch : MonoBehaviour
{
    public GameObject shell;
    public int shellSpeed = 1;
    public AudioClip audioClip;

    public KeyCode fireKey = KeyCode.Space;

    private Transform firePosition;
    // Start is called before the first frame update
    void Start()
    {
        firePosition = transform.Find("FirePosition");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(fireKey))
        {
            AudioSource.PlayClipAtPoint(audioClip,this.transform.position);
            GameObject go = GameObject.Instantiate(shell, firePosition.position, firePosition.rotation);
            go.GetComponent<Rigidbody>().velocity = this.transform.forward * shellSpeed;
        }
    }
}
