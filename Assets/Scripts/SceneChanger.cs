using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Slider progressBar;
    public Text progressText; 

    void Start()
    {
        
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        
        progressBar.value = 0f;

       
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene1");
        asyncLoad.allowSceneActivation = false; 

        while (asyncLoad.progress < 0.9f) 
        {
           
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.value = progress;

            
            if (progressText != null)
            {
                progressText.text = $"로딩 중... {Mathf.Floor(progress * 100)}%";
            }

            yield return null;
        }

        
        float fakeProgress = progressBar.value;
        while (fakeProgress < 1.0f)
        {
            fakeProgress += 0.01f;
            progressBar.value = fakeProgress;

            if (progressText != null)
            {
                progressText.text = $"로딩 중... {Mathf.Floor(fakeProgress * 100)}%";
            }

            yield return new WaitForSeconds(0.05f);
        }

        
        progressBar.value = 1.0f;
        if (progressText != null)
        {
            progressText.text = "로딩 완료!";
        }

        
        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
    }
}