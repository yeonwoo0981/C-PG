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
    private float currentTime;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
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
                rb.linearVelocity = new Vector2(dashdir, 0) * power;
                StartCoroutine(EndVelocity());
                currentTime = 0;
            }
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
    }
}