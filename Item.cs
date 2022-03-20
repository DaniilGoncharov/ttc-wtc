using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Item
    {
        public string Name { get; set; }

        public Item(string name)
        {
            Name = name;
        }

        public static List<Item> Items = new List<Item>();

        public static Weapon TrainingSword = new Weapon("Тренировачный меч", PutOnItem.Slot.RightHand, 50, 0);

        public static Weapon TrainingShield = new Weapon("Тренировачный щит", PutOnItem.Slot.LeftHand, 0, 20);

        public static Armor OldHelmet = new Armor("Старый шлем", PutOnItem.Slot.Head, 10);

        public static Armor OldBreastPlate = new Armor("Старый нагрудник", PutOnItem.Slot.Body, 20);

        public static Armor OldGreave = new Armor("Старые поножи", PutOnItem.Slot.Legs, 15);

        public static Weapon HeroesSword = new Weapon("Меч героя", PutOnItem.Slot.RightHand, 200, 0);

        public static Consumable HealPotion = new Consumable("Зелье лечения", (Player player) => 
        { 
            if (player.HP.MaximumHP - player.HP.CurrentHP <= 500)
            {
                player.HP = (player.HP.MaximumHP, player.HP.MaximumHP);
            }
            else
            {
                player.HP = (player.HP.CurrentHP + 500, player.HP.MaximumHP);
            }
        });

        public static Keys UminekoStone = new Keys("Статуэтка чайки");

        public static Keys OldKey = new Keys("Ключ от старых ворот");
    }
}
