using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Containers.Contexts.WinConditions;
using SolStandard.Entity.General.Item;
using SolStandard.Entity.Unit;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Terrain;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.General
{
    public class Bank : TerrainEntity, IActionTile
    {
        public int[] InteractRange { get; }

        public static int RedMoney { get; private set; }
        public static int BlueMoney { get; private set; }

        public Bank(string name, string type, IRenderable sprite, Vector2 mapCoordinates, bool canMove,
            int[] interactRange) :
            base(name, type, sprite, mapCoordinates)
        {
            CanMove = canMove;
            InteractRange = interactRange;
            ResetBank();
        }

        public List<UnitAction> TileActions()
        {
            return new List<UnitAction>
            {
                new BankDeposit(this),
                new BankWithdraw(this)
            };
        }

        public static void Deposit(GameUnit depositer, int goldToDeposit)
        {
            GameContext.InitiativeContext.DeductGoldFromTeam(goldToDeposit, depositer.Team);

            switch (depositer.Team)
            {
                case Team.Blue:
                    BlueMoney += goldToDeposit;
                    break;
                case Team.Red:
                    RedMoney += goldToDeposit;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(depositer), depositer.Team, null);
            }

            AssetManager.CoinSFX.Play();
            GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor(
                $"{depositer.Id} deposits {goldToDeposit}{Currency.CurrencyAbbreviation} to the bank!",
                50
            );
            GameMapContext.GameMapView.GenerateObjectiveWindow();
        }

        public static void Withdraw(GameUnit depositer, int goldToWithdraw)
        {
            GameContext.InitiativeContext.AddGoldToTeam(goldToWithdraw, depositer.Team);

            switch (depositer.Team)
            {
                case Team.Blue:
                    BlueMoney -= goldToWithdraw;
                    break;
                case Team.Red:
                    RedMoney -= goldToWithdraw;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(depositer), depositer.Team, null);
            }

            AssetManager.CoinSFX.Play();
            GameContext.GameMapContext.MapContainer.AddNewToastAtMapCursor(
                $"{depositer.Id} withdraws {goldToWithdraw}{Currency.CurrencyAbbreviation} from the bank!",
                50
            );
            GameMapContext.GameMapView.GenerateObjectiveWindow();
        }

        public static int GetTeamGoldInBank(Team team)
        {
            switch (team)
            {
                case Team.Blue:
                    return BlueMoney;
                case Team.Red:
                    return RedMoney;
                case Team.Creep:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public static void ResetBank()
        {
            RedMoney = 0;
            BlueMoney = 0;
        }

        public override IRenderable TerrainInfo =>
            new WindowContentGrid(
                new[,]
                {
                    {
                        InfoHeader,
                        new RenderBlank()
                    },
                    {
                        UnitStatistics.GetSpriteAtlas(Stats.Mv),
                        new RenderText(AssetManager.WindowFont, (CanMove) ? "Can Move" : "No Move",
                            (CanMove) ? PositiveColor : NegativeColor)
                    },
                    {
                        StatusIconProvider.GetStatusIcon(StatusIcon.PickupRange, GameDriver.CellSizeVector),
                        new RenderText(
                            AssetManager.WindowFont,
                            ": " + $"[{string.Join(",", InteractRange)}]"
                        )
                    },
                    {
                        new Window(
                            new IRenderable[,]
                            {
                                {
                                    ObjectiveIconProvider.GetObjectiveIcon(
                                        VictoryConditions.Taxes,
                                        GameDriver.CellSizeVector
                                    ),
                                    new RenderText(AssetManager.WindowFont,
                                        "Blue Gold: " + BlueMoney + Currency.CurrencyAbbreviation)
                                }
                            },
                            TeamUtility.DetermineTeamColor(Team.Blue)
                        ),
                        new RenderBlank()
                    },
                    {
                        new Window(
                            new IRenderable[,]
                            {
                                {
                                    ObjectiveIconProvider.GetObjectiveIcon(
                                        VictoryConditions.Taxes,
                                        GameDriver.CellSizeVector
                                    ),
                                    new RenderText(AssetManager.WindowFont,
                                        "Red Gold: " + RedMoney + Currency.CurrencyAbbreviation)
                                }
                            },
                            TeamUtility.DetermineTeamColor(Team.Red)
                        ),
                        new RenderBlank()
                    }
                },
                1,
                HorizontalAlignment.Centered
            );
    }
}