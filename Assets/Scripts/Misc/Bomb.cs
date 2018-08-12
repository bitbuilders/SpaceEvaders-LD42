using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject m_boom = null;
    [SerializeField] Transform m_start = null;
    [SerializeField] Transform m_end = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_explosionsPerSecond = 0.1f;
    [SerializeField] [Range(0.0f, 60.0f)] float m_duration = 30.0f;
    [SerializeField] [Range(0.0f, 30.0f)] float m_explosionWidth = 10.0f;
    
    Vector3 m_minPos;
    Vector3 m_maxPos;
    float m_currentTime;
    float m_explosionTimer;

    private void Start()
    {
        m_currentTime = 0.0f;
    }

    private void Update()
    {
        m_currentTime += Time.deltaTime;

        m_explosionTimer += Time.deltaTime;
        if (m_explosionTimer >= m_explosionsPerSecond)
        {
            m_explosionTimer = 0.0f;
            SetPositions();
            Vector3 pos = GetRandomPosition();
            SpawnBoom(pos);
        }
    }

    private void SetPositions()
    {
        Vector3 avg = Vector3.Lerp(m_start.position, m_end.position, m_currentTime / m_duration);
        m_minPos = new Vector3(avg.x - m_explosionWidth, m_start.position.y);
        m_maxPos = new Vector3(avg.x, m_end.position.y);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(m_minPos.x, m_maxPos.x);
        float y = Random.Range(m_minPos.y, m_maxPos.y);
        Vector3 position = new Vector3(x, y);
        return position;
    }

    private void SpawnBoom(Vector3 position)
    {
        GameObject boom = Instantiate(m_boom, position, Quaternion.identity, transform);
        Destroy(boom, 3.0f);
    }
}
