using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour, Interactables
{
    [SerializeField] int levelCounter;

    public void Interact()
    {
        SceneManager.LoadScene("PlatformerLevel" + levelCounter);
        Debug.Log("Load Level" + levelCounter);
    }
}
