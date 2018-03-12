using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatListener : MonoBehaviour
{
    [SerializeField] GameObject m_cube = null;

    void Start()
    {
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);
        processor.onSpectrum.AddListener(onSpectrum);
    }

    void onOnbeatDetected()
    {
        Debug.Log("Beat!!!");
        m_cube.SetActive(true);
        StartCoroutine(HideBeat());
    }
    
    void onSpectrum(float[] spectrum)
    {
        //The spectrum is logarithmically averaged
        //to 12 bands

        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }

    IEnumerator HideBeat()
    {
        yield return new WaitForSeconds(0.1f);
        m_cube.SetActive(false);
    }
}
