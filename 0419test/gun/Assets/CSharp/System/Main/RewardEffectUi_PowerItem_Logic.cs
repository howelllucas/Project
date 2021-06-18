using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{
    public partial class RewardEffectUi_PowerItem : MonoBehaviour
    {
        private int initPower = 0;
        private Animator anim;

        public void Init()
        {
            initPower = (int)PlayerDataMgr.singleton.GetUseWeaponPower();
        }

        public void UpdatePower()
        {
            var power = PlayerDataMgr.singleton.GetUseWeaponPower();
            if ((int)power == initPower)
                return;

            var str = string.Format("Power {0}", (power / TableMgr.singleton.ValueTable.combat_capability).ToString("f0"));
            if (power > initPower)
            {
                GradeText.text.text = str + string.Format("<color=#75C85A>+{0}</color>", ((power - initPower) /
                                                                  TableMgr.singleton.ValueTable.combat_capability).ToString("f0"));
            }
            else
            {
                GradeText.text.text = str + string.Format("<color=#EC8B99>{0}</color>", ((power - initPower) /
                                                                  TableMgr.singleton.ValueTable.combat_capability).ToString("f0"));
            }

            initPower = (int)power;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void End()
        {
            gameObject.SetActive(false);
        }
    }
}