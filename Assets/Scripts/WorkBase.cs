using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HealthType
{
    Home,
    Soldier, 
    Enemy,
    DestroyRiverPoint,
}


public abstract class WorkBase : MonoBehaviour
{
    public int health;
    public HealthType healthType;
    
    
    protected void Death(bool isClose)
    { 
        GameManager.Instance.RemoveList(healthType,this.transform);
       gameObject.SetActive(!isClose);
       gameObject.transform.SetParent(null);
    }
    
   
    public abstract bool TakeDamage(int damage);
}
