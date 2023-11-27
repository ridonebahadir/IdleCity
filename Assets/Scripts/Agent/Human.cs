using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class Human : MonoBehaviour
{
    
    //SHIFT CONTROL
    //public Button shiftControl;
    
    
    
    public List<Transform> _humanPoints = new List<Transform>(); 
    public int _countTarget;
    private NavMeshAgent _agent;
    private bool _onetime; 
    private WaitForSeconds _waitTime;
    private float _dist;
    protected IEnumerator WorkCoroutine;
    protected IEnumerator SleepCoroutine;
    private WaitForSeconds _wait = new(0.5f);
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
       
    }
    

    protected void HumanPoints(List<Transform> buildManager)
    { 
      
        foreach (var t in buildManager)
        {
            var newTransform = t.GetChild(Random.Range(0, t.childCount));
            _humanPoints.Add(newTransform);
        }
        //transform.position = _humanPoints[0].transform.position;
    }
    protected IEnumerator MoveToWork(bool isWait)
    {
        while (true)
        {
            _dist = Vector3.Distance(_humanPoints[_countTarget].position,transform.position);
            //Arrived
            if (_dist<1) 
            {
                transform.GetChild(0).DOScale(Vector3.zero, 0.3f);
                _waitTime = isWait ? new(Random.Range(3,6)) : new(0);
                yield return _waitTime; 
                _countTarget++;
                if (_countTarget>=_humanPoints.Count)  _countTarget = 1;
            }
            //On The Way
            else
            {
                transform.GetChild(0).DOScale(Vector3.one, 0.3f);
            }
            _agent.destination = _humanPoints[_countTarget].position;
            yield return _wait;
        }
       
    }
    protected IEnumerator MoveToSleep()
    {
        while (true)
        {
            _dist = Vector3.Distance(_humanPoints[0].position,transform.position);
            //Arrived
            if (_dist<1) 
            {
                transform.GetChild(0).DOScale(Vector3.zero, 0.3f);
            }
            //On The Way
            else
            {
                transform.GetChild(0).DOScale(Vector3.one, 0.3f);
            }
            _agent.destination = _humanPoints[0].position;
            yield return _wait;
        }
       
    }
    
}


