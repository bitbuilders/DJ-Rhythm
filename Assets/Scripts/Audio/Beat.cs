using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
//[CreateAssetMenu(fileName = "AudioBeat", menuName = "Audio/Beat", order = 1)]
public class Beat
{
    [Range(0.0f, 300.0f)] public float startTime;
    [Range(0.0f, 300.0f)] public float timeWindow;
    [Range(0.0f, 50.0f)] public float shakeAmplitude;
    [Range(0.0f, 50.0f)] public float shakeRate;
    [Range(0.0f, 1.0f)] public float shakeDuration;
    [System.NonSerialized] public bool hasTriggered;
}
