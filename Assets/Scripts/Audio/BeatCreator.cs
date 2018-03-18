﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BeatCreator : MonoBehaviour
{
    [SerializeField] AudioSource m_music = null;
    [SerializeField] Image m_screenFlare = null;
    [SerializeField] Slider m_songProgress = null;
    [SerializeField] BeatTrack m_beatTrack = null;
    [SerializeField] List<Beat> m_beats = null;
    [SerializeField] string m_level = null;

    string m_trackPath;
    bool m_paused = false;

    private void Awake()
    {
        m_trackPath = "Assets\\Resources\\CustomLevels\\" + m_level + "\\Track.asset";
    }

    private void Start()
    {
        StopMusic();
        //m_beatTrack = ScriptableObject.CreateInstance<BeatTrack>();
        //m_beatTrack.level = m_level;
        //m_beatTrack.beats = new List<Beat>();
        
        //Object file = Resources.Load(path);
    }

    private void Update()
    {
        if (!m_paused)
        {
            if (Input.GetButtonDown("Jump"))
            {
                CreateBeat();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_paused = !m_paused;
            if (m_paused)
            {
                SaveBeats();
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        float progress = m_music.time / m_music.clip.length;
        m_songProgress.value = progress;
    }

    void CreateBeat()
    {
        Beat beat = ScriptableObject.CreateInstance<Beat>();
        beat.startTime = m_music.time;
        beat.timeWindow = 0.5f;
        beat.shakeAmplitude = 6.0f;
        beat.shakeRate = 3.0f;
        beat.shakeDuration = 0.3f;

        m_beats.Add(beat);
        m_screenFlare.gameObject.SetActive(true);
        StopCoroutine(FadeFlare());
        StartCoroutine(FadeFlare());
    }

    void SaveBeats()
    {
        string[] paths = GetBeatPaths();

        CreateNeededFolders();

        for (int i = 0; i < m_beats.Count; ++i)
        {
            Beat beat = AssetDatabase.LoadAssetAtPath<Beat>(paths[i]);
            if (beat != m_beats[i])
            {
                AssetDatabase.CreateAsset(m_beats[i], paths[i]);
                m_beatTrack.beats.Add(m_beats[i]);
            }
        }

        //FileUtil.DeleteFileOrDirectory(m_trackPath);
        //AssetDatabase.DeleteAsset(m_trackPath);
        //AssetDatabase.Refresh();
        //AssetDatabase.SaveAssets();
        if (AssetDatabase.LoadAssetAtPath<BeatTrack>(m_trackPath) != m_beatTrack)
            AssetDatabase.CreateAsset(m_beatTrack, m_trackPath);

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    void CreateNeededFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets\\Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets\\Resources\\CustomLevels"))
            AssetDatabase.CreateFolder("Assets\\Resources", "CustomLevels");
        if (!AssetDatabase.IsValidFolder("Assets\\Resources\\CustomLevels\\" + m_level))
            AssetDatabase.CreateFolder("Assets\\Resources\\CustomLevels", m_level);
        if (!AssetDatabase.IsValidFolder("Assets\\Resources\\CustomLevels\\" + m_level + "\\Beats"))
            AssetDatabase.CreateFolder("Assets\\Resources\\CustomLevels\\" + m_level, "Beats");
    }

    string[] GetBeatPaths()
    {
        string[] paths = new string[m_beats.Count];

        for (int i = 0; i < paths.Length; ++i)
        {
            paths[i] = "Assets\\Resources\\CustomLevels\\" + m_level + "\\Beats\\Beat" + (i + 1) + ".asset";
        }

        return paths;
    }

    IEnumerator FadeFlare()
    {
        float startingAlpha = 0.75f;

        for (float i = startingAlpha; i >= 0.0f; i -= Time.deltaTime * 2.0f)
        {
            if (m_screenFlare.color.a > i)
            {
                Color color = m_screenFlare.color;
                color.a = i;
                m_screenFlare.color = color;
            }
            yield return null;
        }

        Color c = m_screenFlare.color;
        c.a = startingAlpha;
        m_screenFlare.color = c;
        m_screenFlare.gameObject.SetActive(false);
    }

    void PlayMusic()
    {
        m_music.time = 0.0f;
        m_music.Play();
        m_paused = false;
    }
    void StopMusic()
    {
        m_music.Stop();
        m_paused = true;
    }

    public void LoadTrack(BeatTrack track)
    {
        m_beatTrack = track;
        m_level = m_beatTrack.level;

        foreach (Beat beat in m_beatTrack.beats)
        {
            m_beats.Add(beat);
        }

        PlayMusic();
    }

    public void CreateNewTrack(TMP_InputField level)
    {
        m_beatTrack = ScriptableObject.CreateInstance<BeatTrack>();
        m_level = level.text;
        m_beatTrack.level = m_level;
        m_beatTrack.beats = new List<Beat>();

        PlayMusic();
    }

    public void SkipTime(float seconds)
    {
        float newTime = m_music.time + seconds;
        newTime = newTime >= 0.0f ? newTime : 0.0f;
        m_music.time = newTime;

        //RemoveBeatsOverCurrent();
    }

    private void RemoveBeatsOverCurrent()
    {
        for (int i = m_beats.Count - 1; i >= 0; i--)
        {
            if (m_beats[i].startTime > m_music.time)
            {
                m_beats.Remove(m_beats[i]);
            }
        }
    }

    public void Pause()
    {
        StopMusic();
    }

    public void Play()
    {
        PlayMusic();
    }

    public void Delete()
    {

    }

    public void OpenSettings()
    {

    }

    public void SaveTrack()
    {
        m_beatTrack.level = m_level;
        m_trackPath = "Assets\\Resources\\CustomLevels\\" + m_level + "\\Track.asset";
        SaveBeats();
    }
}
