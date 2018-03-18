using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIcon : MonoBehaviour
{
    [SerializeField] public BeatTrack m_track = null;

    public void LoadLevel()
    {
        WorldSelector.Instance.StartTrack(m_track);
    }
}
