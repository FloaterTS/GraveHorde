using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Active Scene: " + SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayGame()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("Scenes/cemetery");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu");
        SceneManager.LoadScene("Scenes/Menu");
    }

}
