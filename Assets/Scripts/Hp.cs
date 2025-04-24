using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Hp : MonoBehaviour
{
    [SerializeField] private float _curHealth = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private GameObject hpScripts;

    
    [SerializeField] private TextMeshProUGUI hpText; 

    public Slider HpBarSlider;

    private void Start()
    {
        

       
        if (hpText == null)
        {
            Transform textTransform = transform.Find("hpText");
            if (textTransform != null)
            {
                hpText = textTransform.GetComponent<TextMeshProUGUI>();
                
            }
        }

        CheckHp();
    }

    private void SetHp(float amount)
    {
        _maxHealth = amount;
        _curHealth = _maxHealth;
        CheckHp(); 
    }

    public void CheckHp()
    {
        if (HpBarSlider != null)
            HpBarSlider.value = _curHealth / _maxHealth;

       
        if (hpText != null)
            hpText.text = $"{_curHealth}/{_maxHealth}";
    }

    

    public void Damage(float damage)
    {
        if (_maxHealth == 0 || _curHealth <= 0)
            return;

        _curHealth -= damage;
        CheckHp(); 

        if (_curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            _curHealth -= 5;
            Debug.Log(_curHealth);
            CheckHp();
        }
    }
}