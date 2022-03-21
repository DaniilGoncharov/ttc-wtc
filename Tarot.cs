using System.Collections.Generic;

namespace ttc_wtc
{
    delegate void _Ability(Player player, Entity[] entity, int numberOfEnemy = 0);

    class Tarot
    {
        public int HP { get; set; }
        public int Defense { get; set; }
        public int Damage { get; set; }
        public _Ability Ability { get; set; }
        public bool Target { get; set; }

        public static List<Tarot> Tarots;

        public Tarot(int hp, int defense, int damage, _Ability ability, bool target)
        {
            HP = hp;
            Defense = defense;
            Damage = damage;
            Ability = ability;
            Target = target;
        }

        public static void Initialise()
        {
            Tarot theFool = new Tarot(2500, 5, 100, (Player player, Entity[] entities, int numberOfEnemy) =>
            {
                foreach (Entity entity in entities)
                {
                    entity.GetDamaged(200);
                }
                player.AbilityCD = 3;
            }, false);

            Tarot silverChariot = new Tarot(3000, 20, 50, (Player player, Entity[] entities, int numberOfEnemy) =>
            {
                entities[numberOfEnemy].GetDamaged(500, true);
                player.AbilityCD = 2;
            }, true);

            Tarot ZAWARUDO = new Tarot(2000, 15, 200, (Player player, Entity[] entities, int numberOfEnemy) =>
            {
                foreach (Entity entity in entities)
                {
                    entity.Stunned = 3;
                }
                player.AbilityCD = 6;
            }, false);

            Tarots = new List<Tarot> { theFool, silverChariot, ZAWARUDO };
        }
    }
}
