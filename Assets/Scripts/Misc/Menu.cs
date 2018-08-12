using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void LoadGame()
    {
        Game.Instance.LoadGame();
    }

    public void MainMenu()
    {
        Game.Instance.ReturnToMainMenu();
    }

    public void Help()
    {

    }

    public void Quit()
    {
        Game.Instance.Quit();
    }
}
