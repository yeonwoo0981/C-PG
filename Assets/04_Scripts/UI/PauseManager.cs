using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider volumeSlider;
    [SerializeField] private Button reStrBtn;
    [SerializeField] private Button quitBtn;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        reStrBtn.onClick.AddListener(ResumeGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            OpenPauseMenu();
        }
    }

    void OpenPauseMenu()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void QuitGame()
    {  
        Application.Quit(); 
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}
