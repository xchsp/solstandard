﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts;
using SolStandard.Entity.Unit;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Item;
using SolStandard.Entity.Unit.Actions.Terrain;
using SolStandard.Entity.Unit.Statuses;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;
using SolStandard.Utility.Events;

namespace SolStandard.Entity.General.Item
{
    public class BuffItem : TerrainEntity, IActionTile, IConsumable
    {
        private static readonly int[] UseRange = {0, 1};

        public int[] InteractRange { get; private set; }
        public bool IsBroken { get; private set; }
        public string ItemPool { get; private set; }

        private readonly int statModifier;
        private readonly Stats statistic;
        private readonly int buffDuration;
        private readonly Window buffWindow;

        public BuffItem(string name, string type, IRenderable sprite, Vector2 mapCoordinates, string statistic,
            int statModifier, int buffDuration, int[] pickupRange, string itemPool)
            : base(name, type, sprite, mapCoordinates)
        {
            InteractRange = pickupRange;
            ItemPool = itemPool;
            this.statistic = UnitStatistics.Abbreviation.First(key => key.Value == statistic).Key;
            this.statModifier = statModifier;
            this.buffDuration = buffDuration;
            buffWindow = GenerateBuffWindow();
        }

        private Window GenerateBuffWindow()
        {
            return new Window(new WindowContentGrid(new IRenderable[,]
                {
                    {
                        new RenderText(AssetManager.HeaderFont, "~Buff~"),
                        new RenderBlank(), new RenderBlank(), new RenderBlank(), new RenderBlank()
                    },
                    {
                        new RenderText(AssetManager.WindowFont,
                            string.Format("[{0}{1}]", (statModifier > 0) ? "+" : "-", statModifier)
                        ),
                        UnitStatistics.GetSpriteAtlas(statistic),
                        new RenderText(AssetManager.WindowFont, "/"),
                        StatusIconProvider.GetStatusIcon(StatusIcon.Time, new Vector2(GameDriver.CellSize)),
                        new RenderText(AssetManager.WindowFont, string.Format("[{0}]", buffDuration))
                    }
                }, 1, HorizontalAlignment.Centered),
                InnerWindowColor,
                HorizontalAlignment.Centered
            );
        }

        public IRenderable Icon
        {
            get { return Sprite; }
        }

        public void Consume(GameUnit targetUnit)
        {
            IsBroken = true;
            GameContext.ActiveUnit.RemoveItemFromInventory(this);

            switch (statistic)
            {
                case Stats.Atk:
                    GlobalEventQueue.QueueSingleEvent(new CastStatusEffectEvent(targetUnit,
                        new AtkStatUp(buffDuration, statModifier)));
                    break;
                case Stats.Mv:
                    GlobalEventQueue.QueueSingleEvent(new CastStatusEffectEvent(targetUnit,
                        new MoveStatModifier(buffDuration, statModifier)));
                    break;
                case Stats.AtkRange:
                    GlobalEventQueue.QueueSingleEvent(new CastStatusEffectEvent(targetUnit,
                        new AtkRangeStatUp(buffDuration, statModifier)));
                    break;
                case Stats.Luck:
                    GlobalEventQueue.QueueSingleEvent(new CastStatusEffectEvent(targetUnit,
                        new LuckStatUp(buffDuration, statModifier)));
                    break;
                case Stats.Retribution:
                    GlobalEventQueue.QueueSingleEvent(new CastStatusEffectEvent(targetUnit,
                        new RetributionStatUp(buffDuration, statModifier)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            return new ConsumeBuffItemAction(this, statistic, statModifier, buffDuration, UseRange);
        }

        public UnitAction DropAction()
        {
            return new DropGiveItemAction(this);
        }

        public IItem Duplicate()
        {
            return new BuffItem(Name, Type, Sprite, MapCoordinates, statistic.ToString().ToUpper(), statModifier,
                buffDuration, InteractRange, ItemPool);
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
                            buffWindow,
                            new RenderBlank()
                        }
                    },
                    3
                );
            }
        }
    }
}