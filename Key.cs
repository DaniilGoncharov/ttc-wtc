using System;

namespace ttc_wtc
{
    [Serializable]
    class Key : Item
    {
        public const int KeySlot = 6;

        public Key(string name) : base(name)
        { }
    }
}
