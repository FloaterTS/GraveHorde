using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{

    public AudioSource audioSource;
    public float musicVolume = 1f;
	
    void Start()
    {
        audioSource.Play();
    }

    void Update()
    {
        audioSource.volume = musicVolume;
    }
	
    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
    }
}
