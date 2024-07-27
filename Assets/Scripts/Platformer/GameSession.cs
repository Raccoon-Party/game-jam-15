using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathText;
    [SerializeField] GameObject dogInfoPanel;

    private GameStateInfo gameStateInfo;

    private void Awake()
    {
        int numSessions = FindObjectsOfType<GameSession>().Length;
        gameStateInfo = new GameStateInfo();
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
        gameStateInfo = new GameStateInfo();
        deathText.SetText($"Deaths: {gameStateInfo.deathCounter}");
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
        gameStateInfo.deathCounter++;
        deathText.SetText($"Deaths: {gameStateInfo.deathCounter}");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowDogInfoPanel()
    {
        dogInfoPanel.SetActive(true);
    }
    public void HideDogInfoPanel()
    {
        dogInfoPanel.SetActive(false);
    }

    public void SaveOverworldPosition(Vector2 position)
    {
        gameStateInfo.OverworldPosition = position;
    }

    public Vector2 GetSavedOverworldPosition()
    {
        return gameStateInfo.OverworldPosition != null ? gameStateInfo.OverworldPosition : GameStateInfo._defaultPosition;
    }

}
