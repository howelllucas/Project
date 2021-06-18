using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZ;

namespace Game
{
    public class DialogueMgr : Singleton<DialogueMgr>
    {
        private Action<int> callBack = null;

        public void ShowDialogue(int id, Action<int> endCallBack = null)
        {
            if (TableMgr.singleton.DialogueTable.GetDialogueByGroup(id) == null)
                return;

            callBack = endCallBack;
            Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, id.ToString());
            DialogueUI dialogueUI = Global.gApp.gUiMgr.GetPanelCompent<DialogueUI>(Wndid.DialogueUI);
            dialogueUI.SetAciton(EndDialogue);

            Debug.Log("ShowDialogue " + id);
        }

        private void EndDialogue(int id)
        {
            if (IsDialogueFinish(id))
                return;

            if (TableMgr.singleton.DialogueTable.GetDialogueByGroup(id) == null)
                return;

            PlayerDataMgr.singleton.DB.finishDialogueList.Add(id);
            PlayerDataMgr.singleton.NotifySaveData();

            Debug.Log("EndDialogue " + id);

            if (callBack != null)
            {
                callBack(id);
            }

            callBack = null;
        }

        public bool IsDialogueFinish(int id)
        {
            return PlayerDataMgr.singleton.DB.finishDialogueList.Contains(id);
        }
    }
}