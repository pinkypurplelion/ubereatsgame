using System;
using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Holds information about the vehicle upgrades
    /// </summary>
    [Serializable]
    public class VehicleUpgrade
    {
        public static List<VehicleUpgrade> AllUpgrades = new List<VehicleUpgrade>();

        public float cost;
        public bool isPurchased;
        public string name;
        public string vehicleID;
    }
}