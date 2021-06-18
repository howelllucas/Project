using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class gameManager : MonoBehaviour
    {
        public GameObject endUI;
        public static gameManager instance;
        private Button[] buttons;
        private void Awake()
        {
            instance = this;
            buttons = endUI.GetComponentsInChildren<Button>();
        }
        public void WinUI()
        {
            endUI.SetActive(true);
            endUI.GetComponentInChildren<Text>().text = "WIN";
            buttons[0].gameObject.SetActive(true);
            buttons[2].gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        public void failedUI()
        {
            endUI.SetActive(true);
            endUI.GetComponentInChildren<Text>().text = "LOSE";
            Time.timeScale = 0;
        }
        public void victoryUI()
        {
            endUI.SetActive(true);
            endUI.GetComponentInChildren<Text>().text = "通过关卡";
            buttons[0].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void onretryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        public void onbackButton()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        public void nextButton()
        {
            
            SceneManager.LoadScene(2);
            
            
            Time.timeScale = 1;
        }


    }

}

