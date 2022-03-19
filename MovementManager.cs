using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    static class MovementManager
    {
        static private bool Transition(Entity entity, bool endless = false)
        {
            int[,] transitionTo = CollectedMaps.GetTransitionsTo(entity.MapId);
            if (endless && transitionTo[entity.X, entity.Y] != -1)
            {
                CollectedMaps.DelEntity(entity.MapId, entity.X, entity.Y);
                entity.X = 1;
                entity.Y = 1;
                entity.MapId = 0;
                CollectedMaps.EndlessInitialise();
                CollectedMaps.SetEntity(entity.MapId, entity.X, entity.Y, entity);
                return true;
            }
            if (transitionTo[entity.X, entity.Y] != -1)
            {
                CollectedMaps.DelEntity(entity.MapId, entity.X, entity.Y);
                int moveToMap = transitionTo[entity.X, entity.Y];
                Draw.CurrentMapId = moveToMap;
                Point transitionCoords = CollectedMaps.GetTransitionCoords(moveToMap, entity.MapId);
                entity.X = transitionCoords.x;
                entity.Y = transitionCoords.y;
                entity.MapId = moveToMap;
                CollectedMaps.SetEntity(entity.MapId, entity.X, entity.Y, entity);
                return true;
            }
            return false;
        }

        public static bool TryMove(Entity entity, int x, int y, bool endless = false)
        {
            bool canMove = CollectedMaps.CanMoveTo(entity.MapId, entity.X + x, entity.Y + y);
            if (canMove)
            {
                Draw.DrawAtPos(entity.X, entity.Y, CollectedMaps.GetDrawnMap(entity.MapId)[entity.X, entity.Y]);
                CollectedMaps.MoveEntity(entity.MapId, entity.X, entity.Y, x, y, entity);
                entity.X += x;
                entity.Y += y;
                Draw.DrawAtPos(entity.X, entity.Y, entity.Symbol);
            }
            if (canMove && Transition(entity, endless))
            {
                if (entity.MapId == Draw.CurrentMapId && entity is Player)
                {
                    Draw.ReDrawMap(CollectedMaps.GetDrawnMap(entity.MapId), entity.MapId);
                }
            }
            return canMove;
        }

        public static int CantMoveDecider(int mapId, int x, int y)
        {
            int cantGoBecause = CollectedMaps.CantMoveBecause(mapId, x, y);
            switch (cantGoBecause)
            {
                case (int)CantGoBecause.Chest:
                    return (int)Game.Status.ChestOpened;
                case (int)CantGoBecause.Friend:
                    return (int)Game.Status.InNPC;
                case (int)CantGoBecause.Enemy:
                    return (int)Game.Status.InBattle;
                case (int)CantGoBecause.Player:
                    return (int)Game.Status.InBattleForEntity;
                default:
                    return (int)Game.Status.InGame;
            }
        }
    }
}
