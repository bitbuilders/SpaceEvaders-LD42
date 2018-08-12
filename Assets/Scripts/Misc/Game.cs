using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
    Player m_player;
    static Game ms_instance;

    private void Start()
    {
        if (ms_instance == null)
        {
            ms_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_player = FindObjectOfType<Player>();
        m_player.gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level01");
        m_player.gameObject.SetActive(true);
        m_player.Spawn();
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        m_player.gameObject.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
