using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Weapon : PutOnItem
    {
        public int Damage { get; set; }
        public Weapon(string name, PutOnItem.Slot equippmentSlot, int damage) : base(name, equippmentSlot)
        {
            Damage = damage;
        }
    }
}
