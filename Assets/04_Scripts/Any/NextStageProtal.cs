using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextStageProtal : MonoBehaviour
{
    [SerializeField] private GameObject noOpenUI;
    [SerializeField] private GameObject panelUI;
    private bool portalActive = false;

    private IEnumerator HideNoOpenUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        noOpenUI.SetActive(false);
    }

    private void Start()
    {
        panelUI.SetActive(false);
        noOpenUI.SetActive(false);
        InvokeRepeating("CheckEnemy", 1f, 1f);
    }

    private void CheckEnemy()
    { 
        GameObject[] enemyes = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemyes.Length == 0)
        {
            portalActive = true;
            CancelInvoke("CheckEnemy");
            noOpenUI.SetActive(false);
            panelUI.SetActive(true);
            Debug.Log("Æ÷Å» ¿ÀÇÂ");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (portalActive)
            {
                LoadNextScene();
                Debug.Log("»êÀ¸·Î ÀÌµ¿");
                panelUI.SetActive(false);
                noOpenUI.SetActive(false);
            }
            else
            {
                noOpenUI.SetActive(true);
                StartCoroutine(HideNoOpenUIAfterDelay(3f));
                Debug.Log("Æ÷Å» ¹Ì¿ÀÇÂ");
            }
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
