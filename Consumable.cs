using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    [Serializable]
    class Consumable : Item
    {
        public int EffectNumber { get; set; }
        public const int ConsumableSlot = 5;

        public Consumable(string name, int effect) : base(name)
        {
            EffectNumber = effect;
        }

    }
}
