using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManger : MonoBehaviour
{
    private player player;
    public List<GameObject[]> objList = new List<GameObject[]>();
    private GameObject Cube;
    private GameObject Wall;
    private GameObject xianjing_dimian;
    private GameObject xianjing_tiankong;
    private GameObject gem;

    private Transform MapMgr;

    public float buttomLength;

    private int index = 0;
    //概率
    private int pr_hole = 0;
    private int dimian = 0;
    private int tiankong = 0;
    private int pr_gem = 5;

    Color color1 = new Color(124/255f, 155/255f, 230/255f);
    Color color2 = new Color(125/255f, 169/225f, 233/255f);
    Color colorWall = new Color(78 / 255f, 93 / 225f, 169 / 255f);

    // Start is called before the first frame update
    void Awake()
    {
        Cube = Resources.Load<GameObject>("tile_white");
        Wall = Resources.Load<GameObject>("wall2");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        xianjing_dimian= Resources.Load<GameObject>("moving_spikes");
        xianjing_tiankong=Resources.Load<GameObject>("smashing_spikes");
        gem = Resources.Load<GameObject>("gem 2");

        MapMgr = this.transform;

        buttomLength = Mathf.Sqrt(2) * 0.254f;

        CreatMapTiem(0);
    }

   public void CreatMapTiem(float offsetZ)
    {
        GameObject tile;
        
        for (int i = 0; i < 12; i++)
        {
            GameObject[] objAraay = new GameObject[7];
            for (int j = 0; j < 7; j++)
            {
                Vector3 vec = new Vector3(j* buttomLength, 0, i * buttomLength+ offsetZ);
                Vector3 rot = new Vector3(-90, 45, 0);
                if (j==0||j==6)
                {
                    tile = GameObject.Instantiate(Wall, vec, Quaternion.Euler(rot));
                    tile.GetComponent<MeshRenderer>().material.color = colorWall;
                }
                else
                {
                    int pr= caloPR();
                    int gemPr = gemPR();
                    if (pr==0)
                    {
                        tile = GameObject.Instantiate(Cube, vec, Quaternion.Euler(rot));
                        tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = color1;
                        tile.GetComponent<MeshRenderer>().material.color = color1;
                        if (gemPr == 1)
                        {
                            GameObject gemObj = GameObject.Instantiate(gem, tile.transform.position, Quaternion.identity);
                        }
                    }
                    else if (pr == 1)
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position = vec;
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pr==2)
                    {
                        tile = GameObject.Instantiate(xianjing_dimian, vec, Quaternion.Euler(rot));
                    }
                    else
                    {
                        tile = GameObject.Instantiate(xianjing_tiankong, vec, Quaternion.Euler(rot));
                    }
                    
                }
                objAraay[j] = tile;
                tile.transform.SetParent(MapMgr);
            }
            objList.Add(objAraay);
            GameObject[] objAraay2 = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                Vector3 vec = new Vector3(j * buttomLength + buttomLength / 2, 0, i * buttomLength+ offsetZ + buttomLength / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                tile = GameObject.Instantiate(Cube, vec, Quaternion.Euler(rot));
                tile.GetComponent<MeshRenderer>().material.color = color2;
                tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = color2;
                tile.transform.SetParent(MapMgr);
                objAraay2[j] = tile;
            }
            objList.Add(objAraay2);
        }
        
    }
   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < objList.Count; i++)
            {
                for (int j = 0; j < objList[i].Length; j++)
                {
                    objList[i][j].gameObject.name=i+"--"+j;
                }
            }
        }
        
    }

    public void startCon()
    {
        StartCoroutine("dropFloor");
    }
    public void stopCon()
    {
        StopCoroutine("dropFloor");
    }
    //地板掉落
    private IEnumerator dropFloor()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < objList[index].Length; i++)
            {
                
                Rigidbody rd = objList[index][i].AddComponent<Rigidbody>();
                rd.angularVelocity = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) *Random.Range(1.0f,10.0f);
                GameObject.Destroy(objList[index][i], 1.0f);
            }
            if (index>=player.z)
            {
                player.gameObject.AddComponent<Rigidbody>();
                player.StartCoroutine("dead", true);
                stopCon();
            }
            index++;
        }
    }
    //计算宝石概率
    private int gemPR()
    {
        int prGem = Random.Range(1, 100);
        if (prGem<= pr_gem)
        {
            return 1;
        }
        return 0;
    }
    //计算概率
    private int caloPR()
    {
        int pr = Random.Range(1, 100);
        if (pr<=pr_hole)
        {
            return 1;
        }
        else if (pr>30&&pr<30+dimian)
        {
            return 2;
        }
        else if(pr>60 &&pr<60+tiankong )
        {
            return 3;
        }
        return 0;
    }
    // 增加概率
    public void AddPr()
    {
        pr_hole+=1;
        dimian += 2;
        tiankong += 2;
        
    }
}
