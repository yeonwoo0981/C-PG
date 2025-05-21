using System;
using UnityEngine;

public class TutoEnemy : MonoBehaviour
{
    public event Action OnHit;

    
    public void Hit()
    {
        
        OnHit?.Invoke();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            Hit();
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.CompareTag("PlayerAttack"))
        {
            Hit();
        }
    }
}
