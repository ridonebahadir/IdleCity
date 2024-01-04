using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class FogMove : MonoBehaviour
{
    [SerializeField] private GameObject cloud1;
    [SerializeField] private GameObject cloud2;

   

   

    private void Move()
    {
        Tween(cloud1.transform,-4f);
        Tween(cloud2.transform,13f);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void Tween(Transform cloud,float endValue)
    {
        cloud.transform.DOLocalMoveX(endValue, 1);
    }

    private void OnEnable()
    {
        UIManager.OnClickBattle += Move;
        UIManager.OnClickedBattle += Close;
    }

    private void OnDisable()
    {
        UIManager.OnClickBattle -= Move;
        UIManager.OnClickedBattle -= Close;
    }
}
