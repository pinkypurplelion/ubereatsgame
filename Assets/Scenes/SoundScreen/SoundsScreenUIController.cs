using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SoundsScreenUIController : MonoBehaviour
{
    public Button SoundsbackButton;


    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SoundsbackButton = root.Q<Button>("BackBtn");

        SoundsbackButton.clicked += SoundBckBtnPressed; 
    }

    void SoundBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
