using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_text = null;

    private void Start()
    {
        m_text.enabled = false;
    }

    public void Hover()
    {
        m_text.enabled = true;
    }

    public void Exit()
    {
        m_text.enabled = false;
    }
}
