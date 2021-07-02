using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum shortCutGridType
{
    Skill,
    Drug,
    None
}
public class shortCutGrid : MonoBehaviour
{
    public int id = 0;
    public KeyCode keyCode;
    public GameObject skilliconPrefab;
    public GameObject drugPrefab;
    public Text numText;
    int num = 1;

    SkillInfo siInfo;
    ObjectInfo obInfo;
    playerStatus ps;

    shortCutGridType type = shortCutGridType.None;
    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();
        numText.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (type==shortCutGridType.Drug)
            {
                useDrug(obInfo.hp,obInfo.mp);
            }
            else if (type == shortCutGridType.Skill)
            {

            }
        }
    }
    public void setSkill(int id)
    {
        this.id = id;
        GameObject go = GameObject.Instantiate(skilliconPrefab, this.gameObject.transform);
        siInfo = SkillsInfo.instance.GetSkillInfoById(id);
        go.GetComponent<Image>().sprite= Resources.Load<Sprite>("Icon/" + siInfo.icon_name);
        type = shortCutGridType.Skill;
        numText.enabled = true;
    }
    public void setDrug(int id,int num)
    {
        this.id = id;
        this.num = num;
        numText.text = num.ToString();
        GameObject go = GameObject.Instantiate(drugPrefab, this.gameObject.transform);
        obInfo = ObjectsInfo.instance.GetObjectInfoById(id);
        go.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/" + obInfo.icon_name);
        type = shortCutGridType.Drug;
        numText.enabled = true;
    }
    public void ClearInfo()
    {
        skillItemIcon sti= this.GetComponentInChildren<skillItemIcon> ();
        Inventory_item inItem = this.GetComponentInChildren<Inventory_item>();
        if (sti)
        {
            Destroy(sti.gameObject);
        }
        if (inItem)
        {
            Destroy(inItem.gameObject);
        }
    }

    private void useDrug(int hp,int mp)
    {
        bool success = Inventory.instance.MinusID(id, 1);
        if (success)
        {
            ps.useDrug(hp, mp);
            this.num -= 1;
            numText.text = num.ToString();
            if (num==0)
            {
                ClearInfo();
                numText.enabled = false;
            }
        }
        else
        {
            type = shortCutGridType.None;
        }
       
    }

}
