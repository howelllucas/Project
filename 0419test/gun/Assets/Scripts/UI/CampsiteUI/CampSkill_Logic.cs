using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System;
using UnityEngine.UI;

namespace EZ
{
    public partial class CampSkill
    {
        public event Action<CampSkill> onClick;
        private CampGunSkill_TableItem skillRes;

        private void Awake()
        {
            IconBtn.button.onClick.AddListener(OnIconBtnClick);
        }

        public void Init(int skillId)
        {
            skillRes = TableMgr.singleton.CampGunSkillTable.GetItemByID(skillId);
            LvTxt.text.text = string.Format("lv.{0}", skillRes.level);
            IconBtn.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(skillRes.icon);
        }

        public void SetValid(bool valid)
        {
            if (valid)
                UIGray.Recovery(IconBtn.image);
            else
                UIGray.SetUIGray(IconBtn.image);
        }

        private void OnIconBtnClick()
        {
            onClick?.Invoke(this);
        }

        public void SetDescState(GameObject descRoot, Text descTxt)
        {
            if (descRoot.activeSelf && descRoot.transform.parent == DescNode.rectTransform)
            {
                descRoot.SetActive(false);
            }
            else
            {
                descRoot.SetActive(true);
                descRoot.transform.SetParent(DescNode.rectTransform, false);
                descRoot.transform.localPosition = Vector3.zero;
                var buildingRes = TableMgr.singleton.CampBuildingTable.GetItemByID(skillRes.campBuilding);
                descTxt.text = LanguageMgr.GetText(skillRes.tid_desc, buildingRes.buildingName, skillRes.value);
            }
        }
    }
}