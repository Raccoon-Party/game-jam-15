using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour, Interactables
{
    [SerializeField] string levelName;

    public void Interact()
    {
        SceneManager.LoadScene(levelName);

        Debug.Log("Load Level" + levelName);
    }
}
