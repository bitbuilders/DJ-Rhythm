using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeatStamp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_timeMark = null;
    public Beat m_beat;

    BeatCreator m_creator;
    AudioSource m_music;
    RectTransform m_timeMarkTransform;
    RectTransform m_rectTransform;
    bool m_followCursor = false;

    private void Start()
    {
        m_creator = BeatCreator.Instance;
        m_music = Game.Instance.GetComponent<AudioSource>();
        m_rectTransform = GetComponent<RectTransform>();
        m_timeMarkTransform = m_timeMark.GetComponent<RectTransform>();

        Lift();
    }
    
    private void LateUpdate()
    {
        if (m_followCursor)
        {
            SetPosition();
        }
    }

    public void SetPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 position = m_rectTransform.position;
        position.x = mousePos.x;
        position.x = Mathf.Clamp(position.x, 0.0f, Screen.width - 3.0f);
        m_rectTransform.position = position;

        CheckMarkBounds();

        if (m_timeMark)
            m_timeMark.text = GetTimeStamp();
    }

    private void CheckMarkBounds()
    {
        if (m_rectTransform.position.x < 25.0f)
        {
            SetTimeMarkPosition(25.0f);
        }
        else if (Screen.width - m_rectTransform.position.x < 25.0f)
        {
            SetTimeMarkPosition(Screen.width - 25.0f);
        }
    }

    private void SetTimeMarkPosition(float position)
    {
        Vector3 textPos = m_timeMarkTransform.position;
        textPos.x = position;
        m_timeMarkTransform.position = textPos; 
    }

    private string GetTimeStamp()
    {
        string timestamp = "";

        float duration = m_music.clip.length;
        float position = (transform.position.x / Screen.width);
        m_beat.startTime = position * duration;
        if (m_beat.startTime < 0.1f)
        {
            m_beat.startTime = 0.0f;
        }
        int time = (int)(position * duration);
        int minutes = time / 60;
        int seconds = time % 60;
        timestamp = minutes.ToString("D2") + ":" + seconds.ToString("D2");

        return timestamp;
    }

    public void Click()
    {
        if (m_timeMark)
            m_timeMark.gameObject.SetActive(true);
        m_followCursor = true;

        if (m_creator.InEraseMode)
        {
            m_creator.DeleteBeat(m_beat);
            Destroy(gameObject);
        }
    }

    public void Lift()
    {
        m_followCursor = false;
        if (m_timeMark)
            m_timeMark.gameObject.SetActive(false);
    }

    public void Hover()
    {
        if (!m_followCursor)
            CheckMarkBounds();
        m_timeMark.text = GetTimeStamp();
        m_timeMark.gameObject.SetActive(true);
    }

    public void Leave()
    {
        if (!m_followCursor)
        {
            m_timeMark.gameObject.SetActive(false);
        }
    }
}
