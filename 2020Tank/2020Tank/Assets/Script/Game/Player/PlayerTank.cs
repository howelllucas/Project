using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    Vector2 moveDir;
    private Rigidbody rid;
    public float moveSpeed = 8.0f;
    public GameObject bulletPrefab;
    public Transform bulletCreatPoint;
    //坦克开火特效预制体
    public GameObject firePrefab;
    //子弹间隔时间
    float bulletTime=0.5f;
    //下一次发射时间
    float nextTime=0 ;
    void Awake()
    {
        rid = this.transform.GetComponent<Rigidbody>();
    }

    void Start()
    {
        Camera.main.GetComponent<FollowTarget>().Target = this.transform;
    }

    
    void Update()
    {
        if (Input.GetAxis("Horizontal")!=0||Input.GetAxis("Vertical")!=0)
        {
            moveDir.x = Input.GetAxis("Horizontal");
            moveDir.y = Input.GetAxis("Vertical");
            Move(moveDir);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            creatBullet();
        }
        
    }

    void Move(Vector2 dir)
    {
        //旋转
        Vector3 rot = new Vector3(moveDir.x, 0, moveDir.y);
        this.transform.rotation = Quaternion.LookRotation(rot);
        //移动
        Vector3 Dir = this.transform.forward * moveSpeed * Time.deltaTime;
        rid.MovePosition(this.transform.position+ Dir);
    }
    void creatBullet()
    {
        bulletPrefab.GetComponent<bullet>().Owner = this.gameObject;
        if (Time.time> nextTime)
        {
            GameObject.Instantiate(firePrefab, bulletCreatPoint.position, Quaternion.identity);
            GameObject.Instantiate(bulletPrefab, bulletCreatPoint.position, bulletCreatPoint.rotation);
            nextTime = Time.time+ bulletTime;
        }
        
    }
}
