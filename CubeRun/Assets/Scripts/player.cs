using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class player : MonoBehaviour
{
    private MapManger map;

    public bool isFollow = false;

    private Color Color1 = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color Color2 = new Color(126 / 255f, 93 / 255f, 183 / 255f);
    public int z = 2;
    private int x = 3;

    private bool isDead = false;

    public int gemCount=0;
    public int score = 0;
    void Start()
    {
        map = GameObject.Find("MapManger").GetComponent<MapManger>();
        PlayerPrefs.GetInt("gem", 0);
        //PlayerPrefs.GetInt("score", 0);
    }
    private void addGemCount()
    {
        gemCount++;
    }
    private void addScore()
    {
        score++;
    }
    public void move()
    {
        
        Transform playerPos = map.objList[z][x].transform;
        this.gameObject.transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);
        this.gameObject.transform.rotation = playerPos.rotation;

        MeshRenderer meshRenderer = null;

        if (playerPos.tag == "tile")
        {
            meshRenderer = map.objList[z][x].transform.Find("normal_a2").GetComponent<MeshRenderer>();
        }
        else if (playerPos.tag == "dimian")
        {
            meshRenderer = map.objList[z][x].transform.Find("moving_spikes_a2").GetComponent<MeshRenderer>();
        }
        else if (playerPos.tag == "tiankong")
        {
            meshRenderer = map.objList[z][x].transform.Find("smashing_spikes_a2").GetComponent<MeshRenderer>();
        }

        if (meshRenderer!=null)
        {
            if (z % 2 == 0)
            {
                meshRenderer.material.color = Color1;
            }
            else
            {
                meshRenderer.material.color = Color2;
            }
        }
        else
        {
            gameObject.AddComponent<Rigidbody>();
            StartCoroutine("dead", true);
        }
        
        
    }
    private void calePosition()
    {
        if (map.objList.Count-z<=12)
        {
            map.AddPr();
            float z = map.objList[map.objList.Count - 1][0].transform.position.z + map.buttomLength/2; 
            map.CreatMapTiem(z);
        }
    }
    void Update()
    {
        if (!isDead)
        {
            inputFun();
        }
        
        calePosition();
    }

    private void inputFun()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (x != 5 || z % 2 == 0)
            {
                z++;
                addScore();
            }

            if (z % 2 == 0 && x != 5)
            {
                x++;
            }
            move();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (x != 0)
            {
                z++;
                addScore();
            }

            if (z % 2 == 1 && x != 0)
            {

                x--;

            }

            move();
        }
    }

    private void saveData()
    {
        PlayerPrefs.SetInt("gem", gemCount);
        if (score>PlayerPrefs.GetInt("score",0))
        {
            PlayerPrefs.SetInt("score", score);
        }
    }
    public IEnumerator dead(bool b)
    {
        
        Debug.Log("死了");
        if (b)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (!isDead)
        {
            isDead = true;
            isFollow = false;
            //ui相关操作
        }
        saveData();
        //Time.timeScale = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="attack")
        {
            StartCoroutine("dead", false);
        }
        if (other.tag=="gem")
        {
            
            addGemCount();
            Destroy(other.transform.parent.gameObject);
            
        }
    }
}
