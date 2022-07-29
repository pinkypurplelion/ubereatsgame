using System;
using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public int Type { get; set; }
        
        public List<Link> Connections { get; set; } = new List<Link>();

        public double? MinCostToStart { get; set; }
        public Tile NearestToStart { get; set; }
        public bool Visited { get; set; }
    }
    
    public class Link
    {
        public double Cost { get; set; }
        public Tile ConnectedTile { get; set; }
    }
}