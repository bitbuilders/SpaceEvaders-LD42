using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] Player m_player = null;
    [Space(15)]
    [Header("Elements")]
    [SerializeField] Text m_score = null;
    [SerializeField] Text m_speed = null;

    private void Start()
    {
        m_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        m_score.text = m_player.Score.ToString("D8");
        m_speed.text = m_player.DeltaSpeed.ToString();
    }
}
