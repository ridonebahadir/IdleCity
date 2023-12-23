using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LeonBrave;
using UnityEngine;

public class FreeWaterDrop : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject particleObj;
    [SerializeField] private GameObject mesh;
    private readonly WaitForSeconds _waitForSeconds = new(2);
    private SingletonHandler _singletonHandler;
    
    public void Inıt(SingletonHandler singletonHandler)
    {
        _singletonHandler = singletonHandler;
        particleObj.SetActive(false);
        mesh.SetActive(true);
        
        
        StartCoroutine(Move());
        return;

        IEnumerator Move()
        {
            transform.DOMoveY(0, lifeTime).OnComplete(() =>
            {
                particleObj.SetActive(true);
                mesh.SetActive(false);
            });
            yield return _waitForSeconds;
            _singletonHandler.GetSingleton<ObjectPool>().AddObject(gameObject,ObjectType.FreeWaterDrop);
        }
       
    }
}
