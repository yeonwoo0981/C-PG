using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgmSource; 
    [SerializeField] private Slider _Slider;   

    private void Start()
    {
       _bgmSource.volume = 1.5f;
       //_Slider.value = _bgmSource.volume = 5;  
       //_Slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {
        _bgmSource.volume = value;
    }
}
