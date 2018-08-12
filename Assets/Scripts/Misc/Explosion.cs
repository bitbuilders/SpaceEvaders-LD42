using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 20.0f)] float m_hitboxLinger = 0.75f;

    Collider m_collider;
    float m_time = 0.0f;

    private void Start()
    {
        m_collider = GetComponent<Collider>();
        m_collider.enabled = true;
    }

    private void Update()
    {
        m_time += Time.deltaTime;

        if (m_time >= m_hitboxLinger)
        {
            m_collider.enabled = false;
        }
    }
}
