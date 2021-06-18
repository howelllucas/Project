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
    public class SkillInfo
    {
        public SkillType type;
        public int skillID;
    }

    public partial class GunSkillLvUpUI
    {

        private SkillInfo skillInfo = null;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            skillInfo = arg as SkillInfo;
            if (skillInfo == null)
                return;

            if (skillInfo.type == SkillType.Fuse)
            {
                InitFuseSkill(skillInfo.skillID);
            }
            else if (skillInfo.type == SkillType.Camp)
            {
                InitCampSkill(skillInfo.skillID);
            }
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            OkBtn.button.onClick.AddListener(TouchClose);
        }

        private void InitFuseSkill(int id)
        {
            var fuseSkillRes = TableMgr.singleton.FuseGunSkillTable.GetItemByID(skillInfo.skillID);
            if (fuseSkillRes == null)
                return;

            SkillName.text.text = LanguageMgr.GetText(fuseSkillRes.tid_name);
            SkillIcon.image.sprite = Resources.Load(fuseSkillRes.icon, typeof(Sprite)) as Sprite;

            NextLv.text.text = string.Format("Lv.{0}", fuseSkillRes.level);
            NextDesc.text.text = LanguageMgr.GetText(fuseSkillRes.tid_desc, fuseSkillRes.value);

            fuseSkillRes = TableMgr.singleton.FuseGunSkillTable.GetItemByID(skillInfo.skillID - 1);
            if (fuseSkillRes == null)
                return;

            CurLv.text.text = string.Format("Lv.{0}", fuseSkillRes.level);
            CurDesc.text.text = LanguageMgr.GetText(fuseSkillRes.tid_desc, fuseSkillRes.value);
        }

        private void InitCampSkill(int id)
        {
            var campSkillRes = TableMgr.singleton.CampGunSkillTable.GetItemByID(skillInfo.skillID);
            if (campSkillRes == null)
                return;

            SkillName.text.text = LanguageMgr.GetText(campSkillRes.tid_name);
            SkillIcon.image.sprite = Resources.Load(campSkillRes.icon, typeof(Sprite)) as Sprite;

            NextLv.text.text = string.Format("Lv.{0}", campSkillRes.level);
            NextDesc.text.text = string.Format("{0}", campSkillRes.value);

            campSkillRes = TableMgr.singleton.CampGunSkillTable.GetItemByID(skillInfo.skillID - 1);
            if (campSkillRes == null)
                return;

            CurLv.text.text = string.Format("Lv.{0}", campSkillRes.level);
            CurDesc.text.text = string.Format("{0}", campSkillRes.value);
        }

    }
}