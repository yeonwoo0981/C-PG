using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Dash : MonoBehaviour
{
    private float dashdir;
    private Rigidbody2D rb;
    public float power = 12;
    public float DashTime = 0.4f;
    private bool isCanUseDash = true;
    public float DashcoolTime = 1.5f;
    private float currentTime = 0;

    private Animator ani;

    public float stm = 100f;
    public float c_TimeMax = 10f;
    public float maxStm = 100f;
    public StmScript _stmscript;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, c_TimeMax);
        if (Keyboard.current.shiftKey.wasPressedThisFrame && isCanUseDash)
        {
            if (DashcoolTime > currentTime)
            {
                Debug.Log("��ٸ���");
                return;
            }
            else if (DashcoolTime < currentTime)
            {
                isCanUseDash = false;
                ani.SetBool("isDashng", true);
                rb.linearVelocity = new Vector2(dashdir, 0) * power;
                StartCoroutine(EndVelocity());
                currentTime = 0;
            }
        }
        _stmscript.stm_gaugePlus();


    }
    public void OnMove(InputValue value)
    {
        dashdir = value.Get<Vector2>().x;
    }

    private IEnumerator EndVelocity()
    {
        yield return new WaitForSeconds(DashTime);
        rb.linearVelocity = Vector2.zero;
        ani.SetBool("isDashng", false);
        isCanUseDash = true;
        _stmscript.stm_gaugeMin();
    }
}
