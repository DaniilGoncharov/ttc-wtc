using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    static class CollectedMaps
    {
        static private List<Map> allMaps;

        static public void Initialise()
        {
            allMaps = MapSolver.MapCollector();
            //allMaps[0].transitionCoords[3] = new Point(1, 1);
            //allMaps[0].transitionTo[1, 1] = 3;
            //allMaps[0].drawnMap[1, 1] = 'E';
            //allMaps.Add(Generation.GenerateMap());
        }

        static public char[,] GetDrawnMap(int mapId)
        {
            return allMaps[mapId].drawnMap;
        }

        static public string GetMapName(int mapId) 
        {
            return allMaps[mapId].name;
        }

        static public int[,] GetTransitionsTo(int mapId)
        {                                               
            return allMaps[mapId].transitionTo;
        }

        static public Point GetTransitionCoords(int fromMapId, int toMapId) //Возвращает точку перехода
        {
            return allMaps[fromMapId].transitionCoords[toMapId];
        }

        static public bool[,] GetPassable(int mapId)
        {
            return allMaps[mapId].passable;
        }
    }
}
