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
    public GameObject UIHPCount;
    //坦克开火特效预制体
    public GameObject firePrefab;
    //破防特效预制体
    public GameObject hitDefenceBad;
    //重生特效
    public GameObject resetLifeFX;
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
    //防御值
    public float defence;
    //防御条
    Slider defenceSlider;
    //破防次数
    int hitDefenceDad;
    //生命格数
    int HPcount;
    //出生点
    Vector3 pos01;
    //落地点
    Vector3 pos02;
    //黑屏
    public GameObject blackMask;
    void Awake()
    {
        rid = this.transform.GetComponent<Rigidbody>();
        defenceSlider=this.transform.Find("Shield/Slider").GetComponent<Slider>();

        HP = 40;
        currHP = HP;
        hitDefenceDad = 0;
        HPcount = 0;
        UIHPCount.GetComponent<Slider>().value = HPcount;
        
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
        //有防御的判断
        if (defence>0)
        {
            defence -= hitNumber;
            if (defence/20==0)
            {
                defenceSlider.value = 1;
            }
            if (defence / 20 == 1)
            {
                defenceSlider.value = 2;
            }
            if (defence / 20 == 2)
            {
                defenceSlider.value = 3;
            }


        }
        else
        {
            //判断是否第一次打破防御
            defenceSlider.value = 0;
            if (hitDefenceDad==0)
            {
                GameObject.Instantiate(hitDefenceBad, this.transform.position, Quaternion.identity);
                hitDefenceDad++;
            }
            currHP -= hitNumber;
            HPSlider.value = currHP / HP;
            if (currHP <= 0)
            {
                HPcount++;
                UIHPCount.GetComponent<Slider>().value = HPcount;
                currHP = 0;
                
                
                if (HPcount==3)
                {
                    GameObject.Instantiate(deadFX, this.transform.position, Quaternion.identity);
                    GameObject.Destroy(this.gameObject);
                    gameOver();
                    blackMask.SetActive(false);
                    
                }
                //死亡方法
                GameObject.Instantiate(deadFX, this.transform.position, Quaternion.identity);
                
                this.gameObject.SetActive(false);
                //黑屏
                blackMask.SetActive(true);

                //重生方法
                Invoke("resetLife",1);


            }
        }
        
    }

    private void gameOver()
    {

        uiMain1.Instance.gameOver();
    }
    private void resetLife()
    {
        
        currHP = HP;
       
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(0, 0, 0);
        GameObject.Instantiate(resetLifeFX, this.transform.position, resetLifeFX.transform.rotation);
        
        
        blackMask.SetActive(false);
    }
    
}
