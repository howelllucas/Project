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

    public partial class GunUI_GunSkill
    {

        private SkillInfo skillInfo = new SkillInfo();

        private void Awake()
        {
            InitNode();
        }
        public void InitFuseSkill(int id)
        {
            skillInfo.type = SkillType.Fuse;
            skillInfo.skillID = id;

            var fuseSkillRes = TableMgr.singleton.FuseGunSkillTable.GetItemByID(id);
            if (fuseSkillRes == null)
                return;

            SkillName.text.text = LanguageMgr.GetText(fuseSkillRes.tid_name);
            IconBtn.image.sprite = Resources.Load(fuseSkillRes.icon, typeof(Sprite)) as Sprite;
        }

        public void InitCampSkill(int id)
        {
            skillInfo.type = SkillType.Camp;
            skillInfo.skillID = id;

            var campSkillRes = TableMgr.singleton.CampGunSkillTable.GetItemByID(id);
            if (campSkillRes == null)
                return;

            SkillName.text.text = LanguageMgr.GetText(campSkillRes.tid_name);
            IconBtn.image.sprite = Resources.Load(campSkillRes.icon, typeof(Sprite)) as Sprite;
        }

        private void InitNode()
        {
            IconBtn.button.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.GunSkillInfoUI, skillInfo);
        }
    }
}