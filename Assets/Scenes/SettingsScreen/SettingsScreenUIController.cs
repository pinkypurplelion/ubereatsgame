using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsScreenUIController : MonoBehaviour
{
    public Button SettingsbackButton;
    public Button quitButton;

    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SettingsbackButton = root.Q<Button>("BackBtn");
        quitButton = root.Q<Button>("QuitBtn");

        SettingsbackButton.clicked += SBckBtnPressed; 
        quitButton.clicked += Exit;       
    }

    void SBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Exit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
