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

        private Label PlayerMoney;
        private Label PlayerRating;

        private SaveData gameData;

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
            
            // Give me money & rating
            // gameData.PlayerMoney += 1000;
            // gameData.PlayerRating = 5;
            
            // Saves the game data (money spent)
            SaveGame();
            
            SceneManager.LoadScene("MainGameWorld");
        }

        private void ButtonPurchasePlayerUpgrade(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
            Debug.Log($"Player has attempted to purchase upgrade: {button.name}");
            
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
            purchaseButton.text = "Purchase Upgrade";
            
            playerBox.Add(upgradeName);
            playerBox.Add(upgradeCost);
            playerBox.Add(upgradeProgress);
            playerBox.Add(purchaseButton);
            
            purchaseButton.RegisterCallback<ClickEvent>(ButtonPurchasePlayerUpgrade);

            return playerBox;
        }
        
        public void SaveGame()
        {
            Debug.Log("Attempting to Save Game...");
            string jsonData = JsonUtility.ToJson(gameData);
            Debug.Log($"Save Data: {jsonData}");
            FileManager.WriteToFile("testsave.json", jsonData);
            Debug.Log("Game Saved!");
        }

        public SaveData LoadGame()
        {
            if (FileManager.LoadFromFile("testsave.json", out var json))
            {
                Debug.Log("Load complete");
                return JsonUtility.FromJson<SaveData>(json);
            }
            Debug.Log("Load failed");
            return null;
        }

        // Awake is called on object initialisation/activation
        private void Awake()
        {
            _uiDocument = transform.GetComponent<UIDocument>();
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            ButtonCancel = _rootVisualElement.Q<Button>("ButtonCancel");
            ButtonSave = _rootVisualElement.Q<Button>("ButtonSave");
            
            PlayerScroll = _rootVisualElement.Q<ScrollView>("PlayerScroll");
            VehicleScroll = _rootVisualElement.Q<ScrollView>("VehicleScroll");
            
            PlayerMoney = _rootVisualElement.Q<Label>("PlayerMoney");
            PlayerRating = _rootVisualElement.Q<Label>("PlayerReputation");
            
            ButtonCancel.RegisterCallback<ClickEvent>(ButtonCancelEvent);
            ButtonSave.RegisterCallback<ClickEvent>(ButtonSaveEvent);
            
            // Loads the upgrade information from the local save data
            VehicleUpgrade.AllUpgrades = FileManager.LoadData<List<VehicleUpgrade>>("VehicleUpgrades.json", JsonConvert.DeserializeObject<List<VehicleUpgrade>>(DefaultVehicleUpgrades));
            PlayerUpgrade.AllUpgrades = FileManager.LoadData<List<PlayerUpgrade>>("PlayerUpgrades.json", JsonConvert.DeserializeObject<List<PlayerUpgrade>>(DefaultPlayerUpgrades));
            
            // Loads the save data from the local save file
            gameData = LoadGame();
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

            PlayerMoney.text = $"Account Balance: ${gameData.PlayerMoney}";
            PlayerRating.text = $"Driver Rating: {gameData.PlayerRating}";
        }
        
        

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
