using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Camera _camera;
    private CanvasGroup _healthBarCanvasGroup;
    private readonly WaitForSeconds _waitCloseHealthBar = new(2f);
    private IEnumerator _healBarShow;
    [SerializeField] private Image content;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float fillAmount;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color lowColor;
    
    
    
    private void Start()
    {
        content.fillAmount = 1;
        _camera = GameManager.Instance.mainCamera;
        _healthBarCanvasGroup=transform.GetComponent<CanvasGroup>();
        _healthBarCanvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        var rotation = _camera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.back, rotation * Vector3.up);
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

    public void SetHealthBar(float startHealth,float dmg)
    {
        var a = dmg / startHealth;
        content.fillAmount = a;
    }
    
}
