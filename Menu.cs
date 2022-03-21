using System;
using System.Collections.Generic;

namespace ttc_wtc
{

    class Menu
    {
        public static Menu StartMenu = new Menu(new string[] { "Сюжетная игра", "Бесконечный режим", "Загрузить", "Выйти" });
        public static Menu PauseMenu = new Menu(new string[] { "Продолжить игру", "Сохранить", "Загрузить", "Выйти в главное меню" });
        public static Menu TarotMenu = new Menu(new string[] { "The Fool", "Silver Chariot", "The World" });
        public static Menu NPCMenu = new Menu(new string[] { "Поговорить", "Обокрасть", "Ударить", "Выйти" });
        public static Menu BattleMenu = new Menu(new string[] { "АТАКОВАТЬ", "ИСПОЛЬЗОВАТЬ СПОСОБНОСТЬ", "ОТКРЫТЬ ИНВЕНТАРЬ" });

        public string[] MenuItems { get; set; }
        public int Cursor { get; set; }

        public Menu(string[] menuItems)
        {
            MenuItems = menuItems;
        }

        public Menu(List<string> menuItems)
        {
            MenuItems = menuItems.ToArray();
        }
        public void ClearMenu(int x, int y, int length)
        {
            Console.SetCursorPosition(x, y);
            for (int i = 0; i < length; i++)
            {
                for (int z = 0; z < 222; z++)
                {
                    Console.Write(" ");
                }

                Console.SetCursorPosition(x, y + 1 + i);
            }
        }
        public int GetChoice(bool centre = true, bool clearConsole = true)
        {
            Cursor = 0;
            if (clearConsole) Console.Clear();
            ConsoleKeyInfo key;
            bool exit = false;
            do
            {
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    Console.SetCursorPosition(centre ? (45 - ((MenuItems[i].Length + 1) / 2)) : 4, centre ? (16 - ((MenuItems.Length - 1) / 2) + i) : (2 + i));
                    if (Cursor == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    Console.WriteLine(MenuItems[i]);
                    Console.ResetColor();
                }
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) exit = true;
                else
                {
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        Cursor--;
                        if (Cursor == -1) Cursor = MenuItems.Length - 1;
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        Cursor++;
                        if (Cursor == MenuItems.Length) Cursor = 0;
                    }
                }
            } while (!exit);
            return Cursor;
        }
        public int GetChoice(bool centre, bool clearConsole, int x, int y)
        {
            Cursor = 0;
            if (clearConsole) Console.Clear();
            ConsoleKeyInfo key;
            bool exit = false;
            do
            {
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    if (Cursor == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    Console.WriteLine(MenuItems[i]);
                    Console.ResetColor();
                }
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) exit = true;
                else
                {
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        Cursor--;
                        if (Cursor == -1) Cursor = MenuItems.Length - 1;
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        Cursor++;
                        if (Cursor == MenuItems.Length) Cursor = 0;
                    }
                }
            } while (!exit);
            return Cursor;
        }
    }
}
