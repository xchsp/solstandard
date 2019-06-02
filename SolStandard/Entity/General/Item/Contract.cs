using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Terrain;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Monogame;

namespace SolStandard.Entity.General.Item
{
    public class Contract : TerrainEntity, IItem, IActionTile
    {
        private readonly bool forSpecificUnit;
        private readonly Role specificRole;
        public string ItemPool { get; private set; }
        public int[] InteractRange { get; private set; }

        public Contract(string name, string type, IRenderable sprite, Vector2 mapCoordinates, int[] interactRange,
            string itemPool, bool forSpecificUnit, Role specificRole)
            : base(name, type, sprite, mapCoordinates, new Dictionary<string, string>())
        {
            ItemPool = itemPool;
            InteractRange = interactRange;
            this.specificRole = specificRole;
            this.forSpecificUnit = forSpecificUnit;
        }

        public List<UnitAction> TileActions()
        {
            return new List<UnitAction>
            {
                new PickUpItemAction(this, MapCoordinates)
            };
        }

        public UnitAction UseAction()
        {
            return forSpecificUnit
                ? (UnitAction) new SpawnUnitAction(specificRole, this)
                : new DraftNewUnitAction(this);
        }

        public UnitAction DropAction()
        {
            return new DropGiveItemAction(this);
        }

        public IItem Duplicate()
        {
            return new Contract(Name, Type, Sprite, MapCoordinates, InteractRange, ItemPool, forSpecificUnit,
                specificRole);
        }

        public bool IsBroken
        {
            get { return false; }
        }

        public IRenderable Icon
        {
            get { return RenderSprite; }
        }

        private static Window SpecificUnitWindow(Role role, Team team)
        {
            ITexture2D unitPortrait = UnitGenerator.GetUnitPortrait(role, team);

            return new Window(
                new IRenderable[,]
                {
                    {
                        new SpriteAtlas(unitPortrait,
                            new Vector2(unitPortrait.Width, unitPortrait.Height),
                            new Vector2(GameDriver.CellSize)
                        ),
                        new RenderText(AssetManager.WindowFont,
                            ((role == Role.Silhouette) ? "Free" : role.ToString()) + " Contract")
                    }
                },
                InnerWindowColor
            );
        }

        private static Window FreeContractWindow
        {
            get { return SpecificUnitWindow(Role.Silhouette, Team.Creep); }
        }

        public override IRenderable TerrainInfo
        {
            get
            {
                return new WindowContentGrid(
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
                            StatusIconProvider.GetStatusIcon(StatusIcon.PickupRange, new Vector2(GameDriver.CellSize)),
                            new RenderText(
                                AssetManager.WindowFont,
                                ": " + string.Format("[{0}]", string.Join(",", InteractRange))
                            )
                        },
                        {
                            (forSpecificUnit)
                                ? SpecificUnitWindow(specificRole, GameContext.ActiveUnit.Team)
                                : FreeContractWindow,
                            new RenderBlank()
                        }
                    },
                    3,
                    HorizontalAlignment.Centered
                );
            }
        }
    }
}