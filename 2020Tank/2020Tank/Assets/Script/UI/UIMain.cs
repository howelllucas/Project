using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    public static UIMain Instance;
    // 主UI类，主要是控制整个游戏的ui
    public GameObject gameoverUI;
    void Awake()
    {
        
        if (Instance==null)
        {
            Instance = this;
        }
        gameoverUI.SetActive(false);
    }
    
    public void gameOver(bool istue)
    {
        Debug.Log("aaaaaa");
        gameoverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
