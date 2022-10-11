using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The 'audio' class sets up the way the audio throughout the entire game will be used. It does this by recognising the audiosource attached to a gameobject within the scene.
/// With the GameObject recognised, the game audio can be manipulated through the use of an instance variable.
/// If audio is playing, the track will continue to run throughout all scenes (i.e. will not start fresh each time) and the new audio track that is created at the instantiation of
/// a new scene will be destroyed so there aren't multiple versions of the same song playing. If audio is not playing, an instance of the audiosource will be started and the 
/// 'DontDestroyOnLoad' method is run. This method preserves the newly started GameObject and hence, the audio will continue to run regardless of the scene. 
/// </summary>
public class audio : MonoBehaviour
{
    // Define variables to be used for audio manipulation.
    private AudioSource backgroundMusic;
    public static audio instance;

    /// <summary>
    /// Awake is called regardless of whether the script is enabled or not. This method is used to link an AudioSource on a GameObject to the game. 
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has loaded the game.</preCondition>
    /// <postCondition> A player can hear a game theme song.</postCondition>
    void Awake()
    {
        // Set tag for audio source (Cube gameobject)
        gameObject.tag = "Music";
        // If there isn't any music playing, play the music and do not destroy this when loading other scenes.
        if (instance == null)
        {
            // Remember that audio is now playing
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
            // Play audio.
            backgroundMusic = GetComponent<AudioSource>();
        }
        // If there is already music playing, keep the old music playing and destroy the new audio GameObject that spawns.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This method is used to play an AudioSource to the game. 
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has loaded the game.</preCondition>
    /// <postCondition> A player can hear a game theme song.</postCondition>
    public void PlayMusic()
    {
        if (backgroundMusic.isPlaying) return;
        backgroundMusic.Play();
    }

    /// <summary>
    /// This method is used to stop an AudioSource from playing. 
    /// </summary>
    /// <return> The method does not return anything.</return>
    /// <param> There are no parameters.</param>
    /// <preCondition> The player has loaded the game.</preCondition>
    /// <postCondition> Music is stopped.</postCondition>
    public void StopMusic()
    {
        backgroundMusic.Stop();
    }
}
