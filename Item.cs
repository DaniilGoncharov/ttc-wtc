using System;
using System.Collections.Generic;

namespace ttc_wtc
{
    [Serializable]
    class Item
    {
        public enum ItemId
        {
            TrainingShield = 0,
            TrainingSword = 1,
            OldHelmet = 2,
            OldBreastPlate = 3,
            OldGreave = 4,
            HeroesSword = 5,
            HealPotion = 6,
        }

        public string Name { get; set; }
        public static List<Item> Items;

        public Item(string name)
        {
            Name = name;
        }

        public static void Initialise()
        {
            Weapon TrainingSword = new Weapon("Тренировачный меч", PutOnItem.Slot.RightHand, 50, 0);

            Weapon TrainingShield = new Weapon("Тренировачный щит", PutOnItem.Slot.LeftHand, 0, 20);

            Armor OldHelmet = new Armor("Старый шлем", PutOnItem.Slot.Head, 10);

            Armor OldBreastPlate = new Armor("Старый нагрудник", PutOnItem.Slot.Body, 20);

            Armor OldGreave = new Armor("Старые поножи", PutOnItem.Slot.Legs, 15);

            Weapon HeroesSword = new Weapon("Меч героя", PutOnItem.Slot.RightHand, 200, 0);

            Consumable HealPotion = new Consumable("Зелье лечения", 0);

            Items = new List<Item> { TrainingShield, TrainingSword, OldHelmet, OldBreastPlate, OldGreave, HeroesSword, HealPotion };
        }

        public static PutOnItem GenerateItem(int level)
        {
            int slot = new Random().Next(0, 5);
            switch (slot)
            {
                case 0:
                    return new Weapon("Щит " + level, PutOnItem.Slot.LeftHand, 0, 30 + level);
                case 1:
                    return new Weapon("Меч " + level, PutOnItem.Slot.RightHand, 100 + level * 10, 0);
                case 2:
                    return new Armor("Шлем " + level, PutOnItem.Slot.Head, 10 + level);
                case 3:
                    return new Armor("Нагрудник " + level, PutOnItem.Slot.Body, 20 + level);
                case 4:
                    return new Armor("Поножи " + level, PutOnItem.Slot.Legs, 15 + level);
                default:
                    return null;
            }

        }

        public static Key UminekoStone = new Key("Статуэтка чайки");

        public static Key OldKey = new Key("Ключ от старых ворот");

    }
}
