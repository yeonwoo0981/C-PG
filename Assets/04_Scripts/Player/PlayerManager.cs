using UnityEngine;
using System.Collections.Generic;
public class PlayerManager : MonoBehaviour
{
    
    public static PlayerManager Instance { get; private set; }

    
    [Header("Player Stats")]
    public float maxHp = 100f;
    public float currentHp = 100f;
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public int damage = 5;

    
    [Header("Movement Stats")]
    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    public float dashPower = 12f;
    public float dashTime = 0.4f;
    public float dashCoolTime = 1.5f;

    
    [Header("Game Progress")]
    public int level = 1;
    public int exp = 0;
    public int gold = 0;

    
    [Header("Player State")]
    public bool isGrounded = false;
    public int jumpCount = 0;
    public bool canDash = true;

    
    [HideInInspector] public PlayerM playerController;
    [HideInInspector] public Dash dashController;
    [HideInInspector] public Hp hpController;
    [HideInInspector] public StmScript staminaController;

    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerManager ½Ì±ÛÅæ »ý¼ºµÊ");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Áßº¹µÈ PlayerManager Á¦°ÅµÊ");
            return;
        }

        
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerM>();
            dashController = player.GetComponent<Dash>();
            hpController = player.GetComponent<Hp>();

            
            //GameObject staminaUI = GameObject.FindGameObjectWithTag("StaminaUI");
            /*if (staminaUI != null)
            {
                staminaController = staminaUI.GetComponent<StmScript>();
            }*/
        }
    }

   
    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

        
        if (hpController != null)
        {
            hpController._curHealth = currentHp;
            hpController.UpdateHpText();
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

        
        if (hpController != null)
        {
            hpController._curHealth = currentHp;
            hpController.UpdateHpText();
        }
    }

    
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

         
        if (staminaController != null)
        {
            staminaController._stm = currentStamina;
            staminaController.CheckStm();
        }
    }

    public void RecoverStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        
        if (staminaController != null)
        {
            staminaController._stm = currentStamina;
            staminaController.CheckStm();
        }
    }

   

   

   

    
    private void Die()
    {
        Debug.Log("ÇÃ·¹ÀÌ¾î »ç¸Á!");
        
    }

    
    
}