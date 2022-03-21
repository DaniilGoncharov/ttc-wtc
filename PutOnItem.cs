using System;

namespace ttc_wtc
{
    [Serializable]
    class PutOnItem : Item
    {
        public enum Slot
        {
            LeftHand = 0,
            RightHand = 1,
            Head = 2,
            Body = 3,
            Legs = 4,
        }

        public PutOnItem.Slot EquippmentSlot { get; set; }

        public PutOnItem(string name, PutOnItem.Slot equippmentSlot) : base(name)
        {
            EquippmentSlot = equippmentSlot;
        }
    }
}
