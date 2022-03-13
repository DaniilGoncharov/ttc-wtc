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
    }
}
