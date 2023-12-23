using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LeonBrave;
using UnityEngine;
using UnityEngine.Serialization;

public class FreeWaterDrop : MonoBehaviour
{
    private float _dropTime = 0.5f;
    [SerializeField] private GameObject particleObj;
    [SerializeField] private ParticleSystem particleSnowObj;
    [SerializeField] private GameObject mesh;
    private readonly WaitForSeconds _waitForSeconds = new(2);
    private SingletonHandler _singletonHandler;
    private float _lifeTime;
    
    
    public void Inıt(SingletonHandler singletonHandler,float lifeTime)
    {
        WaitForSeconds waitSnow = new(lifeTime);
        _singletonHandler = singletonHandler;
        particleObj.SetActive(false);
        particleSnowObj.gameObject.SetActive(false);
        mesh.SetActive(true);
        
        
        StartCoroutine(Move());
        return;

        IEnumerator Move()
        {
            transform.DOMoveY(0, _dropTime).OnComplete(() =>
            {
                particleObj.SetActive(true);
                mesh.SetActive(false);
            });
            yield return _waitForSeconds;
            particleSnowObj.gameObject.SetActive(true);
            var mainModule = particleSnowObj.main;
            mainModule.startLifetime  = lifeTime-1;
            yield return waitSnow;
            _singletonHandler.GetSingleton<ObjectPool>().AddObject(gameObject,ObjectType.FreeWaterDrop);
        }
       
    }
}
