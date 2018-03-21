using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
    [SerializeField] GameObject m_pauseMenu = null;

    AudioSource m_music;

    private void Start()
    {
        CreateNeededFolders();

        m_music = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!m_pauseMenu.activeInHierarchy)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
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

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        m_pauseMenu.SetActive(true);
        m_music.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        m_pauseMenu.SetActive(false);
        m_music.Play();
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
