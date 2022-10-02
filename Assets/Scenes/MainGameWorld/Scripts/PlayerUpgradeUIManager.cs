using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerUpgradeUIManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;

        private ScrollView PlayerScroll;
        private ScrollView VehicleScroll;
        
        
        private Button ButtonCancel;
        private Button ButtonSave;
        private void Awake()
        {
            _uiDocument = transform.GetComponent<UIDocument>();
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            ButtonCancel = _rootVisualElement.Q<Button>("ButtonCancel");
            ButtonSave = _rootVisualElement.Q<Button>("ButtonSave");
            
            PlayerScroll = _rootVisualElement.Q<ScrollView>("PlayerScroll");
            VehicleScroll = _rootVisualElement.Q<ScrollView>("VehicleScroll");
            
            ButtonCancel.RegisterCallback<ClickEvent>(ButtonCancelEvent);
            ButtonSave.RegisterCallback<ClickEvent>(ButtonSaveEvent);
        }
           
        private void ButtonCancelEvent(ClickEvent evt)
        {
            SceneManager.LoadScene("MainGameWorld");
        }
        
        private void ButtonSaveEvent(ClickEvent evt)
        {
            SceneManager.LoadScene("MainGameWorld");
        }
        
        private void BtnEventTemplate(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
        }

        private Box GenVehicleUpgrade(VehicleUpgrade upgrade)
        {
            Box vehicleBox = new Box();
            
            Label vehicleName = new Label();
            Label vehicleCost = new Label();
            Button purchaseButton = new Button();
            
            vehicleName.text = upgrade.name;
            vehicleCost.text = upgrade.cost.ToString();
            
            if (upgrade.isPurchased)
            {
                purchaseButton.text = "Select Vehicle";
            }
            else
            {
                purchaseButton.text = "Purchase Vehicle";
            }
            purchaseButton.name = upgrade.vehicleID;
            
            vehicleBox.Add(vehicleName);
            vehicleBox.Add(vehicleCost);
            vehicleBox.Add(purchaseButton);
            
            return vehicleBox;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            VehicleUpgrade demo = new VehicleUpgrade
            {
                name = "Demo",
                cost = 100f,
                isPurchased = false,
                vehicleID = "Demo123"
            };
            
            VehicleScroll.Add(GenVehicleUpgrade(demo));
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
