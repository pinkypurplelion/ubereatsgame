using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class LeaderboardData
    {
        public string PlayerName;
        public float PlayerScore;
        
        public string Stringify()
        {
            return JsonUtility.ToJson(this);
        }
        
        public static LeaderboardData Parse(string json)
        {
            return JsonUtility.FromJson<LeaderboardData>(json);
        }
    }
}