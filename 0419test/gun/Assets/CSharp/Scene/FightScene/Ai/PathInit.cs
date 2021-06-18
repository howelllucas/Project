using EZ;
using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathInit : MonoBehaviour {
    //寻路聪明的AI上限
    public static int maxPathNum = 30;
    public static int curPathNum = 0;
    //每次间隔生成的时间
    float useTime = 0.3f;
    float curUseTime = 0;
    public static Queue<AIPursueAct> aiPursueActList = new Queue<AIPursueAct>();
    // Use this for initialization
    private void Awake()
    {
        //GameObject A = GameObject.Find("A");
        //Debug.Log(A);
        //A.GetComponent<AstarPath>().showGraphs = false;
        curPathNum = 0;
        aiPursueActList.Clear();
    }
    private void OnDestroy()
    {
        curPathNum = 0;
        aiPursueActList.Clear();
    }
    // Update is called once per frame
    void Update () {
        if (curPathNum >= maxPathNum || aiPursueActList.Count <= 0)
        {
            return;
        }
        
        float dtTime = BaseScene.GetDtTime();
        
        curUseTime += dtTime;
        if (curUseTime > useTime)
        {
            AIPursueAct curAIPursueAct = aiPursueActList.Dequeue();
            // 没有组件或已经显示
            if (curAIPursueAct == null || (curAIPursueAct.m_AiPath != null && curAIPursueAct.m_AiPath.enabled))
            {
                //if (curAIPursueAct == null)
                //{
                //    Debug.Log("为空了");
                //}
                //if (curAIPursueAct.m_AiPath != null)
                //{
                //    Debug.Log("不为空了");
                //}

                return;
            }
            // 如果组件隐藏隐藏了
            if (!curAIPursueAct.enabled)
            {
                curAIPursueAct.m_PushAutoPath = false;
                //Debug.Log("跳过");
                return;
            }
            curUseTime = 0;
            curAIPursueAct.BeAutoPath();
        }
    }
}
