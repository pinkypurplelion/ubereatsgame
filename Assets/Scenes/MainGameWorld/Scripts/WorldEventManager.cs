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
        
        public List<ShopTile> shops = new();
        public List<HouseTile> houses = new();
        public List<RoadTile> roads = new();
        public List<Customer> customers = new();

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
                        HouseTile h = tileObject.transform.Find("house").GetComponent<HouseTile>();
                        for (int i = 0; i < _random.Next(1,3); i++)
                        {
                            Customer c = new Customer(TempGenerateName(), h);
                            customers.Add(c);
                            h.Customers.Add(c);
                        }
                        houses.Add(h);
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
                OrderValue = _random.Next(15, 80),
                Customer = house.Customers[_random.Next(house.Customers.Count)]
            };
            Orders.Add(order);
            shop.Orders.Add(order.OrderID);
        }
        
        String TempGenerateName()
        {
            List<String> names = new() { "bob", "judy", "jane", "sarah", "adam", "spike", "erandi"};
            return names[_random.Next(names.Count)];
        }
    }
    
        
}

