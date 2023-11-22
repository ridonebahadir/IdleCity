using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;

public class SplineManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    
    [Header("SPLINE SETTINGS")]
    [SerializeField] private SplineMesh dynamicSplineMesh; 
    [SerializeField] private SplineMesh staticSplineMesh; 
   

    private float _curScale = 0.3f;
    private void Start()
    {
        //splineMesh.SetClipRange(0,toSplineLenght);
        //navMeshSurface.BuildNavMesh();
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            _curScale += 0.4f;
            dynamicSplineMesh.GetChannel(0).minScale = new Vector3(_curScale, 0.3f, 0);
            TweenMove(true,0, 1);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            _curScale -= 0.4f;
            dynamicSplineMesh.SetClipRange(0,1);
            staticSplineMesh.GetChannel(0).minScale = new Vector3(_curScale, 0.3f, 0);
            TweenMove(false,1, 0);
            
        }
    }

    void TweenMove(bool isIncrease,float cur,float to)
    {
        DOTween.To(() => cur, x => cur = x, to, 3)
            .OnUpdate(() =>
            {
                dynamicSplineMesh.SetClipRange(0, cur);
            }).OnComplete(() =>
            {
                dynamicSplineMesh.SetClipRange(0,0);
                //navMeshSurface.BuildNavMesh();
                if (isIncrease)
                {
                    staticSplineMesh.GetChannel(0).minScale = new Vector3(_curScale, 0.3f, 0);
                }
            });
    }
}
