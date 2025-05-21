using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2f;
    private Vector2 vec;
    private GameObject player;
    [SerializeField] private GameObject attackprefab;
    [SerializeField] private float jujuj;
    private Animator ani;
    private bool isattack = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackprefab.SetActive(false);
    }

    public void Update()
    {
        vec = player.transform.position - transform.position;
    }
    public void FixedUpdate()
    {
        locate();
        Attack();
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

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    void Attack()
    {
        if (isattack == true)
        {
            LayerMask layer = LayerMask.GetMask("PlayerLevelUp");
            Debug.DrawRay(transform.position, Vector2.right * vec, Color.red, 1.5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * vec, 1.5f, layer);

            if (hit.collider != null)
            {
                StartCoroutine(NomalAttack());
            }
            else
            {
                speed = 2f;
                rigid.linearVelocity = vec.normalized * speed;
            }
        }
    }

    private IEnumerator NomalAttack()
    {
        ani.SetBool("attack", true);
        attackprefab.SetActive(true);
        rigid.linearVelocity = Vector2.zero;
        rigid.linearDamping = 100f;
        yield return new WaitForSeconds(jujuj);
        ani.SetBool("attack", false);
        attackprefab.SetActive(false);
        rigid.linearDamping = 0f;
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        isattack = false;
        yield return new WaitForSeconds(2f);
        isattack = true;
        speed = 2f;
        rigid.linearVelocity = vec.normalized * speed;
    }
}

