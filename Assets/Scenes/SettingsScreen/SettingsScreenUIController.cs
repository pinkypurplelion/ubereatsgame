using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsScreenUIController : MonoBehaviour
{
    public Button SettingsbackButton;

    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SettingsbackButton = root.Q<Button>("BackBtn");
        SettingsbackButton.clicked += SBckBtnPressed;        
    }

    void SBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
