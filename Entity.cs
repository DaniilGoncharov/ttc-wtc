using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Entity
    {
        public int Stunned { get; set; }
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
            Stunned = 0;
            HP = (hp, hp);
            Damage = (damage, damage);
            Defense = (defense, defense);
        }

        public bool Move(int dirX, int dirY)
        {
            int startingMapId = MapId;
            if (!MovementManager.TryMove(this, dirX, dirY))
            {
                int gameStatus = MovementManager.CantMoveDecider(MapId, X + dirX, Y + dirY);
                if (this is Player)
                {
                    Program.CurrentGame.GameStatus = (Game.Status)gameStatus;
                }
                else if (this is Enemy)
                {
                    if (gameStatus == (int)Game.Status.InBattleForEntity)
                    {
                        Program.CurrentGame.GameStatus = Game.Status.InBattle;
                    }
                }
                return false;
            }
            return startingMapId == MapId;
        }

        public void MoveTowards(int x, int y)
        {
            Point direction = Pathfinder.GetPath(X, (Y + 1) / 2, x, (y + 1) / 2, CollectedMaps.GetPassable(MapId));
            Move(direction.x, direction.y);
        }

        public int GetDamaged(int damage, bool ignoreArmor = false)
        {
            int damageRecieved;
            if (!ignoreArmor)
            {
                double percentBlocked = (Defense.CurrentDefense * 0.01) / (1 + Defense.CurrentDefense * 0.01);
                damageRecieved = (int)Math.Round(Convert.ToDouble(damage) * (1 - percentBlocked));
            }
            else damageRecieved = damage;
            if (damageRecieved >= HP.CurrentHP)
            {
                HP = (0, HP.MaximumHP);
                Alive = false;
            }
            else
            {
                HP = (HP.CurrentHP - damageRecieved, HP.MaximumHP);
            }
            return damageRecieved;
        }
    }
}
