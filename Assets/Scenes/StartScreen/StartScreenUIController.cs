using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScreenUIController : MonoBehaviour
{

    public Button startButton;
    public Button rulesButton;
    public Button SettingsbackButton;
    public Button RulebackButton;
    public Button settingsButton;
    public VisualElement rulesScreen;
    public VisualElement mainScreen;
    public VisualElement settingsScreen;

    // Start is called before the first frame update
    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        startButton = root.Q<Button>("StartBtn");
        rulesButton = root.Q<Button>("RulesBtn");
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
       
    }

    void StrtBtnPressed()
    {
        SceneManager.LoadScene("MainGameWorld");
    }

    void SttingBtnPressed()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    void RlsBtnPressed()
    {
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
