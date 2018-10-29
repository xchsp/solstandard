﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.Utility.Monogame;

namespace SolStandard.Utility
{
    public class AnimatedSprite : IRenderable
    {
        protected readonly ITexture2D SpriteMap;
        protected readonly int CellSize;
        protected Vector2 RenderSize;
        private readonly Vector2 spriteFrameCount;
        private int currentRow;
        private int currentColumn;
        private int frameDelayCounter;
        protected int FrameDelay { get; set; }
        protected readonly int DefaultFrameDelay;
        protected readonly bool Reversible;
        private bool reversing;

        public AnimatedSprite(ITexture2D spriteMap, int cellSize, Vector2 renderSize, int frameDelay, bool reversible)
        {
            SpriteMap = spriteMap;
            CellSize = cellSize;
            Reversible = reversible;
            DefaultFrameDelay = frameDelay;
            FrameDelay = frameDelay;
            frameDelayCounter = 0;
            currentRow = 0;
            currentColumn = 0;
            reversing = false;
            spriteFrameCount = CalculateSpriteFrameCount();
            RenderSize = renderSize;
        }

        public AnimatedSprite(ITexture2D spriteMap, int cellSize, int frameDelay, bool reversible) : this(spriteMap,
            cellSize, new Vector2(cellSize), frameDelay, reversible)
        {
        }


        public void SetSpriteCell(int spriteMapColumn, int spriteMapRow)
        {
            currentColumn = spriteMapColumn;
            currentRow = spriteMapRow;
        }

        private Vector2 CalculateSpriteFrameCount()
        {
            float columns = (float) SpriteMap.Width / CellSize;
            float rows = (float) SpriteMap.Width / CellSize;

            return new Vector2(columns, rows);
        }


        private void UpdateFrame()
        {
            if (frameDelayCounter % FrameDelay == 0)
            {
                frameDelayCounter = 0;

                if (currentColumn < spriteFrameCount.X - 1)
                {
                    currentColumn++;
                }
                else
                {
                    currentColumn = 0;
                }
            }

            frameDelayCounter++;
        }

        private void UpdateFrameReversible()
        {
            if (frameDelayCounter % FrameDelay == 0)
            {
                frameDelayCounter = 0;

                if (currentColumn < spriteFrameCount.X - 1 && !reversing)
                {
                    currentColumn++;
                }
                else if (reversing && currentColumn > 0)
                {
                    currentColumn--;
                }

                if (currentColumn >= spriteFrameCount.X - 1 || reversing && currentColumn <= 0)
                {
                    reversing = !reversing;
                }
            }

            frameDelayCounter++;
        }

        public int Height
        {
            get { return (int) RenderSize.Y; }
        }

        public int Width
        {
            get { return (int) RenderSize.X; }
        }

        public void Resize(Vector2 newSize)
        {
            RenderSize = newSize;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color colorOverride)
        {
            if (Reversible)
            {
                UpdateFrameReversible();
            }
            else
            {
                UpdateFrame();
            }

            spriteBatch.Draw(SpriteMap.MonoGameTexture,
                RenderRectangle(position), CurrentCell(), colorOverride);
        }

        private Rectangle RenderRectangle(Vector2 position)
        {
            return new Rectangle((int) position.X, (int) position.Y, (int) RenderSize.X, (int) RenderSize.Y);
        }

        private Rectangle CurrentCell()
        {
            Rectangle rendercell = new Rectangle(CellSize * currentColumn, CellSize * currentRow, CellSize, CellSize);
            return rendercell;
        }

        public virtual AnimatedSprite Clone()
        {
            return new AnimatedSprite(SpriteMap, CellSize, RenderSize, FrameDelay, Reversible);
        }
    }
}