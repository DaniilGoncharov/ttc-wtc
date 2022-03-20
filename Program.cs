using System;
using System.Collections.Generic;

namespace ttc_wtc
{
    class Program
    {
        public static Game CurrentGame;

        static void Main(string[] args)
        {
            Console.Title = "Tokyo Tarot Cards : When They Cry";
            Console.CursorVisible = false;
            Console.SetWindowSize(90, 34);
            Console.SetBufferSize(90, 34);
            Tarot.Initialise();
            SaveAndLoad.Initialise();
            Item.Initialise();
            ConsumableEffects.Initialise();
            Game.GetStartMenuChoice();
        }

        public static Player GenerateStartPlayer(bool endless = false)
        {
            Player player = new Player("Player", 0, 0, 0, 0, endless ? 1 : 2, endless ? 1 : 2);
            return player;
        }

        public static List<Chest> GenerateStartChests()
        {
            List<Chest> chests = new List<Chest>();
            return chests;
        }

        public static List<Entity> GenerateStartEntities()
        {
            List<Entity> entities = new List<Entity>();
            Enemy Volibir = new Enemy("Волибир", 1000, 100, 10, 1, 5, 5);
            entities.Add(Volibir);
            return entities;
        }
    }
}
