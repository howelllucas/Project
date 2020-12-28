using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

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
    public float bulletTime=0.5f;
    //下一次发射时间
    float nextTime=0 ;
    //总血量
    public float HP;
    //当前血量
    public float currHP;
    //死亡特效
    public GameObject deadFX;
    //血条
    public Slider HPSlider;
    void Awake()
    {
        rid = this.transform.GetComponent<Rigidbody>();
        HP = 40;
        currHP = HP;
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
    public void creatBullet()
    {
        
        if (Time.time> nextTime)
        {
            GameObject.Instantiate(firePrefab, bulletCreatPoint.position, Quaternion.identity);
            GameObject bu= GameObject.Instantiate(bulletPrefab, bulletCreatPoint.position, bulletCreatPoint.rotation);
            bu.GetComponent<bullet>().Owner = this.gameObject;
            nextTime = Time.time+ bulletTime;
        }
        
    }

    public void damage(int hitNumber)
    {

        currHP -= hitNumber;
        HPSlider.value = currHP / HP;
        if (currHP <= 0)
        {
            GameObject.Instantiate(deadFX, this.transform.position, Quaternion.identity);
            GameObject.Destroy(this.gameObject);
            currHP = 0;
            gameOver();          
        }
    }

    private void gameOver()
    {
        
        UIMain.Instance.gameOver(true);
    }
}
