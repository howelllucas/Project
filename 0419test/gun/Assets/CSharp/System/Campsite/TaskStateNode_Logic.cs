using EZ.Data;
using EZ.DataMgr;
using UnityEngine;
namespace EZ
{
    public partial class TaskStateNode
    {
        FollowNode m_FollowNode;
        NpcBehavior m_NpcBehavior;

        private int m_TaskId = 0;
        private int m_TaskState = NpcState.None;
        private float m_CurTime = 0;
        private float m_DelayDestroyTime = 3;
        private bool m_InDestroyState = false;
        private void Awake()
        {
            m_FollowNode = GetComponent<FollowNode>();
            CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
            transform.SetParent(campsiteUI.GetTaskStateNodeTsf(), false);
        }
        public void Init(NpcBehavior npcBehavior, Transform followNode,int taskId,int taskState)
        {
            m_NpcBehavior = npcBehavior;
            m_TaskId = taskId;
            m_TaskState = taskState;
            m_FollowNode.SetFloowNode(followNode);
            SetTaskState(taskState);
            TaskStateBg.button.onClick.AddListener(OpenTaskDetails);
            RewardStateBg.button.onClick.AddListener(OpenTaskDetails);
        }
        private void Update()
        {
            if (m_InDestroyState)
            {
                m_CurTime += Time.deltaTime;
                if(m_CurTime > m_DelayDestroyTime)
                {
                    Destroy(gameObject);
                }
            }
        }
        private void OpenTaskDetails()
        {
            m_NpcBehavior.RespondClick();
        }
        public void SetTaskState(int taskState)
        {
            m_CurTime = 0;
            if (taskState == NpcState.OnGoing)
            {
                RewardNode.gameObject.SetActive(false);
                TaskNode.gameObject.SetActive(true);
  
                if (m_NpcBehavior.IsTargetNpc(GameConstVal.Npc_OldWoman))
                {
                    Received.gameObject.SetActive(false);
                    UnReceive.gameObject.SetActive(true);
                    UnReceive.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1);
                }
                else
                {
                    UnReceive.gameObject.SetActive(false);
                    Received.gameObject.SetActive(true);
                }
                Complet.gameObject.SetActive(false);
                m_InDestroyState = false;
            }
            else if (taskState == NpcState.UnReceived)
            {
                RewardNode.gameObject.SetActive(true);
                /// load Sprite 
                CampTasksItem campTasksItem = Global.gApp.gGameData.CampTasksConfig.Get(m_TaskId);
                if (campTasksItem != null)
                {
                    ItemItem rewardItem = Global.gApp.gGameData.ItemData.Get(int.Parse(campTasksItem.reward[0]));
                    if (rewardItem != null)
                    {
                        Reward.image.sprite = Resources.Load(rewardItem.image_grow, typeof(Sprite)) as Sprite;
                    }
                }
                TaskNode.gameObject.SetActive(false);
                m_InDestroyState = false;
            }
            else if (taskState == NpcState.Received)
            {
                RewardNode.gameObject.SetActive(false);
                TaskNode.gameObject.SetActive(true);
                Received.gameObject.SetActive(false);
                UnReceive.gameObject.SetActive(false);
                Complet.gameObject.SetActive(true);
                m_DelayDestroyTime = 3;
                m_InDestroyState = true;
            }
            else if (taskState == NpcState.None)
            {
                RewardNode.gameObject.SetActive(false);
                TaskNode.gameObject.SetActive(false);
                Received.gameObject.SetActive(false);
                UnReceive.gameObject.SetActive(false);
                Complet.gameObject.SetActive(false);
                m_DelayDestroyTime = 0.5f;
                m_InDestroyState = true;
            }
        }
    }
}
