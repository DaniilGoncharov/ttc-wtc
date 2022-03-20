using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    [Serializable]
    class Chest
    {
        public List<Item> Items { get; set; }
        public int MapId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Chest(int mapId, int x, int y, List<Item> items = null)
        {
            if (items != null)
            {
                Items = items;
            }
            else
            {
                Items = new List<Item>();
            }
            MapId = mapId;
            X = x;
            Y = y;
        }

        public string[] GetItemNames()
        {
            string[] result = new string[Items.Count];
            for (int i = 0; i < Items.Count; i++)
            {
                result[i] = Items[i].Name;
            }
            return result;
        }

        public void DeleteItem(int index)
        {
            Items.RemoveAt(index);
        }

        public Item GetItemByIndex(int i)
        {
            return Items[i];
        }

        public int GetItemsAmount()
        {
            return Items.Count;
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void AddItems(Item[] items)
        {
            foreach(Item item in items)
            {
                AddItem(item);
            }
        }
    }
}
