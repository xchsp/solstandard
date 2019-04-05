﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Entity.Unit;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Mage;
using SolStandard.Entity.Unit.Actions.Terrain;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Map.Elements;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.General.Item
{
    public class BlinkItem : TerrainEntity, IItem, IActionTile
    {
        public int[] BlinkRange { get; private set; }
        public int[] InteractRange { get; private set; }
        public int UsesRemaining { get; set; }
        public string ItemPool { get; private set; }

        public BlinkItem(string name, string type, IRenderable sprite, Vector2 mapCoordinates, int[] pickupRange,
            int[] blinkRange, int usesRemaining, string itemPool)
            : base(name, type, sprite, mapCoordinates, new Dictionary<string, string>())
        {
            BlinkRange = blinkRange;
            InteractRange = pickupRange;
            UsesRemaining = usesRemaining;
            ItemPool = itemPool;
        }

        public bool IsBroken
        {
            get { return UsesRemaining < 1; }
        }

        public IRenderable Icon
        {
            get { return Sprite; }
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
            return new Blink(this);
        }

        public UnitAction DropAction()
        {
            return new DropGiveItemAction(this);
        }

        public IItem Duplicate()
        {
            return new BlinkItem(Name, Type, Sprite, MapCoordinates, InteractRange, BlinkRange, UsesRemaining, ItemPool);
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
                            new Window(new IRenderable[,]
                                {
                                    {
                                        SkillIconProvider.GetSkillIcon(SkillIcon.Blink,
                                            new Vector2(GameDriver.CellSize)),
                                        new RenderText(AssetManager.WindowFont,
                                            "Blink Range: [" + string.Join(",", BlinkRange) + "]")
                                    },
                                    {
                                        new RenderText(AssetManager.WindowFont,
                                            "Uses Remaining: [" + UsesRemaining + "]"),
                                        new RenderBlank(),
                                    }
                                },
                                InnerWindowColor,
                                HorizontalAlignment.Centered
                            ),
                            new RenderBlank()
                        }
                    },
                    3
                );
            }
        }
    }
}