using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Game.Util;
using System.IO;

namespace Game.Data
{
    public class ModelsDataKey
    {
        public static readonly string VERSION = "__version";            // 存档版本号
        public static readonly string LAST_TIMESTAMP = "__last";        // 上次存档日期
        public static readonly string SAVE_FILE_ID = "__fileId";        // 存档文件唯一id

        public static readonly string UID = "uid";                      // 用户唯一id
        public static readonly string USER_INFO = "user";               // 用户信息
        public static readonly string DATA = "_data";                   // 通用数据
    }

    public class Models : ModelDataBase
    {
        public static readonly int Version = 0;

        public Models()
        {
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [ModelsDataKey.LAST_TIMESTAMP] = PTUtil.GetTimeStamp(),
                [ModelsDataKey.UID] = strUid,
                [ModelsDataKey.USER_INFO] = playerData.GetJsonData(),
                [ModelsDataKey.VERSION] = strVersion,
                [ModelsDataKey.SAVE_FILE_ID] = strSaveFileId,
            };

            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {
            this.ResetData();

            if (data.Keys.Contains(ModelsDataKey.DATA))
            {
                var subJson = data[ModelsDataKey.DATA];
                foreach (string k2 in subJson.Keys)
                {
                    dicData[k2] = (string)subJson[k2];
                }
            }


            foreach (string key in data.Keys)
            {
                if (key == ModelsDataKey.UID)
                {
                    strUid = (string)data[key];
                }
                else if (key == ModelsDataKey.LAST_TIMESTAMP)
                {
                    nLastTimestamp = (long)data[key];
                }
                else if (key == ModelsDataKey.USER_INFO)
                {
                    PlayerData.InitWithJson(data[key]);
                }
                else if (key == ModelsDataKey.VERSION)
                {
                    strVersion = (string)data[key];
                }
                else if (key == ModelsDataKey.SAVE_FILE_ID)
                {
                    strSaveFileId = (string)data[key];
                }
            }

            return true;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ResetData()
        {
            strUid = "1000";
            nLastTimestamp = -1;
            //strVersion = "0";
            playerData = new PlayerData();
        }

        public string GetUid()
        {
            return this.strUid;
        }

        public void SetUid(string uid)
        {
            this.strUid = uid;
            bDirty = true;
        }

        /// <summary>
        /// 是否是新存档,(第一次进入游戏)
        /// </summary>
        /// <returns></returns>
        public bool GetIsNew()
        {
            return !(nLastTimestamp > 0);
        }

        /// <summary>
        /// 存档文件的uid, 一个存档文件对应一个uid, 具有唯一性
        /// </summary>
        /// <returns>The save file identifier.</returns>
        public string GetSaveFileId()
        {
            return strSaveFileId;
        }

        public void SetSaveFileId(string str)
        {
            strSaveFileId = str;
        }

        /// <summary>
        /// 上次游戏时间. -1:表示之前没有玩过
        /// </summary>
        /// <returns>The last timestamp.</returns>
        public long GetLastTimestamp()
        {
            return this.nLastTimestamp;
        }


        /// <summary>
        /// 首次进入游戏时间
        /// </summary>
        /// <returns>The first timestamp.</returns>
        public long GetFirstTimestamp()
        {
            if (nFirstTimestamp < 1)
            {
                var vec = strSaveFileId.Split('-');
                nFirstTimestamp = Convert.ToInt64(vec[0], 16) / 1000;
            }
            return nFirstTimestamp;
        }

        public int GetVersion()
        {
            return int.Parse(strVersion);
        }

        public void UpdateVersion()
        {
            strVersion = Version.ToString();
        }

        /// <summary>
        /// 获取通用数据, 不存在返回 null
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="key">Key.</param>
        public string GetData(string key)
        {
            if (dicData.TryGetValue(key, out string val))
            {
                return val;
            }
            return null;
        }

        /// <summary>
        /// 存储通用数据, 会覆盖已存在的值
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="val">Value.</param>
        public void SetData(string key, string val)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
            {
                return;
            }
            dicData[key] = val;
            bDirty = true;
        }

        /// <summary>
        /// 移除存储的通用数据
        /// </summary>
        /// <returns><c>true</c>, if data was removed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool RemoveData(string key)
        {
            if (dicData.ContainsKey(key))
            {
                bool flag = dicData.Remove(key);
                bDirty = true;
                return flag;
            }
            return false;
        }

        /// <summary>
        /// 用户数据
        /// </summary>
        /// <value>The player data.</value>
        public PlayerData PlayerData { get { return playerData; } }

        static Models _instance;
        private static System.Object _objLock = new System.Object();
        public static Models Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        _instance = new Models();
                    }
                }
                return _instance;
            }
        }



        // 上次游戏时间戳 , 退出游戏时存入存档文件. 如果这个值小于1. 表示之前没有玩过
        protected long nLastTimestamp = -1;

        // 存档版本
        protected string strVersion = "0";

        // 唯一id
        protected string strUid = "1000";

        // 用户信息.
        protected PlayerData playerData;

        // 存档文件uid
        protected string strSaveFileId = "";

        // 首次进入游戏时间.
        protected long nFirstTimestamp = -1;



        // 通用存档数据
        protected Dictionary<string, string> dicData = new Dictionary<string, string>();


        //--------------------------------------游戏存档 begin--------------------------
        #region 游戏存档
        private void InitDB()
        {
            if (!LoadSave())
            {
                PlayerDataMgr.singleton.InitDefaultPlayerData();
                SaveToFile(true);
            }
        }

        public void Init()
        {
            playerData = new PlayerData();

            InitDB();
        }

        private static readonly string SAVE_FILE_NAME = "/op.d0";
        private static readonly string SAVE_FILE_NAME_BAK = "/op.d1";
        private static readonly bool SAVE_ENCODE = true;

        protected string GetSaveFilePath()
        {
            // Mac /Users/wangning/Library/Application Support/cmplay/PT4
            return Application.persistentDataPath + SAVE_FILE_NAME;
        }

        protected string GetBakSaveFilePath()
        {
            return Application.persistentDataPath + SAVE_FILE_NAME_BAK;
        }

        public string GetSendToServerData()
        {
            var jsonData = Models.Instance.GetJsonData();
            return PTUtil.GetJsonSaveStr(jsonData, SAVE_ENCODE);
        }

        public void SyncServerJson(string serverData)
        {
            var jsonData = PTUtil.LoadJsonFromSaveStr(serverData, SAVE_ENCODE);
            Models.Instance.InitWithJson(jsonData);
        }

        /// <summary>
        /// 写存档到本地json文件
        /// </summary>
        /// <returns><c>true</c>, if to file was saved, <c>false</c> otherwise.</returns>
        /// force: 是否强制保存.(在model没有改变情况下)
        public bool SaveToFile(bool force)
        {
            if (force == false)
            {
                return false; // 只强制保存时保存数据.
            }
            if (force || Models.Dirty)
            {
                SaveFileBak();
                var jsonData = Models.Instance.GetJsonData();
                string savePath = GetSaveFilePath();
                if (PTUtil.SaveJsonToFile(jsonData, savePath, SAVE_ENCODE))
                {
                    Models.Dirty = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 备份存档文件
        /// </summary>
        /// <returns><c>true</c>, if file bak was saved, <c>false</c> otherwise.</returns>
        public bool SaveFileBak()
        {
            bool ret = true;
            string filePath = GetSaveFilePath();
            string bakFilePath = GetBakSaveFilePath();

            if (File.Exists(filePath))
            {
                try
                {
                    File.Copy(filePath, bakFilePath, true);
                }
                catch (IOException copyError)
                {
                    Debug.Log("Save Bak Error:" + copyError.Message);
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// 加载存档
        /// </summary>
        /// <returns><c>true</c>, if save was loaded, <c>false</c> otherwise.</returns>
        public bool LoadSave()
        {
            if (LoadFromFile())
            {
                return true;
            }
            return LoadFromDefaultConfig();
        }

        /// <summary>
        /// 加载存档文件. 用本地文件数据刷新Model
        /// </summary>
        /// <returns><c>true</c>, if from file was loaded, <c>false</c> otherwise.</returns>
        protected bool LoadFromFile()
        {
            string filePath = GetSaveFilePath();
            string bakFilePath = GetBakSaveFilePath();

            //Debug.Log("Save File Path:" + filePath);
            if (File.Exists(filePath))
            {
                JsonData data = PTUtil.LoadJsonFromFile(filePath, SAVE_ENCODE);
                if (data != null)
                {
                    Debug.Log("Load Local Save!!!" + filePath);

                    Models.Instance.InitWithJson(data);
                    CheckSaveUid();
                    //Debug.Log("Load strVersion: " + strVersion);
                    return true;
                }
            }

            // 读备份文件
            if (File.Exists(bakFilePath))
            {
                JsonData data = PTUtil.LoadJsonFromFile(bakFilePath, SAVE_ENCODE);
                if (data != null)
                {
                    Debug.Log("Load Local Bak Save!!!");
                    Models.Instance.InitWithJson(data);
                    CheckSaveUid();
                    return true;
                }
            }

            Debug.Log("Local Save Not Found");
            return false;
        }

        /// <summary>
        /// 从默认配表初始化存档数据
        /// </summary>
        /// <returns><c>true</c>, if from default config was loaded, <c>false</c> otherwise.</returns>
        protected bool LoadFromDefaultConfig()
        {
            //var jsonData = Configures.ParseConfigure("config_default.json");

            //Debug.Log("===>Use Default Save!!!");
            //if (jsonData != null)
            //{
            //    Models.Instance.InitWithJson(jsonData);
            //    CheckSaveUid();
            //    return true;
            //}

            //Debug.LogError("Default Save Error");
            return false;
        }

        /// <summary>
        /// 检查存档的 存档文件uid
        /// </summary>
        protected void CheckSaveUid()
        {
            if (string.IsNullOrEmpty(Models.Instance.GetSaveFileId()))
            {
                // 生成存档文件uid
                TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var timestamp = Convert.ToInt64(tss.TotalMilliseconds);
                string k1 = String.Format("{0:X}", timestamp);
                string k2 = String.Format("{0:X}", UnityEngine.Random.Range(10000000, 99999999));

                Models.Instance.SetSaveFileId(k1 + "-" + k2);
            }
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        public void RemoveSave()
        {
            string filePath = GetSaveFilePath();
            string bakFilePath = GetBakSaveFilePath();

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("===>Remove Save:" + filePath);
            }

            if (File.Exists(bakFilePath))
            {
                File.Delete(bakFilePath);
                Debug.Log("===>Remove Bak Save:" + filePath);
            }
        }

        private bool needSaveData = false;
        public void Update()
        {
            if (needSaveData)
            {
                needSaveData = false;
                Models.Instance.SaveToFile(true);
            }
        }

        public void NotifySaveData()
        {
            needSaveData = true;
        }
        #endregion
        //--------------------------------------游戏存档 end--------------------------
    }
}
