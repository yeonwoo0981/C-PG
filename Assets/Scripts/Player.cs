using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 3f;
    private Vector2 moveDir;
    [SerializeField]private Rigidbody2D rb;
    public bool isGround;
    public int jumpCount = 1;
    private Animator animator;
    public int hp = 100;
    public int damage = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;
        
    }

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        Player_Ani_tra();
    }

    public void Player_Ani_tra()
    {
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            animator.SetBool("isRunning", true);
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            animator.SetBool("isRunning", true);
        }
    }


    public void OnJump()
    {
        if (jumpCount < 2)
        {
            animator.SetBool("isJumping", true);
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
            animator.SetBool("isJumping", false);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health();
        }
    }

    public void health()
    {

        if (hp == 0)
        {
            Debug.Log("ÁïÀ½");
        }
        else
        {
            hp = hp - 10;
        }
    }
}
