using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry(){
        SceneManager.LoadScene(HealthSystem.lastLevel);
    }

    public void Title(){
        SceneManager.LoadScene("Titulo");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
