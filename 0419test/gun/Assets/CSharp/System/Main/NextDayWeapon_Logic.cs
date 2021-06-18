
using EZ;
using EZ.Data;
using UnityEngine;

namespace EZ
{
    public partial class NextDayWeapon
    {
        private ItemItem m_NextGunCfg;

        private GameObject m_Effect;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_NextGunCfg = arg as ItemItem;
            

            bool isWeapon = ItemTypeConstVal.isWeapon(m_NextGunCfg.showtype);
            NextGunName.gameObject.SetActive(isWeapon);
            NextGunIcon.gameObject.SetActive(isWeapon);
            //NextGunDown.gameObject.SetActive(isWeapon);
            NextAwardIcon.gameObject.SetActive(!isWeapon);
            if (isWeapon)
            {
                TitleTxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(3083);
                GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);
                
                NextGunName.text.text = m_NextGunCfg.gamename;
                NextGunIcon.image.sprite = Resources.Load(m_NextGunCfg.image_grow, typeof(Sprite)) as Sprite;
                if (m_NextGunCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                {
                    NextGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, m_NextGunCfg.qualevel), typeof(Sprite)) as Sprite;
                    NextGunName.text.color = ColorUtil.GetColor(colorConfig.contents[m_NextGunCfg.qualevel]);
                }
                else
                {
                    NextGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
                    NextGunName.text.color = ColorUtil.GetColor(colorConfig.contents[2]);
                }
                //NextGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, m_NextGunCfg.qualevel), typeof(Sprite)) as Sprite;
                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
                string effectName = m_NextGunCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? m_NextGunCfg.qualevel.ToString() : "common";
                m_Effect = UiTools.GetEffect(string.Format(effectItem.path, effectName), NextDay.rectTransform);
                Destxt.text.text = FilterFactory.GetInstance().GetMiddleUnfinishTips(m_NextGunCfg.opencondition);
                LeftTxt.text.text = FilterFactory.GetInstance().GetLeftTips(m_NextGunCfg.opencondition);

                m_Effect.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                ParticleSystem[] pss = m_Effect.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem ps in pss)
                {
                    ps.GetComponent<Renderer>().sortingOrder = 45;
                }
            }
            else
            {
                TitleTxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(3084);
                QuestItemDTO nextLevelDetailDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetNextLevelDetailQuest();
                QuestItem questCfg = Global.gApp.gGameData.QuestData.Get(nextLevelDetailDTO.id);
                NextAwardIcon.image.sprite = Resources.Load(questCfg.awardIcon, typeof(Sprite)) as Sprite;
                Destxt.text.text = FilterFactory.GetInstance().GetMiddleUnfinishTips(questCfg.condition);
                LeftTxt.text.text = FilterFactory.GetInstance().GetLeftTips(questCfg.condition);

                NextGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;

            }


            Btn2.button.onClick.AddListener(TouchClose);
            BtnC.button.onClick.AddListener(TouchClose);

            base.ChangeLanguage();
        }


        public void ChangeInfo4SevenDay()
        {
            TitleTxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(3085);
            Destxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(3086);
            LeftTxt.gameObject.SetActive(false);
            Canvas[] canvas = this.gameObject.GetComponentsInParent<Canvas>();
            if (canvas.Length > 0)
            {
                foreach (Canvas mCanvas in canvas)
                {
                    mCanvas.sortingOrder = 50;
                }
            }
            ParticleSystem[] pss = m_Effect.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in pss)
            {
                ps.GetComponent<Renderer>().sortingOrder = 55;
            }
        }
    }

}
