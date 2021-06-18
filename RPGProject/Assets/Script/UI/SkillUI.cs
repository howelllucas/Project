using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    public static SkillUI instance;
    public bool isShow = false;
    void Start()
    {
        instance = this;
    }

    public void showSkillUI()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        isShow = true;
    }
    public void hideSkillUI()
    {
        this.transform.localPosition = new Vector3(-1300, 0, 0);
        isShow = false;
    }
}
