using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
    private void Start()
    {
        CreateNeededFolders();

        //BeatTrack track = new BeatTrack();
        //track.beats = new List<Beat>();

        //Beat beat = new Beat();
        //beat.startTime = 1.0f;
        //Beat beat2 = new Beat();
        //beat2.startTime = 2.0f;

        //track.beats.Add(beat);
        //track.beats.Add(beat2);

        //SaveObjectToJson<BeatTrack>("trackTest", "Bob", track);
    }

    private void Update()
    {

    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SaveObjectToJson<T>(string fileName, string levelName, T objectToSave, bool custom = true)
    {
        string directoryPath = Application.dataPath + "/StreamingAssets/CustomLevels/" + levelName;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = custom ? Application.dataPath + "/StreamingAssets/CustomLevels/" + levelName + "/" + fileName + ".json" : Application.dataPath + "/StreamingAssets/PrebuiltLevels/" + levelName + "/" + fileName + ".json";
        
        string jsonData = JsonUtility.ToJson(objectToSave);
        File.WriteAllText(filePath, jsonData);
    }

    public T LoadObjectFromJson<T>(string fileName, string levelName, bool custom = true)
    {
        string filePath = custom ? Application.dataPath + "/StreamingAssets/CustomLevels/" + levelName + "/" + fileName + ".json" : Application.dataPath + "/StreamingAssets/PrebuiltLevels/" + levelName + "/" + fileName + ".json";

        string jsonData = File.ReadAllText(filePath);
        T test = JsonUtility.FromJson<T>(jsonData);

        return test;
    }

    private void CreateNeededFolders()
    {
        Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/CustomLevels");
        Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/PrebuiltLevels");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
