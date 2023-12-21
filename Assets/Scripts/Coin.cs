using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LeonBrave;
using UnityEngine;
using UnityEngine.Serialization;

public class Coin : MonoBehaviour
{
     public SingletonHandler singletonHandler;
     public GameManager gameManager;
     private Sequence _sequence;
     private Sequence _sequenceSecond;

    public void Inıt(Vector3 startPos,float rewardValue)
    {

        _sequence = DOTween.Sequence();
        transform.position = startPos;
        gameObject.SetActive(true);
        
        var rand = Random.insideUnitCircle*5;
        var target = new Vector3(startPos.x + rand.x, startPos.y, startPos.z + rand.y);
        gameObject.SetActive(true);
        _sequence.Append(transform.DOJump(target, 5, 0, 0.5f).OnComplete(() =>
        {
            GoCanvas(rewardValue);
            
        }).SetEase(Ease.OutExpo));
    }

    private void GoCanvas(float rewardValue)
    {
        _sequenceSecond = DOTween.Sequence();
        var target = gameManager.coinTarget.position;
        gameManager.GetReward(rewardValue);
        _sequenceSecond.SetDelay(1f).Append(transform.DOMove(target, 1f).OnComplete(() =>
            singletonHandler.GetSingleton<ObjectPool>().AddObject(gameObject, ObjectType.Coin)));
    }
}
