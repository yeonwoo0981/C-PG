using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class StmScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stmText;
    public Dash qustn;
    private float _stm;
    private float _maxstm;
    public Slider stmBarSlider;
    void Awake()
    {
        qustn = GameObject.Find("Player").GetComponent<Dash>();
        _stm = qustn.stm;
        _maxstm = qustn.maxStm;
        qustn.GetComponent<Dash>().stm = 100f;
        qustn.GetComponent<Dash>().maxStm = 100f;

    }
    private void Start()
    {
        _stm = _maxstm;
        if (stmText == null)
        {
            Transform textTransform = transform.Find("stmText");
            if (textTransform != null)
            {
                stmText = textTransform.GetComponent<TextMeshProUGUI>();

            }
        }

        CheckHp();
    }

    private void SetHp(float amount)
    {
        _maxstm = amount;
        _stm = _maxstm;
        CheckHp();
    }

    public void CheckHp()
    {
        if (stmBarSlider != null)
            stmBarSlider.value = _stm / _maxstm;


        if (stmText != null)
            stmText.text = $"{_stm:F1}/{_maxstm}";
    }
    public void stm_gaugeMin()
    {
        if (_stm <= 20)
        {
            qustn.power = 0;
        }
        else
        {
            _stm -= 20f;
            _stm = Mathf.Clamp(_stm, 0, _maxstm);

            Debug.Log($"Stamina: {_stm}");


        }
        CheckHp();
    }
    public void stm_gaugePlus()
    {
        _stm += 4 * Time.deltaTime;
        _stm = Mathf.Clamp(_stm, 0, _maxstm);

        Debug.Log($"Stamina: {_stm}");
        CheckHp();

    }
}