using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] Entity m_player = null;
    [Space(15)]
    [Header("Elements")]
    [SerializeField] Text m_score = null;

    private void Start()
    {
        m_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        m_score.text = m_player.Score.ToString("D8");
    }
}
