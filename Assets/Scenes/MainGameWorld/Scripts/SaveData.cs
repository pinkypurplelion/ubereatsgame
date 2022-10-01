using System;

namespace Scenes.MainGameWorld.Scripts
{
    [Serializable]
    public class SaveData
    {
        // Player Data
        public int PlayerOrderLimit;
        public float PlayerRating;
        public float PlayerMoney;
        
        // World Data
        public float WorldTime;
    }
}