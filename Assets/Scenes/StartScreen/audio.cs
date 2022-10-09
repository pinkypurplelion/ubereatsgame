using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    private AudioSource backgroundMusic;

    public static audio instance;
    
    void Awake()
    {
        // Set tag for audio source (Cube gameobject)
        gameObject.tag = "Music";
        // if there isn't any music playing, play the music and do not destroy this load
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
            backgroundMusic = GetComponent<AudioSource>();
        }
        // if there is already music playing, keep the old music playing and destroy a new audi GameObject that spawns
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic()
    {
        if (backgroundMusic.isPlaying) return;
        backgroundMusic.Play();
    }

    public void StopMusic()
    {
        backgroundMusic.Stop();
    }
}
