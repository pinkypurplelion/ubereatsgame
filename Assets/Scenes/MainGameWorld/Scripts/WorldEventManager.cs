using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        // The start time of the world (will be used to continue player progress)
        public float worldStartTimeAdjust;
        public float worldStartTime;
        public float currentTime;

        // World Lighting
        public GameObject worldLight;
        
        // Player Controller
        public PlayerController playerController;
        
        private void Awake()
        {
            // Generates the World based on the WorldDimension and BlockDimension
            map = new WorldMap { WorldDimension = worldDimension, BlockDimension = blockDimension };
            map.GenerateWorld();
            worldStartTime = Time.time;
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
            GameObject player = Instantiate(playerPrefab, new Vector3(randomRoad.x, 5, randomRoad.z), Quaternion.identity);
            playerController = player.GetComponent<PlayerController>();

            // Load Game Data
            Debug.Log("Loading Game Data");
            SaveData data = LoadGame();
            playerController.LoadPlayerData(data);
            LoadWorldData(data);
            Debug.Log("Game Data Loaded");
        }
        
        void FixedUpdate()
        {
            if (Time.fixedTime % orderGenerationTime == 0)
            {
                GenerateOrder();
            }
            
            // World Daylight Cycle Manager
            worldLight.transform.rotation = Quaternion.Euler(currentTime -90 % 360,0,0);
        }

        void Update()
        {
            // Time Management
            currentTime = Time.time - worldStartTime + worldStartTimeAdjust;
        }

        // Will pause/resume the game
        public void PlayPause()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
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

        public String GenerateCurrentTimeString()
        {
            int minutes = (int) Math.Floor(currentTime/0.25 % 60);
            int hours = (int) (currentTime / 15) % 24;
            int days = (int) (currentTime / 360) % 7;
            return $"{days}d, {hours}h {minutes}m";
        }

        public void SaveGame()
        {
            Debug.Log("Attempting to Save Game...");
            SaveData data = new SaveData
            {
                WorldTime = currentTime,
                PlayerOrderLimit = playerController.orderLimit,
                PlayerRating = playerController.playerRating,
                PlayerMoney = playerController.Money
            };
            string jsonData = JsonUtility.ToJson(data);
            Debug.Log($"Save Data: {jsonData}");
            FileManager.WriteToFile("testsave.json", jsonData);
            Debug.Log("Game Saved!");
        }

        public SaveData LoadGame()
        {
            if (FileManager.LoadFromFile("testsave.json", out var json))
            {
                Debug.Log("Load complete");
                return JsonUtility.FromJson<SaveData>(json);
            }
            Debug.Log("Load failed");
            return null;
        }

        void LoadWorldData(SaveData data)
        {
            worldStartTimeAdjust = data.WorldTime;
        }
    }
}

