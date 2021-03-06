using System;
using System.Collections.Generic;
using System.Linq;

namespace ttc_wtc
{
    [Serializable]
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

    [Serializable]
    class Map
    {
        public string name;
        public int[,] transitionTo;
        public char[,] drawnMap;
        public bool[,] passable;
        public Point[] transitionCoords;
        public Entity[,] Entities;
        public Chest[,] Chests;
        public List<Chest> NotEmptyChests = new List<Chest>();
        public Map(string[] map, int numberOfMaps)
        {
            map = HelpFunctions.MST(map);
            int[] connections = MapSolver.ConnectionSolver(map[^2]);
            int sizex = map.Length - 1;
            int sizey = 0;
            for (int i = 0; i < sizex - 1; i++)
            {
                sizey = sizey < map[i].Length ? map[i].Length : sizey;
            }
            transitionCoords = new Point[numberOfMaps + 1];
            Chests = new Chest[sizex, sizey];
            Entities = new Entity[sizex, sizey];
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
            name = map[^1];
            drawnMap = MapSolver.MapSplitter(map, sizey, transitionTo, connections, passable);
            foreach (var chest in Chests)
            {
                if (chest != null)
                {
                    NotEmptyChests.Add(chest);
                }
            }
        }

        public Map(char[,] tiles, int numberOfMaps, bool endless)
        {
            int sizeX = tiles.GetLength(0);
            int sizeY = tiles.GetLength(1);
            passable = new bool[sizeX, sizeY];
            Chests = new Chest[sizeX, sizeY];
            Entities = new Entity[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (tiles[i, j] == '#' || tiles[i, j] == ' ')
                    {
                        passable[i, j] = false;
                    }
                    else
                    {
                        passable[i, j] = true;
                    }
                    if (tiles[i, j] == '$')
                    {
                        Entities[i, j] = new Enemy("Вейн", 1000, 50, 4, CollectedMaps.AllMaps.Count(), i, j);
                        tiles[i, j] = '.';
                    }
                    else if (tiles[i, j] == 'C')
                    {
                        Chest chest = new Chest(CollectedMaps.AllMaps.Count(), i, j);
                        if (!endless)
                        {
                            if (CollectedMaps.AllMaps.Count() == 3)
                            {
                                chest.AddItem(Item.Items[(int)Item.ItemId.HeroesSword]);
                                chest.AddItem(Item.Items[(int)Item.ItemId.HealPotion]);
                            }
                            else
                            {
                                chest.AddItem(Item.Items[(int)Item.ItemId.UminekoStone]);
                            }
                        }
                        else
                        {
                            chest.AddItem(Item.GenerateItem(Program.CurrentGame.player.EndlessLevel));
                            chest.AddItem(Item.Items[(int)Item.ItemId.HealPotion]);
                        }
                        Chests[i, j] = chest;
                        passable[i, j] = false;
                    }
                }
            }
            transitionCoords = new Point[numberOfMaps + 1];
            drawnMap = tiles;
            transitionTo = new int[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    transitionTo[i, j] = -1;
                }
            }
            if (!endless)
            {
                drawnMap[1, 1] = 'E';
                if (CollectedMaps.AllMaps.Count == 3)
                {
                    transitionTo[1, 1] = 0;
                    transitionCoords[0] = new Point(1, 1);
                }
                else
                {
                    transitionTo[1, 1] = 3;
                    transitionCoords[3] = new Point(1, 1);
                }
            }

            name = "Данж";
            foreach (var chest in Chests)
            {
                if (chest != null)
                {
                    NotEmptyChests.Add(chest);
                }

                if (!endless)
                {
                    name = "Данж";
                }
                else if (Program.CurrentGame.player.EndlessLevel == 0)
                {
                    name = "Данж 0";
                }
                else
                {
                    name = "Данж " + Program.CurrentGame.player.EndlessLevel;
                }
            }
        }
    }
}
