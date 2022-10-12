using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// The 'SettingsScreenUIController' class allows a player to quit the game or go into sound controls.
/// </summary>
public class SettingsScreenUIController : MonoBehaviour
{
    // Defining variables to be able to use UI Document control components   
    public Button SettingsbackButton;
    public Button quitButton;
    public Button soundButton;

    /// <summary>
    /// Start is called before the first frame update. This method is used to link functionality to the UI Document to enable actions a user makes (i.e. press 
    /// a button) have a result (i.e. load new scene).
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has loaded the game.</preCondition>
    /// <postCondition> Actions a player tries to make will produce a result.</postCondition>
    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SettingsbackButton = root.Q<Button>("BackBtn");
        quitButton = root.Q<Button>("QuitBtn");
        soundButton = root.Q<Button>("SoundBtn");

        // Assign method to button click
        SettingsbackButton.clicked += SBckBtnPressed; 
        soundButton.clicked += SndBtnPressed;
        quitButton.clicked += Exit;       
    }

    /// <summary>
    /// This method is used to load the 'Sounds Menu' screen.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has requested to go to sounds menu.</preCondition>
    /// <postCondition> Loads sound menu screen to allow volume value altering.</postCondition>
    void SndBtnPressed()
    {
        SceneManager.LoadScene("SoundsMenu");
    }

    /// <summary>
    /// This method is used to load the 'Main Menu' screen.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has requested to go to back.</preCondition>
    /// <postCondition> Player is re-directed to main menu.</postCondition>
    void SBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// This method is used to exit the game.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has requested to shut the game.</preCondition>
    /// <postCondition> Progress is saved and application is exited.</postCondition>
    void Exit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
