using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    public struct Point
    {
        public int x;
        public int y;
        public Point(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    public class Map
    {
        public string name;
        public int[,] transitionTo;
        public char[,] drawnMap;
        public bool[,] passable;
        public Point[] transitionCoords;

        public Map(string[] map, int numberOfMaps)
        {
            int[] connections = MapSolver.ConnectionSolver(map[map.Length - 2]);
            int sizex = map.Length - 1;
            int sizey = 0;
            for (int i = 0; i < sizex - 1; i++) 
                sizey = sizey < map[i].Length ? map[i].Length : sizey;
            transitionCoords = new Point[numberOfMaps + 1];
            //chests = new Chest[sizex, sizey];
            //entities = new Entity[sizex, sizey];
            transitionTo = new int[sizex, sizey];
            for (int i = 0; i < sizex; i++)
            {
                for (int j = 0; j < sizey; j++)
                {
                    transitionTo[i, j] = -1;
                }
            }
            drawnMap = new char[sizex, sizey];
            passable = new bool[sizex, sizey];
            name = map[map.Length - 1];
            drawnMap = MapSolver.MapSplitter(map, sizey, transitionTo, connections, passable);
        }
    }
}
