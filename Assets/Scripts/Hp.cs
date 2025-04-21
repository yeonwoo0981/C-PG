using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] private float curHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject hpScripts;
    
    private void Start()
    {
        Debug.Log($"Ω√¿€ Ω√ HP: {curHealth}/{maxHealth}");
        CheckHp();
    }
    private void SetHp(float amount) 
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }
    public Slider HpBarSlider;
     
    public void CheckHp() 
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;

        
       
    }

    private void Update()
    {
        
       
    }

    public void Damage(float damage) 
    {
        if (maxHealth == 0 || curHealth <= 0) 
            return;
        curHealth -= damage;
         
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
