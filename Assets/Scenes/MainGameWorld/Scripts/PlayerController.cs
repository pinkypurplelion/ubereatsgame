using System;
using System.Collections.Generic;
using System.Linq;
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
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;
        private Box _shopBox;
        private Box _houseBox;
        private Box _inventoryBox;
        
        private Label _orderPlayerCountLabel;
        private Label _playerMoneyLabel;

        // Global Components
        private GameObject _worldEventManagerGameObject;
        private WorldEventManager _worldEventManager;
        
        // Used to setup the current component
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
            _uiDocument = transform.Find("PlayerUI").GetComponent<UIDocument>();
            _worldEventManagerGameObject = GameObject.Find("WorldEventManager");
            _worldEventManager = _worldEventManagerGameObject.GetComponent<WorldEventManager>();
        }

        // Used to configure things that depend on other components
        void Start()
        {
            gameObject.tag = "Player";
            _rootVisualElement = _uiDocument.rootVisualElement;
            _orderPlayerCountLabel = _rootVisualElement.Q<Label>("PlayerOrderCount");
            _playerMoneyLabel = _rootVisualElement.Q<Label>("PlayerBankBalance");
            _orderPlayerCountLabel.text = $"Orders: {Orders.Count}";
            _playerMoneyLabel.text = $"Player Balance: {Money}";
        }

        public Vector2 moveVal;

        // Called based on Movement action
        void OnMovement(InputValue value)
        {
            moveVal = value.Get<Vector2>();
        }

        // Generates the UI to select order from a shop
        private void GenerateShopUI()
        {
            _shopBox = new Box();
            List<Guid> availableOrders = new();
            foreach (var shopCollision in CurrentShopCollisions)
            {
                availableOrders.AddRange(shopCollision.transform.GetComponent<ShopTile>().Orders);
            }
            
            foreach (var order in availableOrders)
            {
                _shopBox.Add(GenerateOrderUI(order.ToString(), "0"));
            }
            
            _rootVisualElement.Add(_shopBox);
            
            var buttons = _rootVisualElement.Query<Button>();
            buttons.ForEach(button => button.RegisterCallback<ClickEvent>(SelectOrderFromShop));
        }

        private void GenerateHouseUI()
        {
            _houseBox = new Box();

            foreach (var houseCollision in CurrentHouseCollisions)
            {
                _houseBox.Add(GenerateHouseUI(houseCollision.transform.GetComponent<HouseTile>().HouseID.ToString()));
            }

            _rootVisualElement.Add(_houseBox);
            
            var buttons = _rootVisualElement.Query<Button>();
            buttons.ForEach(button => button.RegisterCallback<ClickEvent>(SelectHouseToDeliver));
        }

        // Generates a UI element for a house
        private Box GenerateHouseUI(string customerName)
        {
            Box houseBox = new Box();
            Label customerNameLabel = new Label();
            Button selectHouseButton = new Button();
            customerNameLabel.text = customerName;
            selectHouseButton.name = customerName;
            selectHouseButton.text = "Select House";
            houseBox.Add(customerNameLabel);
            houseBox.Add(selectHouseButton);
            return houseBox;
        }
        
        // Generates a UI element for an order
        private Box GenerateOrderUI(string customerName, string orderPrice)
        {
            Box orderBox = new Box();
            Label customerNameLabel = new Label();
            Label orderPriceLabel = new Label();
            Button selectOrderButton = new Button();
            customerNameLabel.text = customerName;
            orderPriceLabel.text = orderPrice;
            selectOrderButton.name = customerName;
            selectOrderButton.text = "Select Order";
            orderBox.Add(customerNameLabel);
            orderBox.Add(orderPriceLabel);
            orderBox.Add(selectOrderButton);
            return orderBox;
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
        }
        
        private void SelectOrderFromShop(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
            if (Orders.Count == 0) // TODO implement multi order collection
            {
                // Adds order to player
                Guid orderID = Guid.Parse(button.name);
                Orders.Add(orderID);
                _orderPlayerCountLabel.text = $"Orders: {Orders.Count}";
            
                // Highlights house that order must be delivered to
                Order order = _worldEventManager.Orders.Find(o => o.OrderID == orderID);
                if (order != null)
                {
                    HouseTile house = _worldEventManager.houses.Find(h => h.HouseID == order.HouseID);
                    house.IsDelivering = true;
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
        }
        
        // FixedUpdate is called once per physics update (constant time irrespective of frame rate)
        void FixedUpdate()
        {
            Vector3 tempVect = new Vector3(moveVal.x, 0, moveVal.y);
            tempVect = tempVect.normalized * (speed * Time.deltaTime);
            _rigidbody.MovePosition(transform.position + tempVect);
        }

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
        }

        // Called when the player presses the pick up drop off button defined by the input system
        void OnPickUpDropOff(InputValue value)
        {
            if (_shopBox == null && CurrentShopCollisions.Count > 0)
                GenerateShopUI();
            
            // TODO: update to UI
            if (_houseBox == null && CurrentHouseCollisions.Count > 0)
                GenerateHouseUI();
            
            _orderPlayerCountLabel.text = $"Orders: {Orders.Count}";
            Debug.Log("Pick up/drop off action");
        }

        // Called when the player presses the interact key defined by the input system
        void OnPlayerInteract(InputValue value)
        {
            Debug.Log("Interact action");
            if (_inventoryBox == null)
            {
                GeneratePlayerInventory();
            }
            else
            {
                _rootVisualElement.Remove(_inventoryBox);
                _inventoryBox = null;
            }
        }

        private void GeneratePlayerInventory()
        {
            _inventoryBox = new Box();
            Label playerName = new Label();
            playerName.text = "Player Name Here";
            _inventoryBox.Add(playerName);

            foreach (var order in Orders)
            {
                _inventoryBox.Add(GenerateOrderUI(order.ToString(), "0"));
            }

            _rootVisualElement.Add(_inventoryBox);
        }
        
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Left collider with " + other.name);
            CurrentShopCollisions.Remove(other);
            CurrentHouseCollisions.Remove(other);
            if (CurrentShopCollisions.Count == 0 && _shopBox != null)
            {
                _rootVisualElement.Remove(_shopBox);
                _shopBox = null;
            }
            if (CurrentHouseCollisions.Count == 0 && _houseBox != null)
            {
                _rootVisualElement.Remove(_houseBox);
                _houseBox = null;
            }
            Debug.Log("CurrentShopCollisions: " + CurrentShopCollisions.Count);
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
                _orderPlayerCountLabel.text = $"Orders: {Orders.Count}";
                _playerMoneyLabel.text = $"Player Balance: {Money}";
            }
            else
            {
                Debug.Log("No orders to deliver");
            }
        }



    }
}
