using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    private EnemyDamage _enemyDamage;
    public Slider HpBarSlider;

    private void Awake()
    {
        
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.hpController = this;
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
    }

    public void UpdateHpText()
    {
        if (HpBarSlider != null)
            HpBarSlider.value = _curHealth / _maxHealth;

        if (_hpText != null)
            _hpText.text = $"{_curHealth}/{_maxHealth}";
    }

    public void Damage()
    {
        if (_curHealth <= 0)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Damage();
        }
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