using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPotionBehavior : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 5f;
    [SerializeField] string nextLevel = "";
    [SerializeField] GameObject UIPopup;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
            //FindObjectOfType<ScenePersist>().ResetScenePersist();
        }
    }

    IEnumerator LoadNextLevel()
    {
        UIPopup.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        UIPopup.SetActive(false);
        Time.timeScale = 1;
        if (SceneManager.GetSceneByName(nextLevel) != null)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
