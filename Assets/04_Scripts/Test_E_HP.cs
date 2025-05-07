using System;
using UnityEngine;

public class Test_E_HP : MonoBehaviour
{
    [SerializeField] Test_P_Level P_Level;
    public int Hp = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(Hp > 0)
            {
                Hp -= 2;
                Debug.Log($"E_HP : {Hp}");
            }
            else
            {
                Debug.Log("Dead");
                E_Dead();
            }

        }
    }

    public void E_Dead()
    {
        P_Level.ExUp();
    }
}
