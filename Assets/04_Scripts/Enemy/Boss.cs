using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private Vector2 vec;
    private Animator ani;
    private GameObject player;

    private bool isattack = true;
    private float speed = 2f;
    private float dashdir;
    private float dashpower;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FixedUpdate()
    {
        dashdir = transform.position.x;
        vec = (player.transform.position - transform.position).normalized;
        locate();
        AnimationRun();
        WhatAttack();
        Attack();
    }

    private void WhatAttack()
    {
        if (isattack == true)
        {
            LayerMask attack = LayerMask.GetMask("PlayerLevelUp");
            RaycastHit2D attackhit = Physics2D.Raycast(transform.position, vec.normalized, 1.5f, attack);
            RaycastHit2D hipphit = Physics2D.Raycast(transform.position, vec.normalized, 3f, attack);
            RaycastHit2D vechit = Physics2D.Raycast(transform.position, vec.normalized, 4.5f, attack);
            Debug.DrawRay(transform.position, vec.normalized, Color.red, 1.5f);
            Debug.DrawRay(transform.position, vec.normalized, Color.yellow, 3f);
            Debug.DrawRay(transform.position, vec.normalized, Color.green, 4.5f);
            if (attackhit.collider != null)
            {
                StartCoroutine(Attack());
            }
            else if (hipphit.collider != null)
            {
                StartCoroutine(Hipp());
            }
            else if (vechit.collider != null)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(2.5f);
        rigid.linearVelocity = new Vector2(dashdir, 0) * dashpower;
        StartCoroutine(Cooltime());
    }

    public IEnumerator Hipp()
    {
        AttackStop();
        ani.SetBool("attack", true);
        yield return new WaitForSeconds(2.5f);
        AttackMove();
        ani.SetBool("attack", false);
        StartCoroutine(Cooltime());
    }

    public IEnumerator Attack()
    {
        AttackStop();
        ani.SetBool("attack", true);
        yield return new WaitForSeconds(2.5f);
        AttackMove();
        ani.SetBool("attack", false);
        StartCoroutine(Cooltime());
    }

    public IEnumerator Cooltime()
    {
        isattack = false;
        yield return new WaitForSeconds(2f);
        isattack = true;
    }

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

    void AttackStop()
    {
        speed = 0f;
        rigid.linearVelocity = Vector2.zero;
    }

    void AttackMove()
    {
        speed = 2f;
        rigid.linearVelocity = vec.normalized * speed;
    }
}
