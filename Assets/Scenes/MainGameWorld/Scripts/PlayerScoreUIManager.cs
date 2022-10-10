using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerScoreUIManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;
        
        private SaveData gameData;

        // Start is called before the first frame update
        void Awake()
        {
            _uiDocument = transform.GetComponent<UIDocument>();
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            gameData = LoadGame();
        }

        private void Start()
        {
            _rootVisualElement.Q<Label>("PlayerScore").text = $"Your Final Score: {gameData.PlayerScore}";
        }

        // Update is called once per frame
        void Update()
        {
        
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
    }
}
