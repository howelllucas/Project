
using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public partial class ExchangeSuccessUI
    {
        WeaponRaiseUI_ExchangeMatItemUI m_ItemUI;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            m_ItemUI = arg as WeaponRaiseUI_ExchangeMatItemUI;
            base.Init(name, info, arg);
            InitNode();

            base.ChangeLanguage();
        }
        private void InitNode()
        {
            CampShopItem campShopItem = m_ItemUI.GetCampItem();
            CloseBtn.button.onClick.AddListener(TouchClose);
            
            ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(campShopItem.propId);
            Debug.Log(itemItem.image_grow);
            MatIcon.image.sprite = Resources.Load(itemItem.image_grow, typeof(Sprite)) as Sprite;
            MatName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(itemItem.sourceLanguage);
            GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);

            if (itemItem.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
            {
                bgBottom.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, itemItem.qualevel), typeof(Sprite)) as Sprite;
                MatName.text.color = ColorUtil.GetColor(colorConfig.contents[itemItem.qualevel]);
            }
            else
            {
                bgBottom.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
                MatName.text.color = ColorUtil.GetColor(colorConfig.contents[2]);
            }
            EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
            string effectName = itemItem.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? itemItem.qualevel.ToString() : "common";
            GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, effectName), EffectPos.rectTransform);
            effect.transform.localPosition = new Vector3(0f, 0f, 0f);
            effect.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
            ParticleSystem[] pss = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in pss)
            {
                ps.GetComponent<Renderer>().sortingOrder = 51;
            }
        }


        private void RegisterListeners()
        {
        }
        
        

        private void UnRegisterListeners()
        {
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
