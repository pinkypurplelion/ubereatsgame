using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// An object holding a 'city blocks' worth of tiles, used as a grouping effect to make city generation more
    /// modular and efficient.
    /// </summary>
    public class CityBlock
    {
        public List<Tile> Tiles { get; } = new (); // The tiles held by the CityBlock object
        public int BlockDimension { get; set; } = 4; // The side length of the block
        public int BlockX { get; set; } // The X coordinate of the block in the world 
        public int BlockY { get; set; } // The Y coordinate of the block in the world

        public Dictionary<string, bool> directions = new(); // The directions of road pieces 
        public Dictionary<string, int> ConnectionDirections { get; set; } = new(){ 
            { "top", 2 }, 
            { "bottom", 2 },
            { "left", 2 }, 
            { "right", 2 } 
        }; // The locations of the connections to other blocks

        private List<Tile> EdgeConnections { get; } = new(); // Enables multiple blocks to connect together
        private List<List<Tile>> ShortestPaths { get; } = new(); // Used to generate the road paths within the block

        /// <summary>
        /// Generates a new city block with the given dimensions, and finds the paths between the edge connections.
        ///
        /// The city block is represented as a graph with Nodes representing the tiles and Edges representing
        /// neighbouring tiles, eg. if two nodes are connected then they are next to each other on the grid.
        /// </summary>
        public void CreateMap()
        {
            Random random = new Random();
            
            // Generates the base graph of tiles (grids)
            for (var i = 0; i < BlockDimension; i++)
            {
                for (var j = 0; j < BlockDimension; j++)
                {
                    Tiles.Add(new Tile {X = i, Y = j, Type = TileType.Building});
                }
            }
            
            // Connects the nodes in the graph together based on their coordinate position. The edges are weighted.
            foreach (var tile in Tiles)
            {
                double cost = random.NextDouble(); // Helps randomise road paths
                const double costEdge = 10; // Ensures road tiles don't get placed along the edge
                
                // Deals with vertical edges
                if (tile.X == 0)
                {
                    Tile below = Tiles.FirstOrDefault(t => t.X == tile.X + 1 && t.Y == tile.Y);
                    
                    if (tile.Y == 0 || tile.Y == BlockDimension - 1)
                        cost = costEdge;
                    Link link = new Link { ConnectedTile = below, Cost = cost};
                    tile.Connections.Add(link);
                }
                else if (tile.X == BlockDimension - 1)
                {
                    Tile above = Tiles.FirstOrDefault(t => t.X == tile.X - 1 && t.Y == tile.Y);
                    if (tile.Y == 0 || tile.Y == BlockDimension - 1)
                        cost = costEdge;
                    Link link = new Link { ConnectedTile = above, Cost = cost};
                    tile.Connections.Add(link);
                }
                else
                {
                    if (tile.Y == 0 || tile.Y == BlockDimension - 1)
                        cost = costEdge;
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
                        cost = costEdge;
                    Link link = new Link { ConnectedTile = right, Cost = cost};
                    tile.Connections.Add(link);
                }
                else if (tile.Y == BlockDimension - 1)
                {
                    Tile left = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1);
                    if (tile.X == 0 || tile.X == BlockDimension - 1)
                        cost = costEdge;
                    Link link = new Link { ConnectedTile = left, Cost = cost};
                    tile.Connections.Add(link);
                }
                else
                {
                    if (tile.X == 0 || tile.X == BlockDimension - 1)
                        cost = costEdge;
                    Tile right = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y + 1);
                    Link link = new Link { ConnectedTile = right, Cost = cost};
                    tile.Connections.Add(link);
                    Tile left = Tiles.FirstOrDefault(t => t.X == tile.X && t.Y == tile.Y - 1);
                    link = new Link { ConnectedTile = left, Cost = cost};
                    tile.Connections.Add(link);
                }
            }
            
            // Adds the edge connection nodes to a separate list.
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == ConnectionDirections["top"] && t.Y == 0));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == ConnectionDirections["bottom"] && t.Y == BlockDimension - 1));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == 0 && t.Y == ConnectionDirections["left"]));
            EdgeConnections.Add(Tiles.FirstOrDefault(t => t.X == BlockDimension - 1 && t.Y == ConnectionDirections["right"]));
            
            // Finds the shortest paths between the edge connections.
            ShortestPaths.Add(GetShortestPathDijkstra(
                EdgeConnections.FirstOrDefault(t => t.X == ConnectionDirections["top"] && t.Y == 0), 
                EdgeConnections.FirstOrDefault(t => t.X == ConnectionDirections["bottom"]  && t.Y == BlockDimension - 1)));
            
            ShortestPaths.Add(GetShortestPathDijkstra(
                EdgeConnections.FirstOrDefault(t => t.X == ConnectionDirections["bottom"]  && t.Y == BlockDimension - 1), 
                EdgeConnections.FirstOrDefault(t => t.X == 0 && t.Y == ConnectionDirections["left"])));
            
            ShortestPaths.Add(GetShortestPathDijkstra(
                EdgeConnections.FirstOrDefault(t => t.X == 0 && t.Y == ConnectionDirections["left"]), 
                EdgeConnections.FirstOrDefault(t => t.X == BlockDimension - 1 && t.Y == ConnectionDirections["right"])));
            
            ParsePaths();
            
            // Enables only drawing tiles that are next to roads.
            foreach (var tile in Tiles)
            {
                foreach (var link in tile.Connections)
                {
                    if (link.ConnectedTile.Type == TileType.Road)
                    {
                        
                        tile.NextToRoad = true;
                        
                        // Creates shops & houses on the tiles next to roads.
                        if (random.NextDouble() < 0.02 && tile.Type == TileType.Building)
                            tile.Type = TileType.Shop;
                        if (random.NextDouble() > 0.98 && tile.Type == TileType.Building)
                            tile.Type = TileType.House;
                    }
                }
                if (!tile.NextToRoad)
                {
                    tile.Type = TileType.Landscape;
                }
            }
        }

        /// <summary>
        /// Changes the block type of the tile to paths instead of buildings.
        /// </summary>
        void ParsePaths()
        {
            foreach (var path in ShortestPaths)
            {
                foreach (var tile in path)
                {
                    tile.Type = TileType.Road;
                }
            }
        }
        
        /// <summary>
        /// Returns the shortest path between two tiles using Dijkstra's algorithm.
        /// </summary>
        private List<Tile> GetShortestPathDijkstra(Tile start, Tile end)
        {
            DijkstraSearch(start, end);
            List<Tile> shortestPath = new List<Tile> { end };
            BuildShortestPath(shortestPath, end);
            shortestPath.Reverse();
            
            // Resets the pathfinding variables to their original value for the next pathfinding.
            foreach (var tile in Tiles)
            {
                tile.Visited = false;
                tile.MinCostToStart = null;
                tile.NearestToStart = null;
            }
            
            return shortestPath;
        }
        
        /// <summary>
        /// Builds the shortest path from the end tile to the start tile.
        /// </summary>
        private void BuildShortestPath(List<Tile> list, Tile node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            BuildShortestPath(list, node.NearestToStart);
        }
        
        /// <summary>
        /// Performs the Dijkstra search algorithm on the graph between the start and end tiles.
        ///
        /// Implemented from this source: https://gist.github.com/DotNetCoreTutorials/08b0210616769e81034f53a6a420a6d9
        /// Original Author: DotNetCoreTutorials
        ///
        /// Modified By: Liam Angus
        /// Modifications: Modified to work with Tile objects and the hidden graph structure.
        /// </summary>
        private void DijkstraSearch(Tile start, Tile end)
        {
            start.MinCostToStart = 0;
            List<Tile> priorityQueue = new List<Tile> { start };
            do {
                priorityQueue = priorityQueue.OrderBy(x => x.MinCostToStart).ToList();
                Tile node = priorityQueue.First();
                priorityQueue.Remove(node);
                foreach (Link cnn in node.Connections.OrderBy(x => x.Cost))
                {
                    Tile childNode = cnn.ConnectedTile;
                    if (childNode.Visited)
                        continue;
                    if (childNode.MinCostToStart == null ||
                        node.MinCostToStart + cnn.Cost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + cnn.Cost;
                        childNode.NearestToStart = node;
                        if (!priorityQueue.Contains(childNode))
                            priorityQueue.Add(childNode);
                    }
                }
                node.Visited = true;
                if (node == end)
                    return;
            } while (priorityQueue.Any());
        }
    }
}