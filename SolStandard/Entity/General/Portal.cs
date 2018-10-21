﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolStandard.Entity.Unit;
using SolStandard.Entity.Unit.Actions;
using SolStandard.Entity.Unit.Actions.Terrain;
using SolStandard.HUD.Window.Content;
using SolStandard.Map.Elements;
using SolStandard.Utility;
using SolStandard.Utility.Assets;

namespace SolStandard.Entity.General
{
    public class Portal : TerrainEntity, IActionTile
    {
        private readonly bool canMove;
        private readonly string destinationId;
        public int[] Range { get; private set; }

        public Portal(string name, string type, IRenderable sprite, Vector2 mapCoordinates,
            Dictionary<string, string> tiledProperties, bool canMove, string destinationId, int[] range) :
            base(name, type, sprite, mapCoordinates, tiledProperties)
        {
            this.canMove = canMove;
            this.destinationId = destinationId;
            Range = range;
        }

        public UnitAction TileAction()
        {
            return new Transport(this, destinationId);
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
                            UnitStatistics.GetSpriteAtlas(StatIcons.Mv),
                            new RenderText(AssetManager.WindowFont, (canMove) ? "Can Move" : "No Move",
                                (canMove) ? PositiveColor : NegativeColor)
                        },
                        {
                            new RenderText(AssetManager.WindowFont, "Destination: " + destinationId),
                            new RenderBlank()
                        },
                        {
                            new RenderText(AssetManager.WindowFont,
                                string.Format("Range: [{0}]", string.Join(",", Range))),
                            new RenderBlank()
                        }
                    },
                    3
                );
            }
        }
    }
}