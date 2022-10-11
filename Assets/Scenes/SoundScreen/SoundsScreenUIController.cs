using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// The 'SoundScreenUIController' class allows the audio's volume in a game to be altered. This is done by using properties of the Slider unity class.
/// </summary>
public class SoundsScreenUIController : MonoBehaviour
{
    // Defining variables to be able to use UI Document control components   
    public Button SoundsbackButton;
    public Button SoundButton;
    public Slider volumeSlider;
    private AudioSource audio;

    /// <summary>
    /// Start is called before the first frame update. This method is used to link functionality to the UI Document to enable actions a user makes (i.e. move slider) 
    /// have a result (i.e. different volume).
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has loaded the game.</preCondition>
    /// <postCondition> Actions a player tries to make will produce a result.</postCondition>
    void Start()
    {
        // Establish connection to document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Assign buttons/slider from UI Builder to script
        SoundsbackButton = root.Q<Button>("BackBtn");
        SoundButton = root.Q<Button>("SaveBtn");
        volumeSlider = root.Q<Slider>("slid");
        
        // Call method when button is clicked
        SoundsbackButton.clicked += SoundBckBtnPressed;
        SoundButton.clicked += ChangeVolume;
    }

    /// <summary>
    /// As monoBehaviour is enabled, Update is called every frame. This means that this method is used to constantly check the value of the slider and ensure the 
    /// label associated with it displays the volume.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The 'Sounds' screen is loaded </preCondition>
    /// <postCondition> The slider label will display the associated volume.</postCondition>
    void Update()
    {
        // Slider value is multiplied by 50 to appear to be on the 0-100 volume scale
        // This is due to the audio file becoming really poor quality if the string value were to be used.
        volumeSlider.label = (volumeSlider.value * 50).ToString();
    }

    /// <summary>
    /// This method loads the ancestor - settings - page after the 'back' button is pressed.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The 'Settings Menu' screen is loaded </preCondition>
    /// <postCondition> The player can change between different scenes.</postCondition>
    void SoundBckBtnPressed()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    /// <summary>
    /// This method allows updates any slider value changes to the game so that it can be maintained (i.e. changing it
    /// in the sounds page keeps it at that volume for all other scenes not just the 'sounds' screen).
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player preferences of volume are saved for the game session. </preCondition>
    /// <postCondition> The volume remains at the specified value until changed again.</postCondition>
    void ChangeVolume()
    {
        // Gets the value of the slide
        float volumeValue = volumeSlider.value;
        // Stores this value in a variable within PlayerPrefs class.
        PlayerPrefs.SetFloat("Volume", volumeValue);
        // Saves the value
        PlayerPrefs.Save();
        LoadValues();
    }

    /// <summary>
    /// This method updates the volume of the audio to reflect that what is displayed on the slider.
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player clicks the 'save' button. </preCondition>
    /// <postCondition> The volume remains the same or changes (quiter or louder) depending on slider value.</postCondition>
    public void LoadValues()
    {
        // Gets the currently stored volume value preference of the player.
        float volumeValue = PlayerPrefs.GetFloat("Volume");
        // Changes the slider to reflect this value.
        volumeSlider.value = volumeValue;
        // Adjusts the volume of AudioListener that picks up the audio playing from the GameObject's AudioSource
        AudioListener.volume = volumeValue;        
    }
}
