using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    static class Draw
    {
        public const int xOffSet = 5;
        public const int yOffSet = 2;
        public static int CurrentMapId { get; set; }

        public static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.SetCursorPosition(xOffSet, yOffSet + i);
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j])
                    {
                        case '#':
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case '.':
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case '$':
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 'C':
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case 'E':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;

                    }
                    Console.Write("{0} ", map[i, j]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public static void DrawAtPos(int x, int y, char symbol)
        {
            Console.SetCursorPosition(x * 2 + xOffSet, y + yOffSet);
            switch (symbol)
            {
                case '@':
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 'N':
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 'U':
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case '$':
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 'E':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.WriteLine(symbol);
            Console.ResetColor();
        }

        public static void ReDrawMap(char[,] drawnMap, int mapId)
        {
            Console.Clear();
            DrawMap(HelpFunctions.MT(drawnMap));
            foreach (Entity entity in CollectedMaps.GetEntities(mapId))
            {
                DrawAtPos(entity.X, entity.Y, entity.Symbol);
            }
        }

        public static void DrawMapInterface(Player player, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("ЛОКАЦИЯ:{0}", CollectedMaps.GetMapName(player.MapId));
            Console.SetCursorPosition(x, y + 1);
            Console.WriteLine("ЗДОРОВЬЕ:{0}/{1}", player.HP.CurrentHP, player.HP.MaximumHP);
            Console.SetCursorPosition(x, y + 2);
            Console.WriteLine("УРОН:{0}", player.Damage.CurrentDamage);
            Console.SetCursorPosition(x, y + 3);
            Console.WriteLine("ЗАЩИТА:{0}", player.Defense.CurrentDefense);
            Console.SetCursorPosition(x, y + 4);
            Console.WriteLine("{0}", player.Quests[player.QuestNumber].questValue);

        }

        public static void DrawBattleInterface(Entity[] enemy, Player player)
        {
            Console.Clear();
            for (int i = enemy.Length - 1; i > -1; i--)
            {
                Console.SetCursorPosition(53, 30 - i * 5);
                Console.WriteLine(enemy[i].Name);
                Console.SetCursorPosition(53, 31 - i * 5);
                Console.WriteLine("УРОН:{0}", enemy[i].Damage.CurrentDamage);
                Console.SetCursorPosition(53, 32 - i * 5);
                Console.WriteLine("ЗДОРОВЬЕ:{0}/{1}", enemy[i].HP.CurrentHP, enemy[i].HP.MaximumHP);
                Console.SetCursorPosition(53, 33 - i * 5);
                Console.WriteLine("ЗАЩИТА:{0}", enemy[i].Defense.CurrentDefense);
            }
            Console.SetCursorPosition(1, 29);
            Console.WriteLine(player.AbilityCD == 0 ? "СПОСОБНОСТЬ ГОТОВА" : "СПОСОБНОСТЬ БУДЕТ ГОТОВА ЧЕРЕЗ {0}", player.AbilityCD);
            Console.SetCursorPosition(1, 30);
            Console.WriteLine("УРОН:{0}", player.Damage.CurrentDamage);
            Console.SetCursorPosition(1, 31);
            Console.WriteLine("ЗДОРОВЬЕ:{0}/{1}", player.HP.CurrentHP, player.HP.MaximumHP);
            Console.SetCursorPosition(1, 32);
            Console.WriteLine("ЗАЩИТА:{0}", player.Defense.CurrentDefense);
        }
    }
}
