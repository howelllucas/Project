using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using EZ.Data;
using LitJson;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace EZ.Util {
    public class ELKLogMgr
    {
        private static ELKLogMgr m_Instance = new ELKLogMgr();

        public static int PASS_FAIL = 0;
        public static int PASS_SUCCESS = 1;
        public static int PASS_QUIT = 2;

        public static ELKLogMgr GetInstance()
        {
            return m_Instance;
        }

        private string mDeviceTag = null;
        public string deviceTag
        {
            get
            {
                if (mDeviceTag == null)
                {
                    mDeviceTag = SystemInfo.deviceUniqueIdentifier;
                }
                return mDeviceTag;
            }
        }

        private string mSyncDataUrl = null;
        public string syncDataUrl
        {
            get
            {
                if (mSyncDataUrl == null)
                {
                    GeneralConfigItem data = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ELK_LOG_URL);
                    mSyncDataUrl = data == null ? null : data.content;
                }
                return mSyncDataUrl;
            }
        }

        private ELKLog4Pass mELKLog4Pass = null;
        public void SendELKLog4Pass(int optType, int reviveTimes, int duration)
        {
            if (mELKLog4Pass == null)
            {
                mELKLog4Pass = new ELKLog4Pass();
            }

            mELKLog4Pass.reportTime = getTimeNow();
            mELKLog4Pass.behaviorTime = getTimeNow();

            mELKLog4Pass.optType = optType;
            mELKLog4Pass.hardId = deviceTag;
            mELKLog4Pass.passId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
            mELKLog4Pass.times = Global.gApp.gSystemMgr.GetPassMgr().GetPassEnterTimes();
            mELKLog4Pass.reviveTimes = reviveTimes;
            mELKLog4Pass.duration = duration;
            mELKLog4Pass.roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            mELKLog4Pass.weaponId = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeaponId();
            mELKLog4Pass.weaponLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon());
            mELKLog4Pass.retainDays = Global.gApp.gSystemMgr.GetMiscMgr().GetRetainDays();
            string json = JsonMapper.ToJson(mELKLog4Pass);

            Post(json);

            //Thread athread = new Thread(new ParameterizedThreadStart(ThreadMain));
            //athread.IsBackground = true;//防止后台现成。相反需要后台线程就设为false
            //athread.Start(json);
        }

        private ELKLog4Destroy mELKLog4Destroy = null;
        public void SendELKLog4Destroy()
        {
            if (mELKLog4Destroy == null)
            {
                mELKLog4Destroy = new ELKLog4Destroy();
            }
            
            MakeELKLog4Destroy(mELKLog4Destroy);
            string json = JsonMapper.ToJson(mELKLog4Destroy);

            //将已获得的武器、技能信息装填日志中
            Dictionary<string, int> weaponLvMap = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLvMap();
            string weaponJson = JsonMapper.ToJson(weaponLvMap);

            Dictionary<string, int> skillLvMap = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLvMap();
            string skillJson = JsonMapper.ToJson(skillLvMap);

            json = json.Remove(json.Length - 1);
            weaponJson = weaponJson.Remove(0, 1);
            weaponJson = weaponJson.Remove(weaponJson.Length - 1);
            skillJson = skillJson.Remove(0, 1);

            json = json + "," + weaponJson + "," + skillJson;
            Post(json);

            //Thread athread = new Thread(new ParameterizedThreadStart(ThreadMain));
            //athread.IsBackground = true;//防止后台现成。相反需要后台线程就设为false
            //athread.Start(json);
        }

        public void MakeELKLog4Destroy(ELKLog4Destroy log)
        {
            log.reportTime = getTimeNow();
            log.behaviorTime = getTimeNow();

            log.hardId = deviceTag;
            log.roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            log.gold = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetGold();
            log.diamond = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetDiamond();
            log.exp = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetExp();
            log.energy = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetEnergy();
            log.mdt = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetMDT();
            log.passId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
            log.weaponId = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeaponId();
            log.weaponLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon());
            log.online = log.reportTime - Global.gApp.gSystemMgr.GetMiscMgr().GetLastLoginTime();
            log.retainDays = Global.gApp.gSystemMgr.GetMiscMgr().GetRetainDays();
        }

        public void SendELKLog4Item(ELKLog4Item log)
        {
            string json = JsonMapper.ToJson(log);

            Post(json);
        }

        private void ThreadMain(object param)
        {
            HttpPost(param as string);
        }

        private void Post(string paramData)
        {

            //byte[] bodyRaw = Encoding.UTF8.GetBytes(paramData);

            //UnityWebRequest webRequest = new UnityWebRequest(syncDataUrl, "POST");

            //webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

            //webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

            //webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            //webRequest.SendWebRequest();
            ////异常处理，很多博文用了error!=null这是错误的，请看下文其他属性部分
            //if (webRequest.isHttpError || webRequest.isNetworkError)
            //    Debug.Log(webRequest.error);
            //else
            //{
            //    string result = webRequest.downloadHandler.text;
            //}
        }


        public string HttpPost(string paramData)
        {
            Debug.Log("ELK Log paramData = " + paramData);
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(syncDataUrl);
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/x-www-form-urlencoded";
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(paramData);
                using (Stream requestStream = wbRequest.GetRequestStream())
                {
                    using (StreamWriter swrite = new StreamWriter(requestStream))
                    {
                        swrite.Write(paramData);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sread = new StreamReader(responseStream))
                    {
                        result = sread.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return result;
        }
       

        private long getTimeNow()
        {
            double nowMills = DateTimeUtil.GetMills(DateTime.Now);
            return (long)nowMills;
        }
    }


    public abstract class ELKLogBase
    {
        public int gameId = 2113;
        public int behaviorType;
        public long behaviorTime;
        public long reportTime;

        public ELKLogBase() { }
    }

    public class ELKLog4Pass : ELKLogBase
    {

        //0 失败，1成功，2中途退出
        public int optType;
        //硬件id
        public string hardId;
        //关卡id
        public int passId;
        //游玩关卡次数
        public int times;
        //中途复活次数
        public int reviveTimes;
        //游戏通关时长，秒为单位（不包括暂停和广告等时间）
        public int duration;
        //角色等级
        public int roleLevel;
        //当前武器id
        public int weaponId;
        //当前武器等级
        public int weaponLv;
        //创建天数
        public int retainDays;


        public ELKLog4Pass() {
            behaviorType = BehaviorTypeConstVal.LOG_FINISH_PASS;
        }

    }

    public class ELKLog4Destroy : ELKLogBase
    {
        //硬件id
        public string hardId;
        //角色等级
        public int roleLevel;
        //金币
        public double gold;
        //钻石
        public double diamond;
        //经验
        public double exp;
        //能量
        public double energy;
        //MDT 狗牌
        public double mdt;
        //关卡id
        public int passId;
        //当前武器id
        public int weaponId;
        //当前武器等级
        public int weaponLv;
        //在线时长
        public long online;
        //创建天数
        public int retainDays;


        public ELKLog4Destroy()
        {
            behaviorType = BehaviorTypeConstVal.LOG_DESTROY;
        }
    }

    public class ELKLog4Item : ELKLog4Destroy
    {
        public int optType;
        public double after;
        public int itemId;
        public double param1;
        public double param2;
        public double param3;
        public string paramStr1;
        public string paramStr2;
        public string paramStr3;

        public ELKLog4Item()
        {

        }

        public ELKLog4Item(int behaviorType, ItemDTO itemDTO)
        {
            this.behaviorType = behaviorType;
            this.optType = itemDTO.type;
            this.itemId = itemDTO.itemId;
            this.after = itemDTO.after;
            this.param1 = Math.Abs(itemDTO.num);
            this.param2 = itemDTO.param2;
            this.param3 = itemDTO.param3;
            this.paramStr1 = itemDTO.paramStr1;
            this.paramStr2 = itemDTO.paramStr2;
            this.paramStr3 = itemDTO.paramStr3;
        }
    }

}

