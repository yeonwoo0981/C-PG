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

    public void CheckHp()
    {
        if (stmBarSlider != null)
            stmBarSlider.value = _stm / _maxstm;


        if (stmText != null)
            stmText.text = $"{_stm:F1}/{_maxstm}";
    }
    public void stm_gaugeMin()
    {
        if (_stm <= 30)
        {
            qustn.power = 0;
        }
        else
        {
            qustn.power = 12;
            _stm -= 30f;
            _stm = Mathf.Clamp(_stm, 0, _maxstm);


        }
        CheckHp();
    }
    public void stm_gaugePlus()
    {
        _stm += 5 * Time.deltaTime;
        _stm = Mathf.Clamp(_stm, 0, _maxstm);

        CheckHp();

    }
}