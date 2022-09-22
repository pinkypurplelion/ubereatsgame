using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScreenUIController : MonoBehaviour
{

    public Button startButton;
    public Button rulesButton;
    public Button quitButton;
    public Button SettingsbackButton;
    public Button RulebackButton;
    public Button settingsButton;
    public VisualElement rulesScreen;
    public VisualElement mainScreen;
    public VisualElement settingsScreen;

    // Start is called before the first frame update
    void Start()
    {
        // Have start screen displayed first
        // rulesScreen.style.display = DisplayStyle.None;
        // mainScreen.style.display = DisplayStyle.Flex;

        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        startButton = root.Q<Button>("StartBtn");
        rulesButton = root.Q<Button>("RulesBtn");
        quitButton = root.Q<Button>("QuitBtn");
        // RulebackButton = root.Q<Button>("RulebackBtn");
        SettingsbackButton = root.Q<Button>("SettingsbackBtn");
        settingsButton = root.Q<Button>("SettingsBtn");

        // Assign 'pages' from UI Builder
        rulesScreen = root.Q<VisualElement>("Rules");
        mainScreen = root.Q<VisualElement>("Background");
        settingsScreen = root.Q<VisualElement>("Settings");

        startButton.clicked += StrtBtnPressed;
        settingsButton.clicked += SttingBtnPressed;
        rulesButton.clicked += RlsBtnPressed;
        quitButton.clicked += Exit;
       
    }

    void Exit()
    {
        Application.Quit();
         Debug.Log("Game is exiting");
    }

    void StrtBtnPressed()
    {
        SceneManager.LoadScene("MainGameWorld");
    }

    void SttingBtnPressed()
    {
        // mainScreen.style.display = DisplayStyle.None;
        // rulesScreen.style.display = DisplayStyle.None;
        // settingsScreen.style.display = DisplayStyle.Flex;
        // SettingsbackButton.clicked += SBckBtnPressed;
        SceneManager.LoadScene("SettingsMenu");
    }

    void RlsBtnPressed()
    {
        // settingsScreen.style.display = DisplayStyle.None;
        // mainScreen.style.display = DisplayStyle.None;
        // rulesScreen.style.display = DisplayStyle.Flex;
        // RulebackButton.clicked += RBckBtnPressed;
        // RulebackButton.onClick.AddListener(RBckBtnPressed);
        SceneManager.LoadScene("RulesMenu");
    }

    void RBckBtnPressed()
    {
        mainScreen.style.display = DisplayStyle.Flex;
        settingsScreen.style.display = DisplayStyle.None;
        rulesScreen.style.display = DisplayStyle.None;
    }

    void SBckBtnPressed()
    {
        mainScreen.style.display = DisplayStyle.Flex;
        settingsScreen.style.display = DisplayStyle.None;
        rulesScreen.style.display = DisplayStyle.None;
    }
}
