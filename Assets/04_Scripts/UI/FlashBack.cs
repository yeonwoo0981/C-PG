using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FlashBack : MonoBehaviour
{
    [Header("UI Components")]
    public Canvas flashbackCanvas;
    public GameObject speechBubble;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject[] progressDots;

    [Header("Settings")]
    public float textDisplayTime = 5f;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;
    public float finalPauseTime = 2f;
    

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] voiceClips; 

    private string[] flashbackTexts = {
        "내 이름은 서준.",
        "내가 사는 곳인 벨모로우시에 어느 날 좀비 바이러스가 퍼지게 되었다.",
        "지금까지 집에 잘 숨어 있었지만 식량이 떨어져서 집을 떠나 벨모로우시를 탈출해야 하는 상황이다.",
        "벨모로우시를 탈출하기 위해 가야하는 곳은 항구이다.",
        "벨모로우시는 바다로 둘러싸여 있는 섬이어서 바깥과 교류하기 위한 항구가 딱 하나 존재한다.",
        "지금 나는 도심가를 가로질러 그 항구로 향하려 한다.",
        "위험한 좀비떼 사이를 뚫고..."
    };

    private int currentTextIndex = 0;
    private bool isTransitioning = false;
    private CanvasGroup canvasGroup;
    private CanvasGroup speechBubbleGroup;

    void Start()
    {
        InitializeComponents();
        StartFlashbackSequence();
    }

    void InitializeComponents()
    {
        
        canvasGroup = flashbackCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = flashbackCanvas.gameObject.AddComponent<CanvasGroup>();

        speechBubbleGroup = speechBubble.GetComponent<CanvasGroup>();
        if (speechBubbleGroup == null)
            speechBubbleGroup = speechBubble.AddComponent<CanvasGroup>();

        
        canvasGroup.alpha = 1f;
        speechBubbleGroup.alpha = 0f;

        if (speakerNameText != null)
            speakerNameText.text = "서준";

        
        UpdateProgressDots();
    }

    void StartFlashbackSequence()
    {
        StartCoroutine(FlashbackCoroutine());
    }

    IEnumerator FlashbackCoroutine()
    {
        for (currentTextIndex = 0; currentTextIndex < flashbackTexts.Length; currentTextIndex++)
        {
            
            if (dialogueText != null)
                dialogueText.text = flashbackTexts[currentTextIndex];


           
            UpdateProgressDots();

           
            yield return StartCoroutine(FadeCanvasGroup(speechBubbleGroup, 0f, 1f, fadeInTime));

            yield return new WaitForSeconds(textDisplayTime - fadeInTime - fadeOutTime);

           
            yield return StartCoroutine(FadeCanvasGroup(speechBubbleGroup, 1f, 0f, fadeOutTime));

            
            if (currentTextIndex < flashbackTexts.Length - 1)
                yield return new WaitForSeconds(0.2f);
        }

       
        yield return new WaitForSeconds(finalPauseTime);

       
        TransitionToNextScene();
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
            if (i < flashbackTexts.Length)
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

    void PlayVoiceClip(int index)
    {
        if (audioSource != null && voiceClips != null && index < voiceClips.Length && voiceClips[index] != null)
        {
            audioSource.clip = voiceClips[index];
            audioSource.Play();
        }
    }

    void TransitionToNextScene()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, 1f));

        
        SceneManager.LoadScene(3);
    }

   

    public void SkipFlashback()
    {
        if (!isTransitioning)
        {
            StopAllCoroutines();
            TransitionToNextScene();
        }
    }

    
    [ContextMenu("Test Flashback")]
    void TestFlashback()
    {
        StartFlashbackSequence();
    }

    
    public void StartFlashback()
    {
        gameObject.SetActive(true);
        StartFlashbackSequence();
    }

   
}


[System.Serializable]
public class ProgressDot : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void SetActive(bool isActive)
    {
        Color color = image.color;
        color.a = isActive ? 0.8f : 0.4f;
        image.color = color;

        
        float scale = isActive ? 1f : 0.8f;
        rectTransform.localScale = Vector3.one * scale;
    }
}
