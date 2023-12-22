using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Camera _camera;
    private CanvasGroup _healthBarCanvasGroup;
    private readonly WaitForSeconds _waitCloseHealthBar = new(2f);
    private IEnumerator _healBarShow;
    [SerializeField] private Image content;
    [SerializeField] private Color lowColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color highColor;
    [SerializeField] private Color healthColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private bool isEnemyBar;
    
    
    private void Start()
    {
         content.fillAmount = 1;
         content.color = isEnemyBar ? enemyColor : highColor;
        _camera = GameManager.Instance.mainCamera;
        _healthBarCanvasGroup=transform.GetComponent<CanvasGroup>();
        _healthBarCanvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        var rotation = _camera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public void HealthBarCanvasGroupShow()
    {
        StopHealthBarShow();
        _healBarShow = HealthBarShow();
        StartCoroutine(_healBarShow);
    }

    public void StopHealthBarShow()
    {
        if (_healBarShow != null)  StopCoroutine(_healBarShow);
        Hide();
    }
    private IEnumerator HealthBarShow()
    {
        Show();
        yield return _waitCloseHealthBar;
        Hide();
    }

    private void Show()
    {
        DOTween.To(() => _healthBarCanvasGroup.alpha, x => _healthBarCanvasGroup.alpha = x, 1, 0.25f);

    }

    private void Hide()
    {
        DOTween.To(() => _healthBarCanvasGroup.alpha, x => _healthBarCanvasGroup.alpha = x, 0, 0.25f);

    }

    public void SetHealthBar(float startHealth,float dmg,AgentType agentType)
    {
        var a = dmg / startHealth;
        var fillAmount = content.fillAmount;
        content.fillAmount = a;
        if (agentType==AgentType.Enemy) return;
        if (isHealth)
        {
            content.color = healthColor;
        }
        else
        {
            content.color = fillAmount switch
            {
                <= 0.45f => lowColor,
                <= 0.75f => mediumColor,
                _ => highColor
            };
        }
        
    }

    public bool isHealth;
   
    
}
