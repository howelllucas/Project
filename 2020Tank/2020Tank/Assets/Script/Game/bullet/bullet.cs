using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour
{
    //子弹碰撞后特效
    public GameObject bulletEXPrefab;
    //子弹拥有者
    public GameObject Owner;
    Rigidbody bulletRid;
    public float bulletSpeed;
    void Awake()
    {
        bulletRid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
        bulletRid.velocity = this.transform.forward * bulletSpeed;

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (Owner == other) return;
        GameObject.Instantiate(bulletEXPrefab, this.transform.position, Quaternion.identity);
        GameObject.Destroy(this.gameObject);
    }


}
