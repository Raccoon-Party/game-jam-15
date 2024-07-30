using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, Interactables
{
    [SerializeField] string sceneName;

    public void Interact()
    {
        FindObjectOfType<GameSession>().SaveOverworldPosition(FindObjectOfType<PlayerBehaviour>().transform.position);
        SceneManager.LoadScene(sceneName);

        Debug.Log("Load Level" + sceneName);
    }
}
