using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2f;
    private Vector2 vec;
    private GameObject player;

    private Animator ani;

    private float attackrange = 2f;
    private float range;
<<<<<<< HEAD
    private float C_Time = 0;
=======
    private float lastattacktime = 0f;
    private float attacktime = 1.5f;
>>>>>>> 472aebe3cf208cb00abd92921a1920613bed60a4

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
        AttackLength();
        AnimationRun();
    }

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void AttackLength()
    {
        if (range <= attackrange && Time.time >= lastattacktime + attacktime)
        {
<<<<<<< HEAD
            ani.SetBool("attack",true);
            speed = 0;
            E_Damage();
=======
            speed = 0f;
            rigid.linearVelocity = Vector2.zero;
            Attack();
            lastattacktime = Time.time;
>>>>>>> 472aebe3cf208cb00abd92921a1920613bed60a4
        }
        if (Time.time! >= lastattacktime + attacktime)
        {
            speed = 2f;
            rigid.linearVelocity = vec.normalized * speed;
            ani.SetBool("attack", false);

        }
    }

    void Attack()
    {
        ani.SetBool("attack", true);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            C_Time = Time.deltaTime;
        else
            C_Time = 0;

        Debug.Log(C_Time);
    }
    public bool E_Damage()
    {
        if (C_Time >= 0.7f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
