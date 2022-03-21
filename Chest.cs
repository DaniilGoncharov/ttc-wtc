using System;
using System.Collections.Generic;

namespace ttc_wtc
{
    [Serializable]
    class Chest
    {
        public List<Item> Items { get; }
        public int MapId { get; }
        public int X { get; }
        public int Y { get; }

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
            foreach (Item item in items)
            {
                AddItem(item);
            }
        }
    }
}
