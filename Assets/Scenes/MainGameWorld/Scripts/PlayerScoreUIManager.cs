using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Used to manage the leaderboard functions on the player score page.
    /// </summary>
    public class PlayerScoreUIManager : UIManager
    {
        private SaveData _gameData;

        private TextField _playerName;
        private ScrollView _scoreList;
        private float score = 0f;

        private new void Awake()
        {
            base.Awake();

            _gameData = FileManager.LoadDataDefault<SaveData>(SaveData.SaveName);
        }

        private void Start()
        {
            _playerName = RootVisualElement.Q<TextField>("PlayerName");
            _scoreList = RootVisualElement.Q<ScrollView>("Leaderboard");

            RootVisualElement.Q<Button>("ExitBtn").RegisterCallback<ClickEvent>(BtnReturnToMenuEvent);
            RootVisualElement.Q<Button>("SubmitExitBtn").RegisterCallback<ClickEvent>(BtnSubmitEvent);
            if (_gameData != null)
            {
                score = _gameData.PlayerScore;
            }

            RootVisualElement.Q<Label>("PlayerScore").text = $"Your Score: {score}";

            // Used to get the leaderboard data from the server.
            StartCoroutine(GetLeaderboard(result =>
            {
                Debug.Log(result);
                JObject json = JsonConvert.DeserializeObject<JObject>(result);
                Debug.Log(json["documents"]);
                JArray documents = (JArray)json["documents"];
                foreach (JObject document in documents)
                {
                    Debug.Log(document);
                    GroupBox box = new GroupBox();
                    Label labelName = new Label();
                    labelName.text = $"Player Name: {document["name"]}";
                    Label labelScore = new Label();
                    labelScore.text = $"Score: {document["score"]}";
                    box.Add(labelName);
                    box.Add(labelScore);
                    _scoreList.Add(box);
                }
            }));
        }

        void BtnReturnToMenuEvent(ClickEvent evt)
        {
            _gameData = null;
            FileManager.SaveData("testsave.json", JsonConvert.SerializeObject(_gameData));
            SceneManager.LoadScene("MainMenu");
        }

        void BtnSubmitEvent(ClickEvent evt)
        {
            Debug.Log("Data Submit");
            StartCoroutine(PushLeaderboard(score.ToString(), _playerName.value, result =>
            {
                Debug.Log(result);
                _gameData = null;
                FileManager.SaveData("testsave.json", JsonConvert.SerializeObject(_gameData));
                SceneManager.LoadScene("MainMenu");
            }));
        }

        /// <summary>
        /// Heavily adapted from a MongoDB tutorial: https://www.mongodb.com/developer/languages/csharp/sending-requesting-data-mongodb-unity-game/
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        IEnumerator GetLeaderboard(System.Action<string> callback = null)
        {
            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("collection", "leaderboard");
            postData.Add("database", "ubereats");
            postData.Add("dataSource", "leaderboard");
            postData.Add("filter", new Dictionary<string, object>());

            Debug.Log(JsonConvert.SerializeObject(postData));
            UnityWebRequest request =
                new UnityWebRequest("https://data.mongodb-api.com/app/data-xbwnt/endpoint/data/v1/action/find", "POST");
            // EXTREMELY BAD SECURITY PRACTICE
            request.SetRequestHeader("api-key", "FsklX9fD0JmQdSqzADkZTiAkwC34C4zqvMmxcKCTaaCe7hYFJgi4cNqEIoHvBGVy");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();


            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke(request.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Heavily adapted from a MongoDB tutorial: https://www.mongodb.com/developer/languages/csharp/sending-requesting-data-mongodb-unity-game/
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        IEnumerator PushLeaderboard(string score, string name, System.Action<bool> callback = null)
        {
            using (UnityWebRequest request =
                   new UnityWebRequest("https://data.mongodb-api.com/app/data-xbwnt/endpoint/data/v1/action/insertOne",
                       "POST"))
            {
                Dictionary<string, object> postData = new Dictionary<string, object>();
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("name", name);
                data.Add("score", score);
                postData.Add("collection", "leaderboard");
                postData.Add("database", "ubereats");
                postData.Add("dataSource", "leaderboard");
                postData.Add("document", data);

                Debug.Log(JsonConvert.SerializeObject(postData));
                // EXTREMELY BAD SECURITY PRACTICE
                request.SetRequestHeader("api-key", "FsklX9fD0JmQdSqzADkZTiAkwC34C4zqvMmxcKCTaaCe7hYFJgi4cNqEIoHvBGVy");
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");

                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.Log(request.error);
                    if (callback != null)
                    {
                        callback.Invoke(false);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback.Invoke(request.downloadHandler.text != "{}");
                    }
                }
            }
        }
    }
}