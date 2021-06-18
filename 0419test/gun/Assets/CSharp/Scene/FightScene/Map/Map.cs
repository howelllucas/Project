using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class Map
    {
        private List<List<MapCell>> m_MapData = new List<List<MapCell>>();
        private AStartPathFind m_AStartTools;
        private GameObject m_MapGo;
        private GameObject m_MainPlayer;
        private int m_MapWidth;
        private int m_MapHeight;
        public Map(int width, int height, GameObject mainPlayer)
        {
            m_MapWidth = width;
            m_MapHeight = height;

            m_MainPlayer = mainPlayer;
            //init go
            m_MapGo = Global.gApp.gResMgr.InstantiateObj("Prefabs/Map0001");
            m_MapGo.transform.SetParent(Global.gApp.gMapNode.transform, false);
            // init mapData
            BoxCollider2D[] colliderCmp = m_MapGo.GetComponentsInChildren<BoxCollider2D>(); 
            InitMapData(width, height,ref colliderCmp);
            // init AStartTools
            m_AStartTools = new AStartPathFind(m_MapData, width, height);
            //CheckMapData();
        }
        // 生成 A* 
        public Location GeneratePursueRoad(GameObject monster)
        {
            Vector3 monsterPos = monster.transform.position;
            Vector3 mainRolePos = m_MainPlayer.transform.position;

            int startX  = m_MapHeight - Mathf.CeilToInt(monsterPos.y + m_MapHeight / 2 * MapCell.CellSize);
            int startY = Mathf.FloorToInt(monsterPos.x + m_MapWidth / 2 * MapCell.CellSize);
            int destX = m_MapHeight - Mathf.CeilToInt(mainRolePos.y + m_MapHeight / 2 * MapCell.CellSize);
            int destY = Mathf.FloorToInt(mainRolePos.x + m_MapWidth / 2 * MapCell.CellSize);
            return m_AStartTools.FindPath(startX, startY, destX, destY);
        }
        public List<List<MapCell>> GetMapData()
        {
            return m_MapData;
        }

        public MapCell GetMapCell(int X,int Y)
        {
            return m_MapData[X][Y];
        }

        // 初始化地图数据
        private void InitMapData(int width, int height, ref BoxCollider2D[] colliderCmp)
        {
            GenerateMap(height);
            float startX = -width / 2.0f + 0.5f;
            float startY = height / 2.0f - 0.5f;
            for (int hIndex = 0; hIndex < height; hIndex++) // 行 o 行 到最大行 有多少行由高度决定
            {
                float posX = startX;
                for (int wIndex = 0; wIndex < width; wIndex++) // 列 o 列 到最大列 有多少列 由 宽度决定
                {
                    GemerateMapCell(hIndex, wIndex, posX, startY, ref colliderCmp);
                    posX = posX + MapCell.CellSize;
                }
                startY = startY - 1;
            }
        }
        // 生成地图数据也就是 一行一个 list
        private void GenerateMap(int height)
        {
            for (int hIndex = 0; hIndex < height; hIndex++)
            {
                List<MapCell> cellList = new List<MapCell>();
                m_MapData.Add(cellList);
            }
        }
        // 行 列 位置 碰撞盒 行 对应 y坐标 列对应  x坐标
        private void GemerateMapCell(int hIndex, int wIndex, float posX, float posY, ref BoxCollider2D[] colliderCmp)
        {
            List<MapCell> cellList = m_MapData[hIndex];
            bool canEnter = CheckCanEnte(posX, posY, ref colliderCmp);
            MapCell mapCell = new MapCell(hIndex, wIndex, posX, posY, canEnter);
            cellList.Add(mapCell);
        }
        // 检测能否进入
        private bool CheckCanEnte(float posX, float posY, ref BoxCollider2D[] colliderCmp)
        {
            float cellRadio = MapCell.CellRadius;
            bool canEnter = true;
            for (int cIndex = 0; cIndex < colliderCmp.Length; cIndex++)
            {
                BoxCollider2D collider = colliderCmp[cIndex];
                Transform colliderTsf = collider.transform;
                Vector3 eulerAngles = colliderTsf.eulerAngles;
                Vector3 lossyScale = colliderTsf.lossyScale;
                Vector3 position = colliderTsf.position;
                float eulerAnglesZ = eulerAngles.z;
                float degree = -eulerAnglesZ * Mathf.Deg2Rad;

                float dtX = posX - position.x;
                float dtY = posY - position.y;

                if (Mathf.Abs(eulerAnglesZ) > 1)
                {
                    float nDtX = dtX * Mathf.Cos(degree) - dtY * Mathf.Sin(degree);
                    float nDtY = dtY * Mathf.Cos(degree) + dtX * Mathf.Sin(degree);
                    dtX = nDtX;
                    dtY = nDtY;
                }
                else
                {
                    dtX = Mathf.Abs(dtX);
                    dtY = Mathf.Abs(dtY);
                }
                float width = collider.size.x * lossyScale.x;
                float height = collider.size.y * lossyScale.y;
                float radius = MapCell.CellRadius;
                // 圆心与矩形中心的相对距离
                float relativeX = dtX;
                float relativeY = dtY;

                float dx = Mathf.Min(relativeX, (width * 0.5f));
                float dx1 = Mathf.Max(dx, (-width * 0.5f));
                float dy = Mathf.Min(relativeY, (height * 0.5f));
                float dy1 = Mathf.Max(dy, (-height * 0.5f));

                bool isCollision = (dx1 - relativeX) * (dx1 - relativeX) + (dy1 - relativeY) * (dy1 - relativeY) < radius * radius;
                if (isCollision)
                {
                    return false;
                }
                //else if(Mathf.Pow(dtX - collider.size.x / 2,2) + Mathf.Pow(dtY - collider.size.y / 2, 2) > Mathf.Pow(MapCell.CellRadius,2))
                //{
                //    return false;
                //}
            }
            return canEnter;
        }
        #region 测试代码
        public void CheckMapData()
        {
            //GameObject BlueCell = (GameObject)Resources.Load("Prefabs/BlueCell");
            GameObject RedCell = (GameObject)Resources.Load("Prefabs/PinkCell");
            for (int hIndex = 0; hIndex < m_MapData.Count; hIndex++)
            {
                List<MapCell> cellList = m_MapData[hIndex];
                for (int mIndex = 0; mIndex < cellList.Count; mIndex++)
                {
                    MapCell mapCell = cellList[mIndex];
                    if (mapCell.CanEnter)
                    {
                        //GameObject cell = Object.Instantiate(BlueCell);
                        //cell.transform.position = new Vector3(mapCell.PosX, mapCell.PosY, -0f);
                    }
                    else
                    {
                        GameObject cell = Object.Instantiate(RedCell);
                        cell.transform.position = new Vector3(mapCell.PosX, mapCell.PosY, -0.1f);
                        cell.transform.SetParent(Global.gApp.gKeepNode.transform);
                    }
                }
            }
        }
        #endregion
    }
}