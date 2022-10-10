using System;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// The game save file.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public static string SaveName = "testsave.json";
        
        // Player Data
        public float PlayerRating;
        public float PlayerMoney;
        public float PlayerScore;
        
        // World Data
        public float WorldTime;

        public static SaveData Defaults()
        {
            return new SaveData()
            {
                PlayerRating = 5,
                PlayerMoney = 0,
                PlayerScore = 0,
                WorldTime = 0,
            };
        }
    }
}