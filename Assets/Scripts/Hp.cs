using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Hp : MonoBehaviour
{
    [SerializeField] private float curHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject hpScripts;

    
    [SerializeField] private TextMeshProUGUI hpText; 

    public Slider HpBarSlider;

    private void Start()
    {
        

       
        if (hpText == null)
        {
            Transform textTransform = transform.Find("StaminaText");
            if (textTransform != null)
            {
                hpText = textTransform.GetComponent<TextMeshProUGUI>();
                
            }
        }

        CheckHp();
    }

    private void SetHp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
        CheckHp(); 
    }

    public void CheckHp()
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;

       
        if (hpText != null)
            hpText.text = $"{curHealth}/{maxHealth}";
    }

    

    public void Damage(float damage)
    {
        if (maxHealth == 0 || curHealth <= 0)
            return;

        curHealth -= damage;
        CheckHp(); 

        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (curHealth < 0) 
            {
                Debug.Log("사망");
                curHealth = 0;
            }
            else if (curHealth > 0)
            {
                curHealth -= 5;
                Debug.Log($"좀비 체력 : {curHealth}");
            }
        }
    }
}