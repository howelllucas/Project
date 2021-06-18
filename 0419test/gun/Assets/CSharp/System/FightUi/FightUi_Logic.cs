using EZ.Data;
using EZ.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public enum FightTaskUiType
    {
        Empty = 0,
        KillApointMonster,
        ArrivePoint,
        ActiveTurret,
        CollectProp,
        ArriveHelicopter,
    }
    public partial class FightUI
    {

        List<FightUI_HeartItem> m_Hearts = new List<FightUI_HeartItem>();
        Dictionary<string, FightUI_Itemtime> m_ItimeItems = new Dictionary<string, FightUI_Itemtime>();
        Dictionary<int, Image> m_TaskIconImages = new Dictionary<int, Image>();
        private int m_LastTaskIconIndex = -1;
        FightAttrUiMgr m_AttrUiMgr;
        private Color m_TaskIconLightColor = new Color(103.0f / 255, 254.0f / 255, 178.0f / 255, 255.0f / 255);
        private Color m_TaskIconGrayColor = new Color(0.1f / 255, 0.1f / 255, 0.1f / 255, 125.0f / 255);
        private float m_CurTime = 0;
        private string m_ArrowName = "Arrow";
        private int m_PreTime = -10000;
        private PassItem m_PassData;
        private bool m_ResumeFromeAd = false;
        private double m_EnterBackTime = 0;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_AttrUiMgr = new FightAttrUiMgr(BossHpNode.gameObject.transform,this);
            HeartItem.gameObject.SetActive(false);



            m_PassData = Global.gApp.CurScene.GetPassData();
            InitUiNode();
            RegisterListeners();

            pause.button.onClick.AddListener(OnPause);
            DelayVisible();
#if (!UNITY_EDITOR)
           DataNode.gameObject.SetActive(false);
#endif

            base.ChangeLanguage();
        }
        private void Update()
        {
#if (UNITY_EDITOR)
            Gun gun = Global.gApp.CurScene.GetMainPlayerComp().GetWeaponMgr().GetFirstGun();
            Atk.text.text = gun.GetBaseDamage().ToString();
            AtkTime.text.text = gun.GetBaseAtkTime().ToString();
            PassParam.text.text = m_PassData.hpParam.ToString();
#endif
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            int time = (int)m_CurTime;
            if (time > m_PreTime)
            {
                m_PreTime = time;
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.TimeCoolDown, time);
            }

        }

        private void OnPause()
        {
            //Time.timeScale = 0;
            //Global.gApp.CurScene.Pause();
            //pause.gameObject.SetActive(false);
            //resume.gameObject.SetActive(true);
            //Global.gApp.gAudioSource.Pause();
            FightResultManager.instance.ShowPausePanel();
        }

        private void InitUiNode()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightPlotUI);
        }

        private void FightModeTopTips(string tips1, string tips2)
        {
            countdownname.text.text = tips1;
            countdowntime.text.text = tips2;
        }
        private void FightTopTips(string tips, bool ingnoreAnim)
        {
            if (!ingnoreAnim && !wavetxt.text.text.Equals(tips))
            {
                wavetxt.gameObject.GetComponent<Animator>().Play("changePlayTips", -1, 0);
            }
            wavetxt.text.text = tips;
        }
        private void GoldChanged(double gold)
        {
            cointxt.text.text = UiTools.FormateMoneyUP(gold);
        }

        private void MainRoleHpChange(float curHp, float maxHp)
        {
            int mHp = (int)maxHp;
            //set hearts
            while (m_Hearts.Count < mHp)
            {
                FightUI_HeartItem item = HeartItem.GetInstance();
                item.gameObject.SetActive(true);
                m_Hearts.Add(item);
            }
            for (int i = m_Hearts.Count - 1, j = 1; i >= 0; i--, j++)
            {
                bool on = j <= curHp;
                FightUI_HeartItem item = m_Hearts[i];
                item.heart.gameObject.SetActive(on);
                item.heartbreak.gameObject.SetActive(!on);
            }

            ////recycle
            //foreach (FightUI_HeartItem item in hearts)
            //{
            //    HeartItem.CacheInstance(item);
            //}
        }

        private void MonsterHpChanged(int guid, float progress)
        {

        }
        public void RemoveCountItem(FightUI_Itemtime item)
        {
            m_ItimeItems.Remove(item.GetName());
            Destroy(item.gameObject);
        }
        private void AddTimeItem(string keyName, string itemName, float time)
        {
            FightUI_Itemtime item;
            if (m_ItimeItems.TryGetValue(keyName, out item))
            {
                ItemItem itemData = Global.gApp.gGameData.GetItemDataByName(itemName);
                item.ResetTime(time, keyName, itemData);
            }
            else if (time > 0)
            {
                ItemItem itemData = Global.gApp.gGameData.GetItemDataByName(itemName);
                if (itemData != null && itemData.image_time != null && !itemData.image_time.Equals(GameConstVal.EmepyStr))
                {
                    item = Itemtime.GetInstance();
                    item.gameObject.SetActive(true);
                    m_ItimeItems.Add(keyName, item);
                    item.Init(time, keyName, this, itemData);
                }
            }
        }

        private void UpdatePlayerEnergy(string keyName, string itemName, float percent)
        {
            FightUI_Itemtime item;
            if (m_ItimeItems.TryGetValue(keyName, out item))
            {
                ItemItem itemData = Global.gApp.gGameData.GetItemDataByName(itemName);
                item.SetPercent(percent);
            }
        }

        private void FocusGameObject(GameObject go, bool isset, int r)
        {
            DarkEffect de = Global.gApp.CurScene.GetCameraObj().GetComponent<DarkEffect>();
            if (de && de.enabled)
            {
                if (isset)
                {
                    de.AddFocus(go, r);
                }
                else
                {
                    de.LostFocus(go);
                }
            }
        }
        private void FreshGunPower(float progress, ItemItem itemData)
        {
            if (progress >= 0f)
            {
                if (progress == 0)
                {
                    if (itemData != null)
                    {
                        Itemicon.image.sprite = Global.gApp.gResMgr.LoadSprite(itemData.image_time, EffectConfig.FightUiAtlas);
                    }
                }
                PowerBg.image.fillAmount = progress;
                GunPowerProgress.gameObject.SetActive(true);
            }
            else
            {
                GunPowerProgress.gameObject.SetActive(false);
            }
        }

        private void FightProgress(float progress)
        {
            if (progress >= 0)
            {
                progress_bg.gameObject.SetActive(true);
                Vector2 offsetMax = new Vector2((progress - 1) * 550, 0);
                content.rectTransform.offsetMax = offsetMax;
            }
            else
            {
                progress_bg.gameObject.SetActive(false);
            }
        }

        private void PointArrowAngleChange(float Angle, string disTip, FightTaskUiType arrowType = FightTaskUiType.Empty)
        {
            if (Angle < 0)
            {
                ArrorNode.gameObject.SetActive(false);
                return;
            }
            else
            {
                ArrorNode.gameObject.SetActive(true);
            }
            float newAngle = Angle + 180.0f + 27.5f;
            newAngle = newAngle % 360;
            Vector3 angleVec = new Vector3(0, 0, newAngle);
            Vector3 pos;
            if (newAngle < 180.0f)
            {
                Vector3 m_DtRightPos = (RightTop.rectTransform.position - RightBottom.rectTransform.position);
                pos = RightBottom.rectTransform.position + m_DtRightPos * (newAngle / 180.0f);

            }
            else
            {
                newAngle = newAngle - 180.0f;
                Vector3 m_DtLeftPos = (LeftBottom.rectTransform.position - LeftTop.rectTransform.position);
                pos = LeftTop.rectTransform.position + m_DtLeftPos * (newAngle / 180.0f); ;
            }
            if (arrowType == FightTaskUiType.Empty)
            {
                ArrowBg.gameObject.SetActive(true);
                ArrowPointBg.gameObject.SetActive(false);
                Distance.text.text = disTip;
                ArrowBg.rectTransform.position = pos;
                RotateNode.rectTransform.localEulerAngles = angleVec;
            }
            else
            {
                RotateNodePoint.rectTransform.localEulerAngles = angleVec;
                ArrowBg.gameObject.SetActive(false);
                ArrowPointBg.gameObject.SetActive(true);
                DistancePoint.text.text = disTip;
                ArrowPointBg.rectTransform.position = pos;
                battery.gameObject.SetActive(arrowType == FightTaskUiType.ActiveTurret);
                capture.gameObject.SetActive(arrowType == FightTaskUiType.ArrivePoint);
                monster.gameObject.SetActive(arrowType == FightTaskUiType.KillApointMonster);
                property.gameObject.SetActive(arrowType == FightTaskUiType.CollectProp);
                copter.gameObject.SetActive(arrowType == FightTaskUiType.ArriveHelicopter);
            }
        }
        public RectTransform GetChipIconRectTsf()
        {
            return ChipIcon.rectTransform;
        }
        private void CreateTaskIcon(int index, FightTaskUiType taskIconType)
        {
            GameObject newGo = null;
            if (taskIconType == FightTaskUiType.ActiveTurret)
            {
                newGo = Instantiate(battery1.gameObject);
            }
            else if (taskIconType == FightTaskUiType.ArrivePoint)
            {
                newGo = Instantiate(capture1.gameObject);
            }
            else if (taskIconType == FightTaskUiType.KillApointMonster)
            {
                newGo = Instantiate(monster1.gameObject);
            }
            else if (taskIconType == FightTaskUiType.CollectProp)
            {
                newGo = Instantiate(property1.gameObject);
            }
            else if (taskIconType == FightTaskUiType.ArriveHelicopter)
            {
                newGo = Instantiate(copter1.gameObject);
            }
            if (newGo != null)
            {
                newGo.SetActive(true);
                newGo.transform.SetParent(TaskIconNode.rectTransform, false);
                Transform arraw = newGo.transform.Find(m_ArrowName);
                Image image = newGo.GetComponent<Image>();
                image.color = m_TaskIconGrayColor;
                arraw.GetComponent<Image>().color = m_TaskIconGrayColor;
                m_TaskIconImages.Add(index, image);
                TaskIconNode.rectTransform.sizeDelta = new Vector2(m_TaskIconImages.Count * 120, 50);
                m_LastTaskIconIndex = index;
            }
        }
        private void TaskIconLight(int index, FightTaskUiType fightTaskIconType)
        {
            if (fightTaskIconType != FightTaskUiType.Empty)
            {
                TaskIconNode.gameObject.SetActive(true);
                Image image;
                if (m_TaskIconImages.TryGetValue(index, out image))
                {
                    image.color = m_TaskIconLightColor;
                    Transform arraw = image.transform.Find(m_ArrowName);
                    arraw.GetComponent<Image>().color = m_TaskIconLightColor;
                }
            }
            else
            {
                TaskIconNode.gameObject.SetActive(false);
            }
        }
        private void WpnChipChanged()
        {
            ChipCount.gameObject.SetActive(true);
            ChipIcon.gameObject.SetActive(true);
            ChipCount.text.text = Global.gApp.CurScene.GetMainPlayerComp().GetPlayerData().GetDropResCount(GameConstVal.WWeaponChip).ToString();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<float>(MsgIds.FightUiProgress, FightProgress);

            Global.gApp.gMsgDispatcher.AddListener<string, bool>(MsgIds.FightUiTopTips, FightTopTips);
            Global.gApp.gMsgDispatcher.AddListener<string, string>(MsgIds.FightUiModeCountDownTips, FightModeTopTips);

            Global.gApp.gMsgDispatcher.AddListener<float, float>(MsgIds.MainRoleHpChange, MainRoleHpChange);
            Global.gApp.gMsgDispatcher.AddListener<double>(MsgIds.FightGainGold, GoldChanged);
            Global.gApp.gMsgDispatcher.AddListener<int, float>(MsgIds.MonsterHpChanged, MonsterHpChanged);
            Global.gApp.gMsgDispatcher.AddListener<string, string, float>(MsgIds.AddFightUICountItem, AddTimeItem);
            Global.gApp.gMsgDispatcher.AddListener<string, string, float>(MsgIds.UpdatePlayerEnergy, UpdatePlayerEnergy);
            Global.gApp.gMsgDispatcher.AddListener<float, string, FightTaskUiType>(MsgIds.PointArrowAngle, PointArrowAngleChange);
            Global.gApp.gMsgDispatcher.AddListener<GameObject, bool, int>(MsgIds.FocusGameObject, FocusGameObject);
            Global.gApp.gMsgDispatcher.AddListener<float, ItemItem>(MsgIds.FreshGunPower, FreshGunPower);

            Global.gApp.gMsgDispatcher.AddListener<int, FightTaskUiType>(MsgIds.CreateTaskIcon, CreateTaskIcon);
            Global.gApp.gMsgDispatcher.AddListener<int, FightTaskUiType>(MsgIds.TaskIconLight, TaskIconLight);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.WpnChipCountChanged, WpnChipChanged);
        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<float>(MsgIds.FightUiProgress, FightProgress);
            Global.gApp.gMsgDispatcher.RemoveListener<string, bool>(MsgIds.FightUiTopTips, FightTopTips);
            Global.gApp.gMsgDispatcher.RemoveListener<string, string>(MsgIds.FightUiModeCountDownTips, FightModeTopTips);

            Global.gApp.gMsgDispatcher.RemoveListener<float, float>(MsgIds.MainRoleHpChange, MainRoleHpChange);
            Global.gApp.gMsgDispatcher.RemoveListener<double>(MsgIds.FightGainGold, GoldChanged);
            Global.gApp.gMsgDispatcher.RemoveListener<int, float>(MsgIds.MonsterHpChanged, MonsterHpChanged);
            Global.gApp.gMsgDispatcher.RemoveListener<string, string, float>(MsgIds.AddFightUICountItem, AddTimeItem);
            Global.gApp.gMsgDispatcher.RemoveListener<string, string, float>(MsgIds.UpdatePlayerEnergy, UpdatePlayerEnergy);
            Global.gApp.gMsgDispatcher.RemoveListener<float, string, FightTaskUiType>(MsgIds.PointArrowAngle, PointArrowAngleChange);
            Global.gApp.gMsgDispatcher.RemoveListener<GameObject, bool, int>(MsgIds.FocusGameObject, FocusGameObject);
            Global.gApp.gMsgDispatcher.RemoveListener<float, ItemItem>(MsgIds.FreshGunPower, FreshGunPower);

            Global.gApp.gMsgDispatcher.RemoveListener<int, FightTaskUiType>(MsgIds.CreateTaskIcon, CreateTaskIcon);
            Global.gApp.gMsgDispatcher.RemoveListener<int, FightTaskUiType>(MsgIds.TaskIconLight, TaskIconLight);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.WpnChipCountChanged, WpnChipChanged);

        }
        private void DelayVisible()
        {
            DynamicNode.gameObject.SetActive(false);
            StaticNode.gameObject.SetActive(false);
            int callTimes = 0;
            DelayCallBack callBack = gameObject.AddComponent<DelayCallBack>();
            callBack.SetCallTimes(3);
            callBack.SetAction(
                () =>
                {
                    callTimes++;
                    if (callTimes == 3)
                    {
                        DynamicNode.gameObject.SetActive(true);
                        StaticNode.gameObject.SetActive(true);
                        if (m_LastTaskIconIndex > 0)
                        {
                            Image image;
                            if (m_TaskIconImages.TryGetValue(m_LastTaskIconIndex, out image))
                            {
                                image.transform.Find(m_ArrowName).gameObject.SetActive(false);
                            }
                            if (m_LastTaskIconIndex > 6)
                            {
                                float m_scale = 6.0f / m_LastTaskIconIndex;
                                TaskIconNode.rectTransform.localScale = new Vector3(m_scale, m_scale, m_scale);
                            }
                        }
                    }
                },
                0.1f, true);
        }
        private void OnApplicationFocus(bool focus)
        {
#if (!UNITY_EDITOR)
            if (focus)
            {
                if (DateTimeUtil.GetMills(DateTime.Now) - m_EnterBackTime > 5000)
                {
                    if (!m_ResumeFromeAd && BaseScene.TimeScale > 0)
                    {
                        if (Global.gApp.gUiMgr.GetPanelCompent<FightPause>(Wndid.FightPausePanel) == null)
                        {
                            gameObject.AddComponent<DelayCallBack>().SetAction(() => { FightResultManager.instance.ShowPausePanel(); }, 0.1f, true);
                        }
                    }
                }
                m_ResumeFromeAd = false;
            }
            else
            {
                m_EnterBackTime = DateTimeUtil.GetMills(DateTime.Now);
            }
#endif
        }
        public void PauseFromAd()
        {
            m_ResumeFromeAd = true;
        }
        public override void Release()
        {
            UnRegisterListeners();
            m_AttrUiMgr.Destroy();
            Global.gApp.gUiMgr.ClosePanel(Wndid.FightPlotUI);
            base.Release();
        }
    }
}

