using System.Collections;
using UnityEngine;
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
    public GameObject blackBackground;

    [Header("Settings")]
    public float textDisplayTime = 5f;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;
    public float finalPauseTime = 2f;
    

    [Header("Audio")]
    public AudioSource audioSource;
    

    private string[] flashbackTexts = 
    {
        "�� �̸��� ����.",
        "���� ��� ���� ����ο�ÿ� ��� �� ���� ���̷����� ������ �Ǿ���.",
        "���ݱ��� ���� �� ���� �־����� �ķ��� �������� ���� ���� ����ο�ø� Ż���ؾ� �ϴ� ��Ȳ�̴�.",
        "����ο�ø� Ż���ϱ� ���� �����ϴ� ���� �ױ��̴�.",
        "����ο�ô� �ٴٷ� �ѷ��ο� �ִ� ���̾ �ٱ��� �����ϱ� ���� �ױ��� �� �ϳ� �����Ѵ�.",
        "���� ���� ���ɰ��� �������� �� �ױ��� ���Ϸ� �Ѵ�.",
        "������ ���� ���̸� �հ�..."
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
        
        flashbackCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        flashbackCanvas.sortingOrder = 1000; 

        canvasGroup = flashbackCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = flashbackCanvas.gameObject.AddComponent<CanvasGroup>();

        speechBubbleGroup = speechBubble.GetComponent<CanvasGroup>();
        if (speechBubbleGroup == null)
            speechBubbleGroup = speechBubble.AddComponent<CanvasGroup>();

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
            speakerNameText.text = "����";

       
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

  

    void TransitionToNextScene()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        
        yield return StartCoroutine(FadeCanvasGroup(speechBubbleGroup, 1f, 0f, 0.5f));

        
        if (progressDots != null)
        {
            foreach (GameObject dot in progressDots)
            {
                CanvasGroup dotGroup = dot.GetComponent<CanvasGroup>();
                if (dotGroup != null)
                {
                    StartCoroutine(FadeCanvasGroup(dotGroup, dotGroup.alpha, 0f, 0.5f));
                }
            }
        }

        
       

       
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