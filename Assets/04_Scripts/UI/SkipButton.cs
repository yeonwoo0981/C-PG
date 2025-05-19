using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    public Button skipButton;

    public void Start()
    {
        skipButton.onClick.AddListener(SkipFB);
    }

    public void SkipFB()
    {
        SceneManager.LoadScene(3);
    }
}
