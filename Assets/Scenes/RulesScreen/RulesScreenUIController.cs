using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RulesScreenUIController : MonoBehaviour
{
    public Button RulebackButton;
    
    // Start is called before the first frame update
    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        RulebackButton = root.Q<Button>("BackBtn");
        RulebackButton.clicked += RBckBtnPressed;        
    }

    void RBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
