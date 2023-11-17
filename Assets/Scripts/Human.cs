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
public class Human : MonoBehaviour
{
    
    //SHIFT CONTROL
    [SerializeField] private Button shiftControl;
    public bool _isWork = true;
    
    
    private readonly List<Transform> _humanPoints = new List<Transform>(); 
    public int _countTarget;
   
    private NavMeshAgent _agent;
    private bool _onetime; 
    private WaitForSeconds _waitTime;
    private float dist;
    private IEnumerator WorkCoroutine;
    private IEnumerator SleepCoroutine;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        var buildManager = GameManager.instance.buildManager.builds;
        foreach (var t in buildManager)
        {
            var newTransform = t.GetChild(Random.Range(0, t.childCount));
            _humanPoints.Add(newTransform);
        }
        //_agent.destination = _humanPoints[0].position;
        transform.position = _humanPoints[0].transform.position;
        shiftControl.onClick.AddListener(ShiftControl);
        
        
        SleepCoroutine = MoveToSleep();
        WorkCoroutine = MoveToWork();
        StartCoroutine(WorkCoroutine);

       
    }
    
   
    IEnumerator MoveToWork()
    {
        while (true)
        {
            dist = Vector3.Distance(_humanPoints[_countTarget].position,transform.position);
            //Arrived
            if (dist<1) 
            {
                transform.GetChild(0).DOScale(Vector3.zero, 0.3f);
                _waitTime =new(Random.Range(1,10));
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
            yield return new WaitForSeconds(0.5f);
        }
       
    }
    IEnumerator MoveToSleep()
    {
        while (true)
        {
            dist = Vector3.Distance(_humanPoints[0].position,transform.position);
            //Arrived
            if (dist<1) 
            {
                transform.GetChild(0).DOScale(Vector3.zero, 0.3f);
            }
            //On The Way
            else
            {
                transform.GetChild(0).DOScale(Vector3.one, 0.3f);
            }
            _agent.destination = _humanPoints[0].position;
            yield return new WaitForSeconds(0.5f);
        }
       
    }
    void ShiftControl()
    {
        if (_isWork)
        {
                
            StopCoroutine(WorkCoroutine);
            StartCoroutine(SleepCoroutine);
            _isWork = false;
        }
        else
        {
            StartCoroutine(WorkCoroutine);
            StopCoroutine(SleepCoroutine);
            _isWork = true;
        }
    }
    
   
}


