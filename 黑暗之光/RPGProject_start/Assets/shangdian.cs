using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shangdian : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMe()
    {
        this.gameObject.SetActive(true);
    }

    public void HideMe()
    {
        this.gameObject.SetActive(false);
    }
}
