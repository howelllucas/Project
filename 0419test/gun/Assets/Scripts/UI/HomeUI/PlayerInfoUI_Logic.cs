using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;
using System;

namespace EZ
{
    public partial class PlayerInfoUI
    {

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            InitPlayerInfo();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        private void InitPlayerInfo()
        {

            RateValue.text.text = string.Format("{0}/s", UiTools.FormateMoney(CampsiteMgr.singleton.TotalRewardRate));

            CampValue.text.text = CampsiteMgr.singleton.Id.ToString();
            LevelValue.text.text = PlayerDataMgr.singleton.GetPlayerLevel().ToString();
            DPSValue.text.text = (PlayerDataMgr.singleton.GetUseWeaponPower() /
                                TableMgr.singleton.ValueTable.combat_capability).ToString("f0");

            Dictionary<CombatAttr.CombatAttrType, float> attrDic = new Dictionary<CombatAttr.CombatAttrType, float>();
            var skills = PlayerDataMgr.singleton.GetFuseSkills();
            for (int i = 0; i < skills.Count; ++i)
            {
                var data = TableMgr.singleton.FuseGunSkillTable.GetItemByID(skills[i]);
                if (data == null)
                    continue;

                var type = (CombatAttr.CombatAttrType)(data.id / 100);
                if (!attrDic.ContainsKey(type))
                    attrDic[type] = data.value;
                else
                    attrDic[type] += data.value;
            }

            var life = Mathf.Ceil(TableMgr.singleton.ValueTable.player_levelup_hp_k1 *
                        Mathf.Pow((1 + TableMgr.singleton.ValueTable.player_levelup_hp_k2),
                        (PlayerDataMgr.singleton.GetPlayerLevel() - 1)));

            var value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Life_Add, out value))
            {
                life *= (1.0f + value);
            }

            HpValue.text.text = life.ToString("f0");
            ///////////////////////////////////////////////////////////
            var atk = PlayerDataMgr.singleton.GetUseWeaponAtk();
            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Attack, out value))
            {
                atk += value;
            }

            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Attack_Add, out value))
            {
                atk *= (1.0f + value);
            }

            AtkValue.text.text = atk.ToString("f0");
            /////////////////////////////////////////////////////////
            var speed = 1.0f;
            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.MoveSpeed, out value))
            {
                speed += value;
            }

            SpeedValue.text.text = speed.ToString("f0");
            /////////////////////////////////////////////////////////
            var crit = 1.0f;
            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Crit_Rate, out value))
            {
                crit += value;
            }

            CritValue.text.text = crit.ToString("f0");
            /////////////////////////////////////////////////////////
            var critDamage = 1.0f;
            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Crit_Damage, out value))
            {
                critDamage += value;
            }

            CritDamageValue.text.text = critDamage.ToString("f0");
            /////////////////////////////////////////////////////////
            var dodge = 1.0f;
            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Dodge, out value))
            {
                dodge += value;
            }

            DodgeValue.text.text = critDamage.ToString("f0");
            /////////////////////////////////////////////////////////
            var atkSpeed = 1.0f;
            value = 0.0f;
            if (attrDic.TryGetValue(CombatAttr.CombatAttrType.Attack_Speed, out value))
            {
                atkSpeed += value;
            }

            AtkSpeedValue.text.text = atkSpeed.ToString("f0");
            /////////////////////////////////////////////////////////
        }



    }
}