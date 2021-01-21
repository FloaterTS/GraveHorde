using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData 
{
    
    public int highscore;

    public ScoreData (ScoreScript scoreScript)
    {
        
        highscore = scoreScript.highscore;
    }
}
