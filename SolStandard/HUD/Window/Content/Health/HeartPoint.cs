﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.Entity.Unit;
using SolStandard.Utility;

namespace SolStandard.HUD.Window.Content.Health
{
    public class HeartPoint : IResourcePoint
    {
        public bool Active { get; set; }
        private SpriteAtlas activeSprite;
        private SpriteAtlas inactiveSprite;

        public HeartPoint(Vector2 size)
        {
            Size = size;
        }

        public Vector2 Size
        {
            set
            {
                activeSprite = UnitStatistics.GetSpriteAtlas(Stats.Hp, value);
                inactiveSprite = UnitStatistics.GetSpriteAtlas(Stats.EmptyHp, value);
            }
        }

        public int Height
        {
            get { return activeSprite.Height; }
        }

        public int Width
        {
            get { return activeSprite.Width; }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color colorOverride)
        {
            if (Active)
            {
                activeSprite.Draw(spriteBatch, position, colorOverride);
            }
            else
            {
                inactiveSprite.Draw(spriteBatch, position, colorOverride);
            }
        }

        public override string ToString()
        {
            return "Heart: { Active=" + Active + "}";
        }
    }
}