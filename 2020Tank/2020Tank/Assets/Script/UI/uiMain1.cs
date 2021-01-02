using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiMain1 : MonoBehaviour
{
    public static uiMain1 Instance;
    // 主UI类，主要是控制整个游戏的ui
    public GameObject gameoverUI;
    //胜利UI
    public GameObject gameoverWinUI;
    string score;
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        gameoverUI.SetActive(false);

        score = this.transform.Find("Scoreboard/Team/Score").GetComponent<Text>().text;
    }

    public void gameOver()
    {
        Debug.Log("aaaaaa");
        gameoverUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void gameOverWin()
    {
        Debug.Log("aaaaaa");
        gameoverWinUI.SetActive(true);
        Time.timeScale = 0;
    }
    

}
