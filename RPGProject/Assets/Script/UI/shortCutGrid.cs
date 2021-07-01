using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shortCutGrid : MonoBehaviour
{
    public int id = 0;
    public KeyCode keyCode;
    public GameObject skilliconPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void setSkill(int id)
    {
        this.id = id;
        GameObject go = GameObject.Instantiate(skilliconPrefab, this.gameObject.transform);
        SkillInfo info = SkillsInfo.instance.GetSkillInfoById(id);
        go.GetComponent<Image>().sprite= Resources.Load<Sprite>("Icon/" + info.icon_name);
    }   
    public void ClearInfo()
    {
        skillItemIcon sti= this.GetComponentInChildren<skillItemIcon> ();
        if (sti)
        {
            Destroy(sti.gameObject);
        }
    }

}
