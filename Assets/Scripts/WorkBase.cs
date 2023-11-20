using System.Collections;
using System.Collections.Generic;
using Agent.Enemy;
using UnityEngine;


public enum HealthType
{
    Home,
    Soldier, 
    Enemy,
    RiverPoint,
}


public abstract class WorkBase : MonoBehaviour
{
    public int health;
    public HealthType healthType;
    
    
    protected void Death()
    { 
        GameManager.Instance.RemoveList(healthType,this.transform);
       gameObject.SetActive(false);
       gameObject.transform.SetParent(null);
    }
    
    public abstract bool DestructRiver();
    public abstract bool TakeDamage(int damage);
}
