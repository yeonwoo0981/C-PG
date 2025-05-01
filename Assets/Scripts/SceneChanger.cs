using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    void Start()
    {
        
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        
        progressBar.value = 0f;

       
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene1");
        asyncLoad.allowSceneActivation = false; 

        while (asyncLoad.progress < 0.9f) 
        {
           
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.value = progress;

            yield return null;
        }

        
        float fakeProgress = progressBar.value;
        while (fakeProgress < 1.0f)
        {
            fakeProgress += 0.01f;
            progressBar.value = fakeProgress;

            yield return new WaitForSeconds(0.1f);
        }

        
        progressBar.value = 1.0f;

        
        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
    }
}