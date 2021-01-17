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
        private void Awake()
        {
            instance = this;
        }
        public void victoryUI()
        {
            endUI.SetActive(true);
            endUI.GetComponentInChildren<Text>().text = "WIN";
            Time.timeScale = 0;
        }
        public void failedUI()
        {
            endUI.SetActive(true);
            endUI.GetComponentInChildren<Text>().text = "LOSE";
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
    
    }

}

