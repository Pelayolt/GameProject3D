using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Se ha pulsado Escape");
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Reactiva el tiempo
        isPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f; // Detiene el tiempo
        isPaused = true;
    }

    public void QuitGame()
    {
        // Esto funciona en build, no en el editor
        Application.Quit();
    }
    
}
