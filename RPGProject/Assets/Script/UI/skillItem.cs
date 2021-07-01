using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UI;

public class skillItem : MonoBehaviour
{
    private SkillInfo info;
    public int id;

    public Image icon_image;
    public Text skillname;
    public Text apply_type;
    public Text des;
    public Text mp;
    public Image icon_mask;
    public int skilllevel;
    
    
    public void getID(int id)
    {
        this.id = id;
        info = SkillsInfo.instance.GetSkillInfoById(id);
        
        icon_image.sprite = Resources.Load<Sprite>("Icon/" + info.icon_name);
        skillname.text = info.name;
        apply_type.text = info.applyType.ToString();
        des.text = info.des;
        mp.text = info.mp.ToString()+"mp";
        skilllevel = info.level;

        icon_mask.gameObject.SetActive(false);
    }

    public void  updateShow(int playerLevel)
    {
        if (skilllevel <= playerLevel)//可用
        {
            icon_mask.gameObject.SetActive(false);
            GetComponentInChildren<skillItemIcon>().enabled = true;
        }
        else
        {
            icon_mask.gameObject.SetActive(true);
            GetComponentInChildren<skillItemIcon>().enabled = false;
        }

    }

}
