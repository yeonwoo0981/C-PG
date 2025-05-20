using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    private float dashdir;
    private Rigidbody2D rb;
    private Animator ani;
    private float currentTime = 0;
    private bool isCanUseDash = true;

    
    public StmScript _stmscript;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.dashController = this;
        }
    }

    private void Start()
    {
        
        if (_stmscript == null)
        {
            GameObject staminaUI = GameObject.FindGameObjectWithTag("StaminaUI");
            if (staminaUI != null)
            {
                _stmscript = staminaUI.GetComponent<StmScript>();
            }
        }
    }

    private void Update()
    {
        
        float coolTime = PlayerManager.Instance != null ? PlayerManager.Instance.dashCoolTime : 1.5f;
        float c_TimeMax = 10f;

        currentTime += Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, c_TimeMax);

        
        if (Keyboard.current.shiftKey.wasPressedThisFrame && isCanUseDash)
        {
            if (coolTime > currentTime)
            {
                Debug.Log("기다리기");
                return;
            }
            else if (coolTime < currentTime)
            {
                
                float requiredStamina = 30f;
                float currentStamina = PlayerManager.Instance != null ?
                    PlayerManager.Instance.currentStamina :
                    (_stmscript != null ? _stmscript._stm : 100f);

                if (currentStamina <= requiredStamina)
                {
                    Debug.Log("스태미나 부족");
                    return;
                }

                isCanUseDash = false;
                ani.SetBool("isDashng", true);

                
                float dashPower = PlayerManager.Instance != null ? PlayerManager.Instance.dashPower : 12f;
                rb.linearVelocity = new Vector2(dashdir, 0) * dashPower;

                StartCoroutine(EndVelocity());
                currentTime = 0;
            }
        }

        
        if (_stmscript != null)
        {
            _stmscript.stm_gaugePlus();
        }
        else if (PlayerManager.Instance != null)
        {
            
            PlayerManager.Instance.RecoverStamina(5f * Time.deltaTime);
        }
    }

    public void OnMove(InputValue value)
    {
        dashdir = value.Get<Vector2>().x;
    }

    private IEnumerator EndVelocity()
    {
        
        float dashTime = PlayerManager.Instance != null ? PlayerManager.Instance.dashTime : 0.4f;
        yield return new WaitForSeconds(dashTime);

        rb.linearVelocity = Vector2.zero;
        ani.SetBool("isDashng", false);
        isCanUseDash = true;

        
        if (_stmscript != null)
        {
            _stmscript.stm_gaugeMin();
        }
        else if (PlayerManager.Instance != null)
        {
            
            PlayerManager.Instance.UseStamina(30f);
        }
    }

    
    public float power
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.dashPower : 12f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.dashPower = value; }
    }

    public float DashTime
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.dashTime : 0.4f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.dashTime = value; }
    }

    public float DashcoolTime
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.dashCoolTime : 1.5f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.dashCoolTime = value; }
    }

    public float stm
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.currentStamina : 100f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.currentStamina = value; }
    }

    public float maxStm
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.maxStamina : 100f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.maxStamina = value; }
    }
}