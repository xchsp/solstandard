﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace SolStandard.Utility.Load
{
    using Monogame;

    /**
     * ContentLoader
     * Holds a series of loader methods that are used for the game
     */
    public class ContentLoader
    {
        public static ISpriteFont LoadWindowFont(ContentManager content)
        {
            return new SpriteFontWrapper(content.Load<SpriteFont>("Fonts/WindowText"));
        }
        
        public static ITexture2D LoadTerrainSpriteTexture(ContentManager content)
        {
            Texture2D spriteTextures = content.Load<Texture2D>("Graphics/Map/Tiles/Tiles");

            return new Texture2DWrapper(spriteTextures);
        }

        public static List<ITexture2D> LoadCursorTextures(ContentManager content)
        {
            List<Texture2D> loadCursorTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("Graphics/Map/Cursor/Cursor"),
                content.Load<Texture2D>("Graphics/Map/Cursor/UnitCursorBlue"),
                content.Load<Texture2D>("Graphics/Map/Cursor/UnitCursorRed")
            };

            List<ITexture2D> cursorTextures = new List<ITexture2D>();
            foreach (Texture2D texture in loadCursorTextures)
            {
                cursorTextures.Add(new Texture2DWrapper(texture));
            }

            return cursorTextures;
        }

        public static List<ITexture2D> LoadWindowTextures(ContentManager content)
        {
            List<Texture2D> loadWindowTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("Graphics/HUD/Window/GreyWindow")
            };

            List<ITexture2D> windowTextures = new List<ITexture2D>();
            foreach (Texture2D texture in loadWindowTextures)
            {
                windowTextures.Add(new Texture2DWrapper(texture));
            }

            return windowTextures;
        }

        public static List<ITexture2D> LoadUnitSpriteTextures(ContentManager content)
        {
            List<Texture2D> loadSpriteTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("Graphics/Map/Units/Blue/BlueArcher"),
                content.Load<Texture2D>("Graphics/Map/Units/Blue/BlueMage"),
                content.Load<Texture2D>("Graphics/Map/Units/Blue/BlueChampion"),

                content.Load<Texture2D>("Graphics/Map/Units/Red/RedArcher"),
                content.Load<Texture2D>("Graphics/Map/Units/Red/RedMage"),
                content.Load<Texture2D>("Graphics/Map/Units/Red/RedChampion")
            };

            List<ITexture2D> spriteTextures = new List<ITexture2D>();
            foreach (Texture2D texture in loadSpriteTextures)
            {
                spriteTextures.Add(new Texture2DWrapper(texture));
            }

            return spriteTextures;
        }

        public static List<ITexture2D> LoadLargePortraits(ContentManager content)
        {
            List<Texture2D> loadPortraitTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("Graphics/Images/Portraits/Large/Blue/Archer"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Large/Blue/Champion"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Large/Blue/Mage"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Large/Red/Archer"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Large/Red/Champion"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Large/Red/Mage")
            };

            List<ITexture2D> portraitTextures = new List<ITexture2D>();
            foreach (Texture2D texture in loadPortraitTextures)
            {
                portraitTextures.Add(new Texture2DWrapper(texture));
            }

            return portraitTextures;
        }
        
        public static List<ITexture2D> LoadMediumPortraits(ContentManager content)
        {
            List<Texture2D> loadPortraitTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("Graphics/Images/Portraits/Medium/Blue/Archer"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Medium/Blue/Champion"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Medium/Blue/Mage"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Medium/Red/Archer"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Medium/Red/Champion"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Medium/Red/Mage")
            };

            List<ITexture2D> portraitTextures = new List<ITexture2D>();
            foreach (Texture2D texture in loadPortraitTextures)
            {
                portraitTextures.Add(new Texture2DWrapper(texture));
            }

            return portraitTextures;
        }
        
        public static List<ITexture2D> LoadSmallPortraits(ContentManager content)
        {
            List<Texture2D> loadPortraitTextures = new List<Texture2D>
            {
                content.Load<Texture2D>("Graphics/Images/Portraits/Small/Blue/Archer"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Small/Blue/Champion"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Small/Blue/Mage"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Small/Red/Archer"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Small/Red/Champion"),
                content.Load<Texture2D>("Graphics/Images/Portraits/Small/Red/Mage")
            };

            List<ITexture2D> portraitTextures = new List<ITexture2D>();
            foreach (Texture2D texture in loadPortraitTextures)
            {
                portraitTextures.Add(new Texture2DWrapper(texture));
            }

            return portraitTextures;
        }

    }
}