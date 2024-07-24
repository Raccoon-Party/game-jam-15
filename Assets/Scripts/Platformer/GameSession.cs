using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathText;

    float deathCounter = 0;

    private void Awake()
    {
        int numSessions = FindObjectsOfType<GameSession>().Length;
        if (numSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        deathText.SetText($"Deaths: {deathCounter}");
    }

    private void Update()
    {

    }

    public void ProcessPlayerDeath()
    {
        TakeLife();
        //ResetGameSession();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        deathCounter++;
        deathText.SetText($"Deaths: {deathCounter}");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
