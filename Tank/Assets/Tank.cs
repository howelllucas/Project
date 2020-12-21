using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public int HP = 100;
    public GameObject TankExplusion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendMassage()
    {
        if (HP <= 0) return;
        if (HP>=0)
        {
            HP -= Random.Range(10, 20);
        }
        if (HP <= 0)
        {
            GameObject.Instantiate(TankExplusion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }

    }
}
