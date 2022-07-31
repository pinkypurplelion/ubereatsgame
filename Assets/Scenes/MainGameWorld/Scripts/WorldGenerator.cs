using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject tilePrefab;

        public List<Order> orders;
        public List<ShopTile> shops;

        private Random random = new();
        private int tileSize = 1;
        
        // Start is called before the first frame update
        void Start()
        {
            int WorldDimension = 1;
            int BlockDimension = 8;
        
            // Generates the World based on the WorldDimension and BlockDimension
            WorldMap map = new WorldMap {WorldDimension = WorldDimension, BlockDimension = BlockDimension};
            map.GenerateWorld();

            // Draws the World based on the WorldMap graph.
            foreach (var block in map.CityBlocks)
            {
                foreach (var tile in block.Tiles)
                {
                    GameObject t = Instantiate(tilePrefab, new Vector3(
                        block.BlockX * BlockDimension * tileSize + tile.X*tileSize, 
                        2, 
                        block.BlockY * BlockDimension * tileSize + tile.Y*tileSize), Quaternion.identity);
                    t.GetComponent<TileObject>().Tile = tile;
                }
            }
            
        }

        private void FixedUpdate()
        {
            // Happens every 10 seconds
            if (Time.fixedTime % 10f == 0)
            {
                // A quarter of the time.
                if (random.NextDouble() < 0.25)
                {
                    Order order = new Order();
                    orders.Add(order);
                }
            }
        }

        void Update()
        {
        }
    }
}
