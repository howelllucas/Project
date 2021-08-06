using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManger : MonoBehaviour
{
    public List<GameObject[]> objList = new List<GameObject[]>();
    private GameObject Cube;
    private GameObject Wall;
    private GameObject player;

    private Transform MapMgr;

    public float buttomLength;

    Color color1 = new Color(124/255f, 155/255f, 230/255f);
    Color color2 = new Color(125/255f, 169/225f, 233/255f);
    Color colorWall = new Color(78 / 255f, 93 / 225f, 169 / 255f);

    // Start is called before the first frame update
    void Start()
    {
        Cube = Resources.Load<GameObject>("tile_white");
        Wall = Resources.Load<GameObject>("wall2");
        player = Resources.Load<GameObject>("cube_battery");

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
                    tile = GameObject.Instantiate(Cube, vec, Quaternion.Euler(rot));
                    tile.transform.Find("normal_a2").GetComponent<MeshRenderer>().material.color = color1;
                    tile.GetComponent<MeshRenderer>().material.color = color1;
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
}
