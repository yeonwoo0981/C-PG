using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStagePortal : MonoBehaviour
{
    [SerializeField] private GameObject noOpenUI;
    [SerializeField] private GameObject panelUI;
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private float delayBetweenTransitions = 5f;
    [SerializeField] private TextMeshProUGUI sceneNameText;
    [SerializeField] private float sceneNameDisplayTime = 3f;

    private bool portalActive = false;
    private RectTransform panelRectTransform;
    private float screenWidth;
    private bool isTransitioning = false;

    private static NextStagePortal instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (transitionPanel == null)
        {
            return;
        }

        panelRectTransform = transitionPanel.GetComponent<RectTransform>();

        screenWidth = Screen.width;
        ResetPanelPosition(true);
        transitionPanel.SetActive(false);

        if (instance == null)
        {
            instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PlayEntranceAnimation());
        FindUIElements();
    }

    private void FindUIElements()
    {
        GameObject noOpenUIObj = GameObject.FindGameObjectWithTag("noOpenUI");
        if (noOpenUIObj != null)
            noOpenUI = noOpenUIObj;

        GameObject panelUIObj = GameObject.FindGameObjectWithTag("PanelUI");
        if (panelUIObj != null)
            panelUI = panelUIObj;

        if (sceneNameText == null)
        {
            GameObject sceneNameTextObj = GameObject.FindGameObjectWithTag("SceneNameText");
            if (sceneNameTextObj != null)
                sceneNameText = sceneNameTextObj.GetComponent<TextMeshProUGUI>();
        }

        if (noOpenUI != null)
            noOpenUI.SetActive(false);
        if (panelUI != null)
            panelUI.SetActive(false);
        if (sceneNameText != null)
            sceneNameText.gameObject.SetActive(false);

        InvokeRepeating("CheckEnemy", 1f, 1f);
    }

    private void Start()
    {
        if (noOpenUI != null)
            noOpenUI.SetActive(false);
        if (panelUI != null)
            panelUI.SetActive(false);
        if (sceneNameText != null)
            sceneNameText.gameObject.SetActive(false);

        InvokeRepeating("CheckEnemy", 1f, 1f);

        if (!isTransitioning && SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(PlayEntranceAnimation());
        }
    }

    private IEnumerator PlayEntranceAnimation()
    {
        isTransitioning = true;

        transitionPanel.SetActive(true);
        ResetPanelPosition(false);

        yield return new WaitForSeconds(0.2f);

        float startX = 0;
        float targetX = screenWidth * 2;
        float animationDuration = 1.0f;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            Vector2 newPos = panelRectTransform.anchoredPosition;
            newPos.x = Mathf.Lerp(startX, targetX, t);
            panelRectTransform.anchoredPosition = newPos;

            yield return null;
        }

        transitionPanel.SetActive(false);
        isTransitioning = false;

        StartCoroutine(DisplaySceneName());
    }

    private IEnumerator DisplaySceneName()
    {
        if (sceneNameText != null)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            sceneNameText.text = currentScene.name;
            sceneNameText.gameObject.SetActive(true);

            yield return new WaitForSeconds(sceneNameDisplayTime);

            sceneNameText.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayExitAnimation()
    {
        if (isTransitioning) yield break;

        isTransitioning = true;

        transitionPanel.SetActive(true);
        ResetPanelPosition(true);

        float startX = screenWidth;
        float targetX = -screenWidth * 0.5f;
        float animationDuration = 1.0f;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            Vector2 newPos = panelRectTransform.anchoredPosition;
            newPos.x = Mathf.Lerp(startX, targetX, t);
            panelRectTransform.anchoredPosition = newPos;

            yield return null;
        }

        Vector2 finalPos = panelRectTransform.anchoredPosition;
        finalPos.x = targetX;
        panelRectTransform.anchoredPosition = finalPos;

        yield return new WaitForSeconds(delayBetweenTransitions);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void ResetPanelPosition(bool isOffScreen)
    {
        if (panelRectTransform != null)
        {
            Vector2 newPos = panelRectTransform.anchoredPosition;
            newPos.x = isOffScreen ? screenWidth : 0;
            panelRectTransform.anchoredPosition = newPos;
        }
    }

    private IEnumerator HideNoOpenUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (noOpenUI != null)
            noOpenUI.SetActive(false);
    }

    private void CheckEnemy()
    {
        GameObject[] enemyes = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyes.Length == 0)
        {
            portalActive = true;
            if (noOpenUI != null)
                noOpenUI.SetActive(false);
            if (panelUI != null)
                panelUI.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (portalActive && !isTransitioning)
            {
                StartCoroutine(PlayExitAnimation());
                if (panelUI != null)
                    panelUI.SetActive(false);
                if (noOpenUI != null)
                    noOpenUI.SetActive(false);
            }
            else if (!portalActive)
            {
                if (noOpenUI != null)
                {
                    noOpenUI.SetActive(true);
                    StartCoroutine(HideNoOpenUIAfterDelay(3f));
                }
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}