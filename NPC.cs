using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class NPC : Entity
    {
        public List<Item> NPCInventory;

        public NPC(string name, int hp, int damage, int defense, int mapId, int x, int y, List<Item> items = null) :
                 base(name, hp, damage, defense, mapId, x, y, 'N')
        {
            if (items != null)
            {
                NPCInventory = items;
            }
            else
            {
                NPCInventory = new List<Item>();
            }
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
