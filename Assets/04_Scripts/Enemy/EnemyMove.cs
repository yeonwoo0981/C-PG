using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2f;
    private Vector2 vec;
    private GameObject player;

    private Animator ani;

    private float attackrange = 2f;
    private float range;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        range = Vector2.Distance(transform.position, player.transform.position);
        vec = player.transform.position - transform.position;
    }
    public void FixedUpdate()
    {
        locate();
        AnimationRun();
        Attack();
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
        LayerMask layer = LayerMask.GetMask("PlayerLevelUp");
        Debug.DrawRay(transform.position, vec.normalized, Color.red, 1.5f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vec.normalized, 1.5f, layer);

        if (hit.collider != null)
        {
            speed = 0f;
            rigid.linearVelocity = Vector2.zero;
            ani.SetBool("attack", true);
        }
        else
        {
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

