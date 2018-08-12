using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour
{
    [SerializeField] Text m_message = null;

    Player m_player;

    private void Start()
    {
        m_player = FindObjectOfType<Player>();

        m_message.text = "Nice Job! You won with " + m_player.Score + " points!";
    }
}
