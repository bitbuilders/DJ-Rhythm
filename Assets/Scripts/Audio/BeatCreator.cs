using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class BeatCreator : MonoBehaviour
{
    [SerializeField] BeatTrack m_beatTrack = null;

    BeatTrack testTrack;

    private void Awake()
    {
        testTrack = ScriptableObject.CreateInstance<BeatTrack>();
        testTrack.beats = new List<Beat>();
    }

    private void Start()
    {
        string path = "Assets\\Resources\\CustomLevels\\Track03.asset";
        string path2 = "Assets\\Resources\\CustomLevels\\Beats\\Beat01.asset";
        Object file = Resources.Load(path);

        Beat beat = ScriptableObject.CreateInstance<Beat>();
        beat.startTime = 2.0f;
        print(beat.startTime);
        // Need to create beat before adding it to track
        AssetDatabase.CreateAsset(beat, path2);
        AssetDatabase.CreateAsset(testTrack, path);
        testTrack.beats.Add(beat);

        AssetDatabase.SaveAssets();
    }
}
