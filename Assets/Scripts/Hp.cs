using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    public float curHealth = 100f;
    public float maxHealth = 100f;

    public Slider HpBarSlider;

    private void Start()
    {
        maxHealth = curHealth;
        UpdateHpText();
    }

    public void UpdateHpText()
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;

        if (hpText != null)
            hpText.text = $"{curHealth}/{maxHealth}";
    }

    public void Damage(float damage)
    {
        if (curHealth <= 0)
            return;

        curHealth -= damage;
        if (curHealth < 0) curHealth = 0;
        Debug.Log($"플레이어 체력: {curHealth}");
        UpdateHpText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Damage(5f);
        }
    }
}