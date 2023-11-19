using System.Collections;
using System.Collections.Generic;
using Agent.Enemy;
using UnityEngine;





public class Health : MonoBehaviour
{
    public int health;
    public HealthType healthType;
    
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
        GameManager.Instance.RemoveList(healthType,this);
       gameObject.SetActive(false);
       gameObject.transform.SetParent(null);
    }
}
