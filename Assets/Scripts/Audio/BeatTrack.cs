using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
//[CreateAssetMenu(fileName = "BeatTrack", menuName = "Audio/BeatTrack", order = 1)]
public class BeatTrack
{
    public string level;
    public List<Beat> beats;
}
