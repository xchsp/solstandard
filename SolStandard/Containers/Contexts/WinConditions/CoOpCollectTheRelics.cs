using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Entity.General.Item;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Containers.Contexts.WinConditions
{
    public class CoOpCollectTheRelics : Objective
    {
        private Window objectiveWindow;
        private readonly int relicsToCollect;

        public CoOpCollectTheRelics(int relicsToCollect)
        {
            this.relicsToCollect = relicsToCollect;
        }

        protected override IRenderable VictoryLabelContent
        {
            get { return new RenderText(AssetManager.ResultsFont, "COLLECTED TARGET RELICS"); }
        }


        public override IRenderable ObjectiveInfo
        {
            get { return objectiveWindow ?? (objectiveWindow = BuildObjectiveWindow()); }
        }

        private Window BuildObjectiveWindow()
        {
            return new Window(
                new WindowContentGrid(
                    new IRenderable[,]
                    {
                        {
                            ObjectiveIconProvider.GetObjectiveIcon(
                                VictoryConditions.CollectTheRelicsVS,
                                new Vector2(GameDriver.CellSize)
                            ),
                            new RenderText(AssetManager.WindowFont, "Collect [" + relicsToCollect + "] Relics (Co-Op)"),
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
            if (PlayerTeamsHaveCollectedEnoughRelics)
            {
                CoOpVictory = true;
                return CoOpVictory;
            }

            if (TeamIsWipedOut(Team.Red) && TeamIsWipedOut(Team.Blue))
            {
                AllPlayersLose = true;
                return AllPlayersLose;
            }

            return false;
        }

        private bool PlayerTeamsHaveCollectedEnoughRelics
        {
            get { return (GetRelicCountForTeam(Team.Red) + GetRelicCountForTeam(Team.Blue)) >= relicsToCollect; }
        }

        private static int GetRelicCountForTeam(Team team)
        {
            return GameContext.Units.Where(unit => unit.Team == team)
                .Sum(unit => unit.Inventory.Count(item => item is Relic));
        }

        private static bool TeamIsWipedOut(Team team)
        {
            return GameContext.Units.Where(unit => unit.Team == team).ToList().TrueForAll(unit => !unit.IsAlive);
        }
    }
}