using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatHit : EventTrigger
{
    public float points = 30.0f;
    public AudioSource music = null;
    public Beat beat = null;
    public BeatPlayer player = null;

    public void OnMouseDown()
    {
        float pointsEarned = points;

        float timeDelta = Mathf.Abs(music.time - beat.startTime);
        float percentage = 1.0f - timeDelta;
        percentage = Mathf.Clamp01(percentage);

        pointsEarned = percentage * points;
        print("Hit Score: " + pointsEarned * player.m_pointAmplifier);

        player.ShakeBeat(beat);
        Destroy(beat.gameObject);
    }
}
