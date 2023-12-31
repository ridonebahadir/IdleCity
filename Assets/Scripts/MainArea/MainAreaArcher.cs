using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LeonBrave;
using UnityEngine;

public class MainAreaArcher : MainAreaAgentBase
{
    //[SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowTarget;
    [SerializeField] private GameObject arrowPrefab;
    
    private SingletonHandler SingletonHandler;
   
    protected override void Attack()
    {
        StartCoroutine(ThrowAttack());
        

    }

    private WaitForSeconds wait = new(1);
    IEnumerator ThrowAttack()
    {
        while (_isAttack)
        {
            var arrow = Instantiate(arrowPrefab,transform);
            arrow.transform.position = transform.position;
            arrow.gameObject.SetActive(true);
            arrow.transform.SetParent(arrowTarget);
            arrow.transform.DOLocalJump(Vector3.zero, 6, 0, 0.5f).OnComplete(() =>
            {
               Destroy(arrow.gameObject);
            });
            yield return wait;
        }
    }
}
