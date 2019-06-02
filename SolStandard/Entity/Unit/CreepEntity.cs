using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Utility;

namespace SolStandard.Entity.Unit
{
    public class CreepEntity : UnitEntity
    {
        private IRenderable routineIcon;

        public CreepEntity(string name, string type, UnitSpriteSheet spriteSheet, Vector2 mapCoordinates,
            bool isCommander, Dictionary<string, string> tiledProperties)
            : base(name, type, spriteSheet, mapCoordinates, isCommander, tiledProperties)
        {
        }

        public void UpdateRoutineIcon(IRoutine routine)
        {
            routineIcon = routine.MapIcon.Clone();
        }

        private static Vector2 CenterTopOfTile(Vector2 tileCoordinates, float iconSize)
        {
            Vector2 centerCoordinates = tileCoordinates * GameDriver.CellSize;
            centerCoordinates.X += (float) GameDriver.CellSize / 2;
            centerCoordinates.X -= iconSize / 2;
            centerCoordinates.Y -= iconSize / 2;
            return centerCoordinates;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            if (routineIcon != null)
            {
                routineIcon.Draw(spriteBatch, CenterTopOfTile(MapCoordinates, routineIcon.Width));
            }
        }
    }
}