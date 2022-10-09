using System;
using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    [Serializable]
    public class PlayerUpgrade
    {
        public static List<PlayerUpgrade> AllUpgrades = new List<PlayerUpgrade>();
        
        public float cost;
        public float costStep;
        
        public float purchasedLevel;
        public float maxLevel;
        public float minLevel;
        public float upgradeStep;
        
        public string name;
        public string upgradeID;
    }
}