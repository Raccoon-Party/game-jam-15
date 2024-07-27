using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public enum GameState
{
    FreeRoam, Dialog
}

public class GameController : MonoBehaviour
{
    PlayerBehaviour playerBehaviour;

    GameState state;

    bool isInOverworld => SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByName("Overworld").buildIndex;

    public void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();
    }

    private void Update()
    {
        if (isInOverworld)
        {
            switch (state)
            {
                case GameState.FreeRoam:
                    playerBehaviour.GetComponent<PlayerInput>().actions.FindAction("Move").Enable();
                    break;

                case GameState.Dialog:
                    playerBehaviour.GetComponent<PlayerInput>().actions.FindAction("Move").Disable();
                    break;

                default:
                    break;
            }
        }
    }
}
