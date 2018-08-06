﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolStandard.Map;
using SolStandard.Map.Objects;
using SolStandard.Utility.Load;
using SolStandard.Utility.Monogame;
using System.Collections.Generic;
using System.Linq.Expressions;
using SolStandard.Containers;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Map.Camera;
using SolStandard.Map.Objects.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Buttons;
using TiledSharp;

namespace SolStandard
{
    enum GameLayer
    {
        Window,
        Map
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameDriver : Game
    {
        //Tile Size of Sprites
        public const int CellSize = 32;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameControlMapper controlMapper;

        private GameContainer container;

        private ITexture2D terrainTextures;
        private List<ITexture2D> unitSprites;
        private List<ITexture2D> guiTextures;
        private List<ITexture2D> windowTextures;
        private ISpriteFont windowFont;
        private MapCamera mapCamera;

        public GameDriver()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1600,
                PreferredBackBufferHeight = 900
            };

            //HACK Move the window away from the top-left corner
            Window.Position = new Point(0, 50);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            const string
                mapPath =
                    "Content/TmxMaps/Arena_3.tmx"; //TODO Hard-coded for now; remove me once map selector implemented
            const string objectTypeDefaults = "Content/TmxMaps/objecttypes.xml";
            TmxMap tmxMap = new TmxMap(mapPath);
            TmxMapParser mapParser = new TmxMapParser(tmxMap, terrainTextures, unitSprites, objectTypeDefaults);
            controlMapper = new GameControlMapper();

            mapCamera = new MapCamera(10);
            mapCamera.SetCameraZoom(1.8f);

            ITexture2D cursorTexture = guiTextures.Find(texture => texture.GetTexture2D().Name.Contains("Cursor"));
            MapLayer gameMap = new MapLayer(mapParser.LoadMapGrid(), cursorTexture);

            //TODO put this window stuff somewhere more useful
            ITexture2D windowTexture =
                windowTextures.Find(texture => texture.GetTexture2D().Name.Contains("GreyWindow"));
            List<IRenderable> windowContents = new List<IRenderable>();
            const string windowText =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. "
                + "\n"
                + "Nam facilisis odio nec molestie suscipit. Aliquam erat volutpat. Vivamus nec nibh luctus, sagittis purus placerat, imperdiet neque."
                + "\n"
                + " Suspendisse pretium blandit purus at blandit. Aliquam consequat leo ante, ut dictum mi cursus in. "
                + "\n"
                + "\n"
                + "Mauris non laoreet metus, condimentum commodo augue. Phasellus ac fringilla purus.";
            windowContents.Add(new RenderText(windowFont, windowText));
            Window testWindow = new Window(windowTexture, windowContents, new Vector2(20, 30));

            List<Window> windowList = new List<Window>
            {
                testWindow
            };

            container = new GameContainer(gameMap, new WindowLayer(windowList));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            terrainTextures = ContentLoader.LoadTerrainSpriteTexture(Content);
            unitSprites = ContentLoader.LoadUnitSpriteTextures(Content);
            guiTextures = ContentLoader.LoadCursorTextures(Content);
            windowTextures = ContentLoader.LoadWindowTextures(Content);
            windowFont = ContentLoader.LoadWindowFont(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (controlMapper.Select())
            {
                Exit();
            }

            if (controlMapper.Start())
            {
                mapCamera.SetTargetCameraPosition(new Vector2(0));
            }

            if (controlMapper.Down())
            {
                container.Map.GetMapCursor().MoveCursorInDirection((MapCursor.CursorDirection.Down));
            }

            if (controlMapper.Left())
            {
                container.Map.GetMapCursor().MoveCursorInDirection((MapCursor.CursorDirection.Left));
            }

            if (controlMapper.Right())
            {
                container.Map.GetMapCursor().MoveCursorInDirection((MapCursor.CursorDirection.Right));
            }

            if (controlMapper.Up())
            {
                container.Map.GetMapCursor().MoveCursorInDirection((MapCursor.CursorDirection.Up));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                mapCamera.MoveCameraInDirection(CameraDirection.Down);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                mapCamera.MoveCameraInDirection(CameraDirection.Left);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                mapCamera.MoveCameraInDirection(CameraDirection.Right);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                mapCamera.MoveCameraInDirection(CameraDirection.Up);
            }

            Vector2 screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Vector2 mapSize = container.Map.MapSize();

            mapCamera.CorrectCameraToCursor(container.Map.GetMapCursor(), screenSize, mapSize);
            mapCamera.PanCameraToTarget();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Deferred, //Use deferred instead of texture to render in order of .Draw() calls
                null, SamplerState.PointClamp, null, null, null, mapCamera.GetCameraMatrix());

            container.Map.Draw(spriteBatch);

            base.Draw(gameTime);

            spriteBatch.End();


            //WINDOW LAYER
            spriteBatch.Begin(
                SpriteSortMode.Deferred, //Use deferred instead of texture to render in order of .Draw() calls
                null, SamplerState.PointClamp, null, null, null, null);

            container.Windows.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}