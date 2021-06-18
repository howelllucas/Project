using UnityEngine;
using System.Collections;
using EZ.Data;
using System.Collections.Generic;
using EZ.DataMgr;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EZ
{
    public partial class LevelDetail: IDropHandler, IPointerDownHandler, IPointerClickHandler
    {

        private float m_CurHeight = -1f;
        private float m_ShowMin = 0f;
        private float m_ShowMax = 0f;
        private int m_CurStep = 0;
        private int m_CurIndex = 0;
        private int m_UserStep = 0;
        private float m_Height = 0f;


        private const float MOVE_SPEED = 1F;
        private const float SMOOTH_TIME = 0.15F;
        private float m_MoveSpeed = 0f;
        private bool m_NeedMove = false;
        private float m_TargetValue = 0f;
        private float m_StartValue = 0f;
        private float m_BackDis = 350f;
        private float m_DR = 0.135f;
        private int m_InitId;
        private bool m_Human;

        private bool isRight = false;
        private Dictionary<int, QuestItem> passQuestDic = new Dictionary<int, QuestItem>();
        private Dictionary<int, int> m_StepIndexDic = new Dictionary<int, int>();
        private List<MonoBehaviour> m_ScrolViewList = new List<MonoBehaviour>();
        private List<float> m_ScrolViewHeightList = new List<float>();
        private Dictionary<int, float> m_StepHeightDic = new Dictionary<int, float>();

        private int m_BgIndex = 0;
        private string[] m_EffectArr = new string[]{"Prefabs/Effect/ui/UI_dt1", "Prefabs/Effect/ui/UI_dt2_icon9", "Prefabs/Effect/ui/UI_dt3_icon4", "Prefabs/Effect/ui/UI_dt4", "Prefabs/Effect/ui/UI_icon1367" };
        private Dictionary<string, string> m_DtEffectDtc = new Dictionary<string, string>();
        private Dictionary<string, string> m_IconEffectDtc = new Dictionary<string, string>();
        private int m_EffectIndex = 0;
        private int m_EffectShowIndex = 0;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            foreach (string effectStr in m_EffectArr)
            {
                Debug.Log("effectStr=" + effectStr);
                string[] strs = effectStr.Split('_');
                for (int i = 1; i < strs.Length; i ++)
                {
                    int dtIndex = strs[i].IndexOf("dt");
                    for (int di = dtIndex + 2; dtIndex != -1 && di < strs[i].Length; di ++)
                    {
                        m_DtEffectDtc[strs[i][di].ToString()] = effectStr;
                    }
                    Debug.Log("dtIndex=" + dtIndex);
                    int iconIndex = strs[i].IndexOf("icon");
                    Debug.Log("iconIndex=" + iconIndex);
                    for (int ii = iconIndex + 4; iconIndex != -1 && ii < strs[i].Length; ii++)
                    {
                        m_IconEffectDtc[strs[i][ii].ToString()] = effectStr;
                    }
                }
            }
            base.Init(name, info, arg);
            InitNode();
            RegisterListeners();

            base.ChangeLanguage();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_Human = true;
            Debug.Log("OnDown = " + m_Human);
        }

        public void OnDrop(PointerEventData eventData)
        {
            m_Human = false;
            Debug.Log("OnDrop = " + m_Human);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_Human = false;
            Debug.Log("OnPointerClick = " + m_Human);
        }

        public void InitNode()
        {

            GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            m_InitId = Convert.ToInt32(initPassIdConfig.content);
            m_BackDis = Viewport.rectTransform.rect.size.y / 6;
            Debug.Log("m_BackDis = " + m_BackDis);
            CloseBtn.button.onClick.AddListener(OnCloseBtn);

            topTip.button.onClick.AddListener(OnToCur);
            bottomTip.button.onClick.AddListener(OnToCur);

            progress.gameObject.SetActive(false);
            step.gameObject.SetActive(false);
            gun.gameObject.SetActive(false);

            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LEVEL_DETAIL];
            foreach (QuestItem questItem in configs)
            {
                passQuestDic.Add((int)questItem.condition[1], questItem);
            }
            int curId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId() - m_InitId;

            m_UserStep = Global.gApp.gSystemMgr.GetPassMgr().GetUserStep();

            for (int i = 0; i < Global.gApp.gGameData.PassStep.Count && i <= m_UserStep; i++)
            {
                m_ScrolViewHeightList.Add(-m_Height - step.rectTransform.sizeDelta.y / 2);
                m_Height += step.rectTransform.sizeDelta.y;
                m_ScrolViewList.Add(null);

                int stepStart = i == 0 ? 1 : Global.gApp.gGameData.PassStep[i - 1] + 1 - m_InitId;
                int stepEnd = Global.gApp.gGameData.PassStep[i] - m_InitId;
                Debug.Log("stepStart = " + stepStart + ", stepEnd = " + stepEnd);
                int start = -1;
                for (int j = stepStart; j <= stepEnd; j++)
                {
                    
                    QuestItem questItemCfg = null;
                    ItemItem iCfg = null;
                    bool isWeapon = false;
                    passQuestDic.TryGetValue(j, out questItemCfg);
                    if (questItemCfg != null)
                    {
                        iCfg = Global.gApp.gGameData.ItemData.Get((int)questItemCfg.award[0]);
                        isWeapon = ItemTypeConstVal.isWeapon(iCfg.showtype);
                    }
                    
                    if (isWeapon)
                    {
                        start = -1;
                        m_ScrolViewHeightList.Add(-m_Height - gun.rectTransform.sizeDelta.y / 2);
                        m_Height += gun.rectTransform.sizeDelta.y;
                        m_ScrolViewList.Add(null);
                    } else
                    {
                        if (start == -1)
                        {
                            start = j;
                        }

                        if ((j - start) % 2 == 0)
                        {
                            m_ScrolViewHeightList.Add(-m_Height - progress.rectTransform.sizeDelta.y / 2);
                            m_Height += progress.rectTransform.sizeDelta.y;
                            m_ScrolViewList.Add(null);
                        }
                    }

                    if (j == curId)
                    {
                        m_CurIndex = m_ScrolViewList.Count - 1;
                        m_CurHeight = m_Height;
                    }
                }
                if (i == m_UserStep && m_UserStep < Global.gApp.gGameData.PassStep.Count - 1)
                {
                    m_ScrolViewHeightList.Add(-m_Height - step.rectTransform.sizeDelta.y / 2);
                    m_Height += step.rectTransform.sizeDelta.y;
                    m_ScrolViewList.Add(null);
                }


                m_StepHeightDic.Add(i, m_Height);
                m_StepIndexDic.Add(i, m_ScrolViewList.Count);
            }
            Debug.Log("m_CurIndex = " + m_CurIndex);
            ShowStepUI(m_UserStep);
            float callSecs = 1f / Application.targetFrameRate;
            int callTimes = m_UserStep - 1;
            if (m_UserStep - 1 >= 0)
            {
                DelayCallBack delayCallBack = gameObject.AddComponent<DelayCallBack>();
                delayCallBack.SetCallTimes(m_UserStep);
                delayCallBack.SetAction(() =>
                {
                    ShowStepUI(callTimes--);
                }, callSecs);
            }
             

            
            m_CurStep = m_UserStep;
            //float baseHeight = m_CurStep == 0 ? 0f : m_StepHeightDic[m_CurStep - 1];
            //Content.rectTransform.sizeDelta = new Vector2(Content.rectTransform.sizeDelta.x, m_StepHeightDic[m_CurStep] - baseHeight);
            Content.rectTransform.sizeDelta = new Vector2(Content.rectTransform.sizeDelta.x, m_Height);
            
            

            m_TargetValue = Math.Abs(m_ScrolViewHeightList[m_CurIndex]) - Viewport.rectTransform.rect.size.y / 2;
            if (m_TargetValue < 0f)
            {
                m_TargetValue = 0f;
            }
            else if (m_TargetValue > m_Height - Viewport.rectTransform.rect.size.y)
            {
                m_TargetValue = m_Height - Viewport.rectTransform.rect.size.y;
            }
            Content.rectTransform.localPosition = new Vector3(Content.rectTransform.localPosition.x, m_TargetValue, Content.rectTransform.localPosition.z);
            m_NeedMove = false;
            PassItem passItemCfg = Global.gApp.gGameData.PassData.Get(Global.gApp.gGameData.PassStep[m_CurStep]);
            //bg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(passItemCfg.mainUIbg);

            m_ShowMin = Content.rectTransform.localPosition.y;
            m_ShowMax = Content.rectTransform.localPosition.y + Viewport.rectTransform.rect.size.y;
            topTip.gameObject.SetActive(false);
            bottomTip.gameObject.SetActive(false);

        }

        public void Update()
        {
            float showMin = Content.rectTransform.localPosition.y;
            float showMax = Content.rectTransform.localPosition.y + Viewport.rectTransform.rect.size.y;
            if (showMin == m_ShowMin && showMax == m_ShowMax)
            {
                return;
            }
            m_ShowMin = showMin;
            m_ShowMax = showMax;
            if (m_CurHeight < showMin)
            {
                //Debug.Log("<  m_CurHeight = " + m_CurHeight + ", showMin = " + showMin + ", showMax = " + showMax);
                topTip.gameObject.SetActive(true);
                bottomTip.gameObject.SetActive(false);
            }
            else if (m_CurHeight > showMax)
            {
                //Debug.Log(">  m_CurHeight = " + m_CurHeight + ", showMin = " + showMin + ", showMax = " + showMax);
                topTip.gameObject.SetActive(false);
                bottomTip.gameObject.SetActive(true);
            }
            else
            {
                //Debug.Log("<>  m_CurHeight = " + m_CurHeight + ", showMin = " + showMin + ", showMax = " + showMax);
                topTip.gameObject.SetActive(false);
                bottomTip.gameObject.SetActive(false);
            }


            ////向下切屏
            //if (Content.rectTransform.localPosition.y + Viewport.rectTransform.rect.size.y - Content.rectTransform.sizeDelta.y > m_BackDis)
            //{
            //    if (m_CurStep < m_UserStep + 1 && m_CurStep < Global.gApp.gGameData.PassStep.Count && !m_NeedMove)
            //    {
            //        Debug.Log("下="+ Content.rectTransform.localPosition.y);
            //        m_CurStep++;

            //        Debug.Log("下=" + Content.rectTransform.localPosition.y);

            //        Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //        float curPos = Content.rectTransform.localPosition.y;
            //        float beforeHeight = m_CurStep - 2 < 0 ? m_StepHeightDic[m_CurStep - 1] : m_StepHeightDic[m_CurStep - 1] - m_StepHeightDic[m_CurStep - 2];
            //        float newPos = curPos - beforeHeight;

            //        m_DR = ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate;
            //        ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;

            //        ResetPosition();
            //        Content.rectTransform.sizeDelta = new Vector2(Content.rectTransform.sizeDelta.x, m_StepHeightDic[m_CurStep] - m_StepHeightDic[m_CurStep - 1]);
            //        Content.rectTransform.localPosition = new Vector3(Content.rectTransform.localPosition.x, newPos, Content.rectTransform.localPosition.z);
            //        m_NeedMove = true;
            //        m_TargetValue = 0;
            //        m_StartValue = newPos;

            //    }
            //}

            ////向上切屏
            //if (Content.rectTransform.localPosition.y < -m_BackDis)
            //{
            //    if (m_CurStep > 0 && !m_NeedMove)
            //    {
            //        Debug.Log("上");
            //        m_CurStep--;
            //        float tmp = m_CurStep == 0 ? 0f : m_StepHeightDic[m_CurStep - 1];
            //        Content.rectTransform.sizeDelta = new Vector2(Content.rectTransform.sizeDelta.x, m_StepHeightDic[m_CurStep] - tmp);
            //        Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //        m_DR = ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate;
            //        ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;
            //        ResetPosition();
            //        m_NeedMove = true;
            //        m_TargetValue = Content.rectTransform.sizeDelta.y - Viewport.rectTransform.rect.size.y;
            //        m_StartValue = Content.rectTransform.localPosition.y + Content.rectTransform.sizeDelta.y;
            //        Content.rectTransform.localPosition = new Vector3(Content.rectTransform.localPosition.x, m_StartValue, Content.rectTransform.localPosition.z);


            //    }
            //}
            //if (!m_Human && !m_NeedMove)
            //{
            //    //是否吸附
            //    foreach (int step in m_StepHeightDic.Keys)
            //    {
            //        float before = step == 0 ? 0f : m_StepHeightDic[step - 1];
            //        float stepHeight = m_StepHeightDic[step] - before;
            //        if (stepHeight >= Viewport.rectTransform.rect.size.y && Math.Abs(Content.rectTransform.localPosition.y + Viewport.rectTransform.rect.size.y - m_StepHeightDic[step]) <= m_BackDis)
            //        {//下边界吸附

            //            m_CurStep = step;
            //            m_NeedMove = true;
            //            m_TargetValue = m_StepHeightDic[step] - Viewport.rectTransform.rect.size.y;
            //            m_StartValue = Content.rectTransform.localPosition.y;
            //            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //            m_DR = ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate;
            //            ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;
            //            break;
            //        } else if (Math.Abs(Content.rectTransform.localPosition.y - m_StepHeightDic[step]) <= m_BackDis)
            //        {//上边界吸附
            //            m_CurStep = step + 1;
            //            m_NeedMove = true;
            //            m_TargetValue = m_StepHeightDic[step];
            //            m_StartValue = Content.rectTransform.localPosition.y;
            //            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //            m_DR = ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate;
            //            ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;
            //            break;
            //        }
            //    }
            //}
            if (!m_NeedMove)
            {
                //是否换背景
                //foreach (int step in m_StepHeightDic.Keys)
                //{
                //    float before = step == 0 ? 0f : m_StepHeightDic[step - 1];
                //    if (Content.rectTransform.localPosition.y >= before - Viewport.rectTransform.rect.size.y / 2 && Content.rectTransform.localPosition.y < m_StepHeightDic[step] - Viewport.rectTransform.rect.size.y / 2)
                //    {
                //        if (step != m_CurStep)
                //        {
                //            m_CurStep = step;
                //            PassItem passItemCfg = Global.gApp.gGameData.PassData.Get(Global.gApp.gGameData.PassStep[m_CurStep]);
                //            bg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(passItemCfg.mainUIbg);
                //        }
                //        break;
                //    }
                //}
            }


        }

        public void LateUpdate()
        {
            if (m_NeedMove && !m_Human)
            {
                if (Mathf.Abs(m_StartValue - m_TargetValue) < 30f)
                {
                    Content.rectTransform.localPosition = new Vector3(Content.rectTransform.localPosition.x, m_TargetValue, Content.rectTransform.localPosition.z);
                    m_NeedMove = false;

                    //PassItem passItemCfg = Global.gApp.gGameData.PassData.Get(Global.gApp.gGameData.PassStep[m_CurStep]);
                    //bg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(passItemCfg.mainUIbg);
                    ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate = m_DR;

                    //Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
                    return;
                }
                m_StartValue = Mathf.SmoothDamp(m_StartValue, m_TargetValue, ref m_MoveSpeed, SMOOTH_TIME);
                Debug.Log("Content.rectTransform.localPosition.y=" + "m_StartValue=" + m_StartValue);
                Content.rectTransform.localPosition = new Vector3(Content.rectTransform.localPosition.x, m_StartValue, Content.rectTransform.localPosition.z);
            }
        }

        private void OnToCur()
        {
            m_Human = false;
            m_TargetValue = Math.Abs(m_ScrolViewHeightList[m_CurIndex]) - Viewport.rectTransform.rect.size.y / 2;
            //跳到当前位置
            ScrollView.gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;
            m_NeedMove = true;
            if (m_TargetValue < 0f)
            {
                m_TargetValue = 0f;
            }
            else if (m_TargetValue > m_Height - Viewport.rectTransform.rect.size.y)
            {
                m_TargetValue = m_Height - Viewport.rectTransform.rect.size.y;
            }
            Debug.Log("m_TargetValue = " + m_TargetValue);
            m_StartValue = Content.rectTransform.localPosition.y;
            //Global.gApp.gGameCtrl.AddGlobalTouchMask();
        }


        private void ResetPosition()
        {
            float baseHeight = 0;// m_CurStep == 0 ? 0f : m_StepHeightDic[m_CurStep - 1];
            for (int i = 0; i < m_ScrolViewList.Count; i ++)
            {
                MonoBehaviour m = m_ScrolViewList[i];
                if (m == null)
                {
                    continue;
                }
                m.transform.localPosition = new Vector3(Viewport.rectTransform.rect.size.x / 2, m_ScrolViewHeightList[i] + baseHeight, m.transform.localPosition.z);
            }
        }

        private void ShowStepUI(int stepId)
        {
            isRight = false;
            int index = stepId == 0 ? 0 : m_StepIndexDic[stepId - 1]; 
            if (stepId == -1)
            {
                m_UserStep = Global.gApp.gSystemMgr.GetPassMgr().GetUserStep();
                m_CurStep = m_UserStep;
            }
            else
            {
                m_CurStep = stepId;
            }

            int stepStart = m_CurStep == 0 ? 1 : Global.gApp.gGameData.PassStep[m_CurStep - 1] + 1 - m_InitId;
            int stepEnd = Global.gApp.gGameData.PassStep[m_CurStep] - m_InitId;

            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LEVEL_DETAIL];
            int curId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId() - m_InitId;

            //显示顶部标识
            LevelDetail_step stepUI = Instantiate<LevelDetail_step>(step.step);
            stepUI.StepName.gameObject.SetActive(false);
            m_ScrolViewList[index] = stepUI;
            stepUI.transform.localPosition = new Vector3(Viewport.rectTransform.rect.size.x / 2, m_ScrolViewHeightList[index], stepUI.transform.localPosition.z);
            index++;
            stepUI.transform.SetParent(step.step.transform.parent, false);
            PassItem passItemCfg = Global.gApp.gGameData.PassData.Get(Global.gApp.gGameData.PassStep[m_CurStep]);
            string bgIndex = System.Text.RegularExpressions.Regex.Replace(passItemCfg.mainUIbg, @"[^0-9]+", "");
            stepUI.StepIcon.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.RESOURCE_LEVEL_DETAIL_ICON, bgIndex), typeof(Sprite)) as Sprite;
            string effctUri;
            if (m_IconEffectDtc.TryGetValue(bgIndex, out effctUri))
            {
                GameObject effect = UiTools.GetEffect(effctUri, stepUI.transform, 45);
            }
            stepUI.gameObject.SetActive(true);
            //stepUI.GunItem.gameObject.SetActive(i != configs.Count - 1);
            for (int i = stepStart; i <= stepEnd; i ++)
            {

                QuestItem questItemCfg = null;
                ItemItem iCfg = null;
                bool isWeapon = false;
                passQuestDic.TryGetValue(i, out questItemCfg);
                if (questItemCfg != null)
                {
                    iCfg = Global.gApp.gGameData.ItemData.Get((int)questItemCfg.award[0]);
                    isWeapon = ItemTypeConstVal.isWeapon(iCfg.showtype);
                }
                if (isWeapon)
                {
                    LevelDetail_gun gunUI = Instantiate<LevelDetail_gun>(gun.gun);

                    gunUI.transform.SetParent(gun.gun.transform.parent, false);
                    gunUI.gameObject.SetActive(true);
                    
                    gunUI.num.text.text = i.ToString();
                    int passId = m_InitId + int.Parse(gunUI.num.text.text);
                    gunUI.GunRetry.button.onClick.AddListener(() => { OnStartGame(passId); });
                    gunUI.GunRetry.gameObject.SetActive(passId < Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId());
                    gunUI.GunIcon.image.sprite = Resources.Load(iCfg.image_grow, typeof(Sprite)) as Sprite;
                    gunUI.GunBottom.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, iCfg.qualevel), typeof(Sprite)) as Sprite;
                    string effectName = iCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? iCfg.qualevel.ToString() : "common";
                    EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
                    GameObject gunEffect = UiTools.GetEffect(string.Format(effectItem.path, iCfg.qualevel), gunUI.EffectNode.rectTransform);

                    gunEffect.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                    ParticleSystem[] pss = gunEffect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in pss)
                    {
                        ps.GetComponent<Renderer>().sortingOrder = 45;
                    }

                    PassItem passItem = Global.gApp.gGameData.PassData.Get(m_InitId + (int)questItemCfg.condition[1]);
                    SetPassIcon(gunUI.level.rectTransform, passItem, m_InitId + curId);
                    if (isRight)
                    {
                        index++;
                    }
                    m_ScrolViewList[index] = gunUI;
                    gunUI.transform.localPosition = new Vector3(Viewport.rectTransform.rect.size.x / 2, m_ScrolViewHeightList[index], gunUI.transform.localPosition.z);
                    index++;
                    isRight = false;

                    if (i >= curId)
                    {
                        gunUI.GunIcon.image.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.BLACK_COLOR).content);
                        gunUI.GunName.text.text = "???";
                        GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.BLACK_COLOR);
                        gunUI.GunName.text.color = ColorUtil.GetColor(colorConfig.content);
                        GeneralConfigItem color1Config = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.BLACK_1_COLOR);
                        gunUI.GunBottom.image.color = ColorUtil.GetColor(color1Config.content);
                    } else
                    {
                        
                        gunUI.GunName.text.text = iCfg.gamename;
                        GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);
                        //gunUI.GunName.text.color = ColorUtil.GetColor(colorConfig.contents[iCfg.qualevel]);
                        if (iCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                        {
                            gunUI.GunBottom.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, iCfg.qualevel), typeof(Sprite)) as Sprite;
                            gunUI.GunName.text.color = ColorUtil.GetColor(colorConfig.contents[iCfg.qualevel]);
                        }
                        else
                        {
                            gunUI.GunBottom.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
                            gunUI.GunName.text.color = ColorUtil.GetColor(colorConfig.contents[2]);
                        }
                    }
                }
                else
                {
                    //有奖励
                    QuestItem questItem;
                    LevelDetail_progress progressUI;
                    if (!isRight)
                    {
                        progressUI = Instantiate<LevelDetail_progress>(progress.progress);
                        progressUI.LeftGiftBg.gameObject.SetActive(false);
                        progressUI.RightGiftBg.gameObject.SetActive(false);
                        progressUI.LeftGift.gameObject.SetActive(false);
                        progressUI.RightGift.gameObject.SetActive(false);

                        m_ScrolViewList[index] = progressUI;
                    } else
                    {
                        progressUI = m_ScrolViewList[index] as LevelDetail_progress;
                    }
                    if (passQuestDic.TryGetValue(i, out questItem))
                    {
                        MakeProgressItem(questItem, curId, progressUI);
                    }
                    else
                    {
                        MakeNormalProgress(i, curId, progressUI);
                    }

                    progressUI.transform.localPosition = new Vector3(Viewport.rectTransform.rect.size.x / 2, m_ScrolViewHeightList[index], progressUI.transform.localPosition.z);
                    
                    if (isRight)
                    {
                        index++;
                    }
                    isRight = !isRight;
                }
                
                

            }
            if (m_CurStep == m_UserStep && m_UserStep < Global.gApp.gGameData.PassStep.Count - 1)
            {
                LevelDetail_step nextStepUI = Instantiate<LevelDetail_step>(step.step);
                nextStepUI.StepName.gameObject.SetActive(true);
                m_ScrolViewList[m_ScrolViewHeightList.Count - 1] = stepUI;
                nextStepUI.transform.localPosition = new Vector3(Viewport.rectTransform.rect.size.x / 2, m_ScrolViewHeightList[m_ScrolViewHeightList.Count - 1], nextStepUI.transform.localPosition.z);
                nextStepUI.transform.SetParent(step.step.transform.parent, false);
                PassItem nextPassItemCfg = Global.gApp.gGameData.PassData.Get(Global.gApp.gGameData.PassStep[m_CurStep + 1]);
                string nextBgIndex = System.Text.RegularExpressions.Regex.Replace(nextPassItemCfg.mainUIbg, @"[^0-9]+", "");
                nextStepUI.StepIcon.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.RESOURCE_LEVEL_DETAIL_ICON, nextBgIndex), typeof(Sprite)) as Sprite;

                nextStepUI.gameObject.SetActive(true);
                nextStepUI.StepIcon.image.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);

                string nextEffctUri;
                if (m_IconEffectDtc.TryGetValue(nextBgIndex, out nextEffctUri))
                {
                    GameObject effect = UiTools.GetEffect(nextEffctUri, nextStepUI.transform, 45);
                }
            }

            isRight = false;
            //显示顶部标识
            //LevelDetail_boss bossUI = Instantiate<LevelDetail_boss>(boss.boss);
            //m_ScrolViewList.Add(bossUI);
            //m_ScrolViewHeightList.Add(-m_Height - boss.rectTransform.sizeDelta.y / 2);
            //bossUI.transform.SetParent(boss.boss.transform.parent, false);
            //m_Height += boss.rectTransform.sizeDelta.y;
            //bossUI.gameObject.SetActive(true);



        }

        private void MakeNormalProgress(int id, int curId, LevelDetail_progress curProgressUI)
        {

            curProgressUI.transform.SetParent(progress.progress.transform.parent, false);
            curProgressUI.gameObject.SetActive(true);
            
            PassItem passItem = Global.gApp.gGameData.PassData.Get(m_InitId + id);

            if (isRight)
            {
                curProgressUI.RightGiftBg.gameObject.SetActive(isRight);
                m_BgIndex++;
            }
            else
            {
                if (id == curId)
                {
                    curProgressUI.mask.image.fillAmount = 0.5f;
                }
                else if (id < curId)
                {
                    curProgressUI.mask.image.fillAmount = 1f;
                }
                else
                {
                    curProgressUI.mask.image.fillAmount = 0f;
                }
                curProgressUI.LeftGiftBg.gameObject.SetActive(!isRight);
                m_BgIndex++;
            }
            if (isRight)
            {
                curProgressUI.RightRetry.button.onClick.AddListener(() => { OnStartGame(m_InitId + id); });
                curProgressUI.RightRetry.gameObject.SetActive(m_InitId + id < Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId());
            } else
            {
                curProgressUI.LeftRetry.button.onClick.AddListener(() => { OnStartGame(m_InitId + id); });
                curProgressUI.LeftRetry.gameObject.SetActive(m_InitId + id < Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId());
            }
            Text text = isRight ? curProgressUI.RightNum.text : curProgressUI.LeftNum.text;
            text.text = id.ToString();
            Image giftImageBg = isRight ? curProgressUI.RightBgImg.image : curProgressUI.LeftBgImg.image;
            giftImageBg.rectTransform.Rotate(new Vector3(0, 180, 0));
            int ri = (m_BgIndex % 3) + 1;
            giftImageBg.sprite = Resources.Load(string.Format(CommonResourceConstVal.LEVEL_DETAIL_BG_PATH, passItem.dtType, ri), typeof(Sprite)) as Sprite;

            if (passItem.dtType == 4 && ri == 2)
            {
                string effctUri;
                if (m_DtEffectDtc.TryGetValue(passItem.dtType.ToString(), out effctUri))
                {
                    GameObject effect = UiTools.GetEffect(effctUri, giftImageBg.transform, 45);
                }
            } else if (passItem.dtType != 4)
            {
                if (m_EffectIndex == m_EffectShowIndex)
                {
                    string effctUri;
                    if (m_DtEffectDtc.TryGetValue(passItem.dtType.ToString(), out effctUri))
                    {
                        GameObject effect = UiTools.GetEffect(effctUri, giftImageBg.transform, 45);
                    }
                    m_EffectShowIndex = m_EffectIndex + UnityEngine.Random.Range(2, 4);
                }
                m_EffectIndex++;
            }


            Image giftImage = isRight ? curProgressUI.RightGift.image : curProgressUI.LeftGift.image;
            Image openedImage = isRight ? curProgressUI.RightOpened.image : curProgressUI.LeftOpened.image;
            openedImage.gameObject.SetActive(false);
            Image awardBgImage = isRight ? curProgressUI.RightAwardBg.image : curProgressUI.LeftAwardBg.image;
            awardBgImage.gameObject.SetActive(false);
            if (id >= curId)
            {
                giftImageBg.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
                giftImage.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
                openedImage.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
                awardBgImage.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
            }
            RectTransform levelBg = isRight ? curProgressUI.RightLevel.rectTransform : curProgressUI.LeftLevel.rectTransform;
            SetPassIcon(levelBg, passItem, m_InitId + curId);
        }

        private void MakeProgressItem(QuestItem itemCfg, int curId, LevelDetail_progress progressUI)
        {
            float thisId = itemCfg.condition[1];

            progressUI.transform.SetParent(progress.progress.transform.parent, false);
            progressUI.gameObject.SetActive(true);

            
            PassItem passItem = Global.gApp.gGameData.PassData.Get(m_InitId + (int)itemCfg.condition[1]);
            ItemItem itemItemCfg = Global.gApp.gGameData.ItemData.Get((int)itemCfg.award[0]);
            if (isRight)
            {
                progressUI.RightGiftBg.gameObject.SetActive(isRight);
                progressUI.RightGift.gameObject.SetActive(isRight);

            } else
            {
                if (itemCfg.condition[1] == curId)
                {
                    progressUI.mask.image.fillAmount = 0.5f;
                }
                else if (itemCfg.condition[1] < curId)
                {
                    progressUI.mask.image.fillAmount = 1f;
                }
                else
                {
                    progressUI.mask.image.fillAmount = 0f;
                }
                progressUI.LeftGiftBg.gameObject.SetActive(!isRight);
                progressUI.LeftGift.gameObject.SetActive(!isRight);
            }
            if (isRight)
            {
                progressUI.RightRetry.button.onClick.AddListener(() => { OnStartGame(passItem.id); });
                progressUI.RightRetry.gameObject.SetActive(passItem.id < Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId());
            }
            else
            {
                progressUI.LeftRetry.button.onClick.AddListener(() => { OnStartGame(passItem.id); });
                progressUI.LeftRetry.gameObject.SetActive(passItem.id < Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId());
            }
            QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(itemCfg.quest_id);
            Image giftImageBg = isRight ? progressUI.RightBgImg.image : progressUI.LeftBgImg.image;
            giftImageBg.rectTransform.Rotate(new Vector3(0, 180, 0));
            giftImageBg.sprite = Resources.Load(string.Format(CommonResourceConstVal.LEVEL_DETAIL_BG_PATH, passItem.dtType, 0), typeof(Sprite)) as Sprite;

            if (passItem.dtType != 4)
            {
                if (m_EffectIndex == m_EffectShowIndex)
                {
                    string effctUri;
                    if (m_DtEffectDtc.TryGetValue(passItem.dtType.ToString(), out effctUri))
                    {
                        GameObject effect = UiTools.GetEffect(effctUri, giftImageBg.transform, 45);
                    }
                    m_EffectShowIndex = m_EffectIndex + UnityEngine.Random.Range(2, 4);
                }
                m_EffectIndex++;
            }
            

            Image giftImage = isRight ? progressUI.RightGift.image : progressUI.LeftGift.image;
            giftImage.sprite = Resources.Load(itemItemCfg.closeBoxImg, typeof(Sprite)) as Sprite;
            Image openedImage = isRight ? progressUI.RightOpened.image : progressUI.LeftOpened.image;
            Image awardBgImage = isRight ? progressUI.RightAwardBg.image : progressUI.LeftAwardBg.image;
            if (questItemDTO.state == QuestStateConstVal.RECEIVED)
            {
                openedImage.gameObject.SetActive(true);
            } else
            {
                openedImage.gameObject.SetActive(false);
            }
            Text text = isRight ? progressUI.RightNum.text : progressUI.LeftNum.text;
            text.text = ((int)itemCfg.condition[1]).ToString();
            if (thisId >= curId)
            {
                giftImageBg.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
                giftImage.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
                openedImage.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
                awardBgImage.color = ColorUtil.GetTextColor(false, Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GREY_COLOR).content);
            }
            RectTransform levelBg = isRight ? progressUI.RightLevel.rectTransform : progressUI.LeftLevel.rectTransform;
            SetPassIcon(levelBg, passItem, m_InitId + curId);
        }

        private void SetPassIcon(RectTransform passUI, PassItem passItem, int curPassId)
        {


            bool isBoss = passItem.bossLevel > 0;
            passUI.GetChild(0).gameObject.SetActive(!isBoss && passItem.id < curPassId);
            passUI.GetChild(1).gameObject.SetActive(!isBoss && passItem.id == curPassId);
            passUI.GetChild(2).gameObject.SetActive(!isBoss && passItem.id > curPassId);

            passUI.GetChild(3).gameObject.SetActive(isBoss && passItem.id < curPassId);
            passUI.GetChild(4).gameObject.SetActive(isBoss && passItem.id == curPassId);
            passUI.GetChild(5).gameObject.SetActive(isBoss && passItem.id > curPassId);

        }

        private void OnStartGame(int passId)
        {
            Debug.Log("OnStartGame passId = " + passId);
            if (passId > Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId())
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByID, 1004);
                return;
            }
            PassItem newPassItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItemById(passId);
            if (newPassItem != null)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.ConfirmEnterPassUI, newPassItem);
                return;
            }
        }


        private void OnCloseBtn()
        {
            OnClick();
            Global.gApp.gUiMgr.ClosePanel(Wndid.LevelDetail);
            //Global.gApp.gUiMgr.OpenPanel(Wndid.MainPanel); 
            //Global.gApp.gUiMgr.OpenPanel(Wndid.CommonPanel);
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

