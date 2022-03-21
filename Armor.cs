using System;

namespace ttc_wtc
{
    [Serializable]
    class Armor : PutOnItem
    {
        public int Defense { get; }
        public Armor(string name, PutOnItem.Slot equippmentSlot, int defense) : base(name, equippmentSlot)
        {
            Defense = defense;
        }
    }
}
