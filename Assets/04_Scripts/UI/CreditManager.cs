using UnityEngine;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    public GameObject panelUI;
    public Button optionButton;
    public Button closeButton;
    public Button exitButton;
    void Start()
    {
        panelUI.SetActive(false);
        optionButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);
        exitButton.onClick.AddListener(ExitPanel);
    }

    void OpenPanel()
    {
        panelUI.SetActive(true);
    }

    void ClosePanel()
    {
        panelUI.SetActive(false);
    }

    public void ExitPanel()
    {
        Application.Quit();
    }
}
