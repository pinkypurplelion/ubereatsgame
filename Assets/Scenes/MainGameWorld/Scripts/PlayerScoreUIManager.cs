using Newtonsoft.Json;
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
            
            _gameData = FileManager.LoadDataDefault<SaveData>(SaveData.SaveName);
        }

        private void Start()
        {
            RootVisualElement.Q<Label>("PlayerScore").text = $"Your Final Score: {_gameData.PlayerScore}";
            RootVisualElement.Q<Button>("ExitBtn").RegisterCallback<ClickEvent>(BtnReturnToMenuEvent);
        }

        void BtnReturnToMenuEvent(ClickEvent evt)
        {
            _gameData = null;
            FileManager.SaveData("testsave.json", JsonConvert.SerializeObject(_gameData));
            SceneManager.LoadScene("MainMenu");
        }
    }
}
