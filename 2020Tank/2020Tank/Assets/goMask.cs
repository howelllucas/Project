using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class goMask : MonoBehaviour
{
    public int speed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void fadeIn()
    {
        this.transform.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime * speed);
    }
    void fadeOut()
    {
        this.transform.GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime * speed);
    }

    // Update is called once per frame
    void Update()
    {

        fadeOut();

        Invoke("fadeIn", 3);
        
    }

    
}
