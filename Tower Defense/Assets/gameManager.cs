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
            endUI.GetComponent<Text>().text = "WIN";
        }
        public void failedUI()
        {
            endUI.SetActive(true);
            endUI.GetComponent<Text>().text = "LOSE";
        }

        public void onretryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void onbackButton()
        {
            SceneManager.LoadScene(0);
        }
    
    }

}

