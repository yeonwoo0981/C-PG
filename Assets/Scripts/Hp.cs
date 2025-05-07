using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    private EnemyDamage _enemyDamage;
    public float _curHealth = 100f;
    public float _maxHealth = 100f;

    public Slider HpBarSlider;

    private void Start()
    {
        _maxHealth = _curHealth;
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

        _curHealth -= 5f;
        if (_curHealth < 0) _curHealth = 0;
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
}