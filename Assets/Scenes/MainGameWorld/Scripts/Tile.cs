using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /**
     * Holds the information about the Tile. This class is referenced as the Node in the WorldMap graph.
     *
     * Attached to the instantiated GameObjects at runtime.
     */
    public class Tile
    {
        public Guid TileID = Guid.NewGuid();
        public int X { get; set; }
        public int Y { get; set; }
        
        public TileType Type { get; set; }
        
        public List<Link> Connections { get; } = new();

        public double? MinCostToStart { get; set; }
        public Tile NearestToStart { get; set; }
        public bool Visited { get; set; }

        public bool NextToRoad { get; set; }
    }
    
    public class Link
    {
        public double Cost { get; set; }
        public Tile ConnectedTile { get; set; }
    }

    public enum TileType
    {
        Road,
        Building,
        House,
        Shop,
        Landscape
    }
}