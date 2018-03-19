using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float m_score;

    public float Score { get { return m_score; } }

    public void AddPoints(float points)
    {
        m_score += points;
        //print("Points added: " + points + " | New Score:" + Score);
    }
}
