using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveScoreScript(ScoreScript scoreScript)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/tabela.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        ScoreData data = new ScoreData(scoreScript);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreData LoadScoreScript()
    {
        string path= Application.persistentDataPath + "/tabela.fun";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ScoreData data =formatter.Deserialize(stream) as ScoreData;
            stream.Close();
            return data;
        }
        
        
            Debug.LogError("Save file not found in" + path);
            return null;
        
    }

}
