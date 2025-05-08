using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Slider volumeSlider;


    void Start()
    {
        
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.5f);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)
    {
        
        PlayerPrefs.SetFloat("GameVolume", volume);
        AudioListener.volume = volume;
    }

    public void StartGame()
    {
        
        SceneManager.LoadScene(1);
    }

}
