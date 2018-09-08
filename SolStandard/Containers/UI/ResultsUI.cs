﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Monogame;

namespace SolStandard.Containers.UI
{
    public class ResultsUI : IUserInterface
    {
        private const int WindowEdgeBuffer = 5;
        private const int WindowPadding = 10;

        private static readonly Color BackgroundColor = new Color(0, 0, 0, 100);

        private Window BlueTeamLeaderPortrait { get; set; }
        private Window BlueTeamUnitRoster { get; set; }
        private Window BlueTeamResult { get; set; }

        private Window ResultsLabelWindow { get; set; }

        private Window RedTeamLeaderPortrait { get; set; }
        private Window RedTeamUnitRoster { get; set; }
        private Window RedTeamResult { get; set; }

        public string BlueTeamResultText { private get; set; }
        public string RedTeamResultText { private get; set; }

        private readonly ITexture2D windowTexture;

        public bool Visible { get; private set; }

        public ResultsUI(ITexture2D windowTexture)
        {
            this.windowTexture = windowTexture;
            BlueTeamResultText = "FIGHT!";
            RedTeamResultText = "FIGHT!";
        }

        public void UpdateWindows()
        {
            GenerateBlueTeamLeaderPortraitWindow();
            GenerateBlueTeamUnitRosterWindow();
            GenerateBlueTeamResultWindow(BlueTeamResultText);

            GenerateRedTeamLeaderPortraitWindow();
            GenerateRedTeamUnitRosterWindow();
            GenerateRedTeamResultWindow(RedTeamResultText);

            GenerateResultsLabelWindow();
        }

        #region Generation

        private void GenerateResultsLabelWindow()
        {
            ResultsLabelWindow = new Window(
                "Versus Window",
                windowTexture,
                new WindowContentGrid(
                    new IRenderable[,]
                    {
                        {
                            new RenderText(GameDriver.ResultsFont, "-RESULTS-")
                        }
                    },
                    1
                ),
                BackgroundColor
            );
        }

        private void GenerateBlueTeamLeaderPortraitWindow()
        {
            IRenderable[,] blueLeaderContent = {{FindTeamLeader(Team.Blue, Role.Monarch).LargePortrait}};

            BlueTeamLeaderPortrait = new Window(
                "Blue Leader Portrait Window",
                windowTexture,
                new WindowContentGrid(blueLeaderContent, 1),
                TeamUtility.DetermineTeamColor(Team.Blue)
            );
        }

        private void GenerateBlueTeamUnitRosterWindow()
        {
            BlueTeamUnitRoster = new Window(
                "Blue Team Roster",
                windowTexture,
                new WindowContentGrid(GenerateUnitRoster(Team.Blue), 2),
                BackgroundColor
            );
        }

        private void GenerateBlueTeamResultWindow(string windowText)
        {
            BlueTeamResult = new Window(
                "Blue Team Result Window",
                windowTexture,
                new WindowContentGrid(
                    new IRenderable[,]
                    {
                        {
                            new RenderText(GameDriver.ResultsFont, windowText)
                        }
                    },
                    1
                ),
                TeamUtility.DetermineTeamColor(Team.Blue)
            );
        }

        private void GenerateRedTeamLeaderPortraitWindow()
        {
            IRenderable[,] blueLeaderContent = {{FindTeamLeader(Team.Red, Role.Monarch).LargePortrait}};

            RedTeamLeaderPortrait = new Window(
                "Red Leader Portrait Window",
                windowTexture,
                new WindowContentGrid(blueLeaderContent, 1),
                TeamUtility.DetermineTeamColor(Team.Red)
            );
        }

        private void GenerateRedTeamUnitRosterWindow()
        {
            RedTeamUnitRoster = new Window(
                "Red Team Roster",
                windowTexture,
                new WindowContentGrid(GenerateUnitRoster(Team.Red), 2),
                BackgroundColor
            );
        }

        private void GenerateRedTeamResultWindow(string windowText)
        {
            RedTeamResult = new Window(
                "Red Team Result Window",
                windowTexture,
                new WindowContentGrid(
                    new IRenderable[,]
                    {
                        {
                            new RenderText(GameDriver.ResultsFont, windowText)
                        }
                    },
                    1
                ),
                TeamUtility.DetermineTeamColor(Team.Red)
            );
        }


        private static GameUnit FindTeamLeader(Team team, Role role)
        {
            return GameContext.Units.Find(unit => unit.Team == team && unit.Role == role);
        }


        private IRenderable[,] GenerateUnitRoster(Team team)
        {
            List<GameUnit> teamUnits = GameContext.Units.FindAll(unit => unit.Team == team);

            IRenderable[,] unitRosterGrid = new IRenderable[1, teamUnits.Count];

            const int unitListHealthBarHeight = 10;
            int listPosition = 0;

            foreach (GameUnit unit in teamUnits)
            {
                IRenderable[,] unitContent =
                {
                    {
                        new RenderText(GameDriver.MapFont, unit.Id)
                    },
                    {
                        unit.MediumPortrait
                    },
                    {
                        unit.GetInitiativeHealthBar(new Vector2(unit.MediumPortrait.Width, unitListHealthBarHeight))
                    }
                };

                IRenderable singleUnitContent = new Window(
                    "Unit " + unit.Id + " Window",
                    windowTexture,
                    new WindowContentGrid(unitContent, 2),
                    TeamUtility.DetermineTeamColor(unit.Team)
                );

                unitRosterGrid[0, listPosition] = singleUnitContent;
                listPosition++;
            }

            return unitRosterGrid;
        }

        #endregion Generation

        #region Positioning

        //Blue Windows
        private Vector2 BlueTeamLeaderPortraitPosition()
        {
            //Top-Left of screen
            return new Vector2(WindowEdgeBuffer);
        }

        private Vector2 BlueTeamResultPosition()
        {
            //Right of Blue Portrait, Aligned at top
            return new Vector2(
                BlueTeamLeaderPortraitPosition().X + BlueTeamLeaderPortrait.Width + WindowPadding,
                BlueTeamLeaderPortraitPosition().Y
            );
        }

        private Vector2 BlueTeamUnitRosterPosition()
        {
            //Below Blue Result, Aligned at left with Result
            return new Vector2(
                BlueTeamResultPosition().X,
                BlueTeamResultPosition().Y + BlueTeamResult.Height + WindowPadding
            );
        }


        //Versus Window
        private Vector2 ResultsLabelWindowPosition()
        {
            //Center of screen
            return new Vector2(
                GameDriver.ScreenSize.X / 2 - (float) ResultsLabelWindow.Width / 2,
                GameDriver.ScreenSize.Y / 2 - (float) ResultsLabelWindow.Height / 2
            );
        }


        //Red Windows
        private Vector2 RedTeamLeaderPortraitPosition()
        {
            //Bottom-Right of screen
            return new Vector2(
                GameDriver.ScreenSize.X - WindowEdgeBuffer - RedTeamLeaderPortrait.Width,
                GameDriver.ScreenSize.Y - WindowEdgeBuffer - RedTeamLeaderPortrait.Height
            );
        }

        private Vector2 RedTeamResultPosition()
        {
            float redPortraitLeft = RedTeamLeaderPortraitPosition().X;
            float redPortraitBottom = RedTeamLeaderPortraitPosition().Y + RedTeamLeaderPortrait.Height;

            //Left of Red Portrait, Aligned at bottom
            return new Vector2(
                redPortraitLeft - WindowPadding - RedTeamResult.Width,
                redPortraitBottom - RedTeamResult.Height
            );
        }

        private Vector2 RedTeamUnitRosterPosition()
        {
            float redResultRight = RedTeamResultPosition().X + RedTeamResult.Width;
            float redResultTop = RedTeamResultPosition().Y;

            //Above Red Result, Aligned at right with Result
            return new Vector2(
                redResultRight - RedTeamUnitRoster.Width,
                redResultTop - WindowPadding - RedTeamUnitRoster.Height
            );
        }

        #endregion Positioning

        public void ToggleVisible()
        {
            Visible = !Visible;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (BlueTeamLeaderPortrait != null)
                BlueTeamLeaderPortrait.Draw(spriteBatch, BlueTeamLeaderPortraitPosition());

            if (BlueTeamUnitRoster != null)
                BlueTeamUnitRoster.Draw(spriteBatch, BlueTeamUnitRosterPosition());

            if (BlueTeamResult != null)
                BlueTeamResult.Draw(spriteBatch, BlueTeamResultPosition());

            if (ResultsLabelWindow != null)
            {
                ResultsLabelWindow.Draw(spriteBatch, ResultsLabelWindowPosition());
            }

            if (RedTeamLeaderPortrait != null)
                RedTeamLeaderPortrait.Draw(spriteBatch, RedTeamLeaderPortraitPosition());

            if (RedTeamUnitRoster != null)
                RedTeamUnitRoster.Draw(spriteBatch, RedTeamUnitRosterPosition());

            if (RedTeamResult != null)
                RedTeamResult.Draw(spriteBatch, RedTeamResultPosition());
        }
    }
}