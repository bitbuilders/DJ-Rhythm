using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AudioBeat", menuName = "Audio/Beat", order = 1)]
public class Beat : ScriptableObject
{
    public Sprite beatImage;
    public Material materialTemplate;
    [System.NonSerialized] public SpriteRenderer renderer;
    public Vector2 position;
    public Vector2 scale;
    [Range(0.0f, 300.0f)] public float startTime;
    [Range(0.0f, 300.0f)] public float leadUpTime;
    [Range(0.0f, 50.0f)] public float shakeAmplitude;
    [Range(0.0f, 50.0f)] public float shakeRate;
}
