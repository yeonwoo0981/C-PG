using UnityEngine;


public class OptSpt : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }

    public void OpenBtn()
    {
        panel.SetActive(true);
    }

    public void CloseBtn()
    {
        panel.SetActive(false);
    }
}
