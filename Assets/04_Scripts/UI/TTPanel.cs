using UnityEngine;
using UnityEngine.UI;

public class TTPanel : MonoBehaviour
{
    public GameObject pausePanel;
    
    [SerializeField] private Button reStrBtn;
    

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
       
        reStrBtn.onClick.AddListener(ResumeGame);
        
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

    

   
}
