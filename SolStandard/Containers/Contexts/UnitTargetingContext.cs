﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SolStandard.Entity.Unit;
using SolStandard.Map;
using SolStandard.Map.Elements;
using SolStandard.Utility;

namespace SolStandard.Containers.Contexts
{
    public class UnitTargetingContext
    {
        private readonly SpriteAtlas spriteAtlas;
        private readonly bool numbersVisible;

        public UnitTargetingContext(SpriteAtlas spriteAtlas, bool numbersVisible = false)
        {
            this.spriteAtlas = spriteAtlas;
            this.numbersVisible = numbersVisible;
        }

        public void GenerateTargetingGrid(Vector2 origin, int[] ranges, Layer mapLayer = Layer.Dynamic)
        {
            List<MapDistanceTile> visited = GenerateTargetingGrid(origin, ranges, numbersVisible);

            AddVisitedTilesToGameGrid(visited, mapLayer);
        }

        public void GenerateThreatGrid(Vector2 origin, GameUnit unit)
        {
            new UnitMovingContext(MapDistanceTile.GetTileSprite(MapDistanceTile.TileType.Dark))
                .GenerateMoveGrid(origin, unit.Stats.Mv, unit.Team);

            foreach (MapElement mapElement in MapContainer.GameGrid[(int) Layer.Dynamic])
            {
                MapDistanceTile tile = (MapDistanceTile) mapElement;
                if (tile == null) continue;

                //Generate attack tiles for the perimeter of the grid
                GenerateTargetingGrid(tile.MapCoordinates, unit.Stats.CurrentAtkRange, Layer.Preview);
            }

            //Clean up overlapping tiles
            foreach (MapElement mapElement in MapContainer.GameGrid[(int) Layer.Dynamic])
            {
                MapDistanceTile tile = (MapDistanceTile) mapElement;
                if (tile != null)
                {
                    MapContainer.GameGrid[(int) Layer.Preview][(int) tile.MapCoordinates.X, (int) tile.MapCoordinates.Y]
                        = null;
                }
            }
        }

        private List<MapDistanceTile> GenerateTargetingGrid(Vector2 origin, int[] ranges, bool distanceVisible)
        {
            //Breadth First Search Algorithm (with limit)
            Queue<MapDistanceTile> frontier = new Queue<MapDistanceTile>();

            MapDistanceTile startTile = new MapDistanceTile(spriteAtlas, origin, 0, distanceVisible);
            frontier.Enqueue(startTile);

            List<MapDistanceTile> visited = new List<MapDistanceTile> {startTile};

            while (frontier.Count > 0)
            {
                MapDistanceTile currentTile = frontier.Dequeue();

                if (currentTile.Distance >= ranges.Max()) continue;

                List<Vector2> visitedCoordinates = visited.Select(tile => tile.MapCoordinates).ToList();
                IEnumerable<MapDistanceTile> neighbours = GetNeighbours(currentTile, visitedCoordinates, distanceVisible);

                foreach (MapDistanceTile neighbour in neighbours)
                {
                    if (visited.Contains(neighbour)) continue;

                    frontier.Enqueue(neighbour);
                    visited.Add(neighbour);
                }
            }

            visited = RemoveTilesOutOfRange(visited, ranges);
            return visited;
        }

        private static List<MapDistanceTile> RemoveTilesOutOfRange(List<MapDistanceTile> visited, int[] ranges)
        {
            List<MapDistanceTile> tilesToKeep = new List<MapDistanceTile>();

            foreach (int range in ranges)
            {
                foreach (MapDistanceTile tile in visited)
                {
                    if (tile.Distance == range)
                    {
                        tilesToKeep.Add(tile);
                    }
                }
            }

            return tilesToKeep;
        }

        private static IEnumerable<MapDistanceTile> GetNeighbours(MapDistanceTile currentTile,
            IReadOnlyCollection<Vector2> visitedCoordinates, bool distanceVisible)
        {
            List<MapDistanceTile> neighbours = new List<MapDistanceTile>();

            Vector2 north = new Vector2(currentTile.MapCoordinates.X, currentTile.MapCoordinates.Y - 1);
            Vector2 south = new Vector2(currentTile.MapCoordinates.X, currentTile.MapCoordinates.Y + 1);
            Vector2 east = new Vector2(currentTile.MapCoordinates.X + 1, currentTile.MapCoordinates.Y);
            Vector2 west = new Vector2(currentTile.MapCoordinates.X - 1, currentTile.MapCoordinates.Y);

            
            if (CanPlaceTileAtCoordinates(north, visitedCoordinates))
            {
                neighbours.Add(
                    new MapDistanceTile(currentTile.SpriteAtlas, north, currentTile.Distance + 1, distanceVisible)
                );
            }

            if (CanPlaceTileAtCoordinates(south, visitedCoordinates))
            {
                neighbours.Add(
                    new MapDistanceTile(currentTile.SpriteAtlas, south, currentTile.Distance + 1, distanceVisible)
                );
            }

            if (CanPlaceTileAtCoordinates(east, visitedCoordinates))
            {
                neighbours.Add(
                    new MapDistanceTile(currentTile.SpriteAtlas, east, currentTile.Distance + 1, distanceVisible)
                );
            }

            if (CanPlaceTileAtCoordinates(west, visitedCoordinates))
            {
                neighbours.Add(
                    new MapDistanceTile(currentTile.SpriteAtlas, west, currentTile.Distance + 1, distanceVisible)
                );
            }

            return neighbours;
        }

        private static bool CanPlaceTileAtCoordinates(Vector2 coordinates, IEnumerable<Vector2> unavailableCoordinates)
        {
            if (unavailableCoordinates.Contains(coordinates)) return false;

            return GameMapContext.CoordinatesWithinMapBounds(coordinates);
        }

        private static void AddVisitedTilesToGameGrid(IEnumerable<MapDistanceTile> visitedTiles, Layer layer)
        {
            foreach (MapDistanceTile tile in visitedTiles)
            {
                MapContainer.GameGrid[(int) layer][(int) tile.MapCoordinates.X, (int) tile.MapCoordinates.Y] = tile;
            }
        }
    }
}