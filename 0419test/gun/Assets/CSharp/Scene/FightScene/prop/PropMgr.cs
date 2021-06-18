
using EZ.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public class PropMgr
    {
        private string[] m_PropPath =
        {
            "Prefabs/Prop/CarProp",
            "Prefabs/Prop/CureProp",
            "Prefabs/Prop/MedicalKitProp",
            "Prefabs/Prop/RPGProp",
            "Prefabs/Prop/GoldProp",
            "Prefabs/Prop/BuffProp",
        };
        private List<GameObject> m_Props;
        private List<GoldProp> m_CacheGoldProps;
        private AudioClip m_AudioClip;
        private Player m_Player;
        private bool m_CanDropCampRes = true;
        private bool m_CanDropCampNpc = true;
        public PropMgr()
        {
            m_CanDropCampRes = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(3);
            m_CanDropCampNpc = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(4);
            m_Player = Global.gApp.CurScene.GetMainPlayerComp();
            m_AudioClip = Global.gApp.gResMgr.LoadAssets<AudioClip>("Sounds/gold");
            m_Props = new List<GameObject>();
            m_CacheGoldProps = new List<GoldProp>();
            CacheProp();
        }
        private void CacheProp()
        {
            SceneType sceneType = Global.gApp.CurScene.GetSceneType();
            int cacheNum = 150;
            Transform bulletTsf = Global.gApp.gBulletNode.transform;
            for (int i = 0; i< cacheNum;i++)
            {
                GameObject propGo = Global.gApp.gResMgr.InstantiateObj(m_PropPath[4]);
                propGo.transform.position = new Vector3(1000, 0, 0);
                propGo.transform.SetParent(bulletTsf);
                GoldProp gold = propGo.GetComponent<GoldProp>();
                gold.Stop();
                m_CacheGoldProps.Add(gold);
            }
        }
        
        public void RemoveGoldProp(GoldProp prop)
        {
            prop.transform.position = new Vector3(1000, 0, 0);
            prop.enabled = false;
            m_CacheGoldProps.Add(prop);
        }
        public void RemoveProp(GameObject prop)
        {
            Object.Destroy(prop);
            m_Props.Remove(prop);
        }
        public void AddGold(Vector3 position, int count)
        {
            Global.gApp.gAudioSource.PlayOneShot(m_AudioClip);
            if (count <= 0) { return; }
            float expect = count / 15;

            for (int i = 1; i <= count; i++)
            {
                if (m_CacheGoldProps.Count > 0)
                {
                    Vector3 offsetPos = new Vector3(Random.Range(-expect, expect), Random.Range(-expect, expect), Random.Range(0, 2));
                    GoldProp gold = gold = m_CacheGoldProps[0];
                    gold.Init();
                    gold.transform.position = position + offsetPos;
                    m_CacheGoldProps.RemoveAt(0);
                }
                else
                {
                    m_Player.GetPlayerData().AddGold(1.0f);
                }
            }
        }
        public GameObject AddAppointProp(Vector3 position, string dropName)
        {
            string path = "Prefabs/Prop/" + dropName;
            GameObject propGo = Global.gApp.gResMgr.InstantiateObj(path);
            propGo.transform.position = position;
            m_Props.Add(propGo);
            return propGo;
        }
        public void AddWpnChipPropByPass(Vector3 position, int dropId)
        {
            DropItem dropData = Global.gApp.gGameData.DropData.Get(dropId);
            if (dropData != null)
            {
                int curRate = 0;
                int randomRate = Random.Range(0, 10001);
                int index = 1;
                foreach (int rate in dropData.rate)
                {
                    curRate = curRate + rate;
                    if (randomRate < curRate)
                    {
                        if (CheckCanAddProp(dropData.prop[0]))
                        {
                            int dropCount = int.Parse(dropData.prop[index]);
                            if (dropCount > 0)
                            {
                                string path = "Prefabs/Prop/" + dropData.prop[0];
                                GameObject propGo = Global.gApp.gResMgr.InstantiateObj(path);
                                propGo.GetComponent<WeaponChipProp>().SetChipGainTypeTrigger(dropCount);
                                propGo.transform.position = position;
                                m_Props.Add(propGo);
                            }
                        }
                        break;
                    }
                    index++;
                }
            }
        }
        public void AddProp(Vector3 position, int[] dropIds,PassItem passItem)
        {
            if(dropIds != null && dropIds.Length > 0)
            {
                foreach(int dropId in dropIds)
                {
                    float ex = GetExRate(dropId, passItem.dropID, passItem.dropRate);
                    bool isDropSucess = AddProp(position, dropId, ex);
                    if (isDropSucess)
                    {
                        return;
                    }
                }
            }
        }
        private bool CheckCanAddProp(string propName)
        {
            PassItem curPass = Global.gApp.gSystemMgr.GetPassMgr().GetPassItem();
            PassItem fightPass = Global.gApp.CurScene.GetPassData();
            // 非当前关卡 
            if (curPass.id != fightPass.id || Global.gApp.gSystemMgr.GetPassMgr().GetHasPassedMaxPass())
            {
                if (propName.Contains("Camp_Npc_"))
                {
                    if (m_CanDropCampNpc)
                    {
                        int newRate = UnityEngine.Random.Range(0, 10001);
                        string rateStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.REPASS_NPCFALL).content;
                        int rateInt = int.Parse(rateStr);
                        return rateInt >= newRate;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(propName.Contains("Camp_"))
                {
                    if (m_CanDropCampRes)
                    {
                        int newRate = UnityEngine.Random.Range(0, 10001);
                        string rateStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.REPASS_SOURCEFALL).content;
                        int rateInt = int.Parse(rateStr);
                        return rateInt >= newRate;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (propName.Contains("WeaponChipProp"))
                {
                    int newRate = UnityEngine.Random.Range(0, 10001);
                    string rateStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.REPASS_WEAPONFALL).content;
                    int rateInt = int.Parse(rateStr);
                    return rateInt >= newRate;
                }
            }
            else
            {
                if (propName.Contains("Camp_Npc_"))
                {
                    return m_CanDropCampNpc;
                }
                else if (propName.Contains("Camp_"))
                {
                    return m_CanDropCampRes;
                }
            }
            return true;
        }
        private float GetExRate(int targetDropId,int[] dropIds,float[] dropRates)
        {
            if (dropIds != null && dropIds.Length > 0 && dropRates != null && dropRates.Length > 0)
            {
                int index = 0;
                foreach (int dropId in dropIds)
                {
                    if (targetDropId == dropId)
                    {
                        if(index < dropRates.Length)
                        {
                            return dropRates[index];
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    index++;
                }
            }
            return 1;
        }
        public bool AddProp(Vector3 position, int dropId,float ex = 1)
        {
            //DropData dropData = Global.gApp.gGameData.GetDropData().Find(l => l.id == dropId);
            DropItem dropData = Global.gApp.gGameData.DropData.Get(dropId);
            if (dropData!= null)
            {
                int curRate = 0;
                int randomRate = Random.Range(0, 10001);
                int index = 0;
                foreach (int rate in dropData.rate)
                {
                    curRate = curRate + (int)(rate * ex);
                    if (randomRate < curRate)
                    {
                        if (CheckCanAddProp(dropData.prop[index]))
                        {
                            string path = "Prefabs/Prop/" + dropData.prop[index];
                            GameObject propGo = Global.gApp.gResMgr.InstantiateObj(path);
                            propGo.transform.position = position;
                            m_Props.Add(propGo);
                        }
                        return true;
                    }
                    index++;
                }
            }
            return false;
        }
        public void Destroy()
        {
            foreach (GameObject prop in m_Props)
            {
                Object.DestroyImmediate(prop);
            }
            m_Props.Clear();
        }
    }
}

