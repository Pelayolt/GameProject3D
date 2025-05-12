using UnityEngine;

public class MusicFinal : MonoBehaviour
{
    public static MusicFinal Instance;

    private AudioSource audioSource;

    [Header("Canción que se mantiene entre escenas")]
    public AudioClip musicClip;

    [Header("Nombre de la escena final donde se detendrá")]
    public string finalSceneName = "EndScene";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.Play();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // evitar duplicados
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == finalSceneName)
        {
            Destroy(gameObject); // 🔥 al llegar a la última escena, cortamos la música
        }
    }
}