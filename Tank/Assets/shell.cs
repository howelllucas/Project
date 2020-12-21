using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shell : MonoBehaviour
{
    public GameObject ShellExplosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Instantiate(ShellExplosion,this.transform.position, this.transform.rotation);
        GameObject.Destroy(this.gameObject);
        if (other.tag=="Tank")
        {
            other.SendMessage("sendMassage");
        }
    }
}
