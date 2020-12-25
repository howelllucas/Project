using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemyTank: MonoBehaviour
{
    //实时距离
    private float distance;
    //追击距离
    public float followDistance;
    //攻击距离
    public float attackDistance;
    //目标玩家坦克
    private GameObject playerTank;
    //旋转速度
    private float rotationSpeed = 5.0f;
    //移动速度
    private float moveSpeed = 5.0f;
    //子弹间隔时间
    float bulletTime = 0.5f;
    //下一次发射时间
    float nextTime = 0;
    public GameObject bulletPrefab;
    public Transform bulletCreatPoint;
    //坦克开火特效预制体
    public GameObject firePrefab;
    //总血量
    float enemyHP;
    //当前血量
    float currHP;
    //slider
    public Slider HPSlider;
    //死亡特效
    public GameObject deadFX;
    void Start()
    {
        playerTank = GameObject.FindGameObjectWithTag("Player");
        
        enemyHP = 30;
        currHP = enemyHP;
    }

    
    void Update()
    {
        if (playerTank == null) return;
        
        distance = Vector3.Distance(this.transform.position, playerTank.transform.position);
        if (distance < attackDistance)
        {
            //追击
            enemyAttack();
        }
        else if (distance < followDistance)
        {
            //攻击
            followMove();
        }
        else
        {

        }
    }
    //追击方法
    void followMove()
    {
        //旋转
        Vector3 rot = playerTank.transform.position-this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(rot), Time.deltaTime * rotationSpeed);
        //移动
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    void enemyAttack()
    {
        
        Vector3 rot = playerTank.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(rot), Time.deltaTime * rotationSpeed);
        if (Time.time > nextTime)
        {
            GameObject.Instantiate(firePrefab, bulletCreatPoint.position, Quaternion.identity);
            GameObject bu =  GameObject.Instantiate(bulletPrefab, bulletCreatPoint.position, bulletCreatPoint.rotation);
            bu.GetComponent<bullet>().Owner = this.gameObject;
            
            nextTime = Time.time + bulletTime;
        }
    }
    public void damage(int hitNumber)
    {
        currHP -= hitNumber;
        HPSlider.value = currHP / enemyHP;
        if (currHP <= 0)
        {
            currHP = 0;
            
            GameObject.Destroy(this.gameObject);
            GameObject.Instantiate(deadFX, this.transform.position, Quaternion.identity);
        }
    }
}
