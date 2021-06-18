using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginAni : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void SetAniActive()
    {
        GetComponent<Animator>().enabled = true;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
