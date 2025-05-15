using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2f;
    private Vector2 vec;
    private GameObject player;

    private Animator ani;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FixedUpdate()
    {
        locate();
        AnimationRun();
        WhatAttack();
        Attack();
    }

    private void WhatAttack()
    {
        LayerMask attack = LayerMask.GetMask("PlayerLevelUp");
        RaycastHit2D attackhit = Physics2D.Raycast(transform.position, vec.normalized, 1.5f, attack);
        RaycastHit2D hipphit = Physics2D.Raycast(transform.position, vec.normalized, 3f, attack);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vec.normalized, 4.5f, attack);
        Debug.DrawRay(transform.position, vec.normalized, Color.red, 1.5f);
        Debug.DrawRay(transform.position, vec.normalized, Color.red, 3f);
        Debug.DrawRay(transform.position, vec.normalized, Color.red, 4.5f);
        if (hit.collider != null)
        {
            StartCoroutine(Attack());
        }
    }

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    void Attack()
    {
        speed = 0f;
        rigid.linearVelocity = Vector2.zero;
        ani.SetBool("attack", true);
        speed = 2f;
        rigid.linearVelocity = vec.normalized * speed;
        ani.SetBool("attack", false);
    }
}

void AnimationRun()
{
    if (vec.x > 0 || vec.x < 0 && speed != 0)
    {
        ani.SetBool("run", true);
    }
    else
    {
        ani.SetBool("run", false);
    }
}
}
