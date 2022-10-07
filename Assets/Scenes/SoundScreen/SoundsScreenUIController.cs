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
    public AudioSource audio;

    void Start()
    {
        LoadValues();
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons from UI Builder to script
        SoundsbackButton = root.Q<Button>("BackBtn");
        SoundButton = root.Q<Button>("SoundBtn");
        volumeSlider = root.Q<Slider>("Slider");
        //volumeSlider.onValueChanged = ChangeVolume();
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        SoundsbackButton.clicked += SoundBckBtnPressed;
        SoundButton.clicked += ChangeVolume;
    }


    void SoundBckBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ChangeVolume()
    {
        //audio.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
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
