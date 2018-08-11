using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private Vector2 m_ramp;
    private float m_killSpeed;
    private bool m_movingRight;
    private bool m_movingUp;

    static Player ms_instance;
    private void Start()
    {
        if (ms_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            ms_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_movingRight = false;
        m_movingUp = false;
        m_killSpeed = 0.1f;
    }

    void Update()
    {
        float xVelocity = Input.GetAxis("MoveRight");
        float yVelocity = Input.GetAxis("MoveUp");
        m_movingRight = (xVelocity != 0.0f);
        m_movingUp = (yVelocity != 0.0f);

        // Scale Ramp
        if (m_movingRight)
        {
            m_ramp.x += Time.deltaTime * SpeedRamp;
        }
        else
        {
            m_ramp.x -= Time.deltaTime * (SpeedRamp + 10.0f);
        }
        if (m_movingUp)
        {
            m_ramp.y += Time.deltaTime * SpeedRamp;
        }
        else
        {
            m_ramp.y -= Time.deltaTime * (SpeedRamp + 10.0f); ;
        }
        if (m_movingRight && m_movingUp)
        {
            // Limit velocity when traveling diagonally
            m_ramp.x = Mathf.Clamp(m_ramp.x, 0.0f, 0.75f);
            m_ramp.y = Mathf.Clamp(m_ramp.y, 0.0f, 0.75f);
        }
        else
        {
            m_ramp.x = Mathf.Clamp(m_ramp.x, 0.0f, 1.0f);
            m_ramp.y = Mathf.Clamp(m_ramp.y, 0.0f, 1.0f);
        }


        // Calculate Velocity
        Vector3 velocity = Velocity;
        float speed = Speed * Time.deltaTime;
        velocity.x += xVelocity * m_ramp.x * speed;
        velocity.y += yVelocity * m_ramp.y * speed;
        Velocity = velocity;
        if (Velocity.magnitude >= MaxSpeed)
        {
            Velocity = Velocity.normalized * MaxSpeed;
        }
        transform.position += Velocity;
        Velocity *= Dampening;
        
        // Reduce Velocity if Not Moving
        Vector3 v = Velocity;
        if (!m_movingRight && Mathf.Abs(Velocity.x) >= m_killSpeed)
        {
            float oppSin = (v.x >= 0.0f) ? -1.0f : 1.0f;
            v.x += oppSin * SpeedGravity * Time.deltaTime;
        }
        else if (!m_movingRight)
        {
            v.x = 0.0f;
        }
        if (!m_movingUp && Mathf.Abs(Velocity.y) >= m_killSpeed)
        {
            float oppSin = (v.y >= 0.0f) ? -1.0f : 1.0f;
            v.y += oppSin * SpeedGravity * Time.deltaTime;
        }
        else if (!m_movingUp)
        {
            v.y = 0.0f;
        }
        Velocity = v;
    }
}
