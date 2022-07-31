using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject tilePrefab;

        public List<Order> orders;
        public List<ShopTile> shops;

        private readonly Random _random = new();
        private const int TileSize = 1;

        // How many recursions when generating the world.
        public int worldDimension = 1;

        // The width of the CityBlocks.
        public int blockDimension = 8;

        // Start is called before the first frame update
        void Start()
        {
            // Generates the World based on the WorldDimension and BlockDimension
            WorldMap map = new WorldMap { WorldDimension = worldDimension, BlockDimension = blockDimension };
            map.GenerateWorld();

            // Draws the World based on the WorldMap graph.
            foreach (var block in map.CityBlocks)
            {
                foreach (var tile in block.Tiles)
                {
                    GameObject tileObject = Instantiate(tilePrefab, new Vector3(
                        block.BlockX * blockDimension * TileSize + tile.X * TileSize,
                        2,
                        block.BlockY * blockDimension * TileSize + tile.Y * TileSize), Quaternion.identity);
                    tileObject.GetComponent<TileObject>().Tile = tile;
                }
            }

            void FixedUpdate()
            {
                // Happens every 10 seconds
                if (Time.fixedTime % 10f == 0)
                {
                    // A quarter of the time.
                    if (_random.NextDouble() < 0.25)
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
}
