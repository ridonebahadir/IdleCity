using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class DestroyToRiver : WorkBase
{
    [SerializeField] private GameObject obstacleObj;
    
    public override bool TakeDamage(int damage)
    {
        health -= damage;
        if (health>0)
        {
            var obj = Instantiate(obstacleObj,transform.position,UnityEngine.Quaternion.identity,transform);
            var randomPosition = new Vector3(Random.Range(transform.GetChild(0).localPosition.x,transform.GetChild(1).localPosition.x), transform.GetChild(0).localPosition.y, 0);
            obj.transform.DOLocalJump(randomPosition, 5, 0, 0.5f);

        }
        else
        {
            Death(false);
            return true;
        }
        return false;
    }
}
