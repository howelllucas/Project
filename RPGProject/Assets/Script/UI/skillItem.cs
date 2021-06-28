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

    
    public void getID(int id)
    {
        this.id = id;
        info = SkillsInfo.instance.GetSkillInfoById(id);

        icon_image.sprite = Resources.Load<Sprite>("Icon/" + info.icon_name);
        skillname.text = info.name;
        apply_type.text = info.applyType.ToString();
        des.text = info.des;
        mp.text = info.mp.ToString()+"mp";
    }

}
