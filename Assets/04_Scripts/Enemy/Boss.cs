using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private Vector2 vec;
    private Animator ani;
    private GameObject player;
    [SerializeField] private GameObject nomalattackeffect;
    [SerializeField] private GameObject hipattackeffect;
    [SerializeField] private Transform fott;

    private bool isattack = true;
    private bool isattacking = false;
    private float speed = 4f;
    private int whatattack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        nomalattackeffect.SetActive(false);
        hipattackeffect.SetActive(false);
    }

    public void FixedUpdate()
    {
        vec = (player.transform.position - transform.position).normalized;
        WhatAttack();
        locate();
        if (isattack == false)
        {
            ani.SetBool("idle", true);
            rigid.linearVelocity = Vector2.zero;
            rigid.linearDamping = 100f;
        }
        else
        {
            ani.SetBool("idle", false);
            rigid.linearDamping = 0f;
        }
    }

    public void WhatAttack()
    {
        if (isattack == true)
        {
            LayerMask attack = LayerMask.GetMask("PlayerLevelUp");
            RaycastHit2D attackhit = Physics2D.Raycast(fott.transform.position, Vector2.right * vec.x, 5f, attack);
            Debug.DrawRay(fott.transform.position, Vector2.right * vec.x, Color.red, 5f);
            if (attackhit.collider != null)
            {
                Attack();
            }
            else
            {
                speed = 2f;
                rigid.linearVelocity = vec.normalized * speed;
            }
        }
    }

    public void Attack()
    {
        if (isattacking == false)
        {
            whatattack = Random.Range(1, 3);
            switch (whatattack)
            {
                case 1:
                    StartCoroutine(NomalAttack());
                    break;
                case 2:
                    StartCoroutine(HipAttack());
                    break;
            }
        }
    }

    private IEnumerator HipAttack()
    {
        isattacking = true;
        ani.SetBool("attack2", true);
        rigid.linearVelocity = Vector2.zero;
        rigid.linearDamping = 100f;
        yield return new WaitForSeconds(1.2f);
        ani.SetBool("attack2", false);
        hipattackeffect.SetActive(true);
        rigid.linearDamping = 0f;
        StartCoroutine(Cooltime());
    }

    private IEnumerator NomalAttack()
    {
        isattacking = true;
        ani.SetBool("attack", true);
        nomalattackeffect.SetActive(true);
        rigid.linearVelocity = Vector2.zero;
        rigid.linearDamping = 100f;
        yield return new WaitForSeconds(0.6f);
        ani.SetBool("attack", false);
        rigid.linearDamping = 0f;
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        isattack = false;
        yield return new WaitForSeconds(2f);
        isattack = true;
        isattacking = false;
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
}
