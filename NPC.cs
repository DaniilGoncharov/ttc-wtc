using System;
using System.Collections.Generic;

namespace ttc_wtc
{
    [Serializable]
    class NPC : Entity
    {
        public List<Item> NPCInventory;
        public Dialog Dialog { get; set; }
        public NPC(string name, int hp, int damage, int defense, int mapId, int x, int y,char symb, Dialog dialog, List<Item> items = null) :
              base(name, hp, damage, defense, mapId, x, y, symb)
        {
            Dialog = dialog;
            if (items != null)
            {
                NPCInventory = items;
            }
            else
            {
                NPCInventory = new List<Item>();
            }
        }

        public Item GetItemFromThisNPC(string name)
        {
            Item GetItem=null;
            if (NPCInventory.Count>0)
            {
                foreach (Item item in NPCInventory)
                {
                    if (item.Name == name)
                    {
                        GetItem = item;
                        NPCInventory.Remove(item);
                        return GetItem;
                    }
                }
            }     
            return GetItem;
        }

        public bool Have(string name)
        {
            if (NPCInventory.Count > 0)
            {
                foreach (Item item in NPCInventory)
                {
                    if (item.Name == name)
                    {
                        return true;
                    }
                }
            }
            return false;
           
        }

        public List<string> GetTiefsItemNames()
        {
            List<string> TiefsItemsName = new List<string>();
            for (int i = 0; i < NPCInventory.Count; i++)
            {
                TiefsItemsName.Add(NPCInventory[i].Name);
            }
            if (NPCInventory.Count > 0)
            {
                TiefsItemsName.Add("Забрать все");
            }
            TiefsItemsName.Add("Выйти");
            return TiefsItemsName;
        }
    }
}
