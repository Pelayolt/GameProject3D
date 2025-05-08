using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public GameObject optionsPopup;
    public GameObject closeButton;
    public GameObject exitButton;
    public GameObject controlsPopup;

    public AudioMixer mixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        optionsPopup.SetActive(false);
        controlsPopup.SetActive(false);
        TitleMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float value){
        mixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value){
        mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }

    public void SetEffectsVolume(float value){
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }

    public void TitleMode()
    {
        exitButton.SetActive(false);
        closeButton.SetActive(true);
    }

    public void GameMode()
    {
        exitButton.SetActive(true);
        closeButton.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsPopup.SetActive(true);
        controlsPopup.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPopup.SetActive(false);
    }

    public void SeeControls()
    {
        controlsPopup.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPopup.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
