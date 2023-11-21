using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : WorkBase
{

    public override bool TakeDamage(int damage)
    {
        health -= damage;
        if (health>0)
        {
            
        }
        else
        {
            Death(true);
            return true;
        }
        return false;
    }
}
