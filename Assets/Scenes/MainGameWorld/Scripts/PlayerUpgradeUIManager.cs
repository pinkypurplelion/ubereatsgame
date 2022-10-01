using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerUpgradeUIManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;
        
        private Button ButtonCancel;
        private Button ButtonSave;
        private void Awake()
        {
            _uiDocument = transform.GetComponent<UIDocument>();
            _rootVisualElement = _uiDocument.rootVisualElement;
            
            ButtonCancel = _rootVisualElement.Q<Button>("ButtonCancel");
            ButtonSave = _rootVisualElement.Q<Button>("ButtonSave");
            
            ButtonCancel.RegisterCallback<ClickEvent>(ButtonCancelEvent);
            ButtonSave.RegisterCallback<ClickEvent>(ButtonSaveEvent);
        }
        
        private void ButtonCancelEvent(ClickEvent evt)
        {
            SceneManager.LoadScene("MainGameWorld");
        }
        
        private void ButtonSaveEvent(ClickEvent evt)
        {
            SceneManager.LoadScene("MainGameWorld");
        }
        
        private void BtnEventTemplate(ClickEvent evt)
        {
            Button button = evt.currentTarget as Button;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
