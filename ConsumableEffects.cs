using System.Collections.Generic;

namespace ttc_wtc
{
    delegate void _ConsumableEffect(Player player);

    static class ConsumableEffects
    {
        public static List<_ConsumableEffect> Effects { get; set; }

        public static void Initialise()
        {
            _ConsumableEffect HealPotion = (Player player) =>
            {
                if (player.HP.MaximumHP - player.HP.CurrentHP <= 500)
                {
                    player.HP = (player.HP.MaximumHP, player.HP.MaximumHP);
                }
                else
                {
                    player.HP = (player.HP.CurrentHP + 500, player.HP.MaximumHP);
                }
            };

            Effects = new List<_ConsumableEffect> { HealPotion };
        }
    }
}
