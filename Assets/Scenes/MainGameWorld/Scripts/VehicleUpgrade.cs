using System;
using System.Collections.Generic;
using System.Threading;

namespace Scenes.MainGameWorld.Scripts
{
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