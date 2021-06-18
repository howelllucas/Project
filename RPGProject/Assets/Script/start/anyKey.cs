using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anyKey : MonoBehaviour
{
    public bool isOnAnyKey = false;
    private GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        button = this.transform.parent.Find("button").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnAnyKey==false)
        {
            if (Input.anyKey)
            {
                button.SetActive(true);
                this.gameObject.SetActive(false);
                isOnAnyKey = true;
            }
        }
    }
}
