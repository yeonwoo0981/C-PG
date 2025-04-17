using Microsoft.Win32.SafeHandles;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class Dash : MonoBehaviour
{
    private float dashdir;
    private Rigidbody2D rb;
    public float power = 12;
    public float DashTime = 0.4f;
    private bool isCanUseDash = true;
    public float DashcoolTime = 1.5f;
    private float currentTime = 0;
    public float y = 0.1f;
    public float stm = 100;
    public float c_TimeMax = 10f;
    public float maxStm = 100f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, c_TimeMax); // 여기서 클램프 적용

        if (stm>=0)
        {
            if (Keyboard.current.shiftKey.wasPressedThisFrame && isCanUseDash)
            {
                if (DashcoolTime > currentTime)
                {
                    Debug.Log("기다리기");
                    return;
                }
                else if (DashcoolTime < currentTime)
                {
                    isCanUseDash = false;
                    rb.linearVelocity = new Vector2(dashdir, y) * power;
                    StartCoroutine(EndVelocity());
                    currentTime = 0;
                }
            }
        }    
        stm_gaugePlus();

        if (stm <= 0)
        {
            power = 0;
        }
        else
        {
            power = 12;
        }
    }

    public void OnMove(InputValue value)
    {
        dashdir = value.Get<Vector2>().x;
    }

    private IEnumerator EndVelocity()
    {
        yield return new WaitForSeconds(DashTime);
        rb.linearVelocity = Vector2.zero;
        isCanUseDash = true;
        stm_gaugeMin();
    }
    public void stm_gaugeMin()
    {
        stm -= 20f; // 고정으로 20 감소
        stm = Mathf.Clamp(stm, -20, maxStm); // 최소 0까지 제한
        Debug.Log("Stamina: " + stm);
    }
    public void stm_gaugePlus()
    {
        stm += 4* Time.deltaTime; // currentTime 대신 Time.deltaTime 사용
        stm = Mathf.Clamp(stm, -20, maxStm);
        Debug.Log(stm);
    }
}
/*private void Update()
{
    currentTime += Time.deltaTime;
    currentTime = Mathf.Clamp(currentTime, 0f, c_TimeMax); // 여기서 클램프 적용

    if (currentTime >= c_TimeMax)
    {
        if (Keyboard.current.shiftKey.wasPressedThisFrame && isCanUseDash)
        {
            if (DashcoolTime > currentTime)
            {
                Debug.Log("기다리기");
                return;
            }
            else if (DashcoolTime < currentTime)
            {
                isCanUseDash = false;
                rb.linearVelocity = new Vector2(dashdir, y) * power;
                StartCoroutine(EndVelocity());
                currentTime = 0;
            }
        }
    }

    stm_gaugePlus();

    if (stm <= 0)
    {
        power = 0;
    }
}*/