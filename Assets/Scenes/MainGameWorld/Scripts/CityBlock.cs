using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class CityBlock
    {

        public List<Tile> Tiles { get; set; } = new List<Tile>();

        public int BlockDimension { get; set; } = 4;
        
        public List<Tile> EdgeConnections { get; set; } = new List<Tile>();
        
        public List<List<Tile>> ShortestPaths { get; set; } = new List<List<Tile>>();

        public void CreateMap()
        {
            for (var i = 0; i < BlockDimension; i++)
            {
                for (var j = 0; j < BlockDimension; j++)
                {
                    Tiles.Add(new Tile {X = i, Y = j, Type = 0});
                }
            }

            foreach (var tile in Tiles)
                //TODO: Add variation in Cost for links
            {
                // Deals with vertical edges
                if (tile.X == 0)
                {
                    Tile below = Tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y);
                    Link link = new Link { ConnectedTile = below, Cost = 1};
                    tile.Connections.Add(link);
                }
                else if (tile.X == BlockDimension - 1)
                {
                    Tile above = Tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y);
                    Link link = new Link { ConnectedTile = above, Cost = 1};
                    tile.Connections.Add(link);
                }
                else
                {
                    Tile below = Tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y);
                    Link link = new Link { ConnectedTile = below, Cost = 1};
                    tile.Connections.Add(link);
                    Tile above = Tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y);
                    link = new Link { ConnectedTile = above, Cost = 1};
                    tile.Connections.Add(link);
                }

                // Deals with horizontal edges
                if (tile.Y == 0)
                {
                    Tile right = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y + 1);
                    Link link = new Link { ConnectedTile = right, Cost = 1};
                    tile.Connections.Add(link);
                }
                else if (tile.Y == BlockDimension - 1)
                {
                    Tile left = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1);
                    Link link = new Link { ConnectedTile = left, Cost = 1};
                    tile.Connections.Add(link);
                }
                else
                {
                    Tile right = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y + 1);
                    Link link = new Link { ConnectedTile = right, Cost = 1};
                    tile.Connections.Add(link);
                    Tile left = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1);
                    link = new Link { ConnectedTile = left, Cost = 1};
                    tile.Connections.Add(link);
                }
            }
            
            
            // TODO: Make these connections random/given by constructor
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == 2 && t.Y == 0));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == 1 && t.Y == BlockDimension - 1));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == 0 && t.Y == 2));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == BlockDimension - 1 && t.Y == 1));
            
            // TODO: Make these paths dynamic based on above changes
            ShortestPaths.Add(GetShortestPathDijkstra(EdgeConnections.FirstOrDefault(t => t.X == 2 && t.Y == 0), EdgeConnections.LastOrDefault(t => t.X == 1 && t.Y == BlockDimension - 1)));
            ShortestPaths.Add(GetShortestPathDijkstra(EdgeConnections.FirstOrDefault(t => t.X == 2 && t.Y == 0), EdgeConnections.LastOrDefault(t => t.X == 0 && t.Y == 2)));
            ShortestPaths.Add(GetShortestPathDijkstra(EdgeConnections.FirstOrDefault(t => t.X == 2 && t.Y == 0), EdgeConnections.LastOrDefault(t => t.X == BlockDimension - 1 && t.Y == 1)));
            
            Debug.Log(Tiles.Count);
            
        }

        public List<Tile> GetShortestPathDijkstra(Tile start, Tile end)
        {
            DijkstraSearch(start, end);
            var shortestPath = new List<Tile>();
            shortestPath.Add(end);
            BuildShortestPath(shortestPath, end);
            shortestPath.Reverse();
            return shortestPath;
        }
    
        private void BuildShortestPath(List<Tile> list, Tile node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            BuildShortestPath(list, node.NearestToStart);
        }
    
        private void DijkstraSearch(Tile start, Tile end)
        {
            start.MinCostToStart = 0;
            var prioQueue = new List<Tile>();
            prioQueue.Add(start);
            do {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var cnn in node.Connections.OrderBy(x => x.Cost))
                {
                    var childNode = cnn.ConnectedTile;
                    if (childNode.Visited)
                        continue;
                    if (childNode.MinCostToStart == null ||
                        node.MinCostToStart + cnn.Cost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + cnn.Cost;
                        childNode.NearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }
                node.Visited = true;
                if (node == end)
                    return;
            } while (prioQueue.Any());
        }
        
        
        
        
    }
    
    public class Tile
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        
        public int Type { get; set; }
        
        public List<Link> Connections { get; set; } = new List<Link>();

        public double? MinCostToStart { get; set; }
        public Tile NearestToStart { get; set; }
        public bool Visited { get; set; }
        public double StraightLineDistanceToEnd { get; set; }
    }
    
    public class Link
    {
        public double Cost { get; set; }
        public Tile ConnectedTile { get; set; }

        public override string ToString()
        {
            return "-> " + ConnectedTile.ToString();
        }
    }
}