using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    private bool isPaused = false;
    OptionsMenu optionsMenu;

    void Start()
    {
        optionsMenu = FindObjectOfType<OptionsMenu>();
        optionsMenu.GameMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        optionsMenu.CloseOptions();
        Time.timeScale = 1f; // Reactiva el tiempo
        isPaused = false;
    }

    public void Pause()
    {
        optionsMenu.OpenOptions();
        Time.timeScale = 0f; // Detiene el tiempo
        isPaused = true;
    }
    
}
