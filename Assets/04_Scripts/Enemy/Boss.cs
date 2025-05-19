using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private Vector2 vec;
    private Animator ani;
    private GameObject player;
    [SerializeField] private GameObject Nomalattackeffect;

    private bool isattack = true;
    private float speed = 4f;
    private int whatattack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Nomalattackeffect.SetActive(false);
    }

    public void FixedUpdate()
    {
        vec = (player.transform.position - transform.position).normalized;
        WhatAttack();
        locate();
        AnimationRun();
    }

    public void WhatAttack()
    {
        if (isattack == true)
        {
            LayerMask attack = LayerMask.GetMask("PlayerLevelUp");
            RaycastHit2D attackhit = Physics2D.Raycast(transform.position, Vector2.right * vec, 3f, attack);
            Debug.DrawRay(transform.position, Vector2.right * vec, Color.red, 3f);
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
        whatattack = Random.Range(1, 4);
        switch (whatattack)
        {
            case 1:
                StartCoroutine(NomalAttack());
                break;
        }
    }

    private IEnumerator NomalAttack()
    {
        isattack = false;
        AttackStop();
        ani.SetBool("attack", true);
        Nomalattackeffect.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        ani.SetBool("attack", false);
        Nomalattackeffect.SetActive(false);
        AttackMove();
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
