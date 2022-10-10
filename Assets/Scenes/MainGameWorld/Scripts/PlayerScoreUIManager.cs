using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerScoreUIManager : UIManager
    {
        private SaveData _gameData;
        
        private new void Awake()
        {
            base.Awake();
            
            _gameData = LoadGame();
        }

        private void Start()
        {
            RootVisualElement.Q<Label>("PlayerScore").text = $"Your Final Score: {_gameData.PlayerScore}";
            RootVisualElement.Q<Button>("ExitBtn").RegisterCallback<ClickEvent>(BtnReturnToMenuEvent);
        }

        void BtnReturnToMenuEvent(ClickEvent evt)
        {
            _gameData = null;
            SaveGame();
            SceneManager.LoadScene("MainMenu");
        }

        private void SaveGame()
        {
            Debug.Log("Attempting to Save Game...");
            string jsonData = JsonUtility.ToJson(_gameData);
            Debug.Log($"Save Data: {jsonData}");
            FileManager.WriteToFile("testsave.json", jsonData);
            Debug.Log("Game Saved!");
        }

        private SaveData LoadGame()
        {
            if (FileManager.LoadFromFile("testsave.json", out var json))
            {
                Debug.Log("Load complete");
                return JsonUtility.FromJson<SaveData>(json);
            }
            Debug.Log("Load failed");
            return null;
        }
    }
}
