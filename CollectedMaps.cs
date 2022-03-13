using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    public enum CantGoBecause
    {
        Wall = 0,
        Chest = 1,
        Enemy = 2,
        Friend = 3,
        Player = 4,
    }

    static class CollectedMaps
    {
        static public List<Map> AllMaps { get; set; }

        static public void Initialise()
        {
            AllMaps = MapSolver.MapCollector();
        }

        static public char[,] GetDrawnMap(int mapId)
        {
            return AllMaps[mapId].drawnMap;
        }

        static public string GetMapName(int mapId)
        {
            return AllMaps[mapId].name;
        }

        static public int[,] GetTransitionsTo(int mapId)
        {
            return AllMaps[mapId].transitionTo;
        }

        static public Point GetTransitionCoords(int fromMapId, int toMapId)
        {
            return AllMaps[fromMapId].transitionCoords[toMapId];
        }

        static public bool[,] GetPassable(int mapId)
        {
            return AllMaps[mapId].passable;
        }

        static public bool CanMoveTo(int mapId, int x, int y)
        {
            return AllMaps[mapId].passable[x, y];
        }

        static public int CantMoveBecause(int mapId, int x, int y)
        {
            if (AllMaps[mapId].Chests[x, y] != null)
            {
                return (int)CantGoBecause.Chest;
            }
            else if (AllMaps[mapId].Entities[x, y] != null)
            {
                if (AllMaps[mapId].Entities[x, y] is Enemy)
                {
                    return (int)CantGoBecause.Enemy;
                }
                else if (AllMaps[mapId].Entities[x, y] is NPC)
                {
                    return (int)CantGoBecause.Friend;
                }
                else if (AllMaps[mapId].Entities[x, y] is Player)
                {
                    return (int)CantGoBecause.Player;
                }
            }
            return (int)CantGoBecause.Wall;
        }

        static public bool ChestHere(int mapId, int x, int y)
        {
            if (AllMaps[mapId].Chests[x, y] != null) return true;
            return false;
        }

        static public void SetChest(int mapId, int x, int y, Chest chest)
        {
            if (!CollectedMaps.ChestHere(mapId, x, y))
            {
                AllMaps[mapId].Chests[x, y] = chest;
                AllMaps[mapId].passable[x, y] = false;
                AllMaps[mapId].drawnMap[x, y] = 'C';
            }
        }

        public static string[] GetChestItems(int mapId, int x, int y)
        {
            string[] chestItems = AllMaps[mapId].Chests[x, y].GetItemNames();
            int emptyChest = chestItems.Length == 0 ? 0 : 1;
            string[] result = new string[chestItems.Length + emptyChest + 1];
            for (int i = 0; i < chestItems.Length; i++)
            {
                result[i] = chestItems[i];
            }
            if (emptyChest == 1)
            {
                result[^2] = "Забрать все";
            }
            result[^1] = "Вернуться в игру";
            return result;
        }

        public static Item GetItemFromChest(int mapId, int x, int y, int index)
        {
            Chest chest = AllMaps[mapId].Chests[x, y];
            Item result = chest.GetItemByIndex(index);
            chest.DeleteItem(index);
            return result;
        }

        public static Item[] GetAllItemsFromChest(int mapId, int x, int y)
        {
            Chest chest = AllMaps[mapId].Chests[x, y];
            Item[] result = new Item[chest.GetItemsAmount()];
            for (int i = 0; i < result.Length; i++)
                result[i] = GetItemFromChest(mapId, x, y, 0);
            return result;
        }

        public static void SetEntity(int mapId, int x, int y, Entity entity)
        {
            AllMaps[mapId].Entities[x, y] = entity;
            AllMaps[mapId].passable[x, y] = false;
        }

        public static void DelEntity(int mapId, int x, int y)
        {
            AllMaps[mapId].Entities[x, y] = null;
            AllMaps[mapId].passable[x, y] = true;
        }
    }
}
