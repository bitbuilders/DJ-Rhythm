using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BeatCreator : Singleton<BeatCreator>
{
    [SerializeField] AudioSource m_music = null;
    [SerializeField] Image m_screenFlare = null;
    [SerializeField] Slider m_songProgress = null;
    [SerializeField] GameObject m_beatStampTemplate = null;
    [SerializeField] Transform m_beatStampLocation = null;
    [SerializeField] BeatTrack m_beatTrack = null;
    [SerializeField] List<Beat> m_beats = null;
    [SerializeField] string m_level = null;
    [SerializeField] bool m_eraseMode = false;
    
    public bool InEraseMode { get { return m_eraseMode; } }

    Game m_game;
    bool m_paused = false;

    private void Start()
    {
        StopMusic();

        m_game = Game.Instance;
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
        Beat beat = new Beat();
        beat.startTime = m_music.time;
        beat.timeWindow = 0.5f;
        beat.shakeAmplitude = 6.0f;
        beat.shakeRate = 3.0f;
        beat.shakeDuration = 0.3f;

        m_beats.Add(beat);
        m_screenFlare.gameObject.SetActive(true);
        StopCoroutine(FadeFlare());
        StartCoroutine(FadeFlare());

        CreateBeatstamp(beat);
    }

    public void DeleteBeat(Beat beat)
    {
        m_beats.Remove(beat);
    }

    GameObject CreateBeatstamp(Beat beat)
    {
        GameObject stamp = Instantiate(m_beatStampTemplate, Vector3.zero, Quaternion.identity, m_beatStampLocation);
        BeatStamp beatStamp = stamp.GetComponent<BeatStamp>();
        beatStamp.m_beat = beat;

        float time = beatStamp.m_beat.startTime / m_music.clip.length;
        float position = time * Screen.width;
        RectTransform trans = stamp.GetComponent<RectTransform>();
        Vector3 pos = trans.position;
        pos.x = position;
        trans.position = pos;

        return stamp;
    }

    void SaveBeats()
    {
        for (int i = 0; i < m_beats.Count; ++i)
        {
            m_game.SaveObjectToJson("Beat" + i, m_level + "/Beats", m_beats[i]);
            m_beatTrack.beats.Add(m_beats[i]);
        }
        m_game.SaveObjectToJson("Track", m_level, m_beatTrack);
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
            CreateBeatstamp(beat);
        }

        PlayMusic();
    }

    public void CreateNewTrack(TMP_InputField level)
    {
        m_beatTrack = new BeatTrack();
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
        SaveBeats();
    }
}
