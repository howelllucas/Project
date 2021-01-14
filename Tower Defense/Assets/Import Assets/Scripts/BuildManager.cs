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
        //存储实例化的炮台用于确认点击隐藏升级ui
        public GameObject newTurret;

        private int money = 1000;

        public Text text;

        public Animator ani;

        //升级UI面板
        public GameObject upgradeUi;
        //升级按钮
        public Button upgradeButton;
        //存储地图块数据用于调用销毁方法
        public mapCube cube;
        //升级面板是否显示
        private bool isShowUIgrade=false;

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
                        cube = mc;
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
                            //升级ui显示
                            if (isShowUIgrade == false)
                            {
                                if (cube.turretLevel > 1)
                                {
                                    showUpgradeUI(cube.transform.position, false);
                                }
                                else
                                {
                                    showUpgradeUI(cube.transform.position, true);
                                }
                            }
                            else
                            {
                                HideUpgradeUI();
                                
                            }
                            
                            
                            
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
        //ui面板的显示
        private void showUpgradeUI(Vector3 UIPoint, bool interactableBool=false)
        {
            isShowUIgrade = true;
            upgradeUi.transform.position = UIPoint;
            upgradeUi.SetActive(true);
            if (money> selectedTurretDate.costUp&& interactableBool==true)
            {
                upgradeButton.interactable =true;
            }
            else
            {
                upgradeButton.interactable =false;
            }
            
        }
        private void HideUpgradeUI()
        {
            isShowUIgrade = false;
            cube.turretLevel = 0;
            upgradeUi.SetActive(false);
            
        }

        //按钮点击事件
        public void onUpgradeButton()
        {
            if (money < selectedTurretDate.costUp)
            {
                ani.SetTrigger("flicker");
                upgradeButton.interactable = false;
            }
            else
            {
                changeMoney(-selectedTurretDate.costUp);
                //升级操作
                cube.upgradeTurret(selectedTurretDate.turretUpPrefab);
                HideUpgradeUI();
            }
          
            
        }
        public void onDestoryButton()
        {
            HideUpgradeUI();
            cube.destoryTurret();
        }
    }
    

}

