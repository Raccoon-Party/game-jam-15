using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCrystalBehavior : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 5f;
    [SerializeField] string nextLevel = "";
    [SerializeField] GameObject UIPopup;
    [SerializeField] string crystalType;

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
        FindObjectOfType<GameSession>().ShowCrystalInfoPanel(crystalType);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<GameSession>().HideCrystalInfoPanel();
        Time.timeScale = 1;
        if (SceneManager.GetSceneByName(nextLevel) != null)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
