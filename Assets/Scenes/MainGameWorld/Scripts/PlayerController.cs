using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

        // Called based on Movement action
        void OnMovement(InputValue value)
        {
            moveVal = value.Get<Vector2>();
        }
        
        // Called when the player presses the pick up drop off button defined by the input system
        void OnPickUpDropOff(InputValue value)
        {
            // TODO: remove
        }

        // Called when the player presses the interact key defined by the input system
        void OnPlayerInteract(InputValue value)
        {
            Debug.Log("Player Interaction Action");
            _playerUI.ToggleInteractUI(); //TODO: stop player movement when inventory is open
        }
        
        void OnMenu()
        {
            Debug.Log("Menu");
            _worldEventManager.PlayPause();
        }
        
        // FixedUpdate is called once per physics update (constant time irrespective of frame rate)
        void FixedUpdate()
        {
            Vector3 tempVect = new Vector3(moveVal.x, 0, moveVal.y);
            tempVect = tempVect.normalized * (speed * Time.deltaTime);
            _rigidbody.MovePosition(transform.position + tempVect);
            
            // Updates the labels in the UI to the correct values. TODO: move to better place
            _playerUI.PlayerMoneyLabel.text = $"Player Balance: {Money}";
            _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
        }
        
        // Called when the player enters the collider of another object
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided with " + other.name);
            if (other.CompareTag("Shop") && !CurrentShopCollisions.Contains(other))
            {
                CurrentShopCollisions.Add(other);
            }
            else if (other.CompareTag("House") && !CurrentHouseCollisions.Contains(other))
            {
                CurrentHouseCollisions.Add(other);
            }
            Debug.Log("CurrentShopCollisions: " + CurrentShopCollisions.Count);
            Debug.Log("CurrentHouseCollisions: " + CurrentHouseCollisions.Count);
            //Check collider for specific properties (Such as tag=item or has component=item)
            
            _playerUI.UpdateInteractUI();
        }
        
        // Called when the player leaves the collider of another object
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Left collider with " + other.name);
            CurrentShopCollisions.Remove(other);
            CurrentHouseCollisions.Remove(other);
            Debug.Log("CurrentShopCollisions: " + CurrentShopCollisions.Count);
            
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
            Orders.Remove(Guid.Parse(button.name));
            _playerUI.UpdateInteractUI();
            Debug.Log("Simulates order being thrown out of the window.");
        }
        
        private void SelectOrderFromShop(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
            if (Orders.Count == 0) // TODO implement multi order collection
            {
                // Adds order to player
                Guid orderID = Guid.Parse(button.name);
                Orders.Add(orderID);
                _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
            
                // Highlights house that order must be delivered to
                Order order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                if (order != null)
                {
                    HouseTile house = _worldEventManager.houses.Find(h => h.HouseID == order.HouseID);
                    house.isDelivering = true;
                }
                else
                {
                    Debug.Log("Order not found");
                }
                
                // Removes order from shop
                // TODO: is there a more efficient way of doing this?
                foreach (var shopCollision in CurrentShopCollisions)
                {
                    shopCollision.transform.GetComponent<ShopTile>().Orders.Remove(Guid.Parse(button.name));
                }
            }
            else
            {
                Debug.Log("Can only collect one order at a time");
            }
            _playerUI.UpdateInteractUI();
        }
        
        /**
         * This is called when the player is in range of a house and presses the interact button.
         */
        void DeliverOrder(HouseTile tile, Guid orderID)
        {
            if (Orders.Count > 0)
            {
                Debug.Log("Attempting to deliver order");
                Order order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                if (order == null)
                {
                    Debug.Log("Order not found");
                    return;
                } 
                if (order.HouseID == tile.HouseID) // TODO: implement choosing order to deliver (after MVP)
                {
                    tile.DeliveredOrders.Add(orderID);
                    Orders.Remove(orderID);
                    order.Delivered = true;
                    Money += order.OrderValue;
                    Debug.Log("Order delivered");
                }
                else
                {
                    Debug.Log("Order not for this house");
                }
                _playerUI.PlayerMoneyLabel.text = $"Player Balance: {Money}";
                _playerUI.PlayerOrdersLabel.text = $"Player Orders: {Orders.Count}";
            }
            else
            {
                Debug.Log("No orders to deliver");
            }
        }
    }
}
