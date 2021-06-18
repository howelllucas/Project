using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBip_update : MonoBehaviour {

    public Transform weaponBip_Trans;
	// Use this for initialization
	void Start () {
        transform.localScale = weaponBip_Trans.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position = weaponBip_Trans.position;
        if (Mathf.Abs(weaponBip_Trans.localEulerAngles.y) - 90 > 8)
        {
            transform.localEulerAngles = new Vector3(weaponBip_Trans.localEulerAngles.x, -90, weaponBip_Trans.localEulerAngles.z);
        } else
        {
            transform.localEulerAngles = weaponBip_Trans.localEulerAngles;
        }
    }
}
