using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    public class Tile
    {
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
        Shop
    }
}