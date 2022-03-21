using System;
using System.Collections.Generic;
using System.Text;

namespace ttc_wtc
{
    class Program
    {
        public static Game CurrentGame;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
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
            Chest startItemChest = new Chest(1, 14, 3);
            chests.Add(startItemChest);
            return chests;
        }

        public static List<Entity> GenerateStartEntities()
        {
            List<Entity> entities = new List<Entity>();
            NPC ErikaNPC = new NPC("Эрика", 20, 100, 22, 0, 8, 14, 'N',BasicDialogBuilder.EricastartDialogBuilder.dialog);
            ErikaNPC.NPCInventory.Add(Item.OldKey);
            NPC UminekoNPC = new NPC("Чайка", 20, 10, 22, 1, 12, 1, 'U', BasicDialogBuilder.UminekoStartDialogBuilder.dialog);
            UminekoNPC.NPCInventory.Add(Item.OldKey);
            Enemy Kalista = new Enemy("Калиста", 100, 345, 10, 2, 5, 5);
            Enemy OldWarrior1 = new Enemy("Древний Воин", 100, 210, 10, 1, 8, 3); 
            entities.Add(Kalista);
            entities.Add(ErikaNPC);
            entities.Add(UminekoNPC);
            entities.Add(OldWarrior1);
            return entities;
        }
    }
}
