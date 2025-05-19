using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private float gameOverDelay = 2f;

    private EnemyDamage _enemyDamage;
    private bool isDead = false;

    private void Awake()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.hpController = this;
        }


        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (staticEffectCanvasGroup != null)
            staticEffectCanvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            _curHealth = PlayerManager.Instance.currentHp;
            _maxHealth = PlayerManager.Instance.maxHp;
        }
        UpdateHpText();
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


        if (_curHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        Debug.Log("플레이어 사망!");


        Time.timeScale = 0.5f;


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


        Time.timeScale = 1f;
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
}