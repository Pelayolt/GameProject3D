using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour
{
    public static GameMusicManager Instance;

    public AudioClip[] musicTracks; // 0: título/primera, 1: segunda, 2: tercera

    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    void Start()
    {
        if (GameMusicManager.Instance != null)
        {
            GameMusicManager.Instance.PlayTrack(0);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // evita duplicados
        }
    }

    public void PlayTrack(int index)
    {
        if (index < 0 || index >= musicTracks.Length) return;
        if (index == currentTrackIndex) return;

        currentTrackIndex = index;
        audioSource.clip = musicTracks[index];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
