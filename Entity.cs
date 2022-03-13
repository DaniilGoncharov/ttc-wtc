using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    [Serializable]
    class Entity
    {
        public bool Stunned { get; set; }
        public bool Alive { get; set; }
        public char Symbol { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int MapId { get; set; }
        public (int BasicDamage, int CurrentDamage) Damage { get; set; }
        public (int BasicDefense, int CurrentDefense) Defense { get; set; }
        public (int CurrentHP, int MaximumHP) HP { get; set; }

        public Entity(string name, int hp, int damage, int defense, int mapId, int x, int y, char symb)
        {
            Name = name;
            MapId = mapId;
            X = x;
            Y = y;
            Symbol = symb;
            Alive = true;
            Stunned = false;
            HP = (hp, hp);
            Damage = (damage, damage);
            Defense = (defense, defense);
        }
    }
}
