using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [Header("Input")]
    [SerializeField] GameObject m_target = null;
    [Space(15)]
    [Header("Camera Settings")]
    [SerializeField] [Range(0.0f, -50.0f)] float m_cameraDistance = -10.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_cameraLead = 1.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_cameraStiffness = 5.0f;
    [Space(15)]
    [Header("Shake")]
    [SerializeField] [Range(0.0f, 1.0f)] float m_shakeTime = 0.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_shakeRate = 5.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_shakeStrength = 5.0f;

    private Entity m_targetEntity;
    private Vector3 m_targetPosition;
    private Vector3 m_shake;

    private void Start()
    {
        m_target = FindObjectOfType<Player>().gameObject;
        m_targetPosition = m_target.transform.position;
        m_targetEntity = m_target.GetComponent<Entity>();
    }

    private void Update()
    {
        m_shakeTime -= Time.deltaTime;
        m_shakeTime = Mathf.Clamp01(m_shakeTime);

        m_shake = Vector3.zero;
        float t = Time.time * m_shakeRate;
        m_shake.x = m_shakeTime * m_shakeStrength * ((Mathf.PerlinNoise(t, 0.0f) * 2.0f) - 1.0f);
        m_shake.y = m_shakeTime * m_shakeStrength * ((Mathf.PerlinNoise(0.0f, t) * 2.0f) - 1.0f);
    }

    private void LateUpdate()
    {
        float targetDir = (m_targetEntity.Velocity.y > 0.0f) ? 1.0f : -1.0f;
        Vector3 lead = (m_target.transform.up.normalized * (m_cameraLead * targetDir)) * m_targetEntity.Velocity.magnitude / m_targetEntity.MaxSpeed;
        m_targetPosition = m_target.transform.position + lead;
        m_targetPosition.z = m_cameraDistance;

        Vector3 targetPosition = Vector3.Lerp(transform.position, m_targetPosition, Time.deltaTime * m_cameraStiffness);
        transform.position = targetPosition + m_shake;
    }

    public void Shake(float time, float strength = -1.0f, float rate = -1.0f)
    {
        if (strength > 0.0f)
            m_shakeStrength = strength;
        if (rate > 0.0f)
            m_shakeRate = rate;

        m_shakeTime = time;
        m_shakeTime = Mathf.Clamp01(m_shakeTime);
    }
}
