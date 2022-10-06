using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundsScreenUIController : MonoBehaviour
{
    public UnityEngine.UIElements.Button SoundsbackButton;
    public UnityEngine.UIElements.Slider volumeSlider;
    public AudioSource audio;

    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SoundsbackButton = root.Q<UnityEngine.UIElements.Button>("BackBtn");
        volumeSlider = root.Q<UnityEngine.UIElements.Slider>("Slider");
        volumeSlider.onValueChanged = ChangeVolume();

        SoundsbackButton.clicked += SoundBckBtnPressed; 
    }

    void SoundBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ChangeVolume()
    {
        audio.volume = volumeSlider.value;
    }
    

}
