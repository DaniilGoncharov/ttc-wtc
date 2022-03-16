﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Battle
    {
        public Player Player { get; set; }
        public Entity[] Enemies { get; set; }
        public Entity[] AliveEnemies { get; set; }

        public Battle(Player player, Entity[] enemies)
        {
            Player = player;
            Enemies = enemies;
            AliveEnemies = enemies;
        }

        public void Start()
        {
            int target;
            int choice;
            string[] aliveEnemiesNames;
            Menu enemiesMenu;
            while ((AliveEnemies.Length != 0) && (Player.Alive))
            {
                aliveEnemiesNames = GetAliveEnemyNames();
                enemiesMenu = new Menu(aliveEnemiesNames);
                bool end = false;
                while (!end)
                {
                    Draw.DrawBattleInterface(AliveEnemies, Player);
                    choice = Menu.BattleMenu.GetChoice(false, false);
                    switch (choice)
                    {
                        case 0:
                            Draw.DrawBattleInterface(AliveEnemies, Player);
                            target = enemiesMenu.GetChoice(false, false);
                            AliveEnemies[target].GetDamaged(Player.Damage.CurrentDamage);
                            end = true;
                            break;
                        case 1:
                            Draw.DrawBattleInterface(AliveEnemies, Player);
                            target = enemiesMenu.GetChoice(false, false);
                            Tarot.Tarots[Player.TarotNumber].Ability(Player, AliveEnemies, target);
                            end = true;
                            break;
                        case 2:
                            List<string> items = Player.GetNamesBySlot(Consumable.ConsumableSlot);
                            //items.Add("Назад");
                            Menu consumableMenu = new Menu(items);
                            int consumableChoice = consumableMenu.GetChoice();
                            if (consumableChoice < items.Count - 2)
                            {
                                Player.ChangeItemByChoice(consumableChoice, Consumable.ConsumableSlot);
                                end = true;
                            }
                            else
                            {
                                end = false;
                            }
                            break;
                    }
                }
                UpdateAliveEnemies();
                foreach(Enemy enemy in AliveEnemies)
                {
                    if (enemy.Stunned == 0)
                    {
                        Player.GetDamaged(enemy.Damage.CurrentDamage);
                    }
                    else
                    {
                        enemy.Stunned--;
                    }
                }
            }
            if (Player.Alive)
            {
                Program.CurrentGame.GameStatus = Game.Status.InGame;
                foreach(Enemy enemy in Enemies)
                {
                    CollectedMaps.DelEntity(enemy.MapId, enemy.X, enemy.Y);
                }
            }
            else
            {
                Game.StartANewGame();
            }
        }

        public void UpdateAliveEnemies()
        {
            List<Entity> newAliveEnemies = new List<Entity>();
            foreach(Enemy enemy in Enemies)
            {
                if (enemy.Alive)
                {
                    newAliveEnemies.Add(enemy);
                }
            }
            AliveEnemies = newAliveEnemies.ToArray();
        }

        private string[] GetAliveEnemyNames()
        {
            string[] result = new string[AliveEnemies.Length];
            for (int i = 0; i < AliveEnemies.Length; i++)
            {
                result[i] = AliveEnemies[i].Name;
            }
            return result;
        }
    }
}