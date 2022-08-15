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

        public List<AxleInfo> axleInfos;
        public float maxMotorTorque;
        public float maxSteeringAngle;

        public float speed;

        private Rigidbody _rigidbody;
        private BoxCollider _collider;
        
        // UI Elements
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;
        private Box _shopBox;
        private Box _houseBox;
        
        private Label _orderPlayerCountLabel;

        // Used to setup the current component
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
            _uiDocument = transform.Find("PlayerUI").GetComponent<UIDocument>();
        }

        // Used to configure things that depend on other components
        void Start()
        {
            gameObject.tag = "Player";
            _rootVisualElement = _uiDocument.rootVisualElement;
            _orderPlayerCountLabel = _rootVisualElement.Q<Label>("PlayerOrderCount");
        }

        public Vector2 moveVal;

        // Called based on Movement action
        void OnMovement(InputValue value)
        {
            Debug.Log(1.1);
            moveVal = value.Get<Vector2>();
        }

        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            Debug.Log(1);
            /*
            if (collider.transform.childCount == 0)
            {
                Debug.Log(1.5);
                return;
            }
            */
            Debug.Log(2);
            Transform visualWheel = collider.transform;
            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position,out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
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
                Orders.Add(Guid.Parse(button.name));
                _orderPlayerCountLabel.text = $"Orders: {Orders.Count}";
            
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
            /*
            Vector3 tempVect = new Vector3(moveVal.x, 0, moveVal.y);
            tempVect = tempVect.normalized * (speed * Time.deltaTime);
            _rigidbody.MovePosition(transform.position + tempVect);
            */
            
            //float motor = maxMotorTorque * Input.GetAxis("Vertical");
            //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            float motor = maxMotorTorque * moveVal.y;
            float steering = maxSteeringAngle * moveVal.x;
     
            foreach (AxleInfo axleInfo in axleInfos) {
                if (axleInfo.steering) {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor) {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }
            
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
        void DeliverOrder(HouseTile tile, Guid order)
        {
            if (Orders.Count > 0)
            {
                Debug.Log("Delivering order");
                tile.DeliveredOrders.Add(order);
                Orders.RemoveAt(0); // TODO: implement choosing order to deliver (after MVP)
                _orderPlayerCountLabel.text = $"Orders: {Orders.Count}";
            }
            else
            {
                Debug.Log("No orders to deliver");
            }
        }



    }

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }
}
