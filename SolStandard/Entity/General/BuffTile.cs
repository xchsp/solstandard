﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Containers.Contexts.Combat;
using SolStandard.Entity.Unit;
using SolStandard.HUD.Window;
using SolStandard.HUD.Window.Content;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.General
{
    public class BuffTile : TerrainEntity
    {
        private readonly bool canMove;
        public TerrainBonus TerrainBonus { get; private set; }


        public BuffTile(string name, string type, IRenderable sprite, Vector2 mapCoordinates,
            Dictionary<string, string> tiledProperties, int atkBonus, int retBonus, int blockBonus, int luckBonus,
            bool canMove) : base(name, type, sprite, mapCoordinates, tiledProperties)
        {
            this.canMove = canMove;
            TerrainBonus = new TerrainBonus(atkBonus, retBonus, blockBonus, luckBonus);
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
                            new RenderText(AssetManager.WindowFont, (canMove) ? "Can Move" : "No Move",
                                (canMove) ? PositiveColor : NegativeColor)
                        },
                        {
                            new Window(
                                new WindowContentGrid(
                                    new IRenderable[,]
                                    {
                                        {
                                            UnitStatistics.GetSpriteAtlas(Stats.Atk),
                                            new RenderText(AssetManager.WindowFont,
                                                UnitStatistics.Abbreviation[Stats.Atk] + ": +" + TerrainBonus.AtkBonus),
                                        },
                                        {
                                            UnitStatistics.GetSpriteAtlas(Stats.Retribution),
                                            new RenderText(AssetManager.WindowFont, 
                                                UnitStatistics.Abbreviation[Stats.Retribution] + ": +" + TerrainBonus.RetBonus),
                                        },
                                        {
                                            UnitStatistics.GetSpriteAtlas(Stats.Armor),
                                            new RenderText(AssetManager.WindowFont, 
                                                UnitStatistics.Abbreviation[Stats.Armor] + ": +" + TerrainBonus.BlockBonus),
                                        },
                                        {
                                            UnitStatistics.GetSpriteAtlas(Stats.Luck),
                                            new RenderText(AssetManager.WindowFont, 
                                                UnitStatistics.Abbreviation[Stats.Luck] + ": +" + TerrainBonus.LuckBonus),
                                        }
                                    },
                                    0
                                ),
                                InnerWindowColor
                            ),
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