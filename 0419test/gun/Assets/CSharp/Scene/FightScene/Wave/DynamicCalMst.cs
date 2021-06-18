
using UnityEngine;

namespace EZ
{
    public class DynamicCalMst {
        WaveMgr m_WaveMgr;
        private int m_OriMaxCount = 500; // 最大上限
        private int m_OriMinCount = 15; // 最小上限

        private bool isSmooth = true; //  是否顺畅
        private int checkTimes = 3; // 连续监测次数
        private float checkTotalTime = 0; // 监测次数总共花的时间
        private int curCheckTime = 0; // 当前监测到第几次
        private float m_SensitivityTime = 0.034f; // 是否顺畅分界线
        private float m_hightestSensitivityTime = 0.038f; // 顺畅的最低容忍度
        private int smoothAdd = 0;
        private int smoothAddMax = 15;
        public DynamicCalMst(WaveMgr waveMgr)
        {
            m_WaveMgr = waveMgr;
            m_WaveMgr.MaxCreateCount = 100;
        }
        public void Update(float dt)
        {
            // 暂停则返回
            if(dt <= 0)
            {
                return;
            }

            // 判断是否顺畅
            if (dt >= m_hightestSensitivityTime)
            {
                // 如果低于最低容忍，则直接设置为不流畅，并且将监测重置
                isSmooth = false;
                curCheckTime = 0;
                checkTotalTime = 0;
            } else
            {
                // 监测判断
                curCheckTime++;
                checkTotalTime += dt;
                // 如果监测结束
                if (curCheckTime >= checkTimes)
                {
                    float avgTime = checkTotalTime / curCheckTime;
                    if (avgTime > m_SensitivityTime)
                    {
                        isSmooth = false;
                    } else
                    {
                        isSmooth = true;
                    }

                    curCheckTime = 0;
                    checkTotalTime = 0;
                }
            }

            if (isSmooth)
            {
                //Debug.Log("顺畅");
                // 如果顺畅则有10的额度进行创建
                smoothAdd++;
                smoothAdd = smoothAdd > smoothAddMax ? smoothAddMax : smoothAdd;
                m_WaveMgr.MaxCreateCount = m_WaveMgr.CurCreateMonsterCount + smoothAdd;
                m_WaveMgr.MaxCreateCount = Mathf.Min(m_WaveMgr.MaxCreateCount, m_OriMaxCount);
            } else
            {
                //Debug.Log("不顺畅");
                // 如果不顺畅则终止创建
                smoothAdd = 0;
                m_WaveMgr.MaxCreateCount = m_WaveMgr.CurCreateMonsterCount - 5;
                m_WaveMgr.MaxCreateCount = Mathf.Max(m_WaveMgr.MaxCreateCount, m_OriMinCount);
            }
        }
    }
}
