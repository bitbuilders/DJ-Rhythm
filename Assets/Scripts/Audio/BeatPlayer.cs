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
    [SerializeField] Player m_player = null;
    [SerializeField] Image m_screenFlare = null;
    [SerializeField] BeatTrack m_beatTrack = null;
    
    int m_beatIndex = 0;

    private void Start()
    {
        m_beatTrack.beats = m_beatTrack.beats.OrderBy(b => b.startTime).ToList();
    }

    private void Update()
    {
        if (PastTimeWindow(m_beatTrack.beats[m_beatIndex]))
        {
            AddPlayerPoints(BeatResult.MISS);
        }
        else if (Input.GetButtonDown("Jump") && !WithinSafePeriod(m_beatTrack.beats[m_beatIndex]))
        {
            AddPlayerPoints(HitBeat(m_beatTrack.beats[m_beatIndex]));
        }

        if (m_beatIndex >= m_beatTrack.beats.Count)
        {
            FinishLevel();
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
        Beat beat = m_beatTrack.beats[m_beatIndex];
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

    }
}
