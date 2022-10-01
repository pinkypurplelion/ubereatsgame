using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerUIManager : MonoBehaviour
    {
        // Base UI Elements
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;
    
        // player interact ui screen
        public GroupBox PlayerInteractUI { get; set; }
    
        // pages in player interact ui screen
        public GroupBox ShopPageUI { get; set; }
        public GroupBox HousePageUI {get; set;}
        public GroupBox InventoryPageUI { get; set; }
        
        public ScrollView ShopScrollView { get; set; }
        public ScrollView InventoryScrollView { get; set; }
        public ScrollView HouseScrollView { get; set; }

        // Standard UI Components
        public Label PlayerOrdersLabel { get; set; }
        public Label PlayerMoneyLabel {get; set;}
        public Label PlayerTimeLabel { get; set; }
        
        // PlayerController objects used for UI drawing
        public EventCallback<ClickEvent> ShopEventCallback {get; set;}
        public EventCallback<ClickEvent> HouseEventCallback {get; set;}
        public EventCallback<ClickEvent> InventoryEventCallback {get; set;}
        public List<Collider> CurrentShopCollisions { get; set; }
        public List<Collider> CurrentHouseCollisions { get; set; }
        public List<Guid> Orders {get; set;}
        public WorldEventManager WorldEventManager {get; set;}
        

        // Used to setup the current component
        void Awake()
        {
            // Set Base UI Elements
            _uiDocument = transform.GetComponent<UIDocument>();
            _rootVisualElement = _uiDocument.rootVisualElement;

            PlayerInteractUI = _rootVisualElement.Q<GroupBox>("InteractScreen");
            PlayerInteractUI.style.display = DisplayStyle.None;

            ShopPageUI = _rootVisualElement.Q<GroupBox>("ShopPage");
            HousePageUI = _rootVisualElement.Q<GroupBox>("HousePage");
            InventoryPageUI = _rootVisualElement.Q<GroupBox>("InventoryPage");
            
            ShopScrollView = _rootVisualElement.Q<ScrollView>("ShopScrollView");
            InventoryScrollView = _rootVisualElement.Q<ScrollView>("InventoryScrollView");
            HouseScrollView = _rootVisualElement.Q<ScrollView>("HouseScrollView");

            ShopScrollView.style.height = _rootVisualElement.layout.height;
            InventoryScrollView.style.height = _rootVisualElement.layout.height;
            HouseScrollView.style.height = _rootVisualElement.layout.height;
            
            HousePageUI.style.display = DisplayStyle.None;
            InventoryPageUI.style.display = DisplayStyle.None;
        
            // Set Standard UI Components
            PlayerOrdersLabel = _rootVisualElement.Q<Label>("PlayerOrderCount");
            PlayerMoneyLabel = _rootVisualElement.Q<Label>("PlayerBankBalance");
            PlayerTimeLabel = _rootVisualElement.Q<Label>("WorldTime");

            // Enable the page selector buttons in the UI
            var buttons = PlayerInteractUI.Q<GroupBox>("SelectionButtons").Query<Button>();
            buttons.ForEach(button => button.RegisterCallback<ClickEvent>(PageSelectorsEvent));
        }

        // Toggles the Interact UI
        public void ToggleInteractUI()
        {
            PlayerInteractUI.style.display = PlayerInteractUI.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            UpdateInteractUI();
        }
        
        // Used to update the player UpdateInteractUI
        public void UpdateInteractUI()
        {
            UpdateShopUI();
            UpdateHouseUI();
            UpdateInventoryUI();
        }
    
        // Updates the shop page of the player interact UI
        private void UpdateShopUI()
        {
            ShopScrollView.Clear();

            List<Guid> availableOrders = new();
            foreach (var shopCollision in CurrentShopCollisions)
            {
                availableOrders.AddRange(shopCollision.transform.GetComponent<ShopTile>().Orders);
            }
            // add the order elements to the list
            Debug.Log("Available Orders: " + availableOrders.Count);
            foreach (var order in availableOrders)
            {
                Order _order = WorldEventManager.Orders.Find(o => o.OrderID == order);
                ShopScrollView.Add(GenShopOrderElement(_order));
            }
        
            // makes the buttons clickable and work
            var buttons = ShopScrollView.Query<Button>();
            buttons.ForEach(button => button.RegisterCallback(ShopEventCallback));
        }

        // Updates the house page of the player interact UI
        private void UpdateHouseUI()
        {
            HouseScrollView.Clear();

            // add the house elements to the list
            foreach (var houseCollision in CurrentHouseCollisions)
            {
                HouseScrollView.Add(GenHouseElement(houseCollision.transform.GetComponent<HouseTile>()));
            }
        
            // makes the buttons clickable and work
            var buttons = HouseScrollView.Query<Button>();
            buttons.ForEach(button => button.RegisterCallback(HouseEventCallback));
        }
        
        // Updates the inventory page of the player interact UI
        private void UpdateInventoryUI()
        {
            InventoryScrollView.Clear();
            
            // add the order elements to the list
            foreach (var order in Orders)
            {
                Order _order = WorldEventManager.Orders.Find(o => o.OrderID == order);
                InventoryScrollView.Add(GenInvOrderElement(_order));
            }
        
            // makes the buttons clickable and work
            var buttons = InventoryScrollView.Query<Button>();
            buttons.ForEach(button => button.RegisterCallback(InventoryEventCallback));
        }
    
        private void PageSelectorsEvent(ClickEvent evt)
        {
            UpdateInteractUI();
            Button button = evt.currentTarget as Button;
            switch (button.name)
            {
                case "SelectShop":
                    ShopPageUI.style.display = DisplayStyle.Flex;
                    HousePageUI.style.display = DisplayStyle.None;
                    InventoryPageUI.style.display = DisplayStyle.None;
                    break;
                case "SelectHouse":
                    ShopPageUI.style.display = DisplayStyle.None;
                    HousePageUI.style.display = DisplayStyle.Flex;
                    InventoryPageUI.style.display = DisplayStyle.None;
                    break;
                case "SelectInventory":
                    ShopPageUI.style.display = DisplayStyle.None;
                    HousePageUI.style.display = DisplayStyle.None;
                    InventoryPageUI.style.display = DisplayStyle.Flex;
                    break;
            }
        }
    
        // Generates the house components used in the house GUI (each house element in the list)
        private Box GenHouseElement(HouseTile house)
        {
            Box houseBox = new Box();
            Label customerNameLabel = new Label();
            Button selectHouseButton = new Button();

            String customers = "";
            foreach (var customer in house.Customers)
            {
                customers += customer.getName() + ", ";
            }
            
            customerNameLabel.text = $"Household Members: {customers}";
            selectHouseButton.name = house.HouseID.ToString(); // sets house ID as the name of the button
            selectHouseButton.text = "Deliver Order to House";
            houseBox.Add(customerNameLabel);
            houseBox.Add(selectHouseButton);
            return houseBox;
        }
    
        // Generates the order components used in the shop GUI (each order element in the list)
        private Box GenShopOrderElement(Order order)
        {
            Box orderBox = new Box();
            Label customerNameLabel = new Label();
            Label orderPriceLabel = new Label();
            Button selectOrderButton = new Button();
            
            customerNameLabel.text = $"Customer: {order.Customer.getName()}";
            orderPriceLabel.text = $"Delivery Price: ${order.OrderValue}";
            
            // sets hidden name element to orderID so that we can select the order.
            selectOrderButton.name = order.OrderID.ToString();
            selectOrderButton.text = "Pickup Order";
            orderBox.Add(customerNameLabel);
            orderBox.Add(orderPriceLabel);
            orderBox.Add(selectOrderButton);
            return orderBox;
        }

        private Box GenInvOrderElement(Order order)
        {
            Box orderBox = new Box();
            Label customerNameLabel = new Label();
            Label orderPriceLabel = new Label();
            Button selectOrderButton = new Button();
            
            customerNameLabel.text = $"Customer: {order.Customer.getName()}";
            orderPriceLabel.text = $"Delivery Price: ${order.OrderValue}";
            
            // sets hidden name element to orderID so that we can select the order.
            selectOrderButton.name = order.OrderID.ToString();
            selectOrderButton.text = "Ditch Order";
            orderBox.Add(customerNameLabel);
            orderBox.Add(orderPriceLabel);
            orderBox.Add(selectOrderButton);
            return orderBox;
        }
    }
}
