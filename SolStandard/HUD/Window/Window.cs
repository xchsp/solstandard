﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Exceptions;
using SolStandard.Utility.Monogame;

namespace SolStandard.HUD.Window
{
    public class Window : IRenderable
    {
        private readonly string windowLabel;
        private ITexture2D windowTexture;
        private readonly int windowCellSize;
        private readonly WindowContentGrid windowContents;
        private readonly Vector2 windowPixelSize;
        private readonly WindowCell[,] windowCells;
        private readonly Color windowColor;

        private readonly int padding;
        public bool Visible { get; set; }

        //Single Content
        public Window(string windowLabel, ITexture2D windowTexture, IRenderable windowContent, int padding,
            Color windowColor)
        {
            this.windowTexture = windowTexture;
            this.padding = padding;
            this.windowColor = windowColor;
            this.windowLabel = windowLabel;
            windowContents = new WindowContentGrid(new[,] {{windowContent}});
            windowCellSize = CalculateCellSize(windowTexture);
            windowPixelSize = DeriveSizeFromContent();
            windowCells = ConstructWindowCells(WindowPixelSize);
            Visible = true;
        }

        //Grid of Content
        public Window(string windowLabel, ITexture2D windowTexture, WindowContentGrid windowContents, int padding,
            Color windowColor)
        {
            this.windowTexture = windowTexture;
            this.windowContents = windowContents;
            this.padding = padding;
            this.windowColor = windowColor;
            this.windowLabel = windowLabel;
            windowCellSize = CalculateCellSize(windowTexture);
            windowPixelSize = DeriveSizeFromContent();
            windowCells = ConstructWindowCells(WindowPixelSize);
            Visible = true;
        }

        public Vector2 WindowPixelSize
        {
            get { return windowPixelSize; }
        }

        public string WindowLabel
        {
            get { return windowLabel; }
        }


        private int CalculateCellSize(ITexture2D windowTextureTemplate)
        {
            //Window Texture must be a square
            if (windowTextureTemplate.GetWidth() == windowTextureTemplate.GetHeight())
            {
                return windowTexture.GetHeight() / 3;
            }

            throw new InvalidWindowTextureException();
        }

        /*
         * Window Cells
         * [1][2][3]
         * [4][5][6]
         * [7][8][9]
         */
        private WindowCell[,] ConstructWindowCells(Vector2 pixelSize)
        {
            WindowCell[,] windowCellsToConstruct =
                new WindowCell[(int) pixelSize.X / windowCellSize, (int) pixelSize.Y / windowCellSize];

            //Build the GameTile list
            for (int column = 0; column < windowCellsToConstruct.GetLength(0); column++)
            {
                for (int row = 0; row < windowCellsToConstruct.GetLength(1); row++)
                {
                    //Top Border
                    int cellIndex;
                    if (row == 0)
                    {
                        //Top-Left Corner
                        if (column == 0)
                        {
                            cellIndex = 1;
                        }
                        //Top-Right Corner
                        else if (column == windowCellsToConstruct.GetLength(0) - 1)
                        {
                            cellIndex = 3;
                        }
                        //Top Border
                        else
                        {
                            cellIndex = 2;
                        }
                    }
                    //Bottom Border
                    else if (row == windowCellsToConstruct.GetLength(1) - 1)
                    {
                        //Bottom-Left Corner
                        if (column == 0)
                        {
                            cellIndex = 7;
                        }
                        //Bottom-Right Corner
                        else if (column == windowCellsToConstruct.GetLength(0) - 1)
                        {
                            cellIndex = 9;
                        }
                        //Bottom Border
                        else
                        {
                            cellIndex = 8;
                        }
                    }
                    //Left Border
                    else if (column == 0)
                    {
                        cellIndex = 4;
                    }
                    //Right Border
                    else if (column == windowCellsToConstruct.GetLength(0) - 1)
                    {
                        cellIndex = 6;
                    }
                    //Background
                    else
                    {
                        cellIndex = 5;
                    }

                    windowCellsToConstruct[column, row] = new WindowCell(windowCellSize, cellIndex, windowColor,
                        new Vector2(column * windowCellSize, row * windowCellSize));
                }
            }

            return windowCellsToConstruct;
        }

        private Vector2 DeriveSizeFromContent()
        {
            Vector2 calculatedSize = new Vector2();

            Vector2 contentGridSize = DetermineGridSizeInPixels();
            calculatedSize.X = contentGridSize.X;
            calculatedSize.Y = contentGridSize.Y;

            //Adjust for border
            int borderSize = windowCellSize * 2;
            calculatedSize.X += borderSize;
            calculatedSize.Y += borderSize;

            return calculatedSize;
        }


        private void RenderContentGrid(SpriteBatch spriteBatch, Vector2 coordinates)
        {
            Vector2 borderOffset = new Vector2(windowCellSize);
            Vector2 renderPosition = coordinates + borderOffset;

            float highestRowHeight = 0f;

            float horizontalOffset = 0f;
            float verticalOffset = 0f;

            for (int column = 0; column < windowContents.ContentGrid.GetLength(0); column++)
            {
                for (int row = 0; row < windowContents.ContentGrid.GetLength(1); row++)
                {
                    //Draw with offset
                    windowContents.ContentGrid[column, row].Draw(spriteBatch,
                        new Vector2(renderPosition.X + horizontalOffset, renderPosition.Y + verticalOffset));

                    //Adjust offset
                    float contentWidth = windowContents.ContentGrid[column, row].GetWidth();
                    horizontalOffset += contentWidth + padding;

                    float contentHeight = windowContents.ContentGrid[column, row].GetHeight();
                    if (highestRowHeight < contentHeight)
                    {
                        highestRowHeight = contentHeight + padding;
                    }
                }

                verticalOffset = highestRowHeight; //Once I start drawing the next row I should reset the height
                highestRowHeight = 0;
                horizontalOffset = 0;
            }
        }

        //TODO clean this so I'm not duplicating so much of the RenderGrid logic
        private Vector2 DetermineGridSizeInPixels()
        {
            float totalWidth = 0f;
            float totalHeight = 0;

            float highestRowHeight = 0f;
            float horizontalOffset = 0f;

            for (int column = 0; column < windowContents.ContentGrid.GetLength(0); column++)
            {
                for (int row = 0; row < windowContents.ContentGrid.GetLength(1); row++)
                {
                    float contentWidth = windowContents.ContentGrid[column, row].GetWidth();
                    horizontalOffset += contentWidth + padding;

                    float contentHeight = windowContents.ContentGrid[column, row].GetHeight();
                    if (highestRowHeight < contentHeight)
                    {
                        highestRowHeight = contentHeight + padding;
                    }
                }

                //Combination of highest heights determines the height
                totalHeight += highestRowHeight;

                //Widest set of items determines the width
                if (totalWidth < horizontalOffset)
                {
                    totalWidth = horizontalOffset;
                }

                horizontalOffset = 0;
                highestRowHeight = 0;
            }

            return new Vector2(totalWidth, totalHeight);
        }

        public int GetHeight()
        {
            return (int) windowPixelSize.Y;
        }

        public int GetWidth()
        {
            return (int) windowPixelSize.X;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 coordinates)
        {
            if (Visible)
            {
                foreach (WindowCell windowCell in windowCells)
                {
                    windowCell.Draw(spriteBatch, ref windowTexture, coordinates);
                }

                RenderContentGrid(spriteBatch, coordinates);
            }
        }
    }
}