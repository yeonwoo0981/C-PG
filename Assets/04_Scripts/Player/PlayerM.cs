using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerM : MonoBehaviour
{
    [SerializeField] private float jumpForce = 100f;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject Sword;
    public float currentTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.playerController = this;


            hp = (int)PlayerManager.Instance.currentHp;
            damage = PlayerManager.Instance.damage;
        }
        Sword.SetActive(false);
    }




    private void Update()
    {
        float moveSpeed = PlayerManager.Instance != null ? PlayerManager.Instance.moveSpeed : 5f;
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Sword.SetActive(true);
            StartCoroutine(SwordTrue());
        }
    }

    private IEnumerator SwordTrue()
    {
        yield return new WaitForSeconds(1f);
        Sword.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.isGrounded = true;
                PlayerManager.Instance.jumpCount = 1;
            }

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
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveDir.x < 0)
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

        int maxJumpCount = PlayerManager.Instance != null ? PlayerManager.Instance.jumpCount : 1;
        float jumpPower = PlayerManager.Instance != null ? PlayerManager.Instance.jumpForce : 3f;

        if (jumpCount < 2)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);


            ++jumpCount;
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.jumpCount = jumpCount;
            }
        }
    }


    public bool isGround
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.isGrounded : false; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.isGrounded = value; }
    }

    public int jumpCount
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.jumpCount : 1; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.jumpCount = value; }
    }

    public int hp
    {
        get { return PlayerManager.Instance != null ? (int)PlayerManager.Instance.currentHp : 100; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.currentHp = value; }
    }

    public int damage
    {
        get { return PlayerManager.Instance != null ? PlayerManager.Instance.damage : 5; }
        set { if (PlayerManager.Instance != null) PlayerManager.Instance.damage = value; }
    }
}
