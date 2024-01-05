using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LeonBrave;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

public class Coin : MonoBehaviour
{
     public SingletonHandler singletonHandler;
     public GameManager gameManager;
     private Sequence _sequence;
     private Sequence _sequenceSecond;

    public void Inıt(Vector3 startPos,float rewardValue,Transform endPoint,bool isCoin)
    {

        _sequence = DOTween.Sequence();
        transform.position = startPos;
        gameObject.SetActive(true);
        
        var rand = Random.insideUnitCircle*5;
        var target = new Vector3(startPos.x + rand.x, startPos.y, startPos.z + rand.y);
        gameObject.SetActive(true);
        _sequence.Append(transform.DOJump(target, 5, 0, 0.5f).OnComplete(() =>
        {
            GoCanvas(rewardValue,endPoint,isCoin);
            
        }).SetEase(Ease.OutExpo));
    }

    private void GoCanvas(float rewardValue,Transform target,bool isCoin)
    {
        _sequenceSecond = DOTween.Sequence();
        _sequenceSecond.SetDelay(1f).Append(transform.DOMove(target.position, 1f).OnComplete(() =>
        {
            if (isCoin)
            {
                gameManager.GetXpReward((int)rewardValue);
                singletonHandler.GetSingleton<ObjectPool>().AddObject(gameObject, ObjectType.Coin);
            }
            else
            {
                gameManager.GetDiamondReward(1);
                gameObject.SetActive(false);
            }
        }));
    }
}
