using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class headUI : MonoBehaviour
{
    public static headUI instance;

    public Text nameText;
    public  Image hpimage;
    public  Image mpimage;

    private playerStatus ps;
    private void Awake()
    {
        instance = this;
        //nameText = transform.Find("name/Text").GetComponent<Text>().text;
        //hpimage = transform.Find("HPBar/hp").GetComponent<Image>();
        //mpimage = transform.Find("MPBbar/mmp").GetComponent<Image>();
    }

    void Start()
    {
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();
        UpdateShow();
    }

    void UpdateShow()
    {
        nameText.text = "LV." + ps.level + ps.playerName;
        hpimage.fillAmount = ps.nowHp / ps.hp;
        mpimage.fillAmount = ps.nowMp / ps.mp;
    }



    
}
