using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
