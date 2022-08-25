using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    // Base UI Elements
    private UIDocument _uiDocument;
    private VisualElement _rootVisualElement;
    public GroupBox PlayerInteractUI { get; set; }
    public GroupBox ShopPageUI { get; set; }

    // Standard UI Components
    public Label PlayerOrdersLabel { get; set; }
    public Label PlayerMoneyLabel {get; set;}


    // Used to setup the current component
    void Awake()
    {
        // Set Base UI Elements
        _uiDocument = transform.GetComponent<UIDocument>();
        _rootVisualElement = _uiDocument.rootVisualElement;
        
        PlayerInteractUI = _rootVisualElement.Q<GroupBox>("InteractScreen");
        PlayerInteractUI.style.display = DisplayStyle.None;

        ShopPageUI = _rootVisualElement.Q<GroupBox>("ShopPage");
        
        // Set Standard UI Components
        PlayerOrdersLabel = _rootVisualElement.Q<Label>("PlayerOrderCount");
        PlayerMoneyLabel = _rootVisualElement.Q<Label>("PlayerBankBalance");
        
        // Enable the page selector buttons in the UI
        var buttons = PlayerInteractUI.Q<GroupBox>("SelectionButtons").Query<Button>();
        buttons.ForEach(button => button.RegisterCallback<ClickEvent>(InteractUIPageSelectorsEvent));
    }

    // Used to manage the player InteractUI
    public void InteractUI()
    {
        // Toggles the Interact UI
        PlayerInteractUI.style.display = PlayerInteractUI.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public void ShopUI(List<Guid> availableOrders, EventCallback<ClickEvent> eventCallback)
    {
        // add the order elements to the list
        foreach (var order in availableOrders)
        {
            ShopPageUI.Add(GenerateOrderBoxUI(order.ToString(), "0"));
        }
        
        // makes the buttons clickable and work
        var buttons = ShopPageUI.Query<Button>();
        buttons.ForEach(button => button.RegisterCallback(eventCallback));
    }

    public void HouseUI()
    {
        
    }
    
    private void InteractUIPageSelectorsEvent(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        switch (button.name)
        {
            case "SelectShop":
                ShopPageUI.style.display = DisplayStyle.Flex;
                break;
            case "SelectHouse":
                ShopPageUI.style.display = DisplayStyle.None;
                break;
            case "SelectInventory":
                ShopPageUI.style.display = DisplayStyle.None;
                break;
        }
    }
    
    // Generates the house components used in the house GUI (each house element in the list)
    public Box GenerateHouseBoxUI(string customerName)
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
    
    // Generates the order components used in the shop GUI (each order element in the list)
    public Box GenerateOrderBoxUI(string customerName, string orderPrice)
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
}
