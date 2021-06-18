using EZ.DataMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class NpcBehavior : MonoBehaviour
    {
        [Tooltip(" 动画名称必填，第一个为 必须为 idle 动作 ")]
        public List<string> AnimName;
        [Tooltip(" 动画权重 必填 ")]
        public List<int> AnimRate;
        [Tooltip(" 动画播放时长 ")]
        public List<int> AnimTime;
        [Tooltip(" 动画对应的速度 ")]
        public List<int> AnimSpeed;

        protected string m_CurAnimName = string.Empty;


        private Animator m_Animator;
        private int m_TotalRate;
        private int m_Speed = 0;
        private Vector2 m_SpeedVec;
        private float m_AnimTime;
        private float m_RecordTime = -1;
        private Rigidbody2D m_Rigidbody2D;

        private int m_TaskIndex = 0;
        private TaskStateNode m_TaskStateNode;
        private WorkerRewardNode m_WorkerRewardNode;
        private DialogNode m_DialogNode;
        NpcQuestItemDTO m_TaskItem;
        private Transform m_TaskUiNode;
        private List<RedHeartEle> m_RedHeartList = new List<RedHeartEle>();
        private bool m_FreshHeart = false;
        private SkinnedMeshRenderer m_SkinnedMeshRender;
        private bool m_ForceHasOutline = false;
        
        private void Awake()
        {
            m_SkinnedMeshRender = GetComponentInChildren<SkinnedMeshRenderer>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Animator = GetComponentInChildren<Animator>();
            foreach (int rate in AnimRate)
            {
                m_TotalRate += rate;
            }
        }

        private void OnEnable()
        {
            m_CurAnimName = string.Empty;
            CheckAnim();
        }

        public void Init(NpcQuestItemDTO taskItem,int taskIndex)
        {
            m_TaskItem = taskItem;
            m_TaskIndex = taskIndex;
            if(taskItem.state != NpcState.None && m_TaskItem.state != NpcState.Received)
            {
                GameObject go = Global.gApp.gResMgr.InstantiateObj(Wndid.TaskStateNode);
                m_TaskStateNode = go.GetComponent<TaskStateNode>();
                m_TaskStateNode.Init(this,GetTaskUINode(),taskId: taskItem.npcQuestId, taskState:taskItem.state);
                PlayIdle();
            }
            FreshDropInfo();
        }
        public Transform GetTaskUINode()
        {
            if(m_TaskUiNode == null)
            {
                m_TaskUiNode = transform.Find("TaskNode");
            }
            return m_TaskUiNode;
        }
        private void Update()
        {
            if (m_FreshHeart)
            {
                m_FreshHeart = false;
                FreshDropRedHeart();
            }
            m_AnimTime -= BaseScene.GetDtTime();
            if(m_AnimTime < 0)
            {
                CheckAnim();
            }
            Move();
        }
        private void CheckAnim()
        {
            int randomRate = Random.Range(1, m_TotalRate + 1);
            int calRate = 0;
            int index = 0;
            foreach(int rate in AnimRate)
            {
                calRate += rate;
                if (calRate < randomRate)
                {
                    index++;
                }
                else
                {
                    PlayAnim(index);
                    break;
                }
            }
        }
        protected virtual void PlayAnim(int index)
        {
            string animName = AnimName[index];
            SetAnimTime(AnimTime[index]);
            m_Speed = AnimSpeed[index];
            PlayAnim(animName);
            float randomAngle = Mathf.Deg2Rad * Random.Range(0, 360);
            m_SpeedVec = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * m_Speed;
        }
        private void Move()
        {
            if (m_Speed != 0)
            {
                transform.up = m_SpeedVec;
                m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic ;
                m_Rigidbody2D.velocity = m_SpeedVec;
            }
            else
            {
                m_Rigidbody2D.bodyType = RigidbodyType2D.Static ;
            }
        }
        public void SetEnable(bool enable)
        {
            //if (enable)
            //{
            //    CheckAnim();
            //}
            //else
            //{
            //    m_SpeedVec = Vector2.zero;
            //    m_Speed = 0;
            //    Move();
            //}
        }
        private void PlayAnim(string name, bool force = false)
        {
            if (!m_CurAnimName.Equals(name) || force)
            {
                m_CurAnimName = name;
                m_Animator.Play(name, -1, 0);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_Speed > 0)
            {
                //m_SpeedVec = Vector2.zero - m_SpeedVec;
                m_SpeedVec = Quaternion.Euler(new Vector3(0, 0, Random.Range(135, 225))) * m_SpeedVec;
            }
        }
        public string GetNpcId()
        {
            return m_TaskItem.npcId;
        }
        public int GetTaskId()
        {
            return m_TaskItem.npcQuestId;
        }
        public int GetTaskIndex()
        {
            return m_TaskIndex;
        }
        public void FreshDropInfo()
        {
            m_FreshHeart = true;
            FreshWorkerDrop();
        }
        public void FreshHeartInfo()
        {
            m_FreshHeart = true;
        }
        public void FreshWorkerDrop()
        {
            NpcRedHeartItemDTO workerWorkItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockWorkerIndex);
            if (workerWorkItemDTO != null && workerWorkItemDTO.dropDiamondNum > 0)
            {
                PlayIdle();
                if (m_WorkerRewardNode == null)
                {
                    GameObject go = Global.gApp.gResMgr.InstantiateObj(Wndid.WorkerRewardNode);
                    m_WorkerRewardNode = go.GetComponent<WorkerRewardNode>();
                    m_WorkerRewardNode.Init(this, GetTaskUINode(), workerWorkItemDTO.dropDiamondNum);
                }
            }
            else
            {
                DestroyWorkerRewardNode();
            }
        }
        private void DestroyWorkerRewardNode()
        {
            if (m_WorkerRewardNode != null)
            {
                Destroy(m_WorkerRewardNode.gameObject);
                m_WorkerRewardNode = null;
            }
        }
        public void FreshDropRedHeartData()
        {
            NpcRedHeartItemDTO heartItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockRedHeartIndex);
            if (heartItemDTO != null)
            {
                int curHeartCount = heartItemDTO.dropHeartNum;
                Global.gApp.gSystemMgr.GetNpcMgr().DtFreshDropRedHeartInfo(heartItemDTO);
                if (curHeartCount != heartItemDTO.dropHeartNum)
                {
                    FreshDropRedHeart();
                }
            }
        }
        private void FreshDropRedHeart()
        {
            NpcRedHeartItemDTO heartItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockRedHeartIndex);
            if(heartItemDTO != null)
            {
                int dropCount = heartItemDTO.dropHeartNum;
                if (dropCount > 0)
                {
                    PlayIdle();
                }
                float dtDeg = Mathf.PI / dropCount;
                float radio = 1.2f;

                CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
                Canvas parentCanvas = campsiteUI.GetComponentInParent<Canvas>();
                RectTransform adaptParentRectTsf = parentCanvas.GetComponent<RectTransform>();
                Transform heartParentNode = campsiteUI.GetRedHeartNodeTsf();
                for (int i = 0; i < dropCount; i++)
                {
                    RedHeartEle redHeartEle;
                    if (m_RedHeartList.Count < (i + 1))
                    {
                        GameObject redHeartGo = Global.gApp.gResMgr.InstantiateObj(Wndid.ReadHeart);
                        redHeartEle = redHeartGo.GetComponent<RedHeartEle>();
                        m_RedHeartList.Add(redHeartEle);
                        redHeartEle.transform.SetParent(heartParentNode, false);
                    }
                    else
                    {
                        redHeartEle = m_RedHeartList[i];
                    }
                    float posX = radio * Mathf.Cos(dtDeg * i);
                    float posY = radio * Mathf.Sin(dtDeg * i);
                    redHeartEle.SetInfo(this, 1);
                    Vector3 worldPos = transform.position + new Vector3(posX, posY, 0);
                    redHeartEle.SetPos(worldPos, adaptParentRectTsf);
                }
                while (dropCount < m_RedHeartList.Count)
                {
                    int curIndex = m_RedHeartList.Count - 1;
                    Destroy(m_RedHeartList[curIndex].gameObject);
                    m_RedHeartList.RemoveAt(curIndex); 
                }
            }
        }
        public void ForceFreshTaskState()
        {
            if (m_TaskStateNode == null)
            {
                if (m_TaskItem.state != NpcState.None && m_TaskItem.state != NpcState.Received)
                {
                    GameObject go = Global.gApp.gResMgr.InstantiateObj(Wndid.TaskStateNode);
                    m_TaskStateNode = go.GetComponent<TaskStateNode>();
                    m_TaskStateNode.Init(this, GetTaskUINode(), taskId: m_TaskItem.npcQuestId, taskState: m_TaskItem.state);
                }
            }
            FreshTaskState();
        }
        public void FreshTaskState(bool fromEnter = false)
        {
            if (m_TaskStateNode != null)
            {
                m_TaskStateNode.SetTaskState(m_TaskItem.state);
            }
            if (!m_ForceHasOutline)
            {
                if (m_TaskItem.state != NpcState.None && m_TaskItem.state != NpcState.Received)
                {
                    m_SkinnedMeshRender.material.SetColor("_OutlineColor", new Color(0, 0, 0, 1));
                }
                else
                {
                    m_SkinnedMeshRender.material.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
                }
            }
            PlayAnim(m_CurAnimName, fromEnter);
            if (m_TaskItem.state == NpcState.None || m_TaskItem.state == NpcState.Received)
            {
                CheckIdle();
            }
            if(m_TaskItem.state == NpcState.Received)
            {
                FreshDropRedHeart();
            }
        }
        public void SetForceHasOutlineEffect()
        {
            m_ForceHasOutline = true;
        }

        private void CheckIdle()
        {
            // 有红心 以及 有 钻石的时候 不能再移动
            NpcRedHeartItemDTO heartItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockRedHeartIndex);
            if (heartItemDTO != null)
            {
                if (heartItemDTO.dropHeartNum > 0)
                {
                    PlayAnim(0);
                    SetAnimTime(99999);
                    return;
                }
            }
            NpcRedHeartItemDTO workerItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockWorkerIndex);
            if (workerItemDTO != null)
            {
                if (workerItemDTO.dropDiamondNum > 0)
                {
                    PlayAnim(0);
                    SetAnimTime(99999);
                    return;
                }
            }
            if (m_TaskItem.state == NpcState.UnReceived)
            {
                PlayAnim(0);
                SetAnimTime(99999);
            }
        }
        private void PlayIdle()
        {
            PlayAnim(0);
            // 有红心 以及 有 钻石的时候 不能再移动
            NpcRedHeartItemDTO heartItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockRedHeartIndex);
            if (heartItemDTO != null)
            {
                if(heartItemDTO.dropHeartNum > 0)
                {
                    SetAnimTime(99999);
                    return;
                }
            }
            NpcRedHeartItemDTO workerItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockWorkerIndex);
            if (workerItemDTO != null)
            {
                if (workerItemDTO.dropDiamondNum > 0)
                {
                    SetAnimTime(99999);
                    return;
                }
            }
            if (m_TaskItem.state == NpcState.UnReceived)
            {
                SetAnimTime(99999);
            }
        }
        public int GetTaskState()
        {
            return m_TaskItem.state;
        }
        public void RespondClickWorkerDiamond(Vector3 showPosition)
        {
            DestroyWorkerRewardNode();
            int addCount = Global.gApp.gSystemMgr.GetNpcMgr().AddDiamond(m_TaskItem.lockWorkerIndex,showPosition);
            PlayIdle();
        }
        public void RespondClick()
        {
            if (m_TaskItem.npcQuestId > 0 && (m_TaskItem.state != NpcState.None && m_TaskItem.state != NpcState.Received))
            {
                ShowTaskUi();
            }
            else
            {
                ShowPlotUi();
            }
        }
        private void CloseTaskDetailsUiCallBack()
        {
            if (m_TaskStateNode != null)
            {
                m_TaskStateNode.gameObject.SetActive(true);
            }
            PlayIdle();
            if(m_AnimTime < 9999)
            {
                SetAnimTime(0);
            }
        }

        private void ShowTaskUi()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.TaskDetailsUi);
            TaskDetailsUi taskDetailsUi = Global.gApp.gUiMgr.GetPanelCompent<TaskDetailsUi>(Wndid.TaskDetailsUi);
            taskDetailsUi.InitDetailsInfo(m_TaskItem, m_TaskIndex,m_TaskUiNode, CloseTaskDetailsUiCallBack);
            if (m_TaskStateNode != null)
            {
                m_TaskStateNode.gameObject.SetActive(false);
            }
            PlayIdle();
            SetAnimTime(99999);
        }
        private void PlotCallBack()
        {
            if (m_TaskStateNode != null)
            {
                m_TaskStateNode.gameObject.SetActive(true);
            }
            if (m_DialogNode != null)
            {
                Destroy(m_DialogNode.gameObject);
                m_DialogNode = null;
            }
            PlayIdle();
            if (m_AnimTime < 9999)
            {
                SetAnimTime(0);
            }
        }
        public void ShowOldWomanTaskUi(int plotId)
        {
            if (m_DialogNode != null)
            {
                Destroy(m_DialogNode.gameObject);
                m_DialogNode = null;
            }
            if (m_TaskStateNode != null)
            {
                m_TaskStateNode.gameObject.SetActive(false);
            }
            GameObject go = Global.gApp.gResMgr.InstantiateObj(Wndid.DialogUI);
            m_DialogNode = go.GetComponent<DialogNode>();
            m_DialogNode.Init(m_TaskItem, GetTaskUINode(), PlotCallBack);
            m_DialogNode.SetPlotId(plotId);
        }
        private void ShowPlotUi()
        {
            if (m_DialogNode != null)
            {
                Destroy(m_DialogNode.gameObject);
                m_DialogNode = null;
            }
            if (m_TaskStateNode != null)
            {
                m_TaskStateNode.gameObject.SetActive(false);
            }
            GameObject go = Global.gApp.gResMgr.InstantiateObj(Wndid.DialogUI);
            m_DialogNode = go.GetComponent<DialogNode>();
            m_DialogNode.Init(m_TaskItem, GetTaskUINode(), PlotCallBack);
            PlayIdle();
            SetAnimTime(99999);
        }
        public bool GetHasTask()
        {
            return m_TaskItem.state != NpcState.None;
        }
        public void PickRedHeart(RedHeartEle heartEle ,int dropNum)
        {
            int addCount = Global.gApp.gSystemMgr.GetNpcMgr().AddRedHeart(m_TaskItem.lockRedHeartIndex,dropNum, heartEle.transform.position);
            if (addCount > 0)
            {
                m_RedHeartList.Remove(heartEle);
                Destroy(heartEle.gameObject);
                if(m_RedHeartList.Count == 0)
                {
                    PlayAnim(0);
                    NpcRedHeartItemDTO workerItemDTO = Global.gApp.gSystemMgr.GetNpcMgr().GetRedHeartByIndex(m_TaskItem.lockWorkerIndex);
                    if (workerItemDTO != null)
                    {
                        if (workerItemDTO.dropDiamondNum > 0)
                        {
                            PlayAnim(0);
                            SetAnimTime(99999);
                            return;
                        }
                    }
                    else
                    {
                        SetAnimTime(0.1f);
                    }
                }
            }
        }

        public bool IsTargetNpc(string npcName)
        {
            if(m_TaskItem.npcId == npcName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void PlayCheerAnim(bool freshHeart = false)
        {
            if (freshHeart)
            {
                FreshDropRedHeart();
            }
            if(m_TaskItem.npcId != GameConstVal.Npc_OldWoman)
            {
                PlayAnim(GameConstVal.Cheer);
                m_RecordTime = m_AnimTime;

                m_Speed = AnimSpeed[0];
                m_SpeedVec = new Vector2(0,0) * m_Speed;
                SetAnimTime(99999);
            }
        }
        private void SetAnimTime(float animTime)
        {
            m_AnimTime = animTime;
        }
        public void ResetOriAnim()
        {
            PlayIdle();
        }
        public Vector3 GetCalcRightAdaptNodePos()
        {
            if (m_TaskStateNode != null)
            {
                return m_TaskStateNode.transform.position;
            }
            else
            {
                return Vector3.zero;
            }
        }
        public NpcQuestItemDTO GetQuestItem()
        {
            return m_TaskItem;
        }
        public void DestroyNpc()
        {
            foreach(RedHeartEle redHeartEle in m_RedHeartList)
            {
                Destroy(redHeartEle.gameObject);
            }
            if(m_TaskStateNode != null)
            {
                Destroy(m_TaskStateNode.gameObject);
            }
            if(m_TaskUiNode != null)
            {
                Destroy(m_TaskUiNode.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
