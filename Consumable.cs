using System;

namespace ttc_wtc
{
    [Serializable]
    class Consumable : Item
    {
        public int EffectNumber { get; }
        public const int ConsumableSlot = 5;

        public Consumable(string name, int effect) : base(name)
        {
            EffectNumber = effect;
        }

    }
}
