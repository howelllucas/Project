using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class status : MonoBehaviour
{
    public static status instance;
    public Text text_att;
    public Text text_def;
    public Text text_speed;

    public Text text_pointRemain;

    public Text text_zong;

    public Button button_att;
    public Button button_def;
    public Button button_speed;

    playerStatus player_status;

    RectTransform tableRect;

    public bool isShow=false;
    private void Start()
    {
        
        instance = this;
        player_status = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();
        tableRect = this.GetComponent<RectTransform>();
        tableRect.localPosition = new Vector3(-1200, 0, 0);
    }

    private void UpdateShow()
    {
        if (player_status.point_remain>0)
        {
            button_att.gameObject.SetActive(true);
            button_def.gameObject.SetActive(true);
            button_speed.gameObject.SetActive(true);
        }
        else
        {
            button_att.gameObject.SetActive(false);
            button_def.gameObject.SetActive(false);
            button_speed.gameObject.SetActive(false);
        }
        text_att.text = player_status.attack + "+" + player_status.attack_plus;
        text_def.text = player_status.def + "+" + player_status.def_plus;
        text_speed.text = player_status.speed + "+" + player_status.speed_plus;

        text_pointRemain.text = player_status.point_remain.ToString();

        text_zong.text = "攻击：" + (player_status.attack + player_status.attack_plus)
                                        +"    防御："+(player_status.def+player_status.def_plus)
                                        +"    速度："+(player_status.speed+player_status.speed_plus);
    }

    public void showTable()
    {
        tableRect.localPosition = new Vector3(0, 0, 0);
        UpdateShow();
        isShow = true;
        
    }
    public void HideTable()
    {
        tableRect.localPosition = new Vector3(-1200, 0, 0);
        isShow = false;
    }
    public void onAtkButton()
    {
        bool success = player_status.getPoint();
        if (success)
        {
            player_status.attack_plus++;
            UpdateShow();
        }
    }
    public void onDefButton()
    {
        bool success = player_status.getPoint();
        if (success)
        {
            player_status.def_plus++;
            UpdateShow();
        }
    }
    public void onSpeedButton()
    {
        bool success = player_status.getPoint();
        if (success)
        {
            player_status.speed_plus++;
            UpdateShow();
        }
    }
}
