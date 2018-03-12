using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCreator : MonoBehaviour
{
    [SerializeField] List<Beat> m_beats;
    [SerializeField] GameObject m_cube = null;
    [SerializeField] AudioSource m_music = null;
    [SerializeField] List<Material> m_materials = null;
    [SerializeField] List<Sprite> m_sprites = null;

    Material m_currentMaterial;
    Sprite m_currentSprite;

    private void Start()
    {
        m_beats = new List<Beat>();
        m_currentMaterial = m_materials[0];
        m_currentSprite = m_sprites[0];
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Beat beat = new Beat();
            beat.position = mousePos;
            beat.scale = new Vector2(0.2f, 0.2f);
            beat.startTime = m_music.time;
            beat.leadUpTime = m_music.time - 1.0f;
            beat.materialTemplate = m_currentMaterial;
            beat.beatImage = m_currentSprite;
            m_beats.Add(beat);
            Instantiate(m_cube, mousePos, Quaternion.identity);
        }
    }
}
