using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Handles the main player logic and interaction with the world. Interfaces with other classes for specific functions.
    /// </summary>
    /// <author>Liam Angus</author>
    public class PlayerController : MonoBehaviour
    {
        // A list of the orders currently being delivered by the player.
        private List<Guid> Orders { get; } = new();
        
        // The shops that the player is currently next to. TODO: implement way to select between orders/shops to pick up (add shop headings in UI)
        private List<Collider> CurrentShopCollisions { get; } = new();
        
        // The houses that the player is currently next to.
        private List<Collider> CurrentHouseCollisions { get; } = new();

        // Player Components
        private Rigidbody _rigidbody;

        // UI Elements
        private PlayerUIManager _playerUI;

        // Global Components
        private GameObject _worldEventManagerGameObject;
        private WorldEventManager _worldEventManager;
        
        // Used to simulate orders being thrown out of the window
        public GameObject thrownObject;

        // Player UI Menu
        public bool inventoryOpen;
        public bool menuOpen;

        
        // The number of orders the player is able to carry at any time
        public int orderLimit = 2; // Player Upgrade
        // Player order delivery time multiplier
        public float deliveryTimeMultiplier = 1f; // Player Upgrade
        
        // Player order multiplier
        public float orderMultiplier = 1f;
        public int maxMultiplier = 5; // Player Upgrade
        
        // Player final score multiplier
        public float scoreMultiplier = 1f;

        /// <summary>
        /// Used to setup the PlayerController class.
        /// </summary>
        private void Awake()
        {
            // Gets the WorldEventManager Object
            _worldEventManagerGameObject = GameObject.Find("WorldEventManager");
            _worldEventManager = _worldEventManagerGameObject.GetComponent<WorldEventManager>();
            
            
            _rigidbody = GetComponent<Rigidbody>();
            GetComponent<BoxCollider>();
            
            _playerUI = transform.Find("PlayerUI").GetComponent<PlayerUIManager>();

            // Passes the objects in PlayerController through to the uiController to enable simple UI updates
            _playerUI.CurrentHouseCollisions = CurrentHouseCollisions;
            _playerUI.CurrentShopCollisions = CurrentShopCollisions;
            _playerUI.Orders = Orders;
            _playerUI.WorldEventManager = _worldEventManager;
            
            // Provides methods for button click events
            _playerUI.ShopEventCallback = SelectOrderFromShop;
            _playerUI.HouseEventCallback = SelectHouseToDeliver;
            _playerUI.InventoryEventCallback = SelectOrderInInventory;
            _playerUI.MenuMainEventCallback = _ => { _worldEventManager.SaveGame(); SceneManager.LoadScene("MainMenu"); };
            _playerUI.MenuExitEventCallback = _ => { _worldEventManager.SaveGame(); Application.Quit();};
            _playerUI.MenuSaveEventCallback = _ => _worldEventManager.SaveGame();
            
            // Processes upgrade information to apply to the player
            ProcessUpgradeInformation();
        }

        /// <summary>
        /// Configures PlayerController based on other component & world information.
        /// </summary>
        private void Start()
        {
            gameObject.tag = "Player";

            // Updates the labels in the UI to the correct values.
            _playerUI.PlayerMoneyLabel.text = $"Player Balance: {_worldEventManager.data.PlayerMoney}";
            _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
        }

        /// <summary>
        /// Called when the player presses the PlayerInteract key.
        /// </summary>
        /// <param name="value"></param>
        private void OnPlayerInteract(InputValue value)
        {
            if (menuOpen) return;
            _playerUI.ToggleInteractUI(); // Toggles the player UI
            inventoryOpen = !inventoryOpen; // Updates the UI state
        }
        
        /// <summary>
        /// Called when the player presses the TestUpgrades key.
        /// TODO: remove, add button to UI.
        /// </summary>
        private void OnTestUpgrades()
        {
            Debug.Log("Saving Game");
            _worldEventManager.SaveGame();
            Debug.Log("Switching to Upgrades Scene");
            SceneManager.LoadScene("PlayerUpgrades");
        }
        
        /// <summary>
        /// Called when the player presses the Menu key.
        /// </summary>
        private void OnMenu()
        {
            if (inventoryOpen) return;
            menuOpen = !menuOpen; // Toggles the UI state
            _playerUI.ToggleMenuUI(); // Toggles the UI
        }
        
        /// <summary>
        /// Called once per physics update.
        /// </summary>
        private void FixedUpdate()
        {
            // Updates the information displayed in the player UI
            UpdatePlayerUI();
            
            // If the player falls off the world, end their game.
            if (_rigidbody.transform.position.y < -10)
            {
                SceneManager.LoadScene("ScoreScreen");
            }
        }

        /// <summary>
        /// Will update the player UI with the current information.
        /// </summary>
        private void UpdatePlayerUI()
        {
            // Updates the labels in the UI to the correct values.
            _playerUI.PlayerMoneyLabel.text = $"Player Balance: {_worldEventManager.data.PlayerMoney}";
            _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
            _playerUI.PlayerTimeLabel.text = $"Time: {_worldEventManager.GenerateCurrentTimeString()}";
            _playerUI.PlayerScoreLabel.text = $"Score: {_worldEventManager.data.PlayerScore}";
            _playerUI.PlayerChainLabel.text = $"Multiplier: {orderMultiplier}";
            _playerUI.PlayerRatingLabel.text = $"Rating: {_worldEventManager.data.PlayerRating}";
        }
        
        /// <summary>
        /// Called when the player enters another object's collider.
        /// </summary>
        /// <param name="other">The collider of the other object</param>
        private void OnTriggerEnter(Collider other)
        {
            // Will add shops and houses that are current colliders to their respective lists. Updates the UI.
            if (other.CompareTag("Shop") && !CurrentShopCollisions.Contains(other))
            {
                CurrentShopCollisions.Add(other);
            }
            else if (other.CompareTag("House") && !CurrentHouseCollisions.Contains(other))
            {
                CurrentHouseCollisions.Add(other);
            }
            _playerUI.UpdateInteractUI();
        }
        
        /// <summary>
        /// Called when the player exits the collider of another object.
        /// </summary>
        /// <param name="other">The other object's collider</param>
        private void OnTriggerExit(Collider other)
        {
            // Removes shops & houses, then updates the UI.
            CurrentShopCollisions.Remove(other);
            CurrentHouseCollisions.Remove(other);

            _playerUI.UpdateInteractUI();
        }
        
        /// <summary>
        /// The method called when the player selects a house from the UI
        /// </summary>
        /// <param name="evt">The click event</param>
        private void SelectHouseToDeliver(ClickEvent evt)
        {
            if (evt.currentTarget is not Button button) return;
            
            if (CurrentHouseCollisions.Count > 0)
            {
                foreach (var house in from houseCollision in CurrentHouseCollisions where 
                             houseCollision.transform.GetComponent<HouseTile>().HouseID.ToString() 
                                == button.name select houseCollision.transform.GetComponent<HouseTile>())
                {
                    DeliverOrder(house);
                }
            }
            _playerUI.UpdateInteractUI();
        }
        
        /// <summary>
        /// Called when a player selects an order from the inventory.
        /// TODO: update model & make it really fly out of the car
        /// </summary>
        /// <param name="evt"></param>
        private void SelectOrderInInventory(ClickEvent evt)
        {
            if (evt.currentTarget is not Button button) return;
            
            var order = _worldEventManager.Orders.Find(o => o.OrderID == Guid.Parse(button.name));
            if (order != null)
            {
                var house = _worldEventManager.houses.Find(h => h.HouseID == order.HouseID);
                house.isDelivering = false;
            }

            Orders.Remove(Guid.Parse(button.name));
            _playerUI.UpdateInteractUI();
            var t = transform;
            var to = Instantiate(thrownObject, t.position, t.rotation);
            to.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(0.1f, 2f), Random.Range(0.1f,2f), Random.Range(0.1f, 2f)) * 1000);
        }
        
        /// <summary>
        /// Called when the player chooses an order from the shop to pick up and add to their inventory.
        /// </summary>
        /// <param name="evt">The click event</param>
        private void SelectOrderFromShop(ClickEvent evt)
        {
            if (evt.currentTarget is not Button button) return;
            if (Orders.Count <= orderLimit)
            {
                // Adds order to player
                var orderID = Guid.Parse(button.name);
                Orders.Add(orderID);
                _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
        
                // Highlights house that order must be delivered to
                var order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                order.PickupTime = _worldEventManager.currentTime;
                order.TimeToDeliver *= deliveryTimeMultiplier;
                var house = _worldEventManager.houses.Find(h => h.HouseID == order.HouseID);
                house.isDelivering = true;
                
                // Removes order from shop
                foreach (var shopCollision in CurrentShopCollisions)
                {
                    shopCollision.transform.GetComponent<ShopTile>().Orders.Remove(Guid.Parse(button.name));
                }
            }
            else
            {
                _playerUI.NotifyPlayer("Player has reached the order limit. Please deliver an order before collecting another.", 5, true);
            }
            _playerUI.UpdateInteractUI();
        }

        /// <summary>
        /// The logic determining if an order is able to be delivered to a given house.
        /// </summary>
        /// <param name="tile">The tile of the house trying to be delivered to</param>
        private void DeliverOrder(HouseTile tile)
        {
            Debug.Log("Attempting to deliver order");
            foreach (var orderID in Orders)
            {
                var order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                if (order == null) continue;
                
                if (order.HouseID == tile.HouseID)
                {
                    Debug.Log("Found correct order.");
                    tile.DeliveredOrders.Add(orderID);
                    Orders.Remove(orderID);
                    order.Delivered = true;
                    if (_worldEventManager.currentTime - order.PickupTime < order.TimeToDeliver)
                    {
                        Debug.Log("Order delivered on time.");
                        _worldEventManager.data.PlayerMoney += order.OrderValue * orderMultiplier;
                        _worldEventManager.data.PlayerRating = Mathf.Clamp(_worldEventManager.data.PlayerRating * 1.05f, 0, 5);
                        
                        // Increasing Order Multiplier
                        orderMultiplier = Mathf.Clamp(orderMultiplier += 1, 1, maxMultiplier);
                        
                        // Increases Player Score
                        _worldEventManager.data.PlayerScore += 75 * scoreMultiplier * orderMultiplier;
                        
                        Debug.Log($"Player Order Multiplier Increased to {orderMultiplier}");
                    }
                    else
                    {
                        var reduceMoney = order.OrderValue /
                                            (1 + (_worldEventManager.currentTime - order.PickupTime - order.TimeToDeliver) * 0.1f);
                        var reducedScore = 75/
                                            (1 + (_worldEventManager.currentTime - order.PickupTime - order.TimeToDeliver) * 0.1f) * orderMultiplier;
                        
                        _worldEventManager.data.PlayerMoney += reduceMoney;
                        _worldEventManager.data.PlayerRating = Mathf.Clamp(_worldEventManager.data.PlayerRating * 0.95f, 0, 5);
                        _worldEventManager.data.PlayerScore += reducedScore;
                        
                        // Resets Order Multiplier
                        orderMultiplier = 1;
                        Debug.Log("Order delivered late.");
                        Debug.Log($"Order value: {order.OrderValue}, money given: {reduceMoney}, player rating: {_worldEventManager.data.PlayerRating}");
                    }
                    tile.isDelivering = false;

                    Debug.Log("Order delivered");
                }
                else
                {
                    Debug.Log("Order not for this house");
                }
            }
        }

        /// <summary>
        /// Parses the information from the player upgrade file and applies it to the player.
        /// </summary>
        private void ProcessUpgradeInformation()
        {
            Debug.Log("Processing Player Upgrading Information");
            // Player Upgrades
            var upgradeOrderCapacity = PlayerUpgrade.AllUpgrades.Find(u => u.upgradeID == "playerOrderCapacity");
            if (upgradeOrderCapacity != null)
            {
                orderLimit = (int) upgradeOrderCapacity.purchasedLevel;
            }
            var upgradeDeliveryTime = PlayerUpgrade.AllUpgrades.Find(u => u.upgradeID == "playerTimeMultiplier");
            if (upgradeDeliveryTime != null)
            {
                deliveryTimeMultiplier = upgradeDeliveryTime.purchasedLevel;
            }
            
            var upgradeMultiplier = PlayerUpgrade.AllUpgrades.Find(u => u.upgradeID == "playerMaxMultiplier");
            if (upgradeMultiplier != null)
            {
                maxMultiplier = 5 + (int) upgradeMultiplier.purchasedLevel;
            }
            
            var upgradeScoreMultiplier = PlayerUpgrade.AllUpgrades.Find(u => u.upgradeID == "playerScoreMultiplier");
            if (upgradeScoreMultiplier != null)
            {
                scoreMultiplier = upgradeScoreMultiplier.purchasedLevel;
            }
        }

        private void SwitchPlayerVehicle()
        {
            // Selects proper vehicle
        }
    }
}
