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
    [SerializeField] private SplineMesh splineMesh; 
    [SerializeField] private float toSplineLenght;
    
    private void Start()
    {
        splineMesh.SetClipRange(0,toSplineLenght);
        navMeshSurface.BuildNavMesh();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            float curSplineLenght = Mathf.Clamp(toSplineLenght + 0.03f, 0f, 1f);
            DOTween.To(() => toSplineLenght, x => toSplineLenght = x, curSplineLenght, 0.5f)
                .OnUpdate(() => {
                    splineMesh.SetClipRange(0,toSplineLenght);
                }).OnComplete(()=>navMeshSurface.BuildNavMesh());
        }
    }
}
