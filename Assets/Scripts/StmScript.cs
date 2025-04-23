using TMPro.EditorUtilities;
using UnityEngine;

public class StmScript : MonoBehaviour
{
    public Dash qustn;
    private float _stm;
    private float _maxstm;
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
    }

    public void stm_gaugeMin()
    {
        if(_stm <= 20)
        {
            qustn.power = 0;
        }
        else
        {
            _stm -= 20f;
            _stm = Mathf.Clamp(_stm, 0, _maxstm);
        }
    }
    public void stm_gaugePlus()
    {
        _stm += 4 * Time.deltaTime;
        _stm = Mathf.Clamp(_stm, 0, _maxstm);
    }
}
