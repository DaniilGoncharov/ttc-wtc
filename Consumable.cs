using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Consumable : Item
    {
        public Player.ConsEffect Effect { get; set; }
        public const int ConsumableSlot = 5;

        public Consumable(string name, Player.ConsEffect effect) : base(name)
        {
            Effect = effect;
        }
    }
}
