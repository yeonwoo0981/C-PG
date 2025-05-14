using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageProtal : MonoBehaviour
{
    private bool portalActive = false;

    private void Start()
    {
        InvokeRepeating("CheckEnemy", 1f, 1f);
    }

    private void CheckEnemy()
    { 
        GameObject[] enemyes = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemyes.Length == 0)
        {
            Debug.Log("��Ż ����");
            portalActive = true;
            CancelInvoke("CheckEnemy");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (portalActive)
            {
                Debug.Log("������ �̵�");
                LoadNextScene();
            }
            else
            {
                Debug.Log("��Ż �̿���");
            }
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(3);
    }
}
