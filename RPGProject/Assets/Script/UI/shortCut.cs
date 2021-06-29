using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shortCut : MonoBehaviour
{
    public int id = 0;
    public KeyCode keyCode;
    public GameObject skillItemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public  void setID(int id)
    {
        this.id = id;
        GameObject go = GameObject.Instantiate(skillItemPrefab, this.gameObject.transform);
        go.GetComponent<shortCut>().getID(id);
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
