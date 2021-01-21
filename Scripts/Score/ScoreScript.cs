using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public class ScoreScript : MonoBehaviour
{
    public static int highscoreValue = 0;
    public static int scoreValue = 0;
    public int highscore = 0;
    Text scorec;

    public void SaveScore ()
    {
      SaveSystem.SaveScoreScript(this);   
        
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/tabela.fun";
        if (File.Exists(path))
        {
            ScoreData data = SaveSystem.LoadScoreScript();
            highscoreValue = data.highscore;
            scoreValue = 0;
            
        }   
        else
        {
            highscoreValue = 0;
            scoreValue = 0;
        }
       
        
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        scorec = GetComponent<Text>();
        scorec.text = "HighScore:" + highscoreValue+"\n"+"Score:" + scoreValue;
        highscore = highscoreValue;
        
}

    // Update is called once per frame
    void Update()
    {
        if(scoreValue>highscoreValue)
        {
            highscoreValue = scoreValue;
        }
         highscore = highscoreValue;
         SaveScore();
         scorec.text = "HighScore: " + highscoreValue + "\n" + "Score: " + scoreValue;

    }
}
