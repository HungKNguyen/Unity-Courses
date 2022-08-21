using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    string path; 
    public bool tutorial = true;

    void Awake()
    {
        if(controller == null) {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if(controller != this) {
            Destroy(gameObject);
        }

        path = Path.Combine(Application.persistentDataPath, "data.json");
        if(!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            fs.Close();
            SaveToFile();
        }
        else
        {
            LoadFromFile();
        }
        
    }

    void SaveToFile() 
    {
        GameData data = new GameData();
        data.tutorial = tutorial;
        string json = JsonUtility.ToJson(data);
        using (StreamWriter output = new StreamWriter(path))
        {
            output.WriteLine(json);
        }

    }

    void LoadFromFile()
    {
        using (StreamReader input = new StreamReader(path))
        {
            string json = input.ReadToEnd();
            GameData data = JsonUtility.FromJson<GameData>(json);
            tutorial = data.tutorial;
        }
    }

    public void DisableTutorial(){
        tutorial = false;
    }

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused) {
            Debug.Log("Saving");
            SaveToFile();
        }    
    }
}

[Serializable]
public class GameData
{
    public bool tutorial;
}
