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
            //CollectedMaps.Initialise();
            //Game.StartANewGame();
            Game.GetStartMenuChoice();
        }

        public static Player GenerateStartPlayer()
        {
            Player player = new Player("Player", 0, 0, 0, 0, 2, 2);
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
            NPC npc1 = new NPC("Эрика", 20, 100, 22, 0, 3, 2, 'N',BasicDialogBuilder.EricastartDialogBuilder.dialog);
            npc1.NPCInventory.Add(Item.OldKey);
            NPC npc2 = new NPC("Чайка", 20, 10, 22, 1, 3, 2, 'U', BasicDialogBuilder.UminekoStartDialogBuilder.dialog);
            npc2.NPCInventory.Add(Item.OldKey);
            Enemy Volibir = new Enemy("Волибир", 100, 100, 10, 1, 5, 5);
            entities.Add(Volibir);
            entities.Add(npc1);
            entities.Add(npc2);
            return entities;
        }
    }
}
