using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    delegate void _Ability(Player player, Entity[] entity, int numberOfEnemy = 0);

    class Tarot
    {
        public int HP { get; set; }
        public int Defense { get; set; }
        public int Damage { get; set; }
        public _Ability Ability { get; set; }
        public static List<Tarot> Tarots;

        public Tarot(int hP, int defense, int damage, _Ability ability)
        {
            HP = hP;
            Defense = defense;
            Damage = damage;
            Ability = ability;
        }

        public static void Initialise()
        {
            Tarot theFool = new Tarot(2500, 5, 100, (Player player, Entity[] entities, int numberOfEnemy) =>
            {
                entities[numberOfEnemy].GetDamaged(300);
            });

            Tarot magician = new Tarot(3000, 20, 50, (Player player, Entity[] entities, int numberOfEnemy) =>
            {
                foreach (Entity entity in entities)
                {
                    entity.GetDamaged(200);
                }
            });

            Tarot ZAWARUDO = new Tarot(2000, 15, 200, (Player player, Entity[] entities, int numberOfEnemy) =>
            {
                foreach (Entity entity in entities)
                {
                    entity.Stunned = 3;
                }
            });

            Tarots = new List<Tarot> { theFool, magician, ZAWARUDO };
        }
    }
}
