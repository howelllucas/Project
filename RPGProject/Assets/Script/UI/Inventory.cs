using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 背包类，用于控制背包物品显示在哪
/// </summary>
public class Inventory : MonoBehaviour
{
    public bool isShow = false;
    //单例模式
    public static Inventory instance;
    //所有格子下的脚本
    public List<inventoryItemGrid> list = new List<inventoryItemGrid>();
    public Text coinNum;
    public int coinCount;

    RectTransform tableRect;

    public GameObject inventoryItem;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        tableRect = this.GetComponent<RectTransform>();
        tableRect.localPosition = new Vector3(-1200, 0, 0);
        coinCount = int.Parse(coinNum.text);
    }
    private void Update()
    {//测试
        if (Input.GetKeyDown(KeyCode.X))
        {
            getId(Random.Range(2001, 2004),1);
        }
    }
    public bool getCoin(int num)
    {
        if (coinCount >= num)
        {
            coinCount -= num;
            coinNum.text = coinCount.ToString();
            return true;
        }
        return false;
    }
    
    public void getId(int id,int count=1)
    {
        inventoryItemGrid grid = null;
        //判断是否有空位置
        foreach (inventoryItemGrid item in list)
        {
            if (item.id==id)
            {
                grid = item;break;
            }
        }
        if (grid!=null)//有这个id的物品
        {
            grid.plusNum(count);
        }
        else//没有这个物品，判断是否满了
        {
            foreach (inventoryItemGrid item in list)
            {
                if (item.id == 0)//说明有空位置
                {
                    grid = item;break;
                }
                
            }
            if (grid!=null)
            {
                GameObject go= GameObject.Instantiate(inventoryItem, Vector3.zero, Quaternion.identity, grid.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.SetSiblingIndex(0);
                go.GetComponentInParent<inventoryItemGrid>().setID(id, count);
            }
            
        }
        
    }
    public void showTable()
    {
        tableRect.localPosition = new Vector3(0,0,0);
        isShow = true;

    }
    public void hideTable()
    {
        tableRect.localPosition = new Vector3(-1200, 0, 0);
        isShow = false;
    }
}
