using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    OptionsMenu optionsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        optionsMenu = FindObjectOfType<OptionsMenu>();
        optionsMenu.TitleMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Juego()
    {
        SceneManager.LoadScene("ciudad");
    }

    public void Options(){
        optionsMenu.OpenOptions();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
