using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoreSceneSingle : MonoBehaviour
{
    public TextMeshProUGUI loreText;
    [TextArea(5, 15)]
    public string fullText;

    public float typingSpeed = 0.03f;
    public string nextSceneName;

    private bool isTyping = false;
    private bool finishedTyping = false;

    void Start()
    {
        ChangeMusicForScene();
        StartCoroutine(TypeText());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Mostrar el texto entero si aún se estaba escribiendo
                StopAllCoroutines();
                loreText.text = fullText;
                isTyping = false;
                finishedTyping = true;
            }
            else if (finishedTyping)
            {
                // Ir a la siguiente escena
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    System.Collections.IEnumerator TypeText()
    {
        isTyping = true;
        loreText.text = "";

        foreach (char c in fullText)
        {
            loreText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        finishedTyping = true;
    }

    void ChangeMusicForScene()
    {
        // Detectamos qué escena está activa y cambiamos la canción
        string sceneName = SceneManager.GetActiveScene().name;

        if (GameMusicManager.Instance == null) return;

        if (sceneName == "Dialogo_bosque")
        {
            GameMusicManager.Instance.PlayTrack(1); // Música 2
        }
        else if (sceneName == "Dialogo_isla")
        {
            GameMusicManager.Instance.PlayTrack(2); // Música 3
        }
    }
}
