using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] [Range(0.0f, 900.0f)] float m_rotationSpeed = 180.0f;

    AudioSource m_audio;
    private Rigidbody2D m_rigidbody;
    private MissileManager m_missileManager;
    private Quaternion m_lastRotation;
    private Vector3 m_rotation;
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

        m_rotation = transform.rotation.eulerAngles;
        m_movingRight = false;
        m_movingUp = false;
        m_killSpeed = 0.001f;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_audio = GetComponent<AudioSource>();
        m_missileManager = GetComponent<MissileManager>();
        Health = 100.0f;
    }

    void Update()
    {
        ProcessInput();

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
            m_ramp.y -= Time.deltaTime * (SpeedRamp + 10.0f);
        }

        m_ramp.x = Mathf.Clamp(m_ramp.x, 0.0f, 1.0f);
        m_ramp.y = Mathf.Clamp(m_ramp.y, 0.0f, 1.0f);

        // Calculate Rotation
        float reverse = (Velocity.y >= 0.0f) ? 1.0f : -1.0f;
        m_rotation.z += -xVelocity * m_rotationSpeed * Time.deltaTime * reverse;
        m_rotation.y += -xVelocity * m_rotationSpeed * Time.deltaTime * reverse;
        m_rotation.y = Mathf.Clamp(m_rotation.y, -40.0f, 40.0f);
        if (!m_movingRight && Mathf.Abs(m_rotation.y) > 2.0f)
        {
            float oppSin = (m_rotation.y >= 0.0f) ? -1.0f : 1.0f;
            m_rotation.y += oppSin * m_rotationSpeed * Time.deltaTime;
        }
        else if (!m_movingRight)
        {
            m_rotation.y = 0.0f;
        }

        // Calculate Velocity
        Vector3 velocity = Velocity;
        float speed = yVelocity * m_ramp.y * Speed * Time.deltaTime;
        //velocity.x += xVelocity * m_ramp.x * speed;
        velocity.y += speed;
        velocity.y = Mathf.Clamp(velocity.y, 0.0f, MaxSpeed);
        Velocity = velocity;

        transform.rotation = Quaternion.Euler(m_rotation);
        m_lastRotation = Quaternion.Euler(0.0f, 0.0f, m_rotation.z);

        transform.position += m_lastRotation * Velocity;



        if (Health <= 0.0f)
        {
            // Die
            Spawn();
            gameObject.SetActive(false);
            Game.Instance.LoadScene("Lose");
        }
    }

    private void FixedUpdate()
    {
        // Reduce Velocity if Not Moving
        Vector3 v = Velocity;
        if (!m_movingRight && Mathf.Abs(Velocity.x) >= m_killSpeed)
        {
            //float oppSin = (v.x >= 0.0f) ? -1.0f : 1.0f;
            //v.x += oppSin * SpeedGravity * Time.deltaTime;
            v.x *= Dampening;
        }
        else if (!m_movingRight)
        {
            v.x = 0.0f;
        }
        if (!m_movingUp && Mathf.Abs(Velocity.y) >= m_killSpeed)
        {
            //float oppSin = (v.y >= 0.0f) ? -1.0f : 1.0f;
            //v.y += oppSin * SpeedGravity * Time.deltaTime;
            v.y *= Dampening;
        }
        else if (!m_movingUp)
        {
            v.y = 0.0f;
        }
        Velocity = v;
    }

    private void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, m_rotation.z);
            if (m_missileManager.m_missilePool == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("MissilePool");
                if (go != null)
                {
                    m_missileManager.m_missilePool = go.transform;
                }
            }
            if (m_missileManager.m_missilePool != null)
                m_missileManager.Fire(rotation);
            m_audio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            Health -= 20.0f;
            CameraController.Instance.Shake(1.0f, 1.5f, 1.5f);
        }
        else if (collision.CompareTag("Death"))
        {
            Health -= 100.0f;
        }
    }

    public void Spawn()
    {
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, -90.0f);
        transform.SetPositionAndRotation(Vector3.zero, rot);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        Velocity = Vector3.zero;
        Health = 100.0f;
    }
}
