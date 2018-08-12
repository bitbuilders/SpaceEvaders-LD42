using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] [Range(0.0f, 1000.0f)] float m_speed = 1.0f;
    [SerializeField] [Range(0.0f, 1000.0f)] float m_maxSpeed = 0.3f;
    [Space(15)]
    [Header("Physics")]
    //[SerializeField] [Range(0.0f, 1000.0f)] float m_speedGravity = 1.0f; // If the entity isn't pressing a key, this is how fast the velocity will return to zero.
    [SerializeField] [Range(0.0f, 1000.0f)] float m_speedRamp = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_dampening = 0.9f;

    public Vector3 Velocity { get; set; }
    public float MaxSpeed { get { return m_maxSpeed; } protected set { m_maxSpeed = value; } }
    public float Speed { get { return m_speed; } protected set { m_speed = value; } }
    //public float SpeedGravity { get { return m_speedGravity; } protected set { m_speedGravity = value; } }
    public float SpeedRamp { get { return m_speedRamp; } protected set { m_speedRamp = value; } }
    public float Dampening { get { return m_dampening; } protected set { m_dampening = value; } }
}
