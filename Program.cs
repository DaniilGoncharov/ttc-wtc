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
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Tokyo Tarot Cards : When They Cry";
            Console.CursorVisible = false;
            Console.SetWindowSize(90, 34);
            Console.SetBufferSize(90, 34);
            Tarot.Initialise();
            SaveAndLoad.Initialise();
            Item.Initialise();
            ConsumableEffects.Initialise();
            HelpFunctions.Haise();
            Game.GetStartMenuChoice();
        }

        public static Player GenerateStartPlayer(bool endless = false)
        {
            Player player = new Player("Player", 0, 0, 0, 0, endless ? 1 : 11, endless ? 1 : 16);
            return player;
        }

        public static List<Chest> GenerateStartChests()
        {
            List<Chest> chests = new List<Chest>();
            Chest startItemChest = new Chest(1, 14, 3);
            startItemChest.AddItem(Item.Items[(int)Item.ItemId.TrainingShield]);
            startItemChest.AddItem(Item.Items[(int)Item.ItemId.TrainingSword]);
            startItemChest.AddItem(Item.Items[(int)Item.ItemId.OldHelmet]);
            startItemChest.AddItem(Item.Items[(int)Item.ItemId.OldBreastPlate]);
            startItemChest.AddItem(Item.Items[(int)Item.ItemId.OldGreave]);
            startItemChest.AddItem(Item.Items[(int)Item.ItemId.HealPotion]);
            chests.Add(startItemChest);
            return chests;
        }

        public static List<Entity> GenerateStartEntities()
        {
            List<Entity> entities = new List<Entity>();
            NPC ErikaNPC = new NPC("Эрика", 3000, 200, 20, 0, 8, 14, 'N',BasicDialogBuilder.EricastartDialogBuilder.Dialog);
            ErikaNPC.NPCInventory.Add(Item.Items[(int)Item.ItemId.OldKey]);
            NPC UminekoNPC = new NPC("Чайка", 20, 10, 22, 1, 12, 1, 'U', BasicDialogBuilder.UminekoStartDialogBuilder.Dialog);
            UminekoNPC.NPCInventory.Add(Item.Items[(int)Item.ItemId.OldKey]);
            Enemy Kalista = new Enemy("Калиста", 3000, 300, 30, 2, 5, 5);
            Enemy OldWarrior1 = new Enemy("Древний Воин", 1000, 200, 10, 1, 8, 3); 
            entities.Add(Kalista);
            entities.Add(ErikaNPC);
            entities.Add(UminekoNPC);
            entities.Add(OldWarrior1);
            return entities;
        }
    }
}
