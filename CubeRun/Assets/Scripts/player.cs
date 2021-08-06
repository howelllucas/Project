using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class player : MonoBehaviour
{
    private MapManger map;

    private Color Color1 = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color Color2 = new Color(126 / 255f, 93 / 255f, 183 / 255f);
    private int z = 2;
    private int x = 3;
    void Start()
    {
        map = GameObject.Find("MapManger").GetComponent<MapManger>();
    }

    private void move()
    {
        
        Transform playerPos = map.objList[z][x].transform;
        this.gameObject.transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);
        this.gameObject.transform.rotation = playerPos.rotation;
        if (z%2==0)
        {
            map.objList[z][x].transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = Color1;
        }
        else
        {
            map.objList[z][x].transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = Color2;
        }
        
    }
    private void calePosition()
    {
        if (map.objList.Count-z<=5)
        {
            float z = map.objList[map.objList.Count - 1][0].transform.position.z + map.buttomLength/2; 
            map.CreatMapTiem(z);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            move();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (x != 5||z%2==0)
            {
                z++;
            }
            
            if (z%2==0&&x!=5)
            {
                x++;
            }
            move();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (x!=0)
            {
                z++;
            }
            
            if (z % 2 == 1&&x!=0)
            {
               
                x--;
                
            }
          
            move();
        }
        calePosition();
    }
}
