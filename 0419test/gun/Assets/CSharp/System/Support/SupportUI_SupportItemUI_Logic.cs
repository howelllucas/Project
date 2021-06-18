using EZ.Data;
using EZ.DataMgr;
using System;
using UnityEngine;

namespace EZ
{
    public partial class SupportUI_SupportItemUI
    {

        //武器名
        private string m_WeaponName;
        //消耗道具
        private int m_ConsumItemId;



        private void Awake()
        {
            RegisterListeners();
        }

        public void Init(ItemItem itemConfig)
        {
            WeaponIcon.image.sprite = Resources.Load(itemConfig.image_grow, typeof(Sprite)) as Sprite;
            WeaponNameTxt.text.text = itemConfig.gamename;
            Deal4Level(itemConfig);
            gameObject.SetActive(true);
            transform.SetSiblingIndex(itemConfig.showorder);

            if (!Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(itemConfig))
            {
                Mask1.gameObject.SetActive(true);
                MaskTxt.text.text = FilterFactory.GetInstance().GetUnfinishTips(itemConfig.opencondition);
            }
        }

        private void Deal4Level(ItemItem itemConfig)
        {
            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(itemConfig.name);
            WeaponLvTxt.text.text = "lv: " + gunLevel;
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);


            double[] costValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(itemConfig.name + "_cost", weaponLevelData);
            CmNum.text.text = costValue[1].ToString();
            CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, costValue[0]));
            m_ConsumItemId = (int)costValue[0];

            double[] paramValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(itemConfig.name, weaponLevelData);
            Param1Txt.text.text = paramValue[0].ToString();
            if (paramValue.Length <= 1)
            {
                Param2Txt.text.text = "None";
            }
            else
            {
                Param2Txt.text.text = paramValue[1].ToString();
            }
            m_WeaponName = itemConfig.name;

            int m_QualityLevel = itemConfig.qualevel;
            GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);
            WeaponNameTxt.text.color = ColorUtil.GetColor(colorConfig.contents[m_QualityLevel]);
            //if (m_QualityLevel < itemConfig.qualevel.Length - 1 && gunLevel - itemConfig.qualevel[m_QualityLevel] <= itemConfig.qualevel[m_QualityLevel + 1] - itemConfig.qualevel[m_QualityLevel])
            //{
            //    LvPro.image.fillAmount = (float)(gunLevel - itemConfig.qualevel[m_QualityLevel]) / (itemConfig.qualevel[m_QualityLevel + 1] - itemConfig.qualevel[m_QualityLevel]);
            //}
            //else if (m_QualityLevel >= itemConfig.qualevel.Length - 1)
            //{
            //    LvPro.image.fillAmount = 0;
            //}
            //else
            //{
            //    LvPro.image.fillAmount = 1;
            //}
        }

        private void RegisterListeners()
        {
            UpBtn.button.onClick.AddListener(OnLevelUp);
        }


        private void OnLevelUp()
        {
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(m_WeaponName);
            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(itemConfig.name);
            if (gunLevel == Global.gApp.gGameData.GunDataConfig.items.Length)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3046);
                return;
            }
            Guns_dataItem levelItem = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            Guns_dataItem nextLevelItem = Global.gApp.gGameData.GunDataConfig.Get(gunLevel + 1);
            double[] nextValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_WeaponName, nextLevelItem);
            if (nextValue == null || nextValue.Length == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1004);
                return;
            }

            //是否成功扣钱
            bool reduceResult = false;
            double[] costValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_WeaponName + "_cost", levelItem);
            ItemDTO reduceItemDTO = new ItemDTO(m_ConsumItemId, Convert.ToSingle(costValue[1]), BehaviorTypeConstVal.OPT_SUPPORT_LEVEL_UP);
            reduceItemDTO.paramStr1 = m_WeaponName;
            GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
            reduceResult = reduceItemDTO.result;

            if (!reduceResult)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1002);
                return;
            }

            bool levelUpResult = Global.gApp.gSystemMgr.GetWeaponMgr().LevelUp(m_WeaponName);
            if (levelUpResult)
            {
                Deal4Level(itemConfig);
            }
            else
            {
                ItemDTO addItemDTO = new ItemDTO(m_ConsumItemId, Convert.ToSingle(costValue[1]), BehaviorTypeConstVal.OPT_SUPPORT_LEVEL_UP);
                addItemDTO.paramStr1 = m_WeaponName;
                addItemDTO.paramStr2 = "MakeUp4Fail";
                GameItemFactory.GetInstance().AddItem(addItemDTO);
            }
        }

        public string GetWeaponName()
        {
            return m_WeaponName;
        }
    }
}