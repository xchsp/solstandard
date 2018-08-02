﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using SolStandard.Map.Objects;
using SolStandard.Map.Objects.Cursor;
using SolStandard.Utility;
using SolStandard.Utility.Monogame;

namespace SolStandard.Map
{
    public class MapContainer
    {
        private readonly List<MapObject[,]> gameGrid;
        private readonly MapCursor mapCursor;

        public MapContainer(List<MapObject[,]> gameGrid, ITexture2D cursorTexture)
        {
            this.gameGrid = gameGrid;
            this.mapCursor = BuildMapCursor(cursorTexture);
        }

        private static MapCursor BuildMapCursor(ITexture2D cursorTexture)
        {
            TileCell cursorCell = new TileCell(cursorTexture, GameDriver.CELL_SIZE, 1);
            Vector2 cursorStartPosition = new Vector2(0);
            MapCursor mapCursor = new MapCursor(cursorCell, cursorStartPosition);

            return mapCursor;
        }

        public MapCursor GetMapCursor()
        {
            return mapCursor;
        }
        
        public ReadOnlyCollection<MapObject[,]> GetGameGrid()
        {
            return gameGrid.AsReadOnly();
        }
    }
}
