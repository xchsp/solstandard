using System.Linq;
using SolStandard.Entity.General.Item;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Containers.Contexts.WinConditions
{
    public class CollectTheRelics : Objective
    {
        private readonly int relicsToCollect;

        public CollectTheRelics(int relicsToCollect)
        {
            this.relicsToCollect = relicsToCollect;
        }

        protected override IRenderable VictoryLabelContent =>
            new RenderText(AssetManager.ResultsFont, "COLLECTED TARGET RELICS");


        public override IRenderable ObjectiveInfo => BuildObjectiveWindow();

        private Window BuildObjectiveWindow()
        {
            Window blueRelicCount = new Window(
                new RenderText(AssetManager.WindowFont,
                    $"Blue: {GetRelicCountForTeam(Team.Blue)}/{relicsToCollect}"),
                TeamUtility.DetermineTeamColor(Team.Blue)
            );

            Window redRelicCount = new Window(
                new RenderText(AssetManager.WindowFont,
                    $"Red: {GetRelicCountForTeam(Team.Red)}/{relicsToCollect}"),
                TeamUtility.DetermineTeamColor(Team.Red)
            );

            return new Window(
                new WindowContentGrid(
                    new IRenderable[,]
                    {
                        {
                            blueRelicCount,
                            ObjectiveIconProvider.GetObjectiveIcon(
                                VictoryConditions.CollectTheRelicsVS,
                                GameDriver.CellSizeVector
                            ),
                            new RenderText(AssetManager.WindowFont, "Collect [" + relicsToCollect + "] Relics (VS)"),
                            redRelicCount
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
            if (TeamHasCollectedTargetNumberOfRelics(Team.Red))
            {
                RedTeamWins = true;
                return RedTeamWins;
            }

            if (TeamHasCollectedTargetNumberOfRelics(Team.Blue))
            {
                BlueTeamWins = true;
                return BlueTeamWins;
            }

            if (TeamIsWipedOut(Team.Red) && TeamIsWipedOut(Team.Blue))
            {
                GameIsADraw = true;
                return GameIsADraw;
            }

            return false;
        }

        private bool TeamHasCollectedTargetNumberOfRelics(Team team)
        {
            return GetRelicCountForTeam(team) >= relicsToCollect;
        }

        public static int GetRelicCountForTeam(Team team)
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