using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip heal, pistol, rifle, speed, zombiedeath1, zombiedeath2, footstep, changeweapon;
    static AudioSource audioSrc;

    void Start()
    {
        heal = Resources.Load<AudioClip>("heal");
        pistol = Resources.Load<AudioClip>("pistol");
        rifle = Resources.Load<AudioClip>("rifle");
        speed = Resources.Load<AudioClip>("speed");
        zombiedeath1 = Resources.Load<AudioClip>("zombiedeath1");
        zombiedeath2 = Resources.Load<AudioClip>("zombiedeath2");
        footstep = Resources.Load<AudioClip>("footstep");
        changeweapon = Resources.Load<AudioClip>("changeweapon");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "heal":
                audioSrc.PlayOneShot(heal);
                break;
            case "pistol":
                audioSrc.PlayOneShot(pistol);
                break;
            case "rifle":
                audioSrc.PlayOneShot(rifle);
                break;
            case "speed":
                audioSrc.PlayOneShot(speed);
                break;
            case "zombiedeath1":
                audioSrc.PlayOneShot(zombiedeath1);
                break;
            case "zombiedeath2":
                audioSrc.PlayOneShot(zombiedeath2);
                break;
            case "footstep":
                audioSrc.PlayOneShot(footstep);
                break;
            case "changeweapon":
                audioSrc.PlayOneShot(changeweapon);
                break;
        }
    }
}
