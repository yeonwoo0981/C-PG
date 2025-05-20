using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LodingText;

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        float displayedProgress = 0f;

        LodingText.text = "0%";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        asyncLoad.allowSceneActivation = false;

        
        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f) * 70f;

            
            while (displayedProgress < targetProgress)
            {
                displayedProgress += 0.5f;
                LodingText.text = Mathf.RoundToInt(displayedProgress) + "%";
                yield return new WaitForSeconds(0.2f);
            }

            yield return null;
        }

        
        while (displayedProgress < 100f)
        {
            displayedProgress += 0.5f;
            LodingText.text = Mathf.RoundToInt(displayedProgress) + "%";
            yield return new WaitForSeconds(0.02f);
        }

        
        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
    }
}