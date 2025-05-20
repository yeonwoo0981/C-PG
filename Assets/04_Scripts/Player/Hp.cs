using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Hp : MonoBehaviour
{
    [Header("HP UI Elements")]
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] public Slider HpBarSlider;

    [Header("Game Over Effects")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private CanvasGroup staticEffectCanvasGroup;
    [SerializeField] private Image staticEffectImage;
    [SerializeField] private AudioSource staticAudioSource;
    [SerializeField] private StaticEffect staticEffectScript;
    [SerializeField] private float gameOverDelay = 2f;

    [Header("Game Over Buttons")]
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button titleBtn;

    private EnemyDamage _enemyDamage;
    private bool isDead = false;

    [System.Obsolete]
    private void Awake()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.hpController = this;
        }


        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
            Debug.Log("GameOver UI 초기화 완료");
        }
        else
        {
            Debug.LogWarning("GameOver UI가 할당되지 않았습니다!");
        }


        if (staticEffectCanvasGroup != null)
        {
            staticEffectCanvasGroup.alpha = 0f;
        }


        if (staticEffectScript == null)
        {
            staticEffectScript = FindObjectOfType<StaticEffect>();
        }
    }

    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            _curHealth = PlayerManager.Instance.currentHp;
            _maxHealth = PlayerManager.Instance.maxHp;
        }
        UpdateHpText();


        if (restartBtn != null)
        {
            restartBtn.onClick.AddListener(RestartGame);
        }

        if (titleBtn != null)
        {
            titleBtn.onClick.AddListener(GoToTitle);
        }
    }

    public void UpdateHpText()
    {
        if (HpBarSlider != null)
            HpBarSlider.value = _curHealth / _maxHealth;
        if (_hpText != null)
            _hpText.text = $"{_curHealth:F0}/{_maxHealth:F0}";
    }

    public void Damage()
    {
        if (isDead || _curHealth <= 0)
            return;

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.TakeDamage(5f);
            _curHealth = PlayerManager.Instance.currentHp;
        }
        else
        {
            _curHealth -= 5f;
            if (_curHealth < 0) _curHealth = 0;
        }

        Debug.Log($"플레이어 체력: {_curHealth}");
        UpdateHpText();

        // 체력이 0 이하가 되었을 때
        if (_curHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {


        Time.timeScale = 0.5f;


        if (staticEffectScript != null)
        {
            staticEffectScript.StartStaticEffect();
        }

        if (staticEffectCanvasGroup != null && staticEffectImage != null)
        {
            StartCoroutine(StaticEffect());
        }


        if (staticAudioSource != null)
        {
            staticAudioSource.Play();
        }


        yield return new WaitForSecondsRealtime(gameOverDelay);


        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            StartCoroutine(FadeInGameOverUI());
        }


    }

    private IEnumerator StaticEffect()
    {
        float duration = gameOverDelay;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;


            float intensity = Mathf.Lerp(0f, 1f, timer / duration);


            if (staticEffectImage != null)
            {
                Color color = staticEffectImage.color;
                color.a = Random.Range(0f, intensity);
                staticEffectImage.color = color;
            }


            if (staticEffectCanvasGroup != null)
            {
                staticEffectCanvasGroup.alpha = intensity * Random.Range(0.5f, 1f);
            }

            yield return null;
        }


        if (staticEffectCanvasGroup != null)
            staticEffectCanvasGroup.alpha = 1f;

    }

    private IEnumerator FadeInGameOverUI()
    {
        if (gameOverUI == null) yield break;

        CanvasGroup gameOverCanvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        if (gameOverCanvasGroup == null)
        {
            gameOverCanvasGroup = gameOverUI.AddComponent<CanvasGroup>();
        }

        gameOverCanvasGroup.alpha = 0f;
        float duration = 1.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;


        Time.timeScale = 0f;


        if (staticEffectScript != null)
        {
            staticEffectScript.StopStaticEffect();
        }


        if (staticEffectCanvasGroup != null)
        {
            StartCoroutine(FadeOutStaticEffect());
        }
    }

    private IEnumerator FadeOutStaticEffect()
    {
        float duration = 1f;
        float timer = 0f;
        float startAlpha = staticEffectCanvasGroup.alpha;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            staticEffectCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timer / duration);
            yield return null;
        }

        staticEffectCanvasGroup.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            Damage();
        }
    }

    public void ResetHP()
    {
        isDead = false;
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.currentHp = PlayerManager.Instance.maxHp;
            _curHealth = PlayerManager.Instance.currentHp;
        }
        else
        {
            _curHealth = _maxHealth;
        }

        UpdateHpText();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (staticEffectCanvasGroup != null)
            staticEffectCanvasGroup.alpha = 0f;

        if (staticEffectScript != null)
            staticEffectScript.StopStaticEffect();


        Time.timeScale = 1f;
    }

    public float _curHealth
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.currentHp : 100f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.currentHp = value; }
    }

    public float _maxHealth
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.maxHp : 100f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.maxHp = value; }
    }

    
    public void RestartGame()
    {

        
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.currentHp = PlayerManager.Instance.maxHp;
            Debug.Log($"PlayerManager 체력 초기화: {PlayerManager.Instance.currentHp}/{PlayerManager.Instance.maxHp}");
        }

        
        Time.timeScale = 1f;

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitle()
    {

        
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.currentHp = PlayerManager.Instance.maxHp;
        }

        
        Time.timeScale = 1f;

        
        SceneManager.LoadScene(0);
    }
}