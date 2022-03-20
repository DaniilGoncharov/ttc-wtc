using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ttc_wtc
{
    static class SaveAndLoad
    {
        public static string[] Saves { get; set; }
        public static bool[] EmptySave { get; set; }

        public static void Initialise()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream("saves.dat", FileMode.OpenOrCreate))
            {
                if (fileStream.Length != 0)
                {
                    string[] saves = (string[])formatter.Deserialize(fileStream);
                    bool[] emptySave = (bool[])formatter.Deserialize(fileStream);
                    Saves = saves;
                    EmptySave = emptySave;
                }
                else
                {
                    Saves = new string[3] { "None", "None", "None" };
                    EmptySave = new bool[3] { true, true, true };
                }
            }
        }

        public static void Save(Player player, List<Entity> entities, List<Chest> chests, bool endless)
        {
            List<string> saveMenuItems = new List<string>(Saves);
            saveMenuItems.Add("Выйти");
            Menu saveMenu = new Menu(saveMenuItems);
            int choice = saveMenu.GetChoice();
            if (choice != saveMenuItems.Count - 1)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fileStream = new FileStream("saves.dat", FileMode.OpenOrCreate))
                {
                    Saves[choice] = ("save" + choice + ".dat");
                    EmptySave[choice] = false;
                    formatter.Serialize(fileStream, Saves);
                    formatter.Serialize(fileStream, EmptySave);
                }
                using (FileStream fileStream = new FileStream(Saves[choice], FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fileStream, endless);
                    formatter.Serialize(fileStream, CollectedMaps.AllMaps);
                    formatter.Serialize(fileStream, player);
                    if (!endless)
                    {
                        formatter.Serialize(fileStream, entities);
                        formatter.Serialize(fileStream, chests);
                    }
                }
            }
        }

        public static bool Load(ref Player player, ref List<Entity> entities, ref List<Chest> chests, out bool? endless)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<string> loadMenuItems = new List<string>();
            for (int i = 0; i < Saves.Length; i++)
            {
                if (!EmptySave[i])
                {
                    loadMenuItems.Add(Saves[i]);
                }
            }
            loadMenuItems.Add("Выйти");
            Menu loadMenu = new Menu(loadMenuItems);
            int choice = loadMenu.GetChoice();
            if (choice == loadMenuItems.Count - 1)
            {
                endless = null;
                return false;
            }
            using (FileStream fileStream = new FileStream(Saves[choice], FileMode.OpenOrCreate))
            {
                endless = (bool)formatter.Deserialize(fileStream);
                CollectedMaps.AllMaps = (List<Map>)formatter.Deserialize(fileStream);
                player = (Player)formatter.Deserialize(fileStream);
                if (!(bool)endless)
                {
                    entities = (List<Entity>)formatter.Deserialize(fileStream);
                    chests = (List<Chest>)formatter.Deserialize(fileStream);
                }
                else
                {
                    entities = null;
                    chests = null;
                }
                /*Game.StartGame(player, entities, chests, endless);*/
                return true;
            }
        }
    }
}
