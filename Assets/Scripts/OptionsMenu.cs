using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;

    public GameObject optionsPopup;
    public GameObject closeButton;
    public GameObject exitButton;
    public GameObject controlsPopup;

    private float volMusic;
    private float volSfx;

    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioMixer mixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        
    }
    void Start()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    public void SetResolution(int width, int height){
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void ToggleMuteMusic(bool mute){
        float vol;
        if(mute){
            if (mixer.GetFloat("MusicVol", out vol))
                volMusic = vol;
            mixer.SetFloat("MusicVol", -80);
            musicSlider.interactable = false;
        }else{
            mixer.SetFloat("MusicVol", volMusic);
            musicSlider.interactable = true;
        }
    }

    public void ToggleMuteSfx(bool mute){
        float vol;
        if(mute){
            if (mixer.GetFloat("SFXVol", out vol))
                volSfx = vol;
            mixer.SetFloat("SFXVol", -80);
            sfxSlider.interactable = false;
        }else{
            mixer.SetFloat("SFXVol", volSfx);
            sfxSlider.interactable = true;
        }
    }

    public void ResolutionOptionSelected(int op){
        if (op == 0){
            SetResolution(1920, 1080);
        }else if(op == 1){
            SetResolution(960, 540);
        }else if(op == 2){
            SetResolution(2880, 1620);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
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
