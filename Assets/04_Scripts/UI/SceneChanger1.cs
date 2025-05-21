using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger1 : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    private void Start()
    {

        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {

        progressBar.value = 0f;


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EndingScene");
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