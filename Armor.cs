using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    [Serializable]
    class Armor : PutOnItem
    {
        public int Defense { get; set; }
        public Armor(string name, PutOnItem.Slot equippmentSlot, int defense) : base(name, equippmentSlot)
        {
            Defense = defense;
        }
    }
}
