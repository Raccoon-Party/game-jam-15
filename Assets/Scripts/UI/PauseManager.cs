using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    private PlayerInput playerInput;

    bool isPaused = false;

    private void Awake()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
    }

    public void Pause()
    {
        isPaused = true;
        playerInput.actions.FindActionMap("Player").FindAction("Move").Disable();
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        playerInput.actions.FindActionMap("Player").Enable();
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Save()
    {
        FindObjectOfType<GameSession>().SaveGame();
        Resume();
    }

    public void MainMenu()
    {
        Save();
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    void OnPause(InputValue inputValue)
    {
        if (inputValue.isPressed && !isPaused)
        {
            Pause();
        }
        else if (isPaused)
        {
            Resume();
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
