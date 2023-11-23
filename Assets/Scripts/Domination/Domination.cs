using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Serialization;

public class Domination : MonoBehaviour
{
    [SerializeField] private Transform sphere;
    
    [SerializeField] private SplinePositioner splinePositioner;
    [SerializeField] private SplineComputer splineComputer;
    
    [SerializeField] private float speed = 6;
    [SerializeField] private float riverWidth;
    
    
    [SerializeField] private float _currentDistance;
    private float _sizeSpeed;
    [SerializeField] private int _turn = 0;
    [SerializeField] private float _dist;
    [SerializeField] private float _size;
    [SerializeField] private float _goneRoad;
    private WaitForSeconds _wait = new(0.01f);
    private SplinePoint[] _points;
    private IEnumerator _dominationMove;

    private void Start()
    {
         _points = splineComputer.GetPoints();
         Calculate();
         _dominationMove=DominationMove();
         StartCoroutine(_dominationMove);

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
                _size += Time.deltaTime * _sizeSpeed; 
                if (_goneRoad>=_dist)
                {
                    _size = 0;
                    _goneRoad = 0;
                    if (_turn == _points.Length - 2) break;
                    _turn++;
                    Calculate();
                    
                }
            }
            else
            {
                _currentDistance -= Time.deltaTime*speed;
                _goneRoad -= Time.deltaTime*speed;
                _size -= (Time.deltaTime * _sizeSpeed);
               
                if (_goneRoad<=0)
                {
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

     private void Update()
     {
         if (Input.GetKeyUp(KeyCode.Q))
         {
             isWin = true;
             StopCoroutine(_dominationMove);
             _size = riverWidth - splineComputer.GetPoint(_turn).size;
            
            
             StartCoroutine(_dominationMove);

         }
         if (Input.GetKeyUp(KeyCode.A))
         {
             isWin = false;
             StopCoroutine(_dominationMove);
             _size = splineComputer.GetPoint(_turn).size;
             StartCoroutine(_dominationMove);
         }

       
     }

     private void Calculate()
     {
         if (isWin)
         {
             _dist = Vector3.Distance(sphere.position, _points[_turn+1].position);
             _sizeSpeed = (riverWidth-splineComputer.GetPoint(_turn).size) / (_dist / speed);
         }
         else
         {
             _dist = Vector3.Distance(sphere.position, _points[_turn].position);
             _sizeSpeed = splineComputer.GetPoint(_turn).size / (_dist / speed);
         }
        
         
     }
}
