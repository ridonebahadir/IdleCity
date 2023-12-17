using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class DamagePopup : MonoBehaviour
{

    public static DamagePopup Create(Transform parent,float damage)
    {
        var damagePopupTransform=Instantiate(GameManager.Instance.damageTextPrefab,parent).transform;
        var damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        float rand = Random.Range(-1, 1);
        damagePopupTransform.transform.localPosition = new Vector3(rand,1, 0);
        damagePopup.Setup(damage);
        return damagePopup;
    }
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        _textMeshPro = transform.GetComponent<TextMeshProUGUI>();
    }

    private void Setup(float damage)
    {
        _textMeshPro.text = "-"+ damage;
        transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            transform.DOScale(Vector3.zero, 1).OnComplete(()=>Destroy(gameObject));
        });
    }
}
