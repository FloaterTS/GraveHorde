using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text aliveText;
    public float startTime;
	
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;
        string minutes = ((int)timeElapsed / 60).ToString();
        string seconds = (timeElapsed % 60).ToString("f2");
        aliveText.text = minutes + ":" + seconds;
    }
}
