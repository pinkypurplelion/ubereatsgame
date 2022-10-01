using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public List<Guid> Orders { get; set; } = new();
        // List to future proof in case two shops next to each other.
        // TODO: implement way to select between orders/shops to pick up
        public List<Collider> CurrentShopCollisions { get; set; } = new();
        
        // List to future proof in case two houses next to each other.
        // TODO: implement way to select between orders/houses to drop off
        public List<Collider> CurrentHouseCollisions { get; set; } = new();

        public float speed;
        
        // Player Money System
        public float Money { get; set; }

        // Player Components
        private Rigidbody _rigidbody;
        private BoxCollider _collider;
        
        // UI Elements
        private PlayerUIManager _playerUI;
        

        // Global Components
        private GameObject _worldEventManagerGameObject;
        private WorldEventManager _worldEventManager;

        // Used to move the player
        public Vector2 moveVal;

        public GameObject thrownObject;

        public bool InventoryOpen = false;
    
        // the number of orders the player is able to carry at any time
        public int orderLimit = 2;

        // player starts with a 5 star rating
        public float playerRating = 5;
        
        // Used to setup the current component
        private void Awake()
        {
            _worldEventManagerGameObject = GameObject.Find("WorldEventManager");
            _worldEventManager = _worldEventManagerGameObject.GetComponent<WorldEventManager>();

            
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
            
            _playerUI = transform.Find("PlayerUI").GetComponent<PlayerUIManager>();

            // Passes the objects in PlayerController through to the uiController to enable simple UI updates
            _playerUI.CurrentHouseCollisions = CurrentHouseCollisions;
            _playerUI.CurrentShopCollisions = CurrentShopCollisions;
            _playerUI.Orders = Orders;
            _playerUI.WorldEventManager = _worldEventManager;
            _playerUI.ShopEventCallback = SelectOrderFromShop;
            _playerUI.HouseEventCallback = SelectHouseToDeliver;
            _playerUI.InventoryEventCallback = InvEventPOC;
            
        }

        // Used to configure things that depend on other components
        void Start()
        {
            gameObject.tag = "Player";

            // Updates the labels in the UI to the correct values.
            _playerUI.PlayerMoneyLabel.text = $"Player Balance: {Money}";
            _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
        }

        // Called when the player presses the interact key defined by the input system
        void OnPlayerInteract(InputValue value)
        {
            Debug.Log("Player Interaction Action");
            _playerUI.ToggleInteractUI();
            InventoryOpen = !InventoryOpen;
        }
        
        void OnMenu()
        {
            Debug.Log("Saving Game");
            _worldEventManager.SaveGame();
            Debug.Log("Exiting to Menu");
            SceneManager.LoadScene("MainMenu");
        }

        // Called when the player presses the test save key defined by the input system
        void OnTestSave()
        {
            _worldEventManager.SaveGame();
        }

        void OnTestLoad()
        {
            SaveData sd = _worldEventManager.LoadGame();
            Debug.Log(sd.WorldTime);
        }
        
        // FixedUpdate is called once per physics update (constant time irrespective of frame rate)
        void FixedUpdate()
        {
            UpdatePlayerUI();
            
            if (_rigidbody.transform.position.y < -10)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        void UpdatePlayerUI()
        {
            // Updates the labels in the UI to the correct values.
            _playerUI.PlayerMoneyLabel.text = $"Player Balance: {Money}";
            _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
            _playerUI.PlayerTimeLabel.text = $"Time: {_worldEventManager.GenerateCurrentTimeString()}";
        }
        
        // Called when the player enters the collider of another object
        void OnTriggerEnter(Collider other)
        {
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
        
        // Called when the player leaves the collider of another object
        private void OnTriggerExit(Collider other)
        {
            CurrentShopCollisions.Remove(other);
            CurrentHouseCollisions.Remove(other);

            _playerUI.UpdateInteractUI();
        }
        
        private void SelectHouseToDeliver(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
            if (CurrentHouseCollisions.Count > 0)
            {
                HouseTile house;
                foreach (var houseCollision in CurrentHouseCollisions)
                {
                    if (houseCollision.transform.GetComponent<HouseTile>().HouseID.ToString() == button.name)
                    {
                        house = houseCollision.transform.GetComponent<HouseTile>(); 
                        DeliverOrder(house, Guid.Parse(button.name));
                    }
                }
            }
            _playerUI.UpdateInteractUI();
        }
        
        // Inventory button event proof of concept (POC)
        private void InvEventPOC(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
            Order order = _worldEventManager.Orders.Find(o => o.OrderID == Guid.Parse(button.name));
            if (order != null)
            {
                HouseTile house = _worldEventManager.houses.Find(h => h.HouseID == order.HouseID);
                house.isDelivering = false;
            }
            Orders.Remove(Guid.Parse(button.name));
            _playerUI.UpdateInteractUI();
            Debug.Log("Simulates order being thrown out of the window.");
            GameObject to = Instantiate(thrownObject, transform.position, transform.rotation);
            to.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(0.1f, 2f), Random.Range(0.1f,2f), Random.Range(0.1f, 2f)) * 1000);
        }
        
        private void SelectOrderFromShop(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
            if (Orders.Count <= orderLimit) // TODO implement multi order collection
            {
                // Adds order to player
                Guid orderID = Guid.Parse(button.name);
                Orders.Add(orderID);
                _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
            
                // Highlights house that order must be delivered to
                Order order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                order.PickupTime = _worldEventManager.currentTime;
                HouseTile house = _worldEventManager.houses.Find(h => h.HouseID == order.HouseID);
                house.isDelivering = true;

                // Removes order from shop
                // TODO: is there a more efficient way of doing this?
                foreach (var shopCollision in CurrentShopCollisions)
                {
                    shopCollision.transform.GetComponent<ShopTile>().Orders.Remove(Guid.Parse(button.name));
                }
            }
            else
            {
                // TODO: add popup to communicate this with player
                Debug.Log("Player has reached the order limit. Please deliver an order before collecting another.");
            }
            _playerUI.UpdateInteractUI();
        }
        
        /**
         * This is called when the player is in range of a house and presses the interact button.
         */
        void DeliverOrder(HouseTile tile, Guid houseID)
        {
            Debug.Log("Attempting to deliver order");
            foreach (var orderID in Orders)
            {
                Order order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                if (order != null)
                {
                    if (order.HouseID == houseID)
                    {
                        Debug.Log("Found correct order.");
                        tile.DeliveredOrders.Add(orderID);
                        Orders.Remove(orderID);
                        order.Delivered = true;
                        if (_worldEventManager.currentTime - order.PickupTime < order.TimeToDeliver)
                        {
                            Debug.Log("Order delivered on time.");
                            Money += order.OrderValue;
                            playerRating = Mathf.Clamp(playerRating * 1.05f, 0, 5);
                        }
                        else
                        {
                            float reduceMoney = order.OrderValue /
                                                (1 + (_worldEventManager.currentTime - order.PickupTime - order.TimeToDeliver) * 0.1f);
                            Money += reduceMoney;
                            playerRating = Mathf.Clamp(playerRating * 0.95f, 0, 5);
                            Debug.Log("Order delivered late.");
                            Debug.Log($"Order value: {order.OrderValue}, money given: {reduceMoney}, player rating: {playerRating}");
                        }
                        tile.isDelivering = false;
                        Debug.Log("Order delivered");
                    }
                    else
                    {
                        Debug.Log("Order not for this house");
                    }
                    _playerUI.PlayerMoneyLabel.text = $"Player Balance: {Money}";
                    _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
                }
            }
        }

        public void LoadPlayerData(SaveData data)
        {
            orderLimit = data.PlayerOrderLimit;
            Money = data.PlayerMoney;
            playerRating = data.PlayerRating;
        }
    }
}
