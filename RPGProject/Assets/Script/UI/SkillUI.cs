using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkillUI : MonoBehaviour
{
    public static SkillUI instance;
    public bool isShow = false;
    public GameObject skillItemPrefab;
    public Transform content;
    public GameObject sv;
    public int[] MagicianSkillList;
    public int[] SwordmanSkillList;

    private playerStatus ps;
    



    void Start()
    {
        instance = this;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();
        int[] skillList = null;
        switch (ps.playerType)
        {
            case PlayerType.Magician:
                skillList = MagicianSkillList;
                break;
            case PlayerType.Swordman:
                skillList = SwordmanSkillList;
                break;
            default:
                break;
        }
      
        for (int i = 0; i < skillList.Length; i++)
        {
            
            GameObject go = GameObject.Instantiate(skillItemPrefab, content);

           
            go.GetComponent<skillItem>().getID(skillList[i]);
            UpdateShow();


        }
        
    }

    public void showSkillUI()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        isShow = true;
        UpdateShow();
    }
    public void hideSkillUI()
    {
        this.transform.localPosition = new Vector3(-1300, 0, 0);
        isShow = false;
    }

    void UpdateShow()
    {
        skillItem[] items = this.GetComponentsInChildren<skillItem>();
        foreach (var item in items)
        {
            item.updateShow(ps.level);
        }
    }
}
