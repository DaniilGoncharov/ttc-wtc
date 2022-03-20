﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{


    [Serializable]
    class Enemy : NPC

    {
        public Enemy(string name, int hp, int damage, int defense, int mapId, int x, int y) :
                 base(name, hp, damage, defense, mapId, x, y, '$',null)
        { }
    }
}
