using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class BuildManager : MonoBehaviour
    {
        public turretDate LaserTurret;
        public turretDate MissileTurret;
        public turretDate StandardTurret;
        //当前选择的是哪个（需要创建的）
        private turretDate selectedTurretDate;

        private int money = 1000;

        public Text text;

        public Animator ani;

        private void changeMoney( int mon=0)
        {
            money += mon;
            text.text = "¥" + money;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()==false)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    bool iscollider= Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("mapCube"));
                    //判断是否碰到了地图块
                    if (iscollider)
                    {
                        //获取地图块数据
                        mapCube mc= hit.collider.gameObject.GetComponent<mapCube>();
                        if (selectedTurretDate!=null && mc.mapCubeGo==null)
                        {
                            //判断钱够不够
                            if (money>= selectedTurretDate.cost)
                            {
                                //可以创建,先减钱再创建
                                changeMoney(-selectedTurretDate.cost);
                                mc.creatTurret(selectedTurretDate.turretPrefab);
                            }
                            else
                            {
                                //播放钱不够动画
                                ani.SetTrigger("flicker");
                            }
                        }
                        else if (mc.mapCubeGo != null)
                        {
                            //升级
                        }
                        
                        
                    }

                }
            }
        }

        public void onLaserTurret(bool isOn)
        {
            if (isOn)
            {
                selectedTurretDate = LaserTurret;
            }
        }
        public void onMissileTurret(bool isOn)
        {
            if (isOn)
            {
                selectedTurretDate = MissileTurret;
            }
        }
        public void onStandardTurret(bool isOn)
        {
            if (isOn)
            {
                selectedTurretDate = StandardTurret;
            }
        }
    }
    

}

