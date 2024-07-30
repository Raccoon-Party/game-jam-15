using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioBehavior : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            int numSessions = FindObjectsOfType<AudioBehavior>().Length;
            if (numSessions > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            Destroy(gameObject);
        }
    }
}
