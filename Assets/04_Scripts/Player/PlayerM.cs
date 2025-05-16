using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerM : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 3f;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    public bool isGround;
    public int jumpCount = 1;
    private Animator animator;
    public int hp = 100;
    public int damage = 5;
    public GameObject Sword;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            Debug.Log("누름");
            Sword.SetActive(true);
        } 
        else
        {
            Debug.Log("안누름");
            Sword.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 1;
            animator.SetBool("isJumping", false);
            if (moveDir.x != 0)
            {
                animator.SetBool("isRunning", true);
            }
        }
    }
    

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        PlayerAnimator();
        PlayerLocation();
    }

    public void PlayerLocation()
    {
        if(moveDir.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if(moveDir.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public void PlayerAnimator()
    {
        if (moveDir.x != 0)
        {   
            animator.SetBool("isRunning", true);
        }
        else
        {

            animator.SetBool("isRunning", false);
        }
    }


    public void OnJump()
    {
        if (jumpCount < 2)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            ++jumpCount;
        }
    }
}
