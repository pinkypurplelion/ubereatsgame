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
            VehicleUpgrade demo = new VehicleUpgrade
            {
                name = "Demo",
                cost = 100f,
                isPurchased = false,
                vehicleID = "Demo123"
            };

            PlayerUpgrade pdemo = new PlayerUpgrade()
            {
                name = "demo 1",
                cost = 50f,
                maxLevel = 10,
                purchasedLevel = 5,
                upgradeID = "demo123"
            };
            
            VehicleUpgrade.AllUpgrades.Add(demo);
            
            FileManager.SaveData("VehicleUpgrades.json", JsonConvert.SerializeObject(VehicleUpgrade.AllUpgrades));
            VehicleUpgrade.AllUpgrades = new List<VehicleUpgrade>();
            Debug.Log(VehicleUpgrade.AllUpgrades.Count);
            VehicleUpgrade.AllUpgrades = FileManager.LoadData<List<VehicleUpgrade>>("VehicleUpgrades.json");;
            Debug.Log(VehicleUpgrade.AllUpgrades.Count);

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
