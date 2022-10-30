using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerUIManager : UIManager
    {
        // player interact ui screen
        public GroupBox PlayerInteractUI { get; set; }
        public GroupBox MenuUI { get; set; }

        // pages in player interact ui screen
        public GroupBox ShopPageUI { get; set; }
        public GroupBox HousePageUI { get; set; }
        public GroupBox InventoryPageUI { get; set; }

        public ScrollView ShopScrollView { get; set; }
        public ScrollView InventoryScrollView { get; set; }
        public ScrollView HouseScrollView { get; set; }

        // Standard UI Components
        public Label PlayerOrdersLabel { get; set; }
        public Label PlayerMoneyLabel { get; set; }
        public Label PlayerTimeLabel { get; set; }
        public Label PlayerRatingLabel { get; set; }
        public Label PlayerScoreLabel { get; set; }
        public Label PlayerChainLabel { get; set; }
        public Label PlayerNotificationLabel { get; set; }

        // PlayerController objects used for UI drawing
        public EventCallback<ClickEvent> ShopEventCallback { get; set; }
        public EventCallback<ClickEvent> HouseEventCallback { get; set; }
        public EventCallback<ClickEvent> InventoryEventCallback { get; set; }
        public List<Collider> CurrentShopCollisions { get; set; }
        public List<Collider> CurrentHouseCollisions { get; set; }
        public List<Guid> Orders { get; set; }
        public WorldEventManager WorldEventManager { get; set; }

        // Menu Buttons
        public Button MenuSaveButton { get; set; }
        public Button MenuMainButton { get; set; }
        public Button MenuExitButton { get; set; }
        public EventCallback<ClickEvent> MenuSaveEventCallback { get; set; }
        public EventCallback<ClickEvent> MenuMainEventCallback { get; set; }
        public EventCallback<ClickEvent> MenuExitEventCallback { get; set; }

        private static Queue<Tuple<string, int>> _notificationQueue;
        private static bool _isNotification;

        // Used to setup the current component
        private new void Awake()
        {
            base.Awake();

            PlayerInteractUI = RootVisualElement.Q<GroupBox>("InteractScreen");
            PlayerInteractUI.style.display = DisplayStyle.None;

            MenuUI = RootVisualElement.Q<GroupBox>("MenuScreen");
            MenuUI.style.display = DisplayStyle.None;

            ShopPageUI = RootVisualElement.Q<GroupBox>("ShopPage");
            HousePageUI = RootVisualElement.Q<GroupBox>("HousePage");
            InventoryPageUI = RootVisualElement.Q<GroupBox>("InventoryPage");

            ShopScrollView = RootVisualElement.Q<ScrollView>("ShopScrollView");
            InventoryScrollView = RootVisualElement.Q<ScrollView>("InventoryScrollView");
            HouseScrollView = RootVisualElement.Q<ScrollView>("HouseScrollView");

            MenuSaveButton = RootVisualElement.Q<Button>("BtnMenuSave");
            MenuSaveButton.RegisterCallback(MenuSaveEventCallback);
            MenuMainButton = RootVisualElement.Q<Button>("BtnMenuHome");
            MenuMainButton.RegisterCallback(MenuMainEventCallback);
            MenuExitButton = RootVisualElement.Q<Button>("BtnMenuQuit");
            MenuExitButton.RegisterCallback(MenuExitEventCallback);

            ShopScrollView.style.height = RootVisualElement.layout.height;
            InventoryScrollView.style.height = RootVisualElement.layout.height;
            HouseScrollView.style.height = RootVisualElement.layout.height;

            HousePageUI.style.display = DisplayStyle.None;
            InventoryPageUI.style.display = DisplayStyle.None;

            // Set Standard UI Components
            PlayerOrdersLabel = RootVisualElement.Q<Label>("PlayerOrderCount");
            PlayerMoneyLabel = RootVisualElement.Q<Label>("PlayerBankBalance");
            PlayerTimeLabel = RootVisualElement.Q<Label>("WorldTime");

            PlayerChainLabel = RootVisualElement.Q<Label>("PlayerChain");
            PlayerRatingLabel = RootVisualElement.Q<Label>("PlayerRating");
            PlayerScoreLabel = RootVisualElement.Q<Label>("PlayerScore");

            PlayerNotificationLabel = RootVisualElement.Q<Label>("PlayerNotification");

            // Enable the page selector buttons in the UI
            var buttons = PlayerInteractUI.Q<GroupBox>("SelectionButtons").Query<Button>();
            buttons.ForEach(button => button.RegisterCallback<ClickEvent>(PageSelectorsEvent));
        }

        // Toggles the Interact UI
        public void ToggleInteractUI()
        {
            PlayerInteractUI.style.display = PlayerInteractUI.style.display == DisplayStyle.None
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            UpdateInteractUI();
        }

        public void ToggleMenuUI()
        {
            MenuUI.style.display = MenuUI.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
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
                customers += customer.GetName() + ", ";
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

            customerNameLabel.text = $"Customer: {order.Customer.GetName()}";
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
            Label orderDeliveryTime = new Label();
            Button selectOrderButton = new Button();

            customerNameLabel.text = $"Customer: {order.Customer.GetName()}";
            orderPriceLabel.text = $"Delivery Price: ${order.OrderValue}";
            // TODO: add upgrade to player that shows time remaining for each order
            orderDeliveryTime.text =
                $"Delivery Time: {WorldEventManager.ConvertTimeToString(order.PickupTime + order.TimeToDeliver)}";

            // sets hidden name element to orderID so that we can select the order.
            selectOrderButton.name = order.OrderID.ToString();
            selectOrderButton.text = "Ditch Order";
            orderBox.Add(customerNameLabel);
            orderBox.Add(orderPriceLabel);
            orderBox.Add(orderDeliveryTime);
            orderBox.Add(selectOrderButton);
            return orderBox;
        }

        /// <summary>
        /// Used to send notifications to the player UI from other scripts.
        /// TODO: update to manage queue of notifications
        /// </summary>
        /// <param name="message"></param>
        /// <param name="seconds"></param>
        /// <param name="immediate"></param>
        public void NotifyPlayer(string message, int seconds, bool immediate)
        {
            Debug.Log($"Player Notified: {message}");
            PlayerNotificationLabel.text = message;
        }
    }
}