using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    
    public bool TakeDamage(int damage)
    {
        
        health -= damage;
        if (health>0)
        {
            
        }
        else
        {
            Death();
            return true;
        }
        return false;
    }

    private void Death()
    {
       gameObject.SetActive(false);
       gameObject.transform.SetParent(null);
    }
}
