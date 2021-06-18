using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class MapCell
    {

        public static readonly float CellSize = 1f;
        public static readonly float CellRadius = 0.25f;//内接圆 

        private readonly float m_PosX;
        private readonly float m_PosY;
        private readonly float m_HIndex;
        private readonly float m_WIndex;
        private readonly bool m_CanEnter;

        public float PosX
        {
            get
            {
                return m_PosX;
            }
        }

        public float PosY
        {
            get
            {
                return m_PosY;
            }
        }

        public float HIndex
        {
            get
            {
                return m_HIndex;
            }
        }

        public float WIndex
        {
            get
            {
                return m_WIndex;
            }
        }

        public bool CanEnter
        {
            get
            {
                return m_CanEnter;
            }
        }
        //（m_HIndex，m_WIndex）可以加读取 数组坐标 m_HIndex 表示第几行 m_WIndex 表示横坐标
        public MapCell(int hIndex, int wIndex, float posX, float posY, bool canEnter)
        {
            m_HIndex = hIndex;
            m_WIndex = wIndex;
            m_PosX = posX;
            m_PosY = posY;
            m_CanEnter = canEnter;
        }
    }
}
