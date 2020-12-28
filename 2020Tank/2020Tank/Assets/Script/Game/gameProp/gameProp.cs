using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameProp : MonoBehaviour
{
    public GameObject[] gameObjects;
    public GameObject[] gamePorp;

    void Start()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject.Instantiate(gamePorp[i], gameObjects[i].transform.position, Quaternion.identity);
        }
        
    }

    
    void Update()
    {
        
    }
}
