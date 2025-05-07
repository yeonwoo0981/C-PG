using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int e_HP = 20;
    
    private bool E_isDead = false;

    private void Start()
    {
        E_isDead = false;

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !E_isDead)
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
        e_HP -= 5;
        Debug.Log($"Enemy HP: {e_HP}");
    }

    public void E_Dead()
    {
        Debug.Log("Enemy Dead");

       

        Destroy(gameObject);
    }
}