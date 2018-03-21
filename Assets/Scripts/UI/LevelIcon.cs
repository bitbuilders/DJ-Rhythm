using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIcon : MonoBehaviour
{
    [SerializeField] public BeatTrack m_track = null;
    [SerializeField] public bool m_canBeClicked = true;

    public void LoadLevel()
    {
        if (m_canBeClicked)
        {
            WorldSelector.Instance.StartTrack(m_track);
        }
    }
}
