using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private Vector2 vec;
    private Animator ani;
    private GameObject player;

    private bool isattack = true;
    private float speed = 4f;
    private float dashdir;
    private float dashpower;
    private int whatattack;

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
        WhatAttack();
        locate();
        AnimationRun();
        if (player.transform.position.x - transform.position.x == 2)
        {
            AttackStop();
        }
    }

    public void WhatAttack()
    {
        if (isattack == true)
        {
            LayerMask attack = LayerMask.GetMask("PlayerLevelUp");
            RaycastHit2D attackhit = Physics2D.Raycast(transform.position, vec.normalized, 5f, attack);
            Debug.DrawRay(transform.position, vec.normalized, Color.red, 5f);
            if (attackhit.collider != null)
            {
                Attack();
            }
            else
            {
                AttackMove();
            }
        }
    }
    public void Attack()
    {
        whatattack = Random.Range(1, 3);
        switch (whatattack)
        {
            case 1:
                StartCoroutine(NomalAttack());
                break;
                //case 2:
                //    StartCoroutine(HipAttack());
                //    break;
                //case 3:
                //    StartCoroutine(DashAttack());
                //    break;
        }
    }

    private IEnumerator NomalAttack()
    {
        AttackStop();
        ani.SetBool("attack", true);
        yield return new WaitForSeconds(0.6f);
        ani.SetBool("attack", false);
        StartCoroutine(Cooltime());
    }

    private IEnumerator HipAttack()
    {
        AttackStop();
        ani.SetBool("attack", true);
        yield return new WaitForSeconds(2.5f);
        AttackMove();
        ani.SetBool("attack", false);
        StartCoroutine(Cooltime());
    }

    private IEnumerator DashAttack()
    {
        AttackStop();
        rigid.linearVelocity = new Vector2(dashdir, 0) * dashpower;
        yield return new WaitForSeconds(2.5f);
        AttackMove();
        StartCoroutine(Cooltime());
    }

    public IEnumerator Cooltime()
    {
        isattack = false;
        yield return new WaitForSeconds(3f);
        isattack = true;
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
        else if (isattack == false)
        {
            ani.SetBool("run", false);
        }
    }
}
