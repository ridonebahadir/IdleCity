using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Domination : MonoBehaviour
{
    [SerializeField] private Transform sphere;
    
    [SerializeField] private SplinePositioner splinePositioner;
    [SerializeField] private SplineComputer splineComputer;
    
    [SerializeField] private float speed = 6;
    [SerializeField] private float riverWidth;

    public List<AgentBase> _enemies;
    public List<AgentBase> _soldiers;
    
    private float _currentDistance;
    private float _sizeSpeed;
    private int _turn = 0;
    private float _dist;
    private float _size;
    private float _goneRoad;
    private WaitForSeconds _wait = new(0.01f);
    private SplinePoint[] _points;
    private IEnumerator _dominationMove;

    private void Start()
    {
         _points = splineComputer.GetPoints();
         Calculate();
         _dominationMove=DominationMove();
         StartCoroutine(_dominationMove);
         StartCoroutine(StopDomination());

    }

    IEnumerator StopDomination()
    {
        yield return new WaitForSeconds(3);
        _stop = true;
    }


    [SerializeField] private bool isWin = true;
    
   
     private IEnumerator DominationMove()
    {
        while (true)
        {
            if (isWin)
            {
                _currentDistance += Time.deltaTime*speed;
                _goneRoad += Time.deltaTime*speed;
                if (_size<=riverWidth) _size += Time.deltaTime * _sizeSpeed; 
                if (_goneRoad>=_dist)
                {
                    _size = 0.1f;
                    _goneRoad = 0;
                    if (_turn == _points.Length - 2)
                    {
                        Debug.Log("WIN");
                        break;
                    }
                    _turn++;
                    Calculate();
                    
                }
            }
            else
            {
                _currentDistance -= Time.deltaTime*speed;
                _goneRoad -= Time.deltaTime*speed;
                if (_size>=0.1f) _size -= (Time.deltaTime * _sizeSpeed);
          
                if (_goneRoad<=0)
                {
                    if (_turn == 0)
                    {
                        Debug.Log("LOSE");
                        break;
                    }
                    _size = riverWidth;
                    _turn--;
                    Calculate();
                    _goneRoad = _dist;
                   

                }
            }
            splinePositioner.SetDistance(_currentDistance); 
            splineComputer.SetPointSize(_turn,_size);
            yield return _wait;
        }
       
    }

     private bool _stop;
     private void Update()
     {
         if (_stop)
         {
             ControlMove();
         }
        
         if (Input.GetKeyUp(KeyCode.DownArrow))
         {
             SoldierMove();

         }
         if (Input.GetKeyUp(KeyCode.UpArrow))
         {
             EnemyMove();
         }

         if (Input.GetKeyUp(KeyCode.Space))
         {
             StopCoroutine(_dominationMove);
         }

         if (Input.GetKeyUp(KeyCode.P))
         {
             StartCoroutine(_dominationMove);
         }

       
     }

     private void EnemyMove()
     {
         speed = _enemies.Count * 0.5f;
         isWin = false;
         StopCoroutine(_dominationMove);
         StartCoroutine(_dominationMove);
     }

     private void SoldierMove()
     {
         speed = _soldiers.Count * 0.5f;
         isWin = true;
         StopCoroutine(_dominationMove);
         StartCoroutine(_dominationMove);
     }

     private void Calculate()
     {
         if (isWin)
         {
             _dist = Vector3.Distance(sphere.position, _points[_turn+1].position);
             _sizeSpeed = (riverWidth-(splineComputer.GetPoint(_turn).size)) / (_dist / speed);
         }
         else
         {
             _dist = Vector3.Distance(sphere.position, _points[_turn].position);
             _sizeSpeed = (splineComputer.GetPoint(_turn).size) / (_dist / speed);
         }
        
         
     }

     private void OnTriggerEnter(Collider other)
     {
         if (other.TryGetComponent(out Enemy enemy))
         {
            
             if (!_enemies.Contains(enemy))  _enemies.Add(enemy);
             if (_enemies.Count <= 0 || _soldiers.Count <= 0) return;
             AttackEnemy(); 
             AttackSoldier();

         }
         if (other.TryGetComponent(out Soldier soldier))
         {
             if (!_soldiers.Contains(soldier))_soldiers.Add(soldier);
             if (_enemies.Count <= 0 || _soldiers.Count <= 0) return;
             AttackEnemy(); 
             AttackSoldier();

         }
     }
    
     public void AttackEnemy()
     {
         for (int i = 0; i < _enemies.Count; i++)
         {
             _enemies[i].Attack(_soldiers[0].transform);
         }
     }
     public void AttackSoldier()
     {
         
         for (int i = 0; i < _soldiers.Count; i++)
         {
             _soldiers[i].Attack(_enemies[0].transform);
         }
         
     }
     
     private void ControlMove()
     {
         
         if (_enemies.Count == 0) SoldierMove();
         if (_soldiers.Count==0) EnemyMove();
         if (_enemies.Count > 0 && _soldiers.Count > 0)   StopCoroutine(_dominationMove);
        
     }


     public Transform CloseAgentEnemy(Transform who)
     {
          return _enemies.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
     }
     public Transform CloseAgentSoldier(Transform who)
     {
         return _soldiers.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
     }
}
