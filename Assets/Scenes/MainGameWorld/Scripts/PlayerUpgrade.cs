using System;
using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    [Serializable]
    public class PlayerUpgrade
    {
        public static List<PlayerUpgrade> AllUpgrades = new List<PlayerUpgrade>();
        
        public float cost;
        public int purchasedLevel;
        public int maxLevel;
        public string name;
        public string upgradeID;
    }
}