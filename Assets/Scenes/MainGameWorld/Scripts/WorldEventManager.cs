using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldEventManager : MonoBehaviour
    {
        public GameObject tilePrefab;
        public GameObject playerPrefab;

        public WorldMap map;

        public List<Order> Orders = new();
        
        public List<ShopTile> shops;
        public List<HouseTile> houses;
        public List<RoadTile> roads;

        private readonly Random _random = new();
        private const int TileSize = 1;

        // How many recursions when generating the world.
        public int worldDimension = 1;

        // The width of the CityBlocks.
        public int blockDimension = 8;

        // How often orders are generated (in seconds)
        public float orderGenerationTime = 20f;


        private void Awake()
        {
            // Generates the World based on the WorldDimension and BlockDimension
            map = new WorldMap { WorldDimension = worldDimension, BlockDimension = blockDimension };
            map.GenerateWorld();
        }

        // Start is called before the first frame update
        void Start()
        {
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
                    
                    if (tile.Type == TileType.Shop)
                    {
                        shops.Add(tileObject.transform.Find("shop").GetComponent<ShopTile>());
                    }
                    else if (tile.Type == TileType.House)
                    {
                        houses.Add(tileObject.transform.Find("house").GetComponent<HouseTile>());
                    } else if (tile.Type == TileType.Road)
                    {
                        roads.Add(tileObject.transform.Find("road").GetComponent<RoadTile>());
                    }
                }
            }
            
            // Spawns player at a random road
            Vector3 randomRoad = roads[_random.Next(roads.Count)].transform.position;
            Instantiate(playerPrefab, new Vector3(randomRoad.x, 5, randomRoad.z), Quaternion.identity);

            Debug.Log("number of shops: " + shops.Count);
            Debug.Log("number of houses: " + houses.Count);
        }
        
        void FixedUpdate()
        {
            if (Time.fixedTime % orderGenerationTime == 0)
            {
                GenerateOrder();
            }
        }

        void Update()
        {
        }

        // Used to generate a order, randomly choosing a shop and a house.
        void GenerateOrder()
        {
            ShopTile shop = shops[_random.Next(shops.Count)];
            HouseTile house = houses[_random.Next(houses.Count)];
            Order order = new Order
            {
                ShopID = shop.ShopID,
                HouseID = house.HouseID,
                OrderValue = _random.Next(15, 80)
            };
            Orders.Add(order);
            shop.Orders.Add(order.OrderID);
        }
    }
    
        
}

