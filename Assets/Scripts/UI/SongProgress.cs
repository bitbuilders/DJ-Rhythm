using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongProgress : MonoBehaviour
{
    [SerializeField] AudioSource m_music = null;
    [SerializeField] GameObject m_timestamp = null;
    [SerializeField] GameObject m_timestampContainer = null;
    [SerializeField] [Range(1, 100)] int m_ticks = 10;

    private void Start()
    {
        AddTimestamps();
    }

    private void Update()
    {
        AddTimestamps();
    }

    void AddTimestamps()
    {
        RemoveTimeStamps();

        float width = Screen.width;
        float spaceBetweenTicks = width / (float)m_ticks;

        float x = spaceBetweenTicks / 2.0f;
        for (int i = 0; i < m_ticks; ++i)
        {
            Vector3 position = new Vector3(x, 10.0f);
            GameObject textObj = Instantiate(m_timestamp, position, Quaternion.identity, m_timestampContainer.transform);
            textObj.transform.position = position;

            float time = (x / width) * m_music.clip.length;
            int minutes = (int)(time / 60);
            int seconds = (int)(time % 60);
            string timeText = minutes.ToString("D2") + ":" + seconds.ToString("D2");
            textObj.GetComponent<TextMeshProUGUI>().text = timeText;

            x += spaceBetweenTicks;
        }
    }

    void RemoveTimeStamps()
    {
        if (m_timestampContainer)
        {
            Transform[] timestamps = m_timestampContainer.GetComponentsInChildren<Transform>();
            
            for (int i = timestamps.Length - 1; i >= 0; i--)
            {
                if (timestamps[i].gameObject != m_timestampContainer)
                    Destroy(timestamps[i].gameObject);
            }
        }
    }
}
