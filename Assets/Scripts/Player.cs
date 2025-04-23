using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 3f;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    public bool isGround;
    public int jumpCount = 1;
    private Animator ani;
    public int hp = 100;
    public int damage = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;
    }

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        PlayerAnimator();
    }

    public void PlayerAnimator()
    {
        if(moveDir.x != 0)
        {
            ani.SetBool("isRunning", true);
        }
        else
        {
            ani.SetBool("isRunning", false);
        }
    }


    public void OnJump()
    {
        if (jumpCount < 2)
        {
            ani.SetBool("isJumping", true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            ++jumpCount;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 1;
            ani.SetBool("isJumping", false);
        }
    }
}
