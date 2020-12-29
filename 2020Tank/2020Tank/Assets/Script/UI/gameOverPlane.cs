using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class gameOverPlane : MonoBehaviour
{
    public void BackStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    
}
