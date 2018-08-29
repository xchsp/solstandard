﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.Utility;

namespace SolStandard.HUD.Window.Content.Combat
{
    public class Die : IRenderable
    {
        public enum FaceValue
        {
            Blank,
            Shield,
            Sword
        }

        public enum DieFaces
        {
            One,
            Two,
            Three,
            Four,
            Five,
            Six
        }

        private static readonly Dictionary<DieFaces, FaceValue> DieValues =
            new Dictionary<DieFaces, FaceValue>
            {
                {DieFaces.One, FaceValue.Blank},
                {DieFaces.Two, FaceValue.Shield},
                {DieFaces.Three, FaceValue.Shield},
                {DieFaces.Four, FaceValue.Sword},
                {DieFaces.Five, FaceValue.Sword},
                {DieFaces.Six, FaceValue.Sword}
            };

        private readonly SpriteAtlas dieAtlas;
        private DieFaces currentFace;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Die(DieFaces initialFace)
        {
            currentFace = initialFace;
            dieAtlas = new SpriteAtlas(GameDriver.DiceTexture, GameDriver.DiceTexture.Height, 1);
            Height = dieAtlas.Height;
            Width = dieAtlas.Width;
        }

        public FaceValue Roll()
        {
            int randomValue = GameDriver.Random.Next(0, 5);
            currentFace = (DieFaces) randomValue;
            dieAtlas.CellIndex = (int) currentFace + 1;
            return DieValues[currentFace];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            dieAtlas.Draw(spriteBatch, position, color);
        }

        public override string ToString()
        {
            return "Die: {Value=" + currentFace + ", Atlas=" + dieAtlas + "}";
        }
    }
}