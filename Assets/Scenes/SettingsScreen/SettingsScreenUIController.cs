using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsScreenUIController : MonoBehaviour
{
    public Button SettingsbackButton;
    public Button quitButton;
    public Button soundButton;

    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SettingsbackButton = root.Q<Button>("BackBtn");
        quitButton = root.Q<Button>("QuitBtn");
        soundButton = root.Q<Button>("SoundBtn");

        SettingsbackButton.clicked += SBckBtnPressed; 
        soundButton.clicked += SndBtnPressed;
        quitButton.clicked += Exit;       
    }

    void SndBtnPressed()
    {
        SceneManager.LoadScene("SoundsMenu");
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
