using System;

namespace ttc_wtc
{
    [Serializable]
    class Enemy : NPC
    {
        public Enemy(string name, int hp, int damage, int defense, int mapId, int x, int y) :
                 base(name, hp, damage, defense, mapId, x, y, '$', null)
        { }
    }
}
