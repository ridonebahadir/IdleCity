using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyToRiver : WorkBase
{
    public override bool DestructRiver()
    {
        return false;
    }

    public override bool TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
