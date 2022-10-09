using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerUpgradeUIManager : MonoBehaviour
    {
        public String DefaultPlayerUpgrades;
        public String DefaultVehicleUpgrades;
        
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
            
            // Loads the upgrade information from the local save data
            VehicleUpgrade.AllUpgrades = FileManager.LoadData<List<VehicleUpgrade>>("VehicleUpgrades.json", JsonConvert.DeserializeObject<List<VehicleUpgrade>>(DefaultVehicleUpgrades));
            PlayerUpgrade.AllUpgrades = FileManager.LoadData<List<PlayerUpgrade>>("PlayerUpgrades.json", JsonConvert.DeserializeObject<List<PlayerUpgrade>>(DefaultPlayerUpgrades));

        }
           
        private void ButtonCancelEvent(ClickEvent evt)
        {
            Debug.Log("Exiting Upgrade Screen Without Saving");
            SceneManager.LoadScene("MainGameWorld");
        }
        
        private void ButtonSaveEvent(ClickEvent evt)
        {
            Debug.Log("Exiting Upgrade Screen and Saving");
            // Saves the upgrade data before leaving
            FileManager.SaveData("VehicleUpgrades.json", JsonConvert.SerializeObject(VehicleUpgrade.AllUpgrades));
            FileManager.SaveData("PlayerUpgrades.json", JsonConvert.SerializeObject(PlayerUpgrade.AllUpgrades));
            
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
        
        private Box GenPlayerUpgrade(PlayerUpgrade upgrade)
        {
            Box playerBox = new Box();
            
            Label upgradeName = new Label();
            Label upgradeCost = new Label();
            ProgressBar upgradeProgress = new ProgressBar();
            Button purchaseButton = new Button();
            
            upgradeName.text = upgrade.name;
            upgradeCost.text = upgrade.cost.ToString();
            upgradeProgress.lowValue = 0;
            upgradeProgress.highValue = upgrade.maxLevel;
            upgradeProgress.value = upgrade.purchasedLevel;
            
            purchaseButton.name = upgrade.upgradeID;
            
            playerBox.Add(upgradeName);
            playerBox.Add(upgradeCost);
            playerBox.Add(upgradeProgress);
            playerBox.Add(purchaseButton);
            
            return playerBox;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            // Loads UI elements based on the upgrade data
            foreach (var upgrade in VehicleUpgrade.AllUpgrades)
            {
                VehicleScroll.Add(GenVehicleUpgrade(upgrade));
            }

            foreach (var upgrade in PlayerUpgrade.AllUpgrades)
            {
                PlayerScroll.Add(GenPlayerUpgrade(upgrade));
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
