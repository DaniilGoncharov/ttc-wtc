using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Player : Entity
    {
        public delegate void ConsEffect(Player player);

        public List<Item> Items;
        public PutOnItem[] EquippedItems { get; set; }
        public int TarotNumber { get; set; }

        public Player(string name, int hp, int damage, int defense, int mapId, int x, int y) :
                 base(name, hp, damage, defense, mapId, x, y, '@')
        {
            Items = new List<Item>();
            EquippedItems = new PutOnItem[5];
            Draw.CurrentMapId = MapId;
            TarotNumber = -1;
        }

        public Player() : base("Player", 0, 0, 0, 0, 6, 6, '@') { }

        public void SelectTarot(int tarotNumber)
        {
            if (TarotNumber == -1)
            {
                TarotNumber = tarotNumber;
                Tarot.Initialise();
                HP = (Tarot.Tarots[tarotNumber].HP, Tarot.Tarots[tarotNumber].HP);
                Damage = (Tarot.Tarots[tarotNumber].Damage, Tarot.Tarots[tarotNumber].Damage);
                Defense = (Tarot.Tarots[tarotNumber].Defense, Tarot.Tarots[tarotNumber].Defense);
            }
        }

        public List<string> GetInventorySlotNames()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < EquippedItems.Length; i++)
            {
                result.Add((PutOnItem.Slot)i + ": " + (EquippedItems[i] != null ? EquippedItems[i].Name : "None"));
            }
            result.Add("Consumables");
            result.Add("Other");
            result.Add("Вернуться к игре");
            return result;
        }

        public List<string> GetNamesBySlot(int slot)
        {
            List<string> result = new List<string>();
            result.Add("None");
            if (Items != null)
            {
                foreach (Item item in Items)
                {
                    if (item is PutOnItem)
                    {
                        PutOnItem putOnItem = item as PutOnItem;
                        if ((int)putOnItem.EquippmentSlot == slot)
                        {
                            result.Add(putOnItem.Name);
                        }
                    }
                    else if (item is Consumable)
                    {
                        if (slot == Consumable.ConsumableSlot)
                        {
                            result.Add(item.Name);
                        }
                    }
                }
            }
            return result;
        }

        public void ChangeItemByChoice(int choice, int slot)
        {
            List<Item> slotItems = new List<Item>();
            if (Items != null)
            {
                foreach (Item item in Items)
                {
                    if (item is PutOnItem)
                    {
                        PutOnItem putOnItem = item as PutOnItem;
                        if ((int)putOnItem.EquippmentSlot == slot)
                        {
                            slotItems.Add(putOnItem);
                        }
                    }
                    else if (item is Consumable)
                    {
                        if (slot == Consumable.ConsumableSlot)
                        {
                            slotItems.Add(item);
                        }
                    }
                }
            }
            if (slot < 3)
            {
                EquippedItems[slot] = (Weapon)slotItems[choice - 1];
            }
            else if (slot == Consumable.ConsumableSlot)
            {
                ((Consumable)slotItems[choice - 1]).Effect(this);
                DeleteItem(slotItems[choice - 1]);
            }
            else EquippedItems[slot] = (Armor)slotItems[choice - 1];
        }

        public void DeleteItem(Item item)
        {
            Items.Remove(item);
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void CountStatsByItems()
        {
            Damage = (Damage.BasicDamage, Damage.BasicDamage + CountDamage());
            Defense = (Defense.BasicDefense, Defense.BasicDefense + CountDefense());
        }

        public void AddItems(Item[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                AddItem(items[i]);
            }
        }

        public int CountDamage()
        {
            int damage = 0;
            for (int i = 0; i < 2; i++)
            {
                if (EquippedItems[i] != null)
                {
                    Weapon weapon = EquippedItems[i] as Weapon;
                    damage += weapon.Damage;
                }
            }
            return damage;
        }

        public int CountDefense()
        {
            int defense = 0;
            for (int i = 2; i < 4; i++)
            {
                if (EquippedItems[i] != null)
                {
                    Armor armor = EquippedItems[i] as Armor;
                    defense += armor.Defense;
                }
            }
            return defense;
        }
    }
}
