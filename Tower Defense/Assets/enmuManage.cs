using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class enmuManage : MonoBehaviour
    {
        public void onstartButton()
        {
            SceneManager.LoadScene(1);
        }
        public void onQuitButton()
        {
            Application.Quit();
        }
    
    }

}

