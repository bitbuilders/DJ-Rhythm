using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPlayer : MonoBehaviour
{
    [SerializeField] AudioSource m_music = null;
    [SerializeField] CameraController m_camera = null;
    [SerializeField] List<Beat> m_beats = null;

    private void Start()
    {
        foreach (Beat beat in m_beats)
        {
            GameObject obj = new GameObject("Sprite Clone");
            obj.transform.position = beat.position;
            obj.transform.localScale = beat.scale;
            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            Sprite sprite = Instantiate(beat.beatImage);
            renderer.sprite = sprite;
            Material material = new Material(beat.materialTemplate);
            renderer.material = material;
            beat.renderer = renderer;
        }
    }

    private void Update()
    {
        foreach (Beat beat in m_beats)
        {
            float time = m_music.time - beat.leadUpTime;
            //print(time + " | " + m_music.time);
            beat.renderer.material.color = new Color(beat.renderer.material.color.r, beat.renderer.material.color.g, beat.renderer.material.color.b, 1.0f);
            if (time <= 1.0f)
            {
                float fade = Mathf.Clamp01(time);
                //print(fade);
                beat.renderer.material.color = new Color(beat.renderer.material.color.r, beat.renderer.material.color.g, beat.renderer.material.color.b, fade);
            }
        }
    }
}
