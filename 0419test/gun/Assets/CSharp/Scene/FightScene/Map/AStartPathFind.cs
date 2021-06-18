using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EZ
{
    public class Location
    {
        public int X;
        public int Y;
        public int F;
        public int G;
        public int H;
        public Location Parent;
    }

    public class AStartPathFind
    {
        public List<List<MapCell>> m_MapData;
        private int m_Width;
        private int m_Height;
        public AStartPathFind(List<List<MapCell>> mapData, int width, int height)
        {
            m_MapData = mapData;
            m_Width = width;
            m_Height = height;
        }
        public Location FindPath(int startX, int startY, int destX, int destY)
        {
            int newStartX = destX;
            int newStartY = destY;

            int newDestX = startX;
            int newDestY = startY;

            List<Location> openList = new List<Location>();
            List<Location> closedList = new List<Location>();
            var start = new Location { X = newStartX, Y = newStartY };
            var target = new Location { X = newDestX, Y = newDestY };
            int g = 1;
            Location current = null;
            openList.Add(start);

            while (openList.Count > 0)
            {
                //
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                closedList.Add(current);
                openList.Remove(current);
                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                    break;
                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, newDestX, newDestY);
                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) == null)
                    {
                        // compute its score, set the parent
                        //修改处,不能这样用g
                        //adjacentSquare.G = g;
                        adjacentSquare.G = g + current.G;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X,
                        adjacentSquare.Y, target.X, target.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        //修改处
                        if (/*g*/ current.G + g < adjacentSquare.G)
                        {
                            //修改处,不能这样用g
                            //adjacentSquare.G = g;
                            adjacentSquare.G = g + current.G;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }
            return current;
        }
        List<Location> GetWalkableAdjacentSquares(int x, int y, int destX, int destY)
        {
            var proposedLocations = new List<Location>();
            for (int wIndex = -1; wIndex <= 1; wIndex++)
            {
                int indexX = wIndex + x;
                if (indexX >= 0 && indexX < m_Width)
                {
                    for (int hIndex = -1; hIndex <= 1; hIndex++)
                    {
                        int indexY = hIndex + y;
                        if (indexY >= 0 && indexY < m_Height)
                        {
                            if (wIndex != 0 || hIndex != 0)
                            {
                                proposedLocations.Add(new Location { X = indexX, Y = indexY });
                            }
                        }
                    }
                }
            }
            return proposedLocations.Where(
                l => m_MapData[l.X][l.Y].CanEnter ||(l.X == destX && l.Y == destY)).ToList();
        }

        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }
    }
}
