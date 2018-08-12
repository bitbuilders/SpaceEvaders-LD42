using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] GameObject m_target = null;
    [Space(10)]
    [Header("Camera Settings")]
    [SerializeField] [Range(0.0f, -50.0f)] float m_cameraDistance = -10.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_cameraLead = 1.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_cameraStiffness = 5.0f;

    private Vector3 m_targetPosition;
    private Entity m_targetEntity;

    private void Start()
    {
        m_target = FindObjectOfType<Player>().gameObject;
        m_targetPosition = m_target.transform.position;
        m_targetEntity = m_target.GetComponent<Entity>();
    }

    private void LateUpdate()
    {
        float targetDir = (m_targetEntity.Velocity.y > 0.0f) ? 1.0f : -1.0f;
        Vector3 lead = (m_target.transform.up.normalized * (m_cameraLead * targetDir)) * m_targetEntity.Velocity.magnitude / m_targetEntity.MaxSpeed;
        m_targetPosition = m_target.transform.position + lead;
        m_targetPosition.z = m_cameraDistance;

        transform.position = Vector3.Lerp(transform.position, m_targetPosition, Time.deltaTime * m_cameraStiffness);

    }
}
