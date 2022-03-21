using System;

namespace ttc_wtc
{
    [Serializable]
    class Weapon : PutOnItem
    {
        public int Damage { get; set; }
        public int Defense { get; set; }

        public Weapon(string name, PutOnItem.Slot equippmentSlot, int damage, int defense) : base(name, equippmentSlot)
        {
            Damage = damage;
            Defense = defense;
        }
    }
}
