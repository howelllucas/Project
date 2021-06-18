using EZ.Data;
using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public partial class DialogueUI
    {
        private int groupId;
        private Dialogue_TableItem dialogueRes;
        private Action<int> m_Action;
        private int m_Index;
        private List<Dialogue_TableItem> dialogueList = new List<Dialogue_TableItem>();
        private string BlankColor = "#7A7979FF";
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            string idStr = arg as string;
            if (!UiTools.IsNumeric(idStr))
            {
                Debug.LogErrorFormat("对话id非法 = {0}", idStr);
                TouchClose();
                return;
            }
            groupId = int.Parse(idStr);
            m_Index = 0;
            dialogueList = TableMgr.singleton.DialogueTable.GetDialogueByGroup(groupId);
            if (dialogueList == null)
            {
                Debug.LogErrorFormat("对话id非法 = {0}", idStr);
                TouchClose();
                return;
            }

            dialogueRes = dialogueList[m_Index];

            m_tip.gameObject.SetActive(false);
            m_person1.gameObject.SetActive(false);
            m_person2.gameObject.SetActive(false);
            //ShowNode.gameObject.SetActive(false);
            //gameObject.AddComponent<DelayCallBack>().SetAction(()=>
            //{
            //    ShowNode.gameObject.SetActive(true);
            //    UIFresh();
            //    BgBtn.button.onClick.AddListener(UIFresh);
            //}, m_Dialogue.startDelay, true);
            //ShowNode.gameObject.SetActive(true);
            UIFresh();
            BgBtn.button.onClick.AddListener(UIFresh);

        }

        public void SetAciton(Action<int> action)
        {
            m_Action = action;
        }

        private void UIFresh()
        {
            //Debug.Log(m_Index);
            //if (!m_is_end && mOffset > 0)
            //{
            //    m_is_end = true;
            //    m_tip.text.text = mText;
            //    nextIcon.gameObject.SetActive(true);
            //    return;

            //}
            if (m_Index < 0 || m_Index >= dialogueList.Count)
            {
                TouchClose();
                if (m_Action != null)
                {
                    m_Action(groupId);
                }
                return;
            }

            dialogueRes = dialogueList[m_Index];
            //if (dialogueRes == null)
            //{
            //    TouchClose();
            //    if (m_Action != null)
            //    {
            //        m_Action();
            //    }
            //    return;
            //}
            //if (m_Index >= m_Dialogue.dialogues.Length)
            //{
            //    BgBtn.button.interactable = false;
            //    gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            //    {
            //        TouchClose();
            //        if (m_Action != null)
            //        {
            //            m_Action();
            //        }
            //    }, m_Dialogue.actionDelay, true);
            //    return;
            //}

        
            var npcRes1 = TableMgr.singleton.NpcTable.GetItemByID(dialogueRes.npc1);
            if (npcRes1 != null)
            {
                person1.gameObject.SetActive(true);
                person1.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(npcRes1.icon);
                if (dialogueRes.pos == 1)
                    m_nameText.text.text = LanguageMgr.GetText(npcRes1.tid_name);
            }
            else
            {
                person1.gameObject.SetActive(false);
            }

            var npcRes2 = TableMgr.singleton.NpcTable.GetItemByID(dialogueRes.npc2);
            if (npcRes2 != null)
            {
                person2.gameObject.SetActive(true);
                person2.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(npcRes2.icon);
                if (dialogueRes.pos == 2)
                    m_nameText.text.text = LanguageMgr.GetText(npcRes2.tid_name);
            }
            else
            {
                person2.gameObject.SetActive(false);
            }



            person1.image.color = ColorUtil.GetSpecialColor(dialogueRes.pos == 2, BlankColor);
            person2.image.color = ColorUtil.GetSpecialColor(dialogueRes.pos == 1, BlankColor);
            m_tip.gameObject.SetActive(true);
            //m_tip.text.text = LanguageMgr.GetText(dialogueRes.dialogues);
            m_tip.text.text = (dialogueRes.dialogues);
            //mText = dialogueRes.dialogues;
            //mText = LanguageMgr.GetText(dialogueRes.dialogues);
            //mNextChar = 0f;
            //mOffset = 0;
            //m_is_end = false;
            m_Index++;
            //nextIcon.gameObject.SetActive(false);
            //dialogueRes = TableMgr.singleton.DialogueTable.GetItemByID(dialogueRes.nextID);
            //nextIcon.gameObject.SetActive(dialogueRes != null);
        }

        int charsPerSecond = 20;
        string mText;
        int mOffset = 0;
        float mNextChar = 0f;
        float mTime = 0;
        private bool m_is_end = false;
        private void Update()
        {
            if (m_is_end || string.IsNullOrEmpty(mText))
            {
                return;
            }

            mTime += Time.deltaTime;
            if (mOffset < mText.Length)
            {
                //if (btnNext.gameObject.activeSelf)
                //{
                //    //StopCoroutine("BtnNextAction");
                //    btnNext.transform.parent.gameObject.SetActive(false);
                //}

                if (mNextChar <= mTime)
                {
                    charsPerSecond = Mathf.Max(1, charsPerSecond);

                    // Periods and end-of-line characters should pause for a longer time.
                    float delay = 1f / charsPerSecond;
                    char c = mText[mOffset];
                    if (c == '.' || c == '\n' || c == '!' || c == '?') delay *= 4f;
                    int len = 1;
                    if (c == '<')
                    {
                        //var start = mText.IndexOf("<color", mOffset);
                        var end = mText.IndexOf("</color>", mOffset);
                        Debug.Log("mOffset " + mOffset);
                        Debug.Log("end " + end);
                        len = end - mOffset + 8;
                        //for (int i = mOffset; i < mText.Length; i++)
                        //{
                        //    if (mText[i] == '>')
                        //    {
                        //        len = i + 2 - mOffset;
                        //        break; 
                        //    }
                        //}
                    }

                    /*                        mNextChar = Time.fixedTime + delay;*/
                    mNextChar = mTime + delay;
                    mOffset += len;
                    if (mOffset > mText.Length)
                    {
                        mOffset = mText.Length;
                    }
                    m_tip.text.text = mText.Substring(0, mOffset);
                }
            }
            else
            {
                m_is_end = true;
                nextIcon.gameObject.SetActive(true);
                //IsShowNextBtn();
            }
        }

    }
}
