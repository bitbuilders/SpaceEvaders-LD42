using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExploder : MonoBehaviour
{
    [SerializeField] Color m_rageColor = Color.red;
    [SerializeField] [Range(0.0f, 10.0f)] float m_detonationTime;
    [SerializeField] [Range(0.0f, 10.0f)] float m_scaleAmount = 2.0f;
    [SerializeField] AnimationCurve m_scaleOverTime = null;

    Material m_material;
    Color m_startColor;
    Collider2D m_collider;
    float m_time;

    private void Start()
    {
        m_material = GetComponentInChildren<Renderer>().material;
        m_startColor = m_material.color;
        m_collider = GetComponent<Collider2D>();
        m_collider.enabled = false;
        print(m_material != null);
    }

    private void Update()
    {
        m_time += Time.deltaTime;

        float t = m_time / m_detonationTime;
        Color c = Color.Lerp(m_startColor, m_rageColor, t);
        m_material.color = c;

        float v = m_scaleOverTime.Evaluate(t);
        float s = Mathf.Lerp(1.0f, m_scaleAmount, v);
        transform.localScale = new Vector3(s, s, s);

        if (m_time >= m_detonationTime)
        {
            // BOOM
        }
    }
}
