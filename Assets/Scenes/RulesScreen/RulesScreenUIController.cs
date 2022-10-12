using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// The 'RulesScreenUIController' class allows the rules screen to have functionality.
/// </summary>
public class RulesScreenUIController : MonoBehaviour
{
    // Defining variable to be able to use button component
    public Button RulebackButton;

    
    /// Start is called before the first frame update. This method is used to link functionality to the UI Document to enable actions a user makes (i.e. press 
    /// a button) have a result (i.e. load new scene).
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has requested the rules screen.</preCondition>
    /// <postCondition> Information of gameplay rules are displayed.</postCondition>
    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        RulebackButton = root.Q<Button>("BackBtn");
        RulebackButton.clicked += RBckBtnPressed;        
    }

    /// <summary>
    /// This method is used to load the 'Main Menu' screen.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has requested to go to back.</preCondition>
    /// <postCondition> Player is re-directed to main menu.</postCondition>
    void RBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
