using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] GameObject m_explosionTemplate = null;
    [SerializeField] [Range(0.0f, 100.0f)] float m_damage = 100.0f;
    [SerializeField] [Range(0.0f, 60.0f)] float m_lifetime = 5.0f;
    [SerializeField] [Range(0.0f, 100.0f)] float m_speed = 10.0f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_warmupTime = 0.2f;
    [SerializeField] AnimationCurve m_noiseAmountOverLife = null;
    [SerializeField] AnimationCurve m_noiseStrengthOverLife = null;
    [SerializeField] AnimationCurve m_noiseRateOverLife = null;

    public float Damage { get { return m_damage; } }
    public Entity Owner { get; set; }

    Quaternion m_startRot;
    Vector3 m_startDir;
    float m_timeToLive;
    float m_warmTime;
    float m_perlinSeed;

    private void Start()
    {
        m_timeToLive = m_lifetime;
        m_warmTime = 0.0f;
        m_startDir = transform.up.normalized;
        m_startRot = transform.rotation;
        m_perlinSeed = Random.Range(0.0f, 1.0f);
    }

    private void Update()
    {
        if (m_warmTime < m_warmupTime)
        {
            m_warmTime += Time.deltaTime;
            if (m_warmTime >= m_warmupTime)
                m_warmTime = m_warmupTime;
        }

        float t = 1.0f - (m_timeToLive / m_lifetime);
        float nAmount = m_noiseAmountOverLife.Evaluate(t);
        float nStrength = m_noiseStrengthOverLife.Evaluate(t);
        float nRate = m_noiseRateOverLife.Evaluate(t);

        Vector3 n = Vector3.zero;
        n.x = nAmount * nStrength * ((Mathf.PerlinNoise(Time.time * nRate + m_perlinSeed, 0.0f) * 2.0f) - 1.0f);
        n = m_startRot * n;

        Vector3 velocity = Vector3.zero;
        velocity = m_startDir * m_speed * Time.deltaTime;
        velocity += n;
        velocity.z = 0.0f;
        transform.position += velocity;

        float angle = Mathf.Atan2(-velocity.x, velocity.y);
        angle *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        m_timeToLive -= Time.deltaTime;

        if (m_timeToLive <= 0.0f)
        {
            // BOOM
            Explode();
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(m_explosionTemplate, transform.position, Quaternion.identity, transform.parent);
        Destroy(explosion, 2.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            Entity e = collision.GetComponent<Entity>();
            if (e != null && e.Health > 0.0f)
            {
                e.Health -= Damage;
                if (e.Health <= 0.0f)
                {
                    Owner.Score += e.PointValue;
                }
            }

            Explode();
        }
    }
}
