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
    }

    public void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        range = Vector2.Distance(transform.position, player.transform.position);
        vec = player.transform.position - transform.position;
    }
    public void FixedUpdate()
    {
        locate();
        AttackLenght();
        AnimationRun();
    }

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void AttackLenght()
    {
        if (range <= attackrange)
        {
            ani.SetTrigger("attack");
            speed = 0f;
        }
        else
        {
            speed = 2f;
            rigid.linearVelocity = vec.normalized * speed;
        }
    }

    void AnimationRun()
    {
        if (vec.x > 0 || vec.x < 0)
        {
            ani.SetBool("run", true);
        }
        else
        {
            ani.SetBool("run", true);
        }
    }
}
