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
            AllMaps[0].transitionCoords[2] = new Point(1, 1);
            AllMaps[0].transitionTo[1, 1] = 2;
            AllMaps[0].drawnMap[1, 1] = 'E';
            Map Dungeon = Generation.GenerateMap();
            AllMaps.Add(Dungeon);
            Point corner = GetCornerCoords(AllMaps.Count - 1);
            AllMaps[AllMaps.Count - 1].transitionCoords[3] = corner;
            AllMaps[AllMaps.Count - 1].transitionTo[corner.x, corner.y] = 3;
            AllMaps[AllMaps.Count - 1].drawnMap[corner.x, corner.y] = 'E';
            Map Dungeon2 = Generation.GenerateMap();
            AllMaps.Add(Dungeon2);
        }

        static public Point GetCornerCoords(int mapId)
        {
            char[,] map = AllMaps[mapId].drawnMap;
            for(int i = map.GetLength(0) - 1; i > 0; i--)
            {
                for(int j = map.GetLength(1) - 1; j > 0; j--)
                {
                    if (map[i, j] != '#' && map[i, j] != 'C' && map[i, j] != ' ')
                    {
                        return new Point(i, j);
                    }
                }
            }
            return new Point(0, 0);
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

        public static Item GetItemFromNPC(int mapId, int x, int y, int index)
        {
            NPC Npc = (NPC)AllMaps[mapId].Entities[x, y];
            Item result = Npc.NPCInventory[index];
            Npc.NPCInventory.RemoveAt(index);
            return result;
        }

        public static Item[] GetAllItemsFromNPC(int mapId, int x, int y)
        {
            NPC Npc = (NPC)AllMaps[mapId].Entities[x, y];
            Item[] result = new Item[Npc.NPCInventory.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetItemFromNPC(mapId, x, y, 0);
            }
            Npc.GetTiefsItemNames().RemoveAt(0);
            return result;
        }

        public static void MoveEntity(int mapId, int x, int y, int moveX, int moveY, Entity entity)
        {
            DelEntity(mapId, x, y);
            SetEntity(mapId, x + moveX, y + moveY, entity);
        }

        public static List<Entity> GetEntities(int mapId)
        {
            List<Entity> result = new List<Entity>();
            foreach (Entity entity in AllMaps[mapId].Entities)
                if (entity != null)
                    result.Add(entity);
            return result;
        }

        public static Entity GetEntity(int mapId, int x, int y)
        {
            return AllMaps[mapId].Entities[x, y];
        }

        public static Entity[] GetNearEntities(int mapId, int x, int y)
        {
            List<Entity> pResult = new List<Entity>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (AllMaps[mapId].Entities[x + i, y + j] is Enemy)
                    {
                        pResult.Add(AllMaps[mapId].Entities[x + i, y + j]);
                    }
                }
            }
            return pResult.ToArray();
        }

        public static void EnemyMovement(int mapId, int x, int y)
        {
            Entity[,] currentMapEntities = AllMaps[mapId].Entities;
            List<Entity> entitiesToMove = new List<Entity>();
            for (int i = 0; i < currentMapEntities.GetLength(0); i++)
            {
                for (int j = 0; j < currentMapEntities.GetLength(1); j++)
                {
                    if (currentMapEntities[i, j] is Enemy)
                    {
                        entitiesToMove.Add(currentMapEntities[i, j]);
                    }
                }
            }
            foreach (Entity entity in entitiesToMove)
            {
                entity.MoveTowards(x, y);
            }
        }
    }
}
