using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles music and sound effects

public class AudioController : MonoBehaviour
{
    public static AudioController audioManager;
    public AudioSource levelMusic;
    public AudioSource deathMusic;
    public AudioSource victoryMusic;

    public AudioSource[] soundEffects;

    // Start is called before the first frame update

    private void Awake()
    {
        audioManager = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playDeathMusic()
    {
        levelMusic.Stop();

        deathMusic.Play();
    }

    public void playVictoryMusic()
    {
        levelMusic.Stop();

        victoryMusic.Play();
    }


    public void playSoundEffect(int index)
    {
        soundEffects[index].Stop();
        soundEffects[index].Play();
    }





}
