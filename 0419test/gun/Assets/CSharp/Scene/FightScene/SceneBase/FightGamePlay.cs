
using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public enum GamePlayType
    {
        ActiveProps = 1,
        TriggerCollider = 2,
        KillAppointMonster = 3,
        KeepLive = 4,
        KillAppointWave = 5,
    }
    class GamePlayData
    {
        public GamePlayType GamePlayType;
        public int TriggerId;
        public int TriggerTimes;
        public int OriTriggerTimes;
        public int RecorcTime = -100;
        public bool IngoreAnim = false;
    }
    public class FightGamePlay
    {
        private FightScene m_FightScene;
        private PassItem m_PassData;
        private List<GamePlayData> m_TriggerEvent = new List<GamePlayData>();
        private GameObject m_MapGo;
        private Transform m_Player;
        private Action<float,bool> m_Action;
        public void Init(PassItem passData, FightScene scene,GameObject mapGo,Transform player)
        {
            m_Player = player;
            m_PassData = passData;
            m_FightScene = scene;
            m_MapGo = mapGo;
            m_Action = TimeCall;
            //if (m_PassData.gamePlay.Length > 0)
            //{

            //    GeneratePlayInfo();
            //    RegisterListener();
            //    scene.GetTimerMgr().AddTimer(0, 0,m_Action);
            //}else
            //{
            //    Global.gApp.gMsgDispatcher.Broadcast<float,string>(MsgIds.PointArrowAngle, -100,GameConstVal.EmepyStr);
            //}
        }
        public void TimeCall(float dt, bool isEnd)
        {
            //foreach (GamePlayData data in m_TriggerEvent)
            //{
            //    if (data.TriggerTimes > 0)
            //    {
            //        float length = 1000000;
            //        if (data.GamePlayType == GamePlayType.ActiveProps)
            //        {
            //            BaseActiveRange[] baseActiveRanges = m_MapGo.GetComponentsInChildren<BaseActiveRange>();
            //            foreach (BaseActiveRange triggerEvent in baseActiveRanges)
            //            {
            //                if (triggerEvent.GetPropId() == data.TriggerId)
            //                {
            //                    if (!triggerEvent.GetActive() && triggerEvent.GetCanActive())
            //                    {
            //                        if (triggerEvent.InCameraView)
            //                        {
            //                            Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr);
            //                            return;
            //                        }
            //                        Vector3 pointVec = triggerEvent.transform.position - m_Player.position;
            //                        float newLength = pointVec.magnitude;
            //                        if(newLength < length)
            //                        {
            //                            length = newLength;
            //                            float angle = EZMath.SignedAngleBetween(pointVec, Vector3.up);
            //                            Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, angle, (int)length + "m");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else if (data.GamePlayType == GamePlayType.TriggerCollider)
            //        {
            //            TriggerEvent[] triggerEvents = m_MapGo.GetComponentsInChildren<TriggerEvent>();
            //            foreach(TriggerEvent triggerEvent in triggerEvents)
            //            {
            //                if(triggerEvent.GetTriggerId() == data.TriggerId && !triggerEvent.GetHasTriggered())
            //                {
                              
            //                    if (!triggerEvent.InCameraView)
            //                    {
            //                        Vector3 pointVec = triggerEvent.transform.position - m_Player.position;
            //                        float newLength = pointVec.magnitude;
            //                        if (newLength < length)
            //                        {
            //                            length = newLength;
            //                            float angle = EZMath.SignedAngleBetween(pointVec, Vector3.up);
            //                            Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, angle, (int)pointVec.magnitude + "m");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr);
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //        else if (data.GamePlayType == GamePlayType.KillAppointMonster)
            //        {
            //            Monster[] baseActiveRanges = Global.gApp.gRoleNode.GetComponentsInChildren<Monster>();
            //            foreach (Monster monster in baseActiveRanges)
            //            {
            //                if (monster.GetMonsterId() == data.TriggerId)
            //                {
            //                    if (!monster.InCameraView)
            //                    {
            //                        Vector3 pointVec = monster.transform.position - m_Player.position;
            //                        float newLength = pointVec.magnitude;
            //                        if (newLength < length)
            //                        {
            //                            length = newLength;
            //                            float angle = EZMath.SignedAngleBetween(pointVec, Vector3.up);
            //                            Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, angle, (int)pointVec.magnitude + "m");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr);
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //        else if(data.GamePlayType == GamePlayType.KeepLive || data.GamePlayType == GamePlayType.KillAppointWave)
            //        {
            //            Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr);
            //            return;
            //        }
            //        if (length == 1000000)
            //        {
            //            Global.gApp.gMsgDispatcher.Broadcast<float, string>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr);
            //        }
            //        break;
            //    }
            //}
        }
        private void GeneratePlayInfo()
        {
            //int[] gamePlay = m_PassData.gamePlay;
            //int length = gamePlay.Length;
            //int index = 0;
            //int step = 3;
            //while (index < length)
            //{
            //    GamePlayData data = new GamePlayData();
            //    data.GamePlayType = (GamePlayType)gamePlay[index];
            //    data.TriggerId = gamePlay[index + 1];
            //    data.TriggerTimes = gamePlay[index + 2];
            //    data.OriTriggerTimes = gamePlay[index + 2];
            //    m_TriggerEvent.Add(data);
            //    index += step;
            //}
        }

        private void ActiveProp(int propId)
        {
            foreach (GamePlayData data in m_TriggerEvent)
            {
                if (data.GamePlayType == GamePlayType.ActiveProps && data.TriggerId == propId)
                {
                    data.TriggerTimes--;
                    if (data.TriggerTimes >= 0)
                    {
                        BroadMsg();
                        if(data.TriggerTimes == 0)
                        {
                            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ActiveProp, ActiveProp);
                        }
                    }
                }
            }
        }

        private void MonsterDead(int guid, int monsterId, Monster monster)
        {
            foreach (GamePlayData data in m_TriggerEvent)
            {
                if (data.GamePlayType == GamePlayType.KillAppointMonster && data.TriggerId == monsterId)
                {
                    data.TriggerTimes--;
                    if (data.TriggerTimes >= 0)
                    {
                        BroadMsg();
                        if (data.TriggerTimes == 0)
                        {
                            Global.gApp.gMsgDispatcher.RemoveListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
                        }
                    }
                }
            }
        }
        private void WaveEnd(int guid, int waveId)
        {
            foreach (GamePlayData data in m_TriggerEvent)
            {
                if (data.GamePlayType == GamePlayType.KillAppointWave && data.TriggerId == waveId)
                {
                    data.TriggerTimes--;
                    if (data.TriggerTimes >= 0)
                    {
                        BroadMsg();
                        if (data.TriggerTimes == 0)
                        {
                            Global.gApp.gMsgDispatcher.RemoveListener<int, int>(MsgIds.WaveEnded, WaveEnd);
                        }
                    }
                }
            }
        }
        private void TriggerCollider(int triggerId, Transform transform)
        {
            foreach (GamePlayData data in m_TriggerEvent)
            {
                if (data.GamePlayType == GamePlayType.TriggerCollider && data.TriggerId == triggerId)
                {
                    data.TriggerTimes--;
                    if (data.TriggerTimes == 0)
                    {
                        BroadMsg();
                        if (data.TriggerTimes == 0)
                        {
                            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
                        }
                    }
                }
            }
        }
        private void KeepTime(int time)
        {
            foreach (GamePlayData data in m_TriggerEvent)
            {
                if (data.GamePlayType == GamePlayType.KeepLive)
                {
                    if(data.RecorcTime < 0)
                    {
                        data.RecorcTime = time - 1;
                    }
                    data.TriggerTimes = (time - data.RecorcTime);
                    BroadMsg();
                    data.IngoreAnim = true;
                    if (data.TriggerTimes == data.OriTriggerTimes)
                    {
                        data.TriggerTimes = 0;
                        BroadMsg();
                        Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TimeCoolDown, KeepTime);
                    }
                }
            }
        }
        public void BroadMsg()
        {
            //int index = -1;
            //foreach (GamePlayData data in m_TriggerEvent)
            //{
            //    index++;
            //    if (data.TriggerTimes > 0)
            //    {
            //        string tips = string.Format(m_PassData.gamePlayTips[index], data.OriTriggerTimes - data.TriggerTimes,data.OriTriggerTimes);
            //        Global.gApp.gMsgDispatcher.Broadcast<string,bool>(MsgIds.FightUiTopTips, tips, data.IngoreAnim);
            //        return;
            //    }
            //}
            //index++;
            //if (m_PassData.gamePlayTips.Length > index)
            //{
            //    string tips = string.Format(m_PassData.gamePlayTips[index]);
            //    Global.gApp.gMsgDispatcher.Broadcast<string, bool>(MsgIds.FightUiTopTips, tips, false);
            //}
            //else
            //{
            //    Global.gApp.gMsgDispatcher.Broadcast<string, bool>(MsgIds.FightUiTopTips, GameConstVal.EmepyStr, false);
            //}
        }
     
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, int,Monster>(MsgIds.MonsterDead, MonsterDead);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ActiveProp, ActiveProp);
            Global.gApp.gMsgDispatcher.AddListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TimeCoolDown, KeepTime);
            Global.gApp.gMsgDispatcher.AddListener<int, int>(MsgIds.WaveEnded, WaveEnd);
        }

        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, int,Monster>(MsgIds.MonsterDead, MonsterDead);
            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ActiveProp, ActiveProp);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TimeCoolDown, KeepTime);
            Global.gApp.gMsgDispatcher.RemoveListener<int, int>(MsgIds.WaveEnded, WaveEnd);
        }
        public void Destroy()
        {
            UnRegisterListener();
        }
    }
}
