﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Containers.Contexts.WinConditions
{
    public class Assassinate : Objective
    {
        private Window objectiveWindow;

        protected override IRenderable VictoryLabelContent
        {
            get { return new RenderText(AssetManager.ResultsFont, "COMMANDER DEFEATED"); }
        }

        public override IRenderable ObjectiveInfo
        {
            get { return objectiveWindow ?? (objectiveWindow = BuildObjectiveWindow()); }
        }

        private static Window BuildObjectiveWindow()
        {
            return new Window(
                new WindowContentGrid(
                    new IRenderable[,]
                    {
                        {
                            ObjectiveIconProvider.GetObjectiveIcon(
                                VictoryConditions.Assassinate,
                                new Vector2(GameDriver.CellSize)
                            ),
                            new RenderText(AssetManager.WindowFont, "Assassinate"),
                        }
                    },
                    2,
                    HorizontalAlignment.Centered
                ),
                ObjectiveWindowColor,
                HorizontalAlignment.Centered
            );
        }

        public override bool ConditionsMet()
        {
            List<GameUnit> blueTeam = GameContext.Units.FindAll(unit => unit.Team == Team.Blue);
            List<GameUnit> redTeam = GameContext.Units.FindAll(unit => unit.Team == Team.Red);

            if (TeamCommandersAreAllDead(blueTeam) && TeamCommandersAreAllDead(redTeam))
            {
                GameIsADraw = true;
                return GameIsADraw;
            }

            if (TeamCommandersAreAllDead(blueTeam))
            {
                RedTeamWins = true;
                return RedTeamWins;
            }

            if (TeamCommandersAreAllDead(redTeam))
            {
                BlueTeamWins = true;
                return BlueTeamWins;
            }

            return false;
        }


        private static bool TeamCommandersAreAllDead(List<GameUnit> team)
        {
            List<GameUnit> teamCommanders = team.FindAll(unit => unit.IsCommander);

            foreach (GameUnit monarch in teamCommanders)
            {
                //Return false if any Monarchs are alive.
                if (monarch.Stats.CurrentHP > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}