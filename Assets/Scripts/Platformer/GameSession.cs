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

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private string selectedProfileId = "test";

    private GameStateInfo gameStateInfo;
    private FileDataHandler dataHandler;

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
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }
    private void Start()
    {
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

    public void NewGame()
    {
        gameStateInfo = new GameStateInfo();
        FindObjectOfType<AudioBehavior>().DestroyAudio();
        ShowUI();
    }

    public void LoadGame()
    {
        gameStateInfo = dataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (gameStateInfo == null)
        {
            NewGame();
        }
        else
        {
            ShowUI();
            deathText.SetText($"Deaths: {gameStateInfo.deathCounter}");
            FindObjectOfType<AudioBehavior>().DestroyAudio();
            SceneManager.LoadScene("Overworld");
        }

        // if no data can be loaded, don't continue
        if (gameStateInfo == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }
    }

    public void SaveGame()
    {
        // if we don't have any data to save, log a warning here
        if (gameStateInfo == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // timestamp the data so we know when it was last saved
        gameStateInfo.lastUpdated = System.DateTime.Now.ToBinary();
        // save that data to a file using the data handler
        dataHandler.Save(gameStateInfo, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void ShowUI()
    {
        deathText.gameObject.SetActive(true);
    }
    public void HideUI()
    {
        deathText.gameObject.SetActive(false);
    }

}
