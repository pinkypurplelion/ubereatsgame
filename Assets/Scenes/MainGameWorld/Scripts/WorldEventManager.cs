using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// The main world event manager, responsible for all global events and data.
    /// </summary>
    /// <author>Liam Angus</author>
    public class WorldEventManager : MonoBehaviour
    {
        public GameObject tilePrefab;
        public GameObject playerPrefab;

        private WorldMap _map;

        public readonly List<Order> Orders = new();
        
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
        // public float worldStartTimeAdjust;
        public float worldStartTime;
        public float currentTime;
        private static readonly string[] Week = {"Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"};
        private static string _day;
        
        // World Lighting
        public GameObject worldLight;
        
        // Player Controller
        public PlayerController playerController;

        public SaveData data;

        private void Awake()
        {
            // Generates the World based on the WorldDimension and BlockDimension
            _map = new WorldMap { WorldDimension = worldDimension, BlockDimension = blockDimension };
            _map.GenerateWorld();
            worldStartTime = Time.time;
        }
        
        private void Start()
        {
            // Draws the World based on the WorldMap graph.
            foreach (var block in _map.CityBlocks)
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
            data = FileManager.LoadDataDefault<SaveData>(SaveData.SaveName) ?? SaveData.Defaults();

            // Loads the upgrade information from the local save data
            VehicleUpgrade.AllUpgrades = FileManager.LoadData(VehicleUpgrade.SaveName, new List<VehicleUpgrade>());
            PlayerUpgrade.AllUpgrades = FileManager.LoadData(PlayerUpgrade.SaveName, new List<PlayerUpgrade>());
        }
        
        private void FixedUpdate()
        {
            if (Time.fixedTime % orderGenerationTime == 0)
            {
                GenerateOrder();
            }
            
            // World Daylight Cycle Manager
            worldLight.transform.rotation = Quaternion.Euler(currentTime -90 % 360,0,0);
        }

        private void Update()
        {
            // Time Management
            currentTime = Time.time - worldStartTime + data.WorldTime;
            SimulateRent(currentTime);
        }

        // Will pause/resume the game
        public void PlayPause()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        // Used to generate a order, randomly choosing a shop and a house.
        private void GenerateOrder()
        {
            ShopTile shop = shops[_random.Next(shops.Count)];
            HouseTile house = houses[_random.Next(houses.Count)];
            Order order = new Order
            {
                ShopID = shop.ShopID,
                HouseID = house.HouseID,
                OrderValue = _random.Next(15, 80),
                Customer = house.Customers[_random.Next(house.Customers.Count)],
                CreationTime = currentTime,
                TimeToDeliver = _random.Next(30, 120) // Delivery timeframe between 30 and 120 seconds, TODO: upgrade for longer delivery times/dependant on reputation
            };
            Orders.Add(order);
            shop.Orders.Add(order.OrderID);
        }
        
        private string TempGenerateName()
        {
            List<String> names = new() { "bob", "judy", "jane", "sarah", "adam", "spike", "erandi"};
            return names[_random.Next(names.Count)];
        }

        public string GenerateCurrentTimeString()
        {
            return ConvertTimeToString(currentTime);
        }

        /// <summary>
        /// Will convert the current world time into a string able to be displayed to the user.
        /// Scale: 360 IRL seconds per game day.
        /// </summary>
        /// <param name="time">The current world time</param>
        /// <returns>A string representation of the current world time</returns>
        public static string ConvertTimeToString(float time)
        {
            int minutes = (int) Math.Floor(time/0.25 % 60);
            int hours = (int) (time / 15) % 24;
            int days = (int) (time / 360) % 7;
            _day = Week[days % 7];

            return $"{_day},{days}d, {hours}h {minutes}m";
        }

        /// <summary>
        /// Used to simulate the rent of the player.
        /// </summary>
        /// <param name="time">The current world time</param>
        /// <author>Jayath Gunawardena</author>
        private void SimulateRent(float time)
        {
            if (time % 2520 == 0 && time > 2610)
            {
                data.PlayerMoney -= 500;
                if (data.PlayerMoney < 0)
                {
                    SceneManager.LoadScene("ScoreScreen");
                }
            }
        }
        
        /// <summary>
        /// Used to save the world state.
        /// </summary>
        public void SaveGame()
        {
            Debug.Log("Attempting to Save Game...");
            FileManager.SaveData(SaveData.SaveName, JsonConvert.SerializeObject(data));
            Debug.Log("Game Saved!");
        }
    }
}

