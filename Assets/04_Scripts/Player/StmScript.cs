using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class StmScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stmText;
    public Dash qustn;
    public Slider stmBarSlider;

    private void Awake()
    {
       
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.staminaController = this;
        }
    }

    private void Start()
    {
        
        if (qustn == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                qustn = player.GetComponent<Dash>();
            }
        }

        
        if (PlayerManager.Instance != null)
        {
            _stm = PlayerManager.Instance.currentStamina;
            _maxstm = PlayerManager.Instance.maxStamina;
        }
        else
        {
            _stm = 100f;
            _maxstm = 100f;

            if (qustn != null)
            {
                qustn.stm = 100f;
                qustn.maxStm = 100f;
            }
        }

        if (stmText == null)
        {
            Transform textTransform = transform.Find("stmText");
            if (textTransform != null)
            {
                stmText = textTransform.GetComponent<TextMeshProUGUI>();
            }
        }

        CheckStm();
    }

    public void CheckStm()
    {
        if (stmBarSlider != null)
            stmBarSlider.value = _stm / _maxstm;

        if (stmText != null)
            stmText.text = $"{_stm:F1}/{_maxstm}";
    }

    
    public void stm_gaugeMin()
    {
        float requiredStamina = 30f;

        if (_stm <= requiredStamina)
        {
            
            if (qustn != null)
                qustn.power = 0;

            Debug.Log("스태미나 부족");
        }
        else
        {
            
            if (qustn != null)
                qustn.power = 12;

            
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.UseStamina(requiredStamina);
                _stm = PlayerManager.Instance.currentStamina; 
            }
            else
            {
                _stm -= requiredStamina;
                _stm = Mathf.Clamp(_stm, 0, _maxstm);
            }
        }

        CheckStm();
    }

   
    public void stm_gaugePlus()
    {
        float recoveryRate = 5f * Time.deltaTime;

        
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.RecoverStamina(recoveryRate);
            _stm = PlayerManager.Instance.currentStamina; 
        }
        else
        {
            _stm += recoveryRate;
            _stm = Mathf.Clamp(_stm, 0, _maxstm);
        }

        CheckStm();
    }

    
    public float _stm
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.currentStamina : 100f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.currentStamina = value; }
    }

    public float _maxstm
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.maxStamina : 100f; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.maxStamina = value; }
    }
}