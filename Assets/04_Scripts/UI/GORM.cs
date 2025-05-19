using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GORM : MonoBehaviour
{
    [Header("Game Over Buttons")]
    public Button restartBtn;
    public Button mainMenuBtn;

    private void Start()
    {
        if (restartBtn != null)
            restartBtn.onClick.AddListener(RestartGame);
        if (mainMenuBtn != null)
            mainMenuBtn.onClick.AddListener(GoToMainMenu);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }
}
