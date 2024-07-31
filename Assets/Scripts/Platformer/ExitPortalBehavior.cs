using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortalBehavior : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] string nextLevel = "";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameSession>().CompleteLevel(SceneManager.GetActiveScene().name);

            StartCoroutine(LoadNextLevel());
            //FindObjectOfType<ScenePersist>().ResetScenePersist();
        }
    }

    IEnumerator LoadNextLevel()
    {
        Debug.Log(nextLevel);
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        if (SceneManager.GetSceneByName(nextLevel) != null)
        {
            FindObjectOfType<AudioBehavior>().DestroyAudio();
            SceneManager.LoadScene(nextLevel);
        }
    }
}
