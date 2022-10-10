using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerUpgradeUIManager : UIManager
    {
        public string DefaultPlayerUpgrades;
        public string DefaultVehicleUpgrades;

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
            VisualElement parent = button.parent;
            Debug.Log($"Player has attempted to purchase upgrade: {button.name}");
            var upgrade = PlayerUpgrade.AllUpgrades.Find(x => x.upgradeID == button.name);
            if (upgrade != null)
            {
                float upgradeCost = upgrade.cost + upgrade.costStep * upgrade.purchasedLevel;
                if (gameData.PlayerMoney >= upgradeCost)
                {
                    gameData.PlayerMoney -= upgradeCost;
                    upgrade.purchasedLevel++;
                    PlayerMoney.text = $"Money: {gameData.PlayerMoney}";
                    parent.Q<Label>("upgradeCost").text = (upgrade.cost + upgrade.costStep * upgrade.purchasedLevel).ToString();
                    parent.Q<ProgressBar>("upgradeProgress").value = upgrade.purchasedLevel;
                    if (upgrade.purchasedLevel >= upgrade.maxLevel)
                    {
                        button.text = "Upgrade Maxed";

                    }
                    Debug.Log($"Player has purchased upgrade: {button.name}");
                }
                else
                {
                    // TODO: add message to player on screen
                    Debug.Log($"Player does not have enough money to purchase upgrade: {button.name}");
                }
            }
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
            upgradeCost.text = (upgrade.cost + upgrade.costStep * upgrade.purchasedLevel).ToString();
            upgradeCost.name = "upgradeCost";
            upgradeProgress.lowValue = 0;
            upgradeProgress.highValue = upgrade.maxLevel;
            upgradeProgress.value = upgrade.purchasedLevel;
            upgradeProgress.name = "upgradeProgress";
            
            purchaseButton.name = upgrade.upgradeID;
            if (upgrade.purchasedLevel < upgrade.maxLevel)
            {
                purchaseButton.text = "Purchase Upgrade";
                purchaseButton.RegisterCallback<ClickEvent>(ButtonPurchasePlayerUpgrade);
            }
            else
            {
                purchaseButton.text = "Upgrade Maxed";
            }
            
            playerBox.Add(upgradeName);
            playerBox.Add(upgradeCost);
            playerBox.Add(upgradeProgress);
            playerBox.Add(purchaseButton);
            

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
        private new void Awake()
        {
            base.Awake();
            
            ButtonCancel = RootVisualElement.Q<Button>("ButtonCancel");
            ButtonSave = RootVisualElement.Q<Button>("ButtonSave");
            
            PlayerScroll = RootVisualElement.Q<ScrollView>("PlayerScroll");
            VehicleScroll = RootVisualElement.Q<ScrollView>("VehicleScroll");
            
            PlayerMoney = RootVisualElement.Q<Label>("PlayerMoney");
            PlayerRating = RootVisualElement.Q<Label>("PlayerReputation");
            
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
            if (PlayerUpgrade.AllUpgrades.Count == 0)
            {
                PlayerUpgrade vu = new PlayerUpgrade()
                {
                    upgradeID = "playerOrderCapacity",
                    name = "Order Capacity",
                    minLevel = 1,
                    maxLevel = 5,
                    purchasedLevel = 1,
                    upgradeStep = 1,
                    cost = 50,
                    costStep = 50
                };
                PlayerUpgrade.AllUpgrades.Add(vu);
            }
            
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
    }
}
