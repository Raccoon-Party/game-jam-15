using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<AudioBehavior>().DestroyAudio();
        SceneManager.LoadScene("Overworld");
    }
}
