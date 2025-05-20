using System.Collections;
using UnityEngine;

public class ZonZombie : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2f;
    private Vector2 vec;
    private GameObject player;
    [SerializeField] private GameObject attackprefab;

    private Animator ani;
    private float range;
    private bool isattack = true;
    private void Awake()
    {
        rigid.linearDamping = 100f;
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
        }
        else
        {
            ani.SetBool("idle", false);
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
            Debug.DrawRay(transform.position, Vector2.right * vec, Color.yellow, 3f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * vec, 3f, layer);

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
        speed = 0f;
        rigid.linearVelocity = Vector2.zero;
        ani.SetBool("attack", true);
        attackprefab.SetActive(true);
        yield return new WaitForSeconds(1f);
        ani.SetBool("attack", false);
        attackprefab.SetActive(false);
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        isattack = false;
        yield return new WaitForSeconds(3f);
        isattack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackprefab.SetActive(false);
        }
    }
}
