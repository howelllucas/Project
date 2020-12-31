using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour
{
    //子弹碰撞后特效
    public GameObject bulletEXPrefab;
    public GameObject bulletEXPrefab2;
    //子弹拥有者
    public GameObject Owner;
    Rigidbody bulletRid;
    public float bulletSpeed;
    //子弹伤害值
    public int demage = 10;
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
        
        if (Owner == other.gameObject) return;

        //建筑触发
        if (Owner != null && Owner.tag == "Player" && other.gameObject.tag == "map")
        {
            //玩家攻击建筑，触发
            
            GameObject.Instantiate(bulletEXPrefab2, this.transform.position, bulletEXPrefab2.transform.rotation);
        }
        //伤害触发
        if (Owner!=null && Owner.tag=="Player"&& other.gameObject.tag=="Monster")
        {
            //玩家攻击敌人，触发
            other.GetComponent<enemyTank>().damage(demage);
            GameObject.Instantiate(bulletEXPrefab, this.transform.position, Quaternion.identity);
        }
        if (Owner != null && Owner.tag  == "Monster" && other.gameObject.tag == "Player")
        {
            
            //敌人攻击玩家，触发玩家受伤函数
            other.GetComponent<PlayerTank>().damage(demage);
            GameObject.Instantiate(bulletEXPrefab, this.transform.position, Quaternion.identity);

        }
        
        
        GameObject.Destroy(this.gameObject);
    }


}
