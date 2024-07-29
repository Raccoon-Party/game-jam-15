using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathText;
    [SerializeField] GameObject dogInfoPanel;
    [SerializeField] GameObject crystalInfoPanel;

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
        UnlockAnimal("dog");
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
        UnlockAnimal("dog");
    }
    public void HideDogInfoPanel()
    {
        dogInfoPanel.SetActive(false);
    }

    public void ShowCrystalInfoPanel(string crystal)
    {
        crystalInfoPanel.SetActive(true);
        gameStateInfo.UnlockedCrystals.Add(crystal);
    }
    public void HideCrystalInfoPanel()
    {
        crystalInfoPanel.SetActive(false);
    }


    public void SaveOverworldPosition(Vector2 position)
    {
        gameStateInfo.OverworldPosition = position;
    }

    public Vector2 GetSavedOverworldPosition()
    {
        return gameStateInfo.OverworldPosition != null ? gameStateInfo.OverworldPosition : GameStateInfo._defaultPosition;
    }

    public void CompleteLevel(string name)
    {
        gameStateInfo.CompletedLevels.Add(name);
    }
    public void UnlockAnimal(string animalName)
    {
        gameStateInfo.UnlockedAnimals.Add(animalName);
    }

    public bool IsAnimalUnlocked(int animalId)
    {
        switch (animalId)
        {
            case 1:
                return true;
            case 2:
                return gameStateInfo.UnlockedAnimals.Contains("dog");
            default:
                return false;
        };
    }

}
