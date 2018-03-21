using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BeatPlayer : MonoBehaviour
{
    public enum BeatResult
    {
        PERFECT = 30,
        GREAT = 20,
        GOOD = 10,
        BAD = 5,
        MISS = 0
    }

    [SerializeField] AudioSource m_music = null;
    [SerializeField] CameraController m_camera = null;
    [SerializeField] GameObject m_endScreen = null;
    [SerializeField] Player m_player = null;
    [SerializeField] Image m_screenFlare = null;
    [SerializeField] Image m_screenTransition = null;
    [SerializeField] List<BeatTrack> m_playList = null;

    int m_songIndex = 0;
    int m_beatIndex = 0;
    bool m_paused = true;

    private void Start()
    {
    }

    private void Update()
    {
        if (!m_paused)
        {
            if (PastTimeWindow(m_playList[m_songIndex].beats[m_beatIndex]))
            {
                AddPlayerPoints(BeatResult.MISS);
            }
            else if (Input.GetButtonDown("Jump") && !WithinSafePeriod(m_playList[m_songIndex].beats[m_beatIndex]))
            {
                AddPlayerPoints(HitBeat(m_playList[m_songIndex].beats[m_beatIndex]));
            }

            if (m_beatIndex >= m_playList[m_songIndex].beats.Count)
            {
                FinishLevel();
            }
        }
    }

    BeatResult HitBeat(Beat beat)
    {
        BeatResult result;

        float timeDelta = m_music.time - beat.startTime;

        if (timeDelta / beat.timeWindow > 1.0f || timeDelta < 0.0f)
        {
            result = BeatResult.MISS;
        }
        else
        {
            if (timeDelta / beat.timeWindow < 0.25f) result = BeatResult.PERFECT;
            else if (timeDelta / beat.timeWindow < 0.5f) result = BeatResult.GREAT;
            else if (timeDelta / beat.timeWindow < 0.75f) result = BeatResult.GOOD;
            else result = BeatResult.BAD;
        }

        return result;
    }

    bool PastTimeWindow(Beat beat)
    {
        return m_music.time - beat.startTime > beat.timeWindow;
    }

    bool WithinSafePeriod(Beat beat)
    {
        // Checks to see if the player is able to hit the beat yet (so they don't press it early)
        return beat.startTime - m_music.time > 0.25f;
    }

    void AddPlayerPoints(BeatResult result)
    {
        Beat beat = m_playList[m_songIndex].beats[m_beatIndex];
        m_camera.ShakeCamera(beat.shakeDuration, beat.shakeAmplitude, beat.shakeRate);

        float points = (int)result;
        m_player.AddPoints(points);
        m_beatIndex++;
        m_screenFlare.gameObject.SetActive(true);
        StartCoroutine(FadeFlare());
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

    void FinishLevel()
    {
        m_songIndex++;
        if (!OutOfSongs())
        {
            PlayNextSong();
        }
        else
        {
            PauseMusic();
            m_endScreen.SetActive(true);
        }
    }

    public void LoadPlaylist(List<GameObject> beatList)
    {
        foreach (GameObject icon in beatList)
        {
            m_playList.Add(icon.GetComponent<LevelIcon>().m_track);
        }

        PlayMusic();
    }

    void PlayNextSong()
    {
        m_music.time = 0.0f;
        m_beatIndex = 0;
        m_playList[m_songIndex].beats = m_playList[m_songIndex].beats.OrderBy(b => b.startTime).ToList();
        PauseMusic();
        StartCoroutine(SongTransition());
    }

    IEnumerator SongTransition()
    {
        m_screenTransition.gameObject.SetActive(true);
        Color color = m_screenTransition.color;

        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * 2.0f)
        {
            Color c = color;
            c.a = i;
            color = c;
            m_screenTransition.color = color;
            
            yield return null;
        }

        // Switch scene here

        yield return new WaitForSeconds(0.25f);

        for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime * 2.0f)
        {
            Color c = color;
            c.a = i;
            color = c;
            m_screenTransition.color = color;

            yield return null;
        }

        Color finalColor = color;
        finalColor.a = 0.0f;
        color = finalColor;
        m_screenTransition.gameObject.SetActive(false);

        PlayMusic();
    }


    bool OutOfSongs()
    {
        return m_songIndex >= m_playList.Count;
    }

    public void PlayMusic()
    {
        m_music.Play();
        m_paused = false;
    }

    public void PauseMusic()
    {
        m_music.Pause();
        m_paused = true;
    }

    public void Restart()
    {
        m_songIndex = 0;
        PlayNextSong();
    }
}
