using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class CityBlock
    {

        public List<Tile> Tiles { get; set; } = new List<Tile>();

        public int BlockDimension { get; set; } = 4;
        
        public int BlockX { get; set; } = 0;
        public int BlockY { get; set; } = 0;        
        
        // public int[] Connections { get; set; } = { 1, 2, 1, 2}; // TOP, BOTTOM, LEFT, RIGHT
        public Dictionary<string, int> ConnectionDirections { get; set; } = new(){ 
            { "top", 1 }, { "bottom", 2 },
            { "left", 3 }, { "right", 4 } 
        };


        public List<Tile> EdgeConnections { get; set; } = new List<Tile>();
        
        public List<List<Tile>> ShortestPaths { get; set; } = new List<List<Tile>>();

        public void CreateMap()
        {
            Random random = new Random();
            
            for (var i = 0; i < BlockDimension; i++)
            {
                for (var j = 0; j < BlockDimension; j++)
                {
                    Tiles.Add(new Tile {X = i, Y = j, Type = 1});
                }
            }

            foreach (var tile in Tiles)
                //TODO: Add variation in Cost for links
            {
                double cost = random.NextDouble();
                Debug.Log(cost);
                // Deals with vertical edges
                if (tile.X == 0)
                {
                    Tile below = Tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y);
                    
                    if (tile.Y == 0 || tile.Y == BlockDimension - 1)
                        cost = 2;
                    Link link = new Link { ConnectedTile = below, Cost = cost};
                    tile.Connections.Add(link);
                }
                else if (tile.X == BlockDimension - 1)
                {
                    Tile above = Tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y);
                    if (tile.Y == 0 || tile.Y == BlockDimension - 1)
                        cost = 2;
                    Link link = new Link { ConnectedTile = above, Cost = cost};
                    tile.Connections.Add(link);
                }
                else
                {
                    if (tile.Y == 0 || tile.Y == BlockDimension - 1)
                        cost = 2;
                    Tile below = Tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y);
                    Link link = new Link { ConnectedTile = below, Cost = cost};
                    tile.Connections.Add(link);
                    Tile above = Tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y);
                    link = new Link { ConnectedTile = above, Cost = cost};
                    tile.Connections.Add(link);
                }

                // Deals with horizontal edges
                if (tile.Y == 0)
                {
                    Tile right = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y + 1);
                    if (tile.X == 0 || tile.X == BlockDimension - 1)
                        cost = 2;
                    Link link = new Link { ConnectedTile = right, Cost = cost};
                    tile.Connections.Add(link);
                }
                else if (tile.Y == BlockDimension - 1)
                {
                    Tile left = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1);
                    if (tile.X == 0 || tile.X == BlockDimension - 1)
                        cost = 2;
                    Link link = new Link { ConnectedTile = left, Cost = cost};
                    tile.Connections.Add(link);
                }
                else
                {
                    if (tile.X == 0 || tile.X == BlockDimension - 1)
                        cost = 2;
                    Tile right = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y + 1);
                    Link link = new Link { ConnectedTile = right, Cost = cost};
                    tile.Connections.Add(link);
                    Tile left = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1);
                    link = new Link { ConnectedTile = left, Cost = cost};
                    tile.Connections.Add(link);
                }
            }
            
            Debug.Log(BlockDimension);
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == ConnectionDirections["top"] && t.Y == 0));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == ConnectionDirections["bottom"] && t.Y == BlockDimension - 1));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == 0 && t.Y == ConnectionDirections["left"]));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == BlockDimension - 1 && t.Y == ConnectionDirections["right"]));
            
            ShortestPaths.Add(GetShortestPathDijkstra(
                EdgeConnections.FirstOrDefault(t => t.X == ConnectionDirections["top"] && t.Y == 0), 
                EdgeConnections.LastOrDefault(t => t.X == ConnectionDirections["bottom"]  && t.Y == BlockDimension - 1)));
            ShortestPaths.Add(GetShortestPathDijkstra(
                EdgeConnections.FirstOrDefault(t => t.X == ConnectionDirections["top"] && t.Y == 0), 
                EdgeConnections.LastOrDefault(t => t.X == 0 && t.Y == ConnectionDirections["left"])));
            ShortestPaths.Add(GetShortestPathDijkstra(
                EdgeConnections.FirstOrDefault(t => t.X == ConnectionDirections["top"] && t.Y == 0), 
                EdgeConnections.LastOrDefault(t => t.X == BlockDimension - 1 && t.Y == ConnectionDirections["right"])));
            
            ParsePaths();
            
            Debug.Log(Tiles.Count);
            
        }


        void ParsePaths()
        {
            foreach (var path in ShortestPaths)
            {
                foreach (var tile in path)
                {
                    tile.Type = 0;
                }
            }
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