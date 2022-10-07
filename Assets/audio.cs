using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    private AudioSource backgroundMusic;
    private bool alreadyPlayed = false;

    public static audio instance;
    
    void Awake()
    {
        gameObject.tag = "Music";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
            backgroundMusic = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    //private void Start()
    //{
      //  gameObject.tag = "Music";
        //DontDestroyOnLoad(transform.gameObject);
        //backgroundMusic = GetComponent<AudioSource>();
    //}

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
