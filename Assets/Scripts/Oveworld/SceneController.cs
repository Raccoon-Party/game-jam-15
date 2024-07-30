using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, Interactables
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject comingSoonPanel;

    public void Interact()
    {

        if (sceneName == "ComingSoon")
        {
            comingSoonPanel.SetActive(true);
            return;
        }
        else
        {
            FindObjectOfType<GameSession>().SaveOverworldPosition(FindObjectOfType<PlayerBehaviour>().transform.position);
            SceneManager.LoadScene(sceneName);
            Debug.Log("Load Level" + sceneName);
        }
    }
}
