using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EZ
{
    public partial class CampBUFF
    {
        private int m_CampLevel;
        private bool m_HaveTip = false;
        private float m_CurY = -100;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            RegisterListeners();
            Global.gApp.gSystemMgr.GetNpcMgr().InitCampBuff(true);
            InitNode();
            base.ChangeLanguage();
        }
        private void Update()
        {
            if (m_HaveTip)
            {
                if (m_CurY == -100)
                {
                    m_CurY = Content.rectTransform.localPosition.y;
                }
                else
                {
                    float y = Content.rectTransform.localPosition.y;
                    if (System.Math.Abs(y - m_CurY) > 1)
                    {
                        CloseDetail();
                    }
                }
            }
            
        }

        private void InitNode()
        {
            ItemBig.gameObject.SetActive(false);
            ItemSmall.gameObject.SetActive(false);
            TopItemDetail.gameObject.SetActive(false);
            ItemDetail.gameObject.SetActive(false);
            maskBtn.gameObject.SetActive(true);
            
            int totalNum = Global.gApp.gSystemMgr.GetNpcMgr().GetTotalNum();
            m_CampLevel = Global.gApp.gGameData.GetCampLevel(totalNum);

            //营地技能初始化
            for (int i = 0; i < Global.gApp.gGameData.CampBuffConfig.items.Length; i++)
            {
                CampBuffItem cbi = Global.gApp.gGameData.CampBuffConfig.items[i];

                RectTransform_Image_Container titleLockIcon = ReflectionUtil.GetValueByProperty<CampBUFF, RectTransform_Image_Container>("lock" + cbi.campLevel, this);
                titleLockIcon.gameObject.SetActive(m_CampLevel < cbi.campLevel);

                SkillItemDTO dto;
                Global.gApp.gSystemMgr.GetNpcMgr().CampBuffMap.TryGetValue(cbi.id, out dto);
                RectTransform_Button_Image_Container con = ReflectionUtil.GetValueByProperty<CampBUFF, RectTransform_Button_Image_Container>(cbi.id, this);
                RectTransform rt;
                bool lockBuff = Global.gApp.gSystemMgr.GetNpcMgr().LockBuff(dto);
                if (i == 0)
                {
                    CampBUFF_ItemBig item = ItemBig.GetInstance();
                    item.gameObject.SetActive(true);
                    item.rectTransform().SetParent(con.rectTransform);
                    item.rectTransform().offsetMax = new Vector2(0, 0);
                    item.rectTransform().offsetMin = new Vector2(0, 0);

                    CampBUFF_TopItemDetail itemDetail = TopItemDetail.GetInstance();
                    itemDetail.rectTransform().position = new Vector3(item.rectTransform().position.x, con.rectTransform.position.y + 1.5f, item.rectTransform().position.z);

                    TopName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(cbi.name);
                    itemDetail.name.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(cbi.name);

                    item.goods.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(cbi.icon);
                    itemDetail.Icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(cbi.icon);
                    item.level.text.text = dto.level.ToString();
                    itemDetail.level.text.text = dto.level.ToString();
                    item.lockIcon.gameObject.SetActive(lockBuff);
                    CampBuff_dataItem curData;
                    CampBuff_dataItem nextData;
                    string curPercent;
                    if (dto.level < Global.gApp.gGameData.CampBuffDataConfig.items.Length)
                    {
                        curData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 1];
                        nextData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level];
                        curPercent = GetPercentStr(cbi, curData.buff_atkNpc[0]);
                        string nextPercent = GetPercentStr(cbi, nextData.buff_atkNpc[0]);
                        //item.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.desc), curPercent, nextPercent);
                        itemDetail.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.desc), curPercent, nextPercent);
                    }
                    else
                    {
                        curData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 2];
                        nextData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 1];
                        curPercent = GetPercentStr(cbi, nextData.buff_atkNpc[0]);
                        //item.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.max_desc), curPercent);
                        itemDetail.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.max_desc), curPercent);
                    }

                    float min = dto.level == 1 ? 0 : Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 2].buff_atkNpc_cost[0];
                    float max = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 1].buff_atkNpc_cost[0];
                    int ignoreTotalNum = Global.gApp.gSystemMgr.GetNpcMgr().GetTotalNum(true);
                    item.CurText.text.text = (ignoreTotalNum).ToString();
                    item.MaxText.text.text = "/" + max.ToString();
                    item.Progress.image.fillAmount = (ignoreTotalNum) / (max);
                    item.AddText.text.text = curPercent;

                    item.NormalLevel.gameObject.SetActive(dto.level < Global.gApp.gGameData.CampBuffDataConfig.items.Length);
                    item.MaxLevel.gameObject.SetActive(dto.level == Global.gApp.gGameData.CampBuffDataConfig.items.Length);

                    rt = itemDetail.rectTransform();
                }
                else
                {
                    CampBUFF_ItemSmall item = ItemSmall.GetInstance();
                    item.gameObject.SetActive(true);
                    item.rectTransform().SetParent(con.rectTransform);
                    item.rectTransform().offsetMax = new Vector2(0, 0);
                    item.rectTransform().offsetMin = new Vector2(0, 0);
                    CampBUFF_ItemDetail itemDetail = ItemDetail.GetInstance();
                    itemDetail.rectTransform().localScale = new Vector3(1, 1, 1);
                    itemDetail.gameObject.SetActive(true);
                    itemDetail.rectTransform().position = new Vector3(item.rectTransform().position.x, con.rectTransform.position.y + 1.5f, item.rectTransform().position.z);

                    FreshItem(cbi, dto, lockBuff, item, itemDetail);
                    itemDetail.Button.button.onClick.AddListener(() =>
                    {
                        ItemDTO reduceItemDTO;
                        if (dto.level == 0)
                        {
                            reduceItemDTO = new ItemDTO(SpecialItemIdConstVal.RED_HEART, cbi.unlockCost, BehaviorTypeConstVal.OPT_CAMP_BUFF_UNLOCK);
                        } else
                        {
                            CampBuff_dataItem curData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 1];
                            float[] cost = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id + "_cost", curData);
                            reduceItemDTO = new ItemDTO(SpecialItemIdConstVal.RED_HEART, cost[0], BehaviorTypeConstVal.OPT_CAMP_BUFF_UPGRADE);
                            reduceItemDTO.paramStr2 = dto.level.ToString();
                        }
                        reduceItemDTO.paramStr1 = cbi.id;
                        GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
                        if (!reduceItemDTO.result)
                        {
                            ItemItem reduceItemCfg = Global.gApp.gGameData.ItemData.Get(reduceItemDTO.itemId);
                            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 1008, Global.gApp.gGameData.GetTipsInCurLanguage(reduceItemCfg.sourceLanguage));
                        }
                        else
                        {
                            dto.level++;
                            Global.gApp.gSystemMgr.GetNpcMgr().SaveData();
                            lockBuff = Global.gApp.gSystemMgr.GetNpcMgr().LockBuff(dto);
                            FreshItem(cbi, dto, lockBuff, item, itemDetail);

                            GameObject effect = UiTools.GetEffect(EffectConfig.Camp_buff_up, item.rectTransform());
                            effect.transform.position = item.rectTransform().position;
                        }
                    });


                    Debug.Log(cbi.id + "|" + lockBuff);
                    rt = itemDetail.rectTransform();
                }
                rt.gameObject.SetActive(false);
                con.button.onClick.AddListener(() =>
                {
                    CloseDetail();
                    rt.gameObject.SetActive(true);
                    rt.position = new Vector3(rt.position.x, con.rectTransform.position.y + 1.5f, rt.position.z);
                    m_HaveTip = true;
                    m_CurY = -100;
                });
            }
            //Content.rectTransform.position = new Vector3(Content.rectTransform.position.x, 0f, Content.rectTransform.position.z);
            maskBtn.button.onClick.AddListener(CloseDetail);
            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        private void FreshItem(CampBuffItem cbi, SkillItemDTO dto, bool lockBuff, CampBUFF_ItemSmall item, CampBUFF_ItemDetail itemDetail)
        {
            item.name.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(cbi.name);
            itemDetail.name.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(cbi.name);
            item.goods.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(cbi.icon);
            itemDetail.icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(cbi.icon);
            item.level.text.text = dto.level.ToString();
            itemDetail.level.text.text = dto.level.ToString();
            CampBuff_dataItem curData;
            CampBuff_dataItem nextData;
            string curPercent;
            float[] cost;

            float heartCost;
            if (dto.level < Global.gApp.gGameData.CampBuffDataConfig.items.Length)
            {
                itemDetail.Button.gameObject.SetActive(dto.state != WeaponStateConstVal.NONE);
                itemDetail.unlockTips.gameObject.SetActive(dto.state == WeaponStateConstVal.NONE);
                if (dto.state == WeaponStateConstVal.NONE)
                {
                    itemDetail.unlockTips.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(4378), cbi.campLevel);
                }
                //升级
                if (dto.level > 0)
                {
                    curData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 1];
                    nextData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level];
                    float[] curVal = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id, curData);
                    float[] nextVal = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id, nextData);
                    curPercent = GetPercentStr(cbi, curVal[0]);
                    string nextPercent = GetPercentStr(cbi, nextVal[0]);
                    itemDetail.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.desc), curPercent, nextPercent);
                    item.text.text.text = "+" + curPercent;

                    cost = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id + "_cost", curData);
                    heartCost = cost[0];
                    itemDetail.upgrade.gameObject.SetActive(true);
                    itemDetail.unlock.gameObject.SetActive(false);
                    itemDetail.cost.text.text = UiTools.FormateMoney(heartCost);
                }
                else
                {
                    //解锁
                    nextData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level];
                    float[] nextVal = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id, nextData);
                    string nextPercent = GetPercentStr(cbi, nextVal[0]);
                    itemDetail.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.desc_nolearn), nextPercent);
                    itemDetail.upgrade.gameObject.SetActive(false);
                    itemDetail.unlock.gameObject.SetActive(true);
                    heartCost = cbi.unlockCost;
                    itemDetail.cost.text.text = UiTools.FormateMoney(heartCost);
                    
                }
                bool red = GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.RED_HEART) < heartCost;
                itemDetail.cost.text.color = ColorUtil.GetTextColor(red, ColorUtil.m_YellowColor);
                itemDetail.up.gameObject.SetActive(!lockBuff && !red);
            }
            else
            {
                //满级
                curData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 2];
                nextData = Global.gApp.gGameData.CampBuffDataConfig.items[dto.level - 1];
                float[] curVal = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id, curData);
                float[] nextVal = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(cbi.id, nextData);
                curPercent = GetPercentStr(cbi, nextVal[0]);
                itemDetail.desc.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(cbi.max_desc), curPercent);

                itemDetail.Button.gameObject.SetActive(false);
                itemDetail.unlockTips.gameObject.SetActive(true);
                itemDetail.unlockTips.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(4383);
                itemDetail.up.gameObject.SetActive(false);
                item.text.text.text = "+" + curPercent;
            }
            item.lockIcon.gameObject.SetActive(lockBuff);
            itemDetail.lockIcon.gameObject.SetActive(lockBuff);
            item.numberBg.gameObject.SetActive(!lockBuff);
            item.text.gameObject.SetActive(!lockBuff);
            item.goods.image.color = ColorUtil.GetSpecialColor(lockBuff, ColorUtil.m_HalfColor);
            itemDetail.icon.image.color = ColorUtil.GetSpecialColor(lockBuff, ColorUtil.m_HalfColor);
            itemDetail.lvBg.gameObject.SetActive(!lockBuff);
            item.lvBg.gameObject.SetActive(!lockBuff);
        }

        private void CloseDetail()
        {
            CampBUFF_ItemDetail[] items = gameObject.GetComponentsInChildren<CampBUFF_ItemDetail>();
            foreach (CampBUFF_ItemDetail item in items)
            {
                item.gameObject.SetActive(false);
            }
            CampBUFF_TopItemDetail[] tItems = gameObject.GetComponentsInChildren<CampBUFF_TopItemDetail>();
            foreach (CampBUFF_TopItemDetail item in tItems)
            {
                item.gameObject.SetActive(false);
            }
            m_HaveTip = false;
        }

        private string GetPercentStr(CampBuffItem cbi, float v)
        {
            int prm = cbi.denominator == 1 ? 100 : 1;
            return ((v - cbi.initValue) * prm).ToString("0.#") + "%";
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
