using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Ending : MonoBehaviour
{
    [Header("UI Components")]
    public Canvas endingCanvas;
    public GameObject speechBubble;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject[] progressDots;
    public GameObject blackBackground;
    public GameObject creditsButton;

    [Header("Settings")]
    public float textDisplayTime = 7f;
    public float fadeInTime = 0.8f;
    public float fadeOutTime = 0.8f;
    public float finalPauseTime = 3.5f;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Visual Effects")]
    public Image backgroundImage;        
    public Image characterImage;         
    public GameObject[] visualEffects;   

    private string[] endingTexts =
    {
        "그렇게 나는 배를 타고 벨모로우시를 탈출하게 되었다.",
        "항구에 도착했을 때, 운 좋게 작은 배 하나를 발견하여 운전하였다.",
        "배를 운전할 수록, 벨모로우시가 점점 작아지는 것을 보며 이 모든 것이 꿈만 같았다.",
        "도시를 떠나는 것은 분명 슬픈 일이었지만, 살아남았다는 사실만으로도 감사했다.",
        "하지만 좀비 바이러스가 벨모로우시에만 국한된 것인지, 아니면 세계적인 재앙인지는 아직 알 수 없었다.",
        "새로운 세상이 어떤 모습일지 두렵지만, 지금까지 살아남았듯이 앞으로도 살아남을 것이다."
    };

    private int currentTextIndex = 0;
    private bool isTransitioning = false;
    private CanvasGroup canvasGroup;
    private CanvasGroup speechBubbleGroup;
    private CanvasGroup characterGroup;
    private CanvasGroup backgroundGroup;

    void Start()
    {
        InitializeComponents();
        StartEndingSequence();
    }

    void InitializeComponents()
    {
        endingCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        endingCanvas.sortingOrder = 1000;

        canvasGroup = endingCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = endingCanvas.gameObject.AddComponent<CanvasGroup>();

        speechBubbleGroup = speechBubble.GetComponent<CanvasGroup>();
        if (speechBubbleGroup == null)
            speechBubbleGroup = speechBubble.AddComponent<CanvasGroup>();

        if (characterImage != null)
        {
            characterGroup = characterImage.GetComponent<CanvasGroup>();
            if (characterGroup == null)
                characterGroup = characterImage.gameObject.AddComponent<CanvasGroup>();
        }

        if (backgroundImage != null)
        {
            backgroundGroup = backgroundImage.GetComponent<CanvasGroup>();
            if (backgroundGroup == null)
                backgroundGroup = backgroundImage.gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1f;
        speechBubbleGroup.alpha = 0f;

        if (blackBackground != null)
        {
            blackBackground.SetActive(true);
            CanvasGroup bgGroup = blackBackground.GetComponent<CanvasGroup>();
            if (bgGroup == null)
                bgGroup = blackBackground.AddComponent<CanvasGroup>();
            bgGroup.alpha = 1f;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.enabled = false;
        }

        if (speakerNameText != null)
            speakerNameText.text = "서준";

        UpdateProgressDots();

        
        if (creditsButton != null)
            creditsButton.SetActive(false);
    }

    void StartEndingSequence()
    {
        StartCoroutine(EndingCoroutine());
    }

    IEnumerator EndingCoroutine()
    {
        
        if (backgroundImage != null && backgroundGroup != null)
            yield return StartCoroutine(FadeCanvasGroup(backgroundGroup, 0f, 1f, fadeInTime * 1.5f));

        if (characterImage != null && characterGroup != null)
            yield return StartCoroutine(FadeCanvasGroup(characterGroup, 0f, 1f, fadeInTime));

        
        ActivateVisualEffects(true);

        
        for (currentTextIndex = 0; currentTextIndex < endingTexts.Length; currentTextIndex++)
        {
            if (dialogueText != null)
                dialogueText.text = endingTexts[currentTextIndex];

            
            PlayTextAudio(currentTextIndex);

            UpdateProgressDots();

           
            yield return StartCoroutine(FadeCanvasGroup(speechBubbleGroup, 0f, 1f, fadeInTime));

           
            yield return new WaitForSeconds(textDisplayTime - fadeInTime - fadeOutTime);

            
            yield return StartCoroutine(FadeCanvasGroup(speechBubbleGroup, 1f, 0f, fadeOutTime));

            
            if (currentTextIndex < endingTexts.Length - 1)
                yield return new WaitForSeconds(0.5f);

            
            if (currentTextIndex >= endingTexts.Length - 3 && backgroundImage != null)
            {
                
                Color currentColor = backgroundImage.color;
                float targetBrightness = 1.0f + (endingTexts.Length - currentTextIndex) * 0.1f;
                backgroundImage.color = new Color(
                    Mathf.Min(currentColor.r * targetBrightness, 1f),
                    Mathf.Min(currentColor.g * targetBrightness, 1f),
                    Mathf.Min(currentColor.b * targetBrightness, 1f),
                    currentColor.a
                );
            }
        }

        
        yield return new WaitForSeconds(finalPauseTime);

        
        if (creditsButton != null)
        {
            creditsButton.SetActive(true);
            CanvasGroup creditsGroup = creditsButton.GetComponent<CanvasGroup>();
            if (creditsGroup == null)
                creditsGroup = creditsButton.AddComponent<CanvasGroup>();

            yield return StartCoroutine(FadeCanvasGroup(creditsGroup, 0f, 1f, fadeInTime));
        }

        
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        group.alpha = endAlpha;
    }

    void UpdateProgressDots()
    {
        if (progressDots == null) return;

        for (int i = 0; i < progressDots.Length; i++)
        {
            if (i < endingTexts.Length)
            {
                progressDots[i].SetActive(true);

                CanvasGroup dotGroup = progressDots[i].GetComponent<CanvasGroup>();
                if (dotGroup == null)
                    dotGroup = progressDots[i].AddComponent<CanvasGroup>();

                dotGroup.alpha = i <= currentTextIndex ? 0.8f : 0.4f;
            }
            else
            {
                progressDots[i].SetActive(false);
            }
        }
    }

    void PlayTextAudio(int textIndex)
    {
        
        if (audioSource == null || !audioSource.isActiveAndEnabled) return;

    }

    void ActivateVisualEffects(bool activate)
    {
        if (visualEffects == null) return;

        foreach (GameObject effect in visualEffects)
        {
            if (effect != null)
                effect.SetActive(activate);
        }
    }

    public void ShowCredits()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionToCredits());
        }
    }

    IEnumerator TransitionToCredits()
    {
        
        if (speechBubbleGroup != null)
            StartCoroutine(FadeCanvasGroup(speechBubbleGroup, speechBubbleGroup.alpha, 0f, fadeOutTime));

        if (characterGroup != null)
            StartCoroutine(FadeCanvasGroup(characterGroup, characterGroup.alpha, 0f, fadeOutTime));

        if (backgroundGroup != null)
            StartCoroutine(FadeCanvasGroup(backgroundGroup, backgroundGroup.alpha, 0f, fadeOutTime));

        
        if (progressDots != null)
        {
            foreach (GameObject dot in progressDots)
            {
                CanvasGroup dotGroup = dot.GetComponent<CanvasGroup>();
                if (dotGroup != null)
                {
                    StartCoroutine(FadeCanvasGroup(dotGroup, dotGroup.alpha, 0f, fadeOutTime));
                }
            }
        }

       
        yield return new WaitForSeconds(fadeOutTime + 0.5f);

       
        SceneManager.LoadScene(0); 
    }

    public void SkipEnding()
    {
        if (!isTransitioning)
        {
            StopAllCoroutines();
            ShowCredits();
        }
    }

    void OnDestroy()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
        }
    }

    void OnDisable()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
        }
    }
}
