using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// The 'StartScreenUIController' class provides an introduction to the game. It provides the player the opportunity to start a game, read the rules or edit settings
/// such as sound controls or exiting safely. It does this appropriately by establishing a connection to the UI Document associated.
/// </summary>
public class StartScreenUIController : MonoBehaviour
{
    // Defining variables to be able to use UI Document control components   
    public Button startButton;
    public Button rulesButton;
    public Button settingsButton;
    public VisualElement rulesScreen;
    public VisualElement mainScreen;
    public VisualElement settingsScreen;

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
        // Establish connection to UI Document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        startButton = root.Q<Button>("StartBtn");
        rulesButton = root.Q<Button>("RulesBtn");
        settingsButton = root.Q<Button>("SettingsBtn");

        // Allocate method to be run when button on UI is pressed 
        startButton.clicked += StrtBtnPressed;
        settingsButton.clicked += SttingBtnPressed;
        rulesButton.clicked += RlsBtnPressed;
    }

    /// <summary>
    /// This method loads the main game world so the player can begin to play.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The user has selected the 'start game' button on the title screen.</preCondition>
    /// <postCondition> A user can begin to play the game.</postCondition>
    void StrtBtnPressed()
    {
        SceneManager.LoadScene("MainGameWorld");
    }

    /// <summary>
    /// This method loads the settings page of the game.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The user has selected the 'settings' button on the title screen.</preCondition>
    /// <postCondition> A user can choose to either; edit game sound controls, quit or return to the main menu.</postCondition>
    void SttingBtnPressed()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    /// <summary>
    /// This method loads the rules page for the game.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The user has selected the 'rules' button on the title screen.</preCondition>
    /// <postCondition> A user can read the rules of the game and understand player controls. They can also then go back to the main game screen.</postCondition>
    void RlsBtnPressed()
    {
        SceneManager.LoadScene("RulesMenu");
    }

}
