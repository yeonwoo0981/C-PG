using System.Collections;
using UnityEngine;

public class ZonZombie : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2f;
    private Vector2 vec;
    private GameObject player;
    [SerializeField] private GameObject attackprefab;
    private GameObject attackprefab1;

    private Animator ani;
    private bool isattack = true;
    private void Awake()
    {
        rigid.linearDamping = 100f;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
            Debug.DrawRay(transform.position, Vector2.right * vec, Color.yellow, 5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * vec, 5f, layer);

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
        yield return new WaitForSeconds(0.3f);
        if (attackprefab1 == null)
        {
            attackprefab1 = Instantiate(attackprefab, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.7f);
        ani.SetBool("attack", false);
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        isattack = false;
        yield return new WaitForSeconds(2f);
        isattack = true;
    }
}
