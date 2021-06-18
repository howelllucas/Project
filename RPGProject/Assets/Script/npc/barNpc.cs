using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barNpc : NPC
{
    public Transform taskUI;
    public Text text;
    public GameObject btn_apply;
    public GameObject btn_OK;
    public GameObject btn_cancel;
    public GameObject btn_close;

    public int teskNum=0;
    private bool isInTask = false;
    private playerStatus player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            showTaskUI();
        }
    }

    private void showTaskUI()
    {
        taskUI.gameObject.SetActive(true);
        if (isInTask)
        {
            showInTask(teskNum);

        }
        else
        {
            showNoTask();
        }
    }
    //任务中面板
    private void showInTask(int num)
    {
        text.GetComponent<Text>().text = "任务1：" + "\n已经击杀" + num + "只小狼" + "\n\n奖励：" + "\n1000金币";
        btn_OK.SetActive(true);
        btn_cancel.SetActive(false);
        btn_apply.SetActive(false);
    }
    //领取面板
    private void showNoTask()
    {
        text.GetComponent<Text>().text = "任务1：" + "\n击杀" + "10" + "只小狼" + "\n\n奖励：" + "\n1000金币";
        btn_OK.SetActive(false);
        btn_cancel.SetActive(true);
        btn_apply.SetActive(true);
    }
    private void HideTaskUI()
    {
        taskUI.gameObject.SetActive(false);
    }


    public void clickClose()
    {
        HideTaskUI();
    }
    public void clickOK()
    {
        if (teskNum>=10)
        {//完成
            player.getCoin(1000);
            teskNum = 0;
            isInTask = false;
            HideTaskUI();
        }
        else
        {//未完成
            HideTaskUI();
        }
        
    }

    public void clickCancel()
    {
        HideTaskUI();
    }
    public void clickApply()
    {
        isInTask = true;
        showInTask(teskNum);
    }
}
