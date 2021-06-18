using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class MainRoleUICtrl : MonoBehaviour
    {
        [SerializeField]
        private Transform heroNode;
        [SerializeField]
        private Transform weaponNode;

        private DragRoleModel curDragCtrl;
        private GameObject showWeapon;
        private string curWeaponPath;

        private int refCount;
        public void SetWeapon(string weaponPath)
        {
            if (weaponPath != curWeaponPath)
            {
                if (showWeapon != null)
                    Destroy(showWeapon);

                GameObject weapon = Global.gApp.gResMgr.InstantiateObj(weaponPath);
                weapon.transform.SetParent(weaponNode, false);
                showWeapon = weapon;

                curWeaponPath = weaponPath;
            }
        }

        public void Show()
        {
            refCount++;
            gameObject.SetActive(refCount > 0);
        }

        public void Hide()
        {
            refCount--;
            gameObject.SetActive(refCount > 0);
        }

        public void AddDragCtrl(DragRoleModel ctrl)
        {
            if (curDragCtrl != null)
            {
                Debug.Log("DragRoleModel clush!" + curDragCtrl.name + "==>" + ctrl.name);
                curDragCtrl.m_MainNode = null;
            }
            heroNode.localRotation = Quaternion.identity;
            ctrl.m_MainNode = heroNode;
            curDragCtrl = ctrl;
        }

        public void RemoveDragCtrl()
        {
            if (curDragCtrl != null)
            {
                curDragCtrl.m_MainNode = null;
            }
            heroNode.localRotation = Quaternion.identity;
            curDragCtrl = null;
        }
    }
}