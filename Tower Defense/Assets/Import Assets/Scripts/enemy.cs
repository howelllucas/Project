using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class enemy : MonoBehaviour
    {
        public float Speed = 10;
        public float interSpeed = 10;
        public float HP = 150;
        private float tatulHP;
        Transform[] wayTrans;
        private int index = 0;
        //怪物价钱
        public int enemyCast=0;
        //总金币
        private GameObject gamemanage;
        

        Rigidbody rid;
        public GameObject deadEffect;
        public Slider slider;
        private void Awake()
        {
            wayTrans = wayPoint.tans;
            rid = this.transform.GetComponent<Rigidbody>();
            tatulHP = HP;

            gamemanage = GameObject.Find("enemyMassage");

        }
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (index>= wayTrans.Length-1)
            {
                return;
            }
            this.transform.LookAt(wayTrans[index + 1]);
            this.transform.position += this.transform.forward * Speed * Time.deltaTime;
            
            if (Vector3.Distance(this.transform.position, wayTrans[index+1].position)<0.2f)
            {
                index++;
            }
            if (index>= wayTrans.Length-1)
            {
                finishEnd();
            }
            
        }
        void finishEnd()
        {
            
            GameObject.Destroy(this.gameObject);
            gameManager.instance.failedUI();
        }
        void OnDestroy()
        {
            enemySpawner.aliveEnemyCount--;
            
        }
        //受到攻击
        public void takeDemage(float demage)
        {
            if (HP<=0)
            {
                return;
            }
            HP -= demage;
            slider.value = (float)HP / tatulHP;
            if (HP <= 0)
            {
                dead();
            }
        }
        private void dead()
        {
            gamemanage.GetComponent<BuildManager>().changeMoney(enemyCast);
            GameObject eff= GameObject.Instantiate(deadEffect, transform.position, Quaternion.identity);
            Destroy(eff, 1.5f);
            Destroy(this.gameObject);
        }

        //减速
        public void slowDown()
        {
            
            if (Speed != interSpeed) return;
            
            
            this.Speed/=2;
            
        }
    }

}

