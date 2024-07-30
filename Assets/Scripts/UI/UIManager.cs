using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void StartGame()
    {
        FindObjectOfType<GameSession>().NewGame();
        SceneManager.LoadScene("Overworld");
    }

    public void LoadGame()
    {
        FindObjectOfType<GameSession>().LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
