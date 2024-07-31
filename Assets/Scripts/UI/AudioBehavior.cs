using UnityEngine;

public class AudioBehavior : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void Awake()
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
    public void DestroyAudio()
    {
        Destroy(gameObject);
    }
}
