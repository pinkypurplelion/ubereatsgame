using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class SoundsScreenUIController : MonoBehaviour
{
    public Button SoundsbackButton;
    public Button SoundButton;
    public Slider volumeSlider;
    private AudioSource audio;

    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SoundsbackButton = root.Q<Button>("BackBtn");
        SoundButton = root.Q<Button>("SaveBtn");
        volumeSlider = root.Q<Slider>("slid");
        
        SoundsbackButton.clicked += SoundBckBtnPressed;
        SoundButton.clicked += ChangeVolume;
    }

    void Update()
    {
        volumeSlider.label = (volumeSlider.value * 50).ToString();
    }

    void SoundBckBtnPressed()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    void ChangeVolume()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeValue);
        PlayerPrefs.Save();
        LoadValues();
    }

    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;        
    }

}
