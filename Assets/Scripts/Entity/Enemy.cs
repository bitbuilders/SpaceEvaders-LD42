using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] GameObject m_deathExplosion = null;
    [SerializeField] GameObject m_bomb = null;
    [SerializeField] [Range(0, 1000)] int m_pointValue;
    [SerializeField] [Range(0.0f, 1000.0f)] float m_health;
    [SerializeField] [Range(0.0f, 10.0f)] float m_deathDuration = 0.5f;
    [SerializeField] Vector2 m_deathForceMinMax = new Vector2(8.0f, 15.0f);
    [SerializeField] [Range(0.0f, 30.0f)] float m_detectionRadius = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_swerveRate;
    [SerializeField] [Range(0.0f, 10.0f)] float m_swerveStrength;
    [SerializeField] Vector2 m_bombDropRateMinMax = new Vector2(0.5f, 2.0f);

    Player m_player;
    Rigidbody2D m_rigidbody;
    Vector3 m_rotation;
    float m_deathTime;
    float m_nextBombDrop;
    float m_bombDropTime;
    bool m_dead;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_player = FindObjectOfType<Player>();
        Health = m_health;
        PointValue = m_pointValue;
        m_nextBombDrop = Random.Range(m_bombDropRateMinMax.x, m_bombDropRateMinMax.y);
    }

    private void Update()
    {
        if (m_dead)
        {
            m_deathTime += Time.deltaTime;

            transform.rotation *= Quaternion.Euler(m_rotation * Time.deltaTime);

            if (m_deathTime >= m_deathDuration)
            {
                GameObject expl = Instantiate(m_deathExplosion, transform.position, Quaternion.identity);
                Destroy(expl, 2.0f);
                Destroy(gameObject);
            }
        }
        else
        {
            Vector3 dir = m_player.transform.position - transform.position;
            if (dir.magnitude <= m_detectionRadius)
            {
                Vector3 velocity = Vector3.zero;
                velocity = -dir.normalized * (Speed * Time.deltaTime);

                float angle = Mathf.Atan2(-velocity.x, velocity.y);
                angle *= Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Vector3 n = Vector3.zero;
                n.x = m_swerveStrength * ((Mathf.PerlinNoise(Time.time * m_swerveStrength, 0.0f) * 2.0f) - 1.0f);
                n.x = m_swerveStrength * 0.2f * ((Mathf.PerlinNoise(Time.time * m_swerveStrength, 0.0f) * 2.0f) - 1.0f);
                n = transform.rotation * n;

                transform.position += velocity + n;

                m_bombDropTime += Time.deltaTime;
                if (m_bombDropTime >= m_nextBombDrop)
                {
                    m_bombDropTime = 0.0f;
                    m_nextBombDrop = Random.Range(m_bombDropRateMinMax.x, m_bombDropRateMinMax.y);
                    Instantiate(m_bomb, transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (Health <= 0.0f && !m_dead)
        {
            Explode();
        }
    }

    private void Explode()
    {
        m_dead = true;
        Vector3 dir = Random.insideUnitCircle.normalized;
        float force = Random.Range(m_deathForceMinMax.x, m_deathForceMinMax.y);
        m_rigidbody.AddForce(dir * force, ForceMode2D.Impulse);

        float s = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
        m_rotation.z = Random.Range(260.0f, 360.0f);
        m_rotation.z *= s;
    }
}
