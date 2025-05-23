using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int e_HP = 20;
    private bool E_isDead = false;
    private PlayerM _player;
    private int p_damage;
    private Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        ani.SetBool("dead", false);
    }

    private void Start()
    {
        E_isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack") && !E_isDead)
        {
            MinusHP();
            if (e_HP <= 0)
            {
                E_isDead = true;
                E_Dead();
            }
        }
    }

    public void MinusHP()
    {
        e_HP -= 10;
        Debug.Log($"Enemy HP: {e_HP}");
    }

    public void E_Dead()
    {
        Debug.Log("Enemy Dead");
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        ani.SetBool("dead", true);
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}