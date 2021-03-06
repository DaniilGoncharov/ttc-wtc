using System;
using System.Collections.Generic;

namespace ttc_wtc
{
    static class Generation
    {
        static Point size;

        static bool Inside(Point point)
        {
            return !(point.x >= size.x - 1 || point.x <= 0 || point.y >= size.y - 1 || point.y <= 0);
        }

        static Point Dir(int choice)
        {
            switch (choice)
            {
                case 0:
                    return new Point(1, 0);
                case 1:
                    return new Point(-1, 0);
                case 2:
                    return new Point(0, 1);
                default:
                    return new Point(0, -1);
            }
        }

        public static int[,] Generate(int sizeX, int sizeY)
        {
            int[,] passable = new int[sizeX, sizeY];
            size = new Point(sizeX, sizeY);
            Random rnd = new Random();
            Point current = new Point(1, 1);
            Point move;
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    passable[i, j] = 1;
                }
            }
            for (int i = 0; i < 888; i++)
            {
                passable[current.x, current.y] = (i == 444) || (passable[current.x, current.y] == 2) ? 2 : 0;
                move = Dir(rnd.Next(0, 4));
                while (!Inside(new Point(current.x + move.x, current.y + move.y)))
                {
                    move = Dir(rnd.Next(0, 4));
                }
                current = new Point(current.x + move.x, current.y + move.y);
            }
            return passable;
        }

        static bool[,] ToDeleteWalls(int[,] map)
        {
            bool[,] result = new bool[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    if (CountAdjasentWalls(i, j, map) == 0)
                    {
                        result[i, j] = true;
                    }
                }
            }
            return result;
        }

        static int CountAdjasentWalls(int x, int y, int[,] map)
        {
            List<Point> move = new List<Point> { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };
            if (x == 0)
            {
                move.Remove(new Point(-1, 0));
            }
            else if (x == map.GetLength(0) - 1)
            {
                move.Remove(new Point(1, 0));
            }
            if (y == 0)
            {
                move.Remove(new Point(0, -1));
            }
            else if (y == map.GetLength(1) - 1)
            {
                move.Remove(new Point(0, 1));
            }
            int result = 0;
            for (int i = 0; i < move.Count; i++)
            {
                if (map[x + move[i].x, y + move[i].y] != 1)
                {
                    result++;
                }
            }
            List<Point> additionalMoves = new List<Point>();
            for (int i = 0; i < move.Count; i++)
            {
                for (int j = i + 1; j < move.Count; j++)
                {
                    if (move[i].x != move[j].x)
                    {
                        additionalMoves.Add(new Point(move[i].x + move[j].x, move[i].y + move[j].y));
                    }
                }
            }
            for (int i = 0; i < additionalMoves.Count; i++)
            {
                if (map[x + additionalMoves[i].x, y + additionalMoves[i].y] == 0)
                {
                    result++;
                }
            }
            return result;
        }

        public static int[,] CleanInt(int[,] map)
        {
            bool[,] ToDelete = ToDeleteWalls(map);
            for (int i = 0; i < ToDelete.GetLength(0); i++)
            {
                for (int j = 0; j < ToDelete.GetLength(1); j++)
                {
                    if (ToDelete[i, j])
                    {
                        map[i, j] = -1;
                    }
                }
            }
            return map;
        }

        static char IntToChar(int n)
        {
            switch (n)
            {
                case 0:
                    return '.';
                case 1:
                    return '#';
                case 2:
                    return 'C';
                case 3:
                    return '$';
                default:
                    return ' ';
            }
        }

        static public char[,] IntToCharMap(int[,] map)
        {
            char[,] result = new char[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    result[i, j] = IntToChar(map[i, j]);
                    if (i == 1 && j == 1 && map[i, j] == 2)
                    {
                        return IntToCharMap(PlaceEnemies(CleanInt(Generate(20, 23)), 0));
                    }
                }
            }
            return result;
        }

        static int[,] PlaceEnemies(int[,] map, int amount)
        {
            int[,] result = map;
            List<Point> pointsToConsider = new List<Point>();
            Point set;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        pointsToConsider.Add(new Point(i, j));
                    }
                }
            }
            for (int i = 1; i <= amount; i++)
            {
                set = pointsToConsider[(pointsToConsider.Count - 1) / (amount + 1) * i];
                result[set.x, set.y] = 3;
            }
            return result;
        }
        public static char[,] GenerateCharMap(int sizeX, int sizeY)
        {
            return IntToCharMap(PlaceEnemies(CleanInt(Generate(sizeX, sizeY)), 5));
        }
        public static Map GenerateMap(bool endless)
        {
            return new Map(IntToCharMap(PlaceEnemies(CleanInt(Generate(20, 23)), 3)), 4, endless);
        }
    }
}
