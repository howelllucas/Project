using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBar : MonoBehaviour
{
    public void onStatusButtonClick()
    {
        if (status.instance.isShow)
        {
            status.instance.HideTable();
        }
        else
        {
            status.instance.showTable();
        }
    }
    public void onBagButtonClick()
    {
        
        if (Inventory.instance.isShow)
        {
            Inventory.instance.hideTable();
        }
        else
        {
            Inventory.instance.showTable();
        }
    }
    public void onEquipButtonClick()
    {
        if (equipment.instance.isshow)
        {
            equipment.instance.hideTable();
        }
        else
        {
            equipment.instance.showTable();
        }
    }
    public void onSkillButtonClick()
    {
        if (SkillUI.instance.isShow)
        {
            SkillUI.instance.hideSkillUI();
        }
        else
        {
            SkillUI.instance.showSkillUI();
        }

    }
    public void onSettingButtonClick() {
    }
}
