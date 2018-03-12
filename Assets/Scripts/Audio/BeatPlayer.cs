using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatPlayer : MonoBehaviour
{
    [SerializeField] AudioSource m_music = null;
    [SerializeField] CameraController m_camera = null;
    [SerializeField] [Range(1.0f, 10.0f)] public float m_pointAmplifier = 1.0f;
    [SerializeField] Transform m_beatContainer = null;
    [SerializeField] List<Beat> m_beats = null;

    private void Start()
    {
        foreach (Beat beat in m_beats)
        {
            GameObject obj = new GameObject("Sprite Clone");
            obj.transform.parent = m_beatContainer;
            obj.transform.position = beat.position;
            obj.transform.localScale = beat.scale;
            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            Sprite sprite = Instantiate(beat.beatImage);
            renderer.sprite = sprite;
            Material material = new Material(beat.materialTemplate);
            renderer.material = material;
            beat.renderer = renderer;

            obj.AddComponent<BoxCollider2D>();
            BeatHit hit = obj.AddComponent<BeatHit>();
            hit.music = m_music;
            hit.beat = beat;
            hit.player = this;

            beat.gameObject = obj;

            //EventTrigger trigger = obj.AddComponent<EventTrigger>();
            //EventTrigger.Entry entry = new EventTrigger.Entry();
            //entry.eventID = EventTriggerType.PointerClick;
            //entry.callback.AddListener((data) => { HitBeat((PointerEventData)data, beat); });
            //trigger.triggers.Add(entry);
        }
    }

    private void Update()
    {
        foreach (Beat beat in m_beats)
        {
            if (beat.gameObject)
            {
                float time = m_music.time - (beat.startTime - beat.leadUpTime);
                //print(time + " | " + m_music.time);
                beat.renderer.material.color = new Color(beat.renderer.material.color.r, beat.renderer.material.color.g, beat.renderer.material.color.b, 1.0f);
                if (time <= 1.0f)
                {
                    float fade = Mathf.Clamp01(time);
                    //print(fade);
                    beat.renderer.material.color = new Color(beat.renderer.material.color.r, beat.renderer.material.color.g, beat.renderer.material.color.b, fade);
                    if (!beat.hasTriggered && fade >= 0.95f)
                    {
                        ShakeBeat(beat);
                        beat.hasTriggered = true;
                        StartCoroutine(FadeBeat(beat));
                    }
                }
            }
        }
    }

    public void ShakeBeat(Beat beat)
    {
        m_camera.ShakeCamera(beat.shakeDuration, beat.shakeAmplitude, beat.shakeRate);
    }

    IEnumerator FadeBeat(Beat beat)
    {
        for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime)
        {
            if (beat.gameObject)
                beat.renderer.material.color = new Color(beat.renderer.material.color.r, beat.renderer.material.color.g, beat.renderer.material.color.b, i);
            yield return null;
        }

        if (beat.gameObject)
            Destroy(beat.gameObject);
    }
}
