using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    [SerializeField] GameObject m_missile = null;
    [SerializeField] ParticleSystem[] m_launchPoints;
    [SerializeField] Transform m_missilePool;

    private int m_launchPoint;

    private void Start()
    {
        m_launchPoint = 0;
    }

    public void Fire(Quaternion rotation)
    {
        SpawnMissile(rotation);
    }

    private void SpawnMissile(Quaternion rotation)
    {
        Vector3 position = m_launchPoints[m_launchPoint].gameObject.transform.position;
        m_launchPoints[m_launchPoint].Play();
        m_launchPoint++;
        m_launchPoint %= m_launchPoints.Length;
        GameObject missile = Instantiate(m_missile, position, rotation, m_missilePool);
    }
}
