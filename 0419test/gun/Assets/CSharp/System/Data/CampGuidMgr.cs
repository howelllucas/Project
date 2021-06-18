using UnityEngine;
using System.Collections;
using System;
using EZ.Data;
using System.Collections.Generic;

namespace EZ.DataMgr
{

    public class CampGuidMgr : BaseDataMgr<CampGuidDTO>
    {
        private Vector3[] m_PosDst = new Vector3[]
        {
            new Vector3(0,2.46f,0),
            new Vector3(-4.17f,-15.4f,0),
            new Vector3(4.51f,-15.42f,0),
            new Vector3(-4.41f,-24.88f,0),
        };

        private Vector3[] m_CampWallDst = new Vector3[]
        {
            new Vector3(-12.8f,14.8f,0),
            new Vector3(-2.4f,15.6f,0),
            new Vector3(4.75f,18,0),
            new Vector3(14.2f,16,0),
            new Vector3(14.34f,5,0),
            new Vector3(15.2f,-2.45f,0),
            new Vector3(13.48f,-24.2f,0),
            new Vector3(11.27f,-36.56f,0),
            new Vector3(-11.4f,-35,0),
            new Vector3(-12.5f,-21.8f,0),
            new Vector3(-12.8f,-8f,0),
            new Vector3(-14f,0,0),
        };
        public CampGuidMgr()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            Init("campGuid");
            if (m_Data == null)
            {
                m_Data = new CampGuidDTO();
            }
        }

        public void AfterInit()
        {
            int step = 0;
            if (m_Data.curStepFinished)
            {
                foreach (CampStepItem config in Global.gApp.gGameData.CampStepConfig.items)
                {

                    if (step <= m_Data.curStep && config.brageId > 0)
                    {
                        if (GameItemFactory.GetInstance().GetItem(config.brageId) == 0)
                        {
                            ItemDTO itemDTO = new ItemDTO(config.brageId, 1, BehaviorTypeConstVal.OPT_CAMP_GENBADGE);
                            GameItemFactory.GetInstance().AddItem(itemDTO);
                        }
                    }
                    step++;
                }
            }
            else
            {
                foreach (CampStepItem config in Global.gApp.gGameData.CampStepConfig.items)
                {
                    if (step < m_Data.curStep && config.brageId > 0)
                    {
                        if (GameItemFactory.GetInstance().GetItem(config.brageId) == 0)
                        {
                            ItemDTO itemDTO = new ItemDTO(config.brageId, 1, BehaviorTypeConstVal.OPT_CAMP_GENBADGE);
                            GameItemFactory.GetInstance().AddItem(itemDTO);
                        }
                    }
                    step++;
                }
            }
        }
        public Vector3[] GetBuidWallPoss()
        {
            return m_CampWallDst;
        }
        public Vector3[] GetBuidPoss()
        {
            return m_PosDst;
        }
        public int GetCurGuidStep()
        {
            if (m_Data.curStepFinished)
            {
                int curStep = m_Data.curStep;
                // find Next 
                CampStepItem[] campStepItems = Global.gApp.gGameData.CampStepConfig.items;
                for (int i = curStep + 1; i < campStepItems.Length; i++)
                {
                    if (FilterFactory.GetInstance().Filter(campStepItems[i].condition))
                    {
                        // step7 特殊处理 需要检测小男孩的可见性
                        if (i == 6)
                        {
                            if(!Global.gApp.gSystemMgr.GetNpcMgr().GetShowBoyNpcState())
                            {
                                return -1;
                            }
                        }
                        m_Data.curStep = i;
                        m_Data.curStepFinished = false;
                        SaveData();
                        return m_Data.curStep;

                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                return m_Data.curStep;
            }
            return -1;
        }
        // step  索引从 0 开始 0- 1；1 - 2步骤
        public bool GetStepFinished(int step)
        {
            if (m_Data.curStep > step)
            {
                return true;
            }
            else if (m_Data.curStep == step)
            {
                return m_Data.curStepFinished;
            }
            else
            {
                return false;
            }
        }
        public void StartCurGuidStep()
        {
            // 如果当前步骤完成了。那就找下一个
            if (m_Data.curStepFinished)
            {
                int curStep = m_Data.curStep;
                // find Next 
                CampStepItem[] campStepItems = Global.gApp.gGameData.CampStepConfig.items;
                for (int i = curStep + 1; i < campStepItems.Length; i++)
                {
                    if (FilterFactory.GetInstance().Filter(campStepItems[i].condition))
                    {
                        m_Data.curStep = i;
                        m_Data.curStepFinished = false;
                        SaveData();
                        StartCurGuidImp(m_Data.curStep);
                    }
                }
            }
            else
            {
                StartCurGuidImp(m_Data.curStep);
            }
        }
        public void StepGuidEnd()
        {
            m_Data.curStepFinished = true;
            SaveData();

            Global.gApp.gSystemMgr.GetNpcMgr().CampStepEnd(m_Data.curStep);
        }

        public void ResetGuid()
        {
            m_Data.curStepFinished = true;
            m_Data.curStep = -1;
            SaveData();
        }
        private void StartCurGuidImp(int step)
        {
            if (step < 0)
            {
                return;
            }
            BaseUi campsiteUI = Global.gApp.gUiMgr.GetPanelCompent(Wndid.CampsiteUI);
            CampNewGuidBase campNewGuid = null;
            if (step == 0)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidOne>();
            }
            else if (step == 1)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidTwo>();
            }
            else if (step == 2)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidThree>();
            }
            else if (step == 3)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidFour>();
            }
            else if (step == 4)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidFive>();
            }
            else if(step == 5)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidSix>();
            }
            else if (step == 6)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidSeven>();
            }
            else if(step == 7)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidEight>();
            }
            else if(step == 8)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidNine>();
            }
            else if(step > 8 && step < 15)
            {
                campNewGuid = campsiteUI.gameObject.AddComponent<CampNewGuidTenToFifteen>();
            }
            campNewGuid.StartCampStep(step);
        }
    }
}
