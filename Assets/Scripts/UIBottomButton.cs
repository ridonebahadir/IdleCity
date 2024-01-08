using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomButton : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    
    [SerializeField] private List<Button> closeButtons;
    [SerializeField] private List<RectTransform> panels;

    [SerializeField] private Image backGround;
    
    public RectTransform currentPanel;
    public RectTransform oldPanel;
    private void Start()
    {
        backGround.raycastTarget = false;
        foreach (var item in panels)
        {
            item.gameObject.SetActive(false);
            item.transform.localPosition = new Vector3(1300, 0);
        }

        foreach (var item in closeButtons)
        {
            item.onClick.AddListener(CloseButton);
        }
        
        for (var i = 0; i < buttons.Count; i++)
        {
            var buttonIndex = i;
            buttons[i].onClick.AddListener(() => OpenPanel(buttonIndex));
        }
        // marketButton.onClick.AddListener(() => OpenPanel(0));
        // skillButton.onClick.AddListener(() => OpenPanel(1));
        // achievementButton.onClick.AddListener(() => OpenPanel(2));
        
      
    }

    public static Action OnBottomButton;
    public static Action OnBottomButtonClose;
    private int _oldButtonIdx;
    private int _newButtonIdx;
    private void ClosePanel()
    {
        if (currentPanel==null) return;
        OnBottomButtonClose?.Invoke();
        oldPanel = currentPanel;
        _oldButtonIdx = _newButtonIdx;
        oldPanel.DOAnchorPos(new Vector2(1300, 0), 0.15f).OnComplete(() =>
            {
                oldPanel.gameObject.SetActive(false);
                buttons[_oldButtonIdx].interactable = true;
            }

        );
    }

    private void OpenPanel(int a)
    {
        if (currentPanel != null)
        {
            ClosePanel();
        }
        OnBottomButton?.Invoke();
        backGround.raycastTarget = true;
        buttons[a].interactable = false;
        _newButtonIdx = a;
        currentPanel = panels[a].transform.GetComponent<RectTransform>();
        currentPanel.gameObject.SetActive(true);
        currentPanel.DOAnchorPos(new Vector2(0, 0), 0.15f);
    }

    private void CloseButton()
    {
        ClosePanel();
        backGround.raycastTarget = false;
        currentPanel = null; 
    }
    private void OnEnable()
    {
        UIManager.OnClickBattle += CloseButton;
    }

    private void OnDisable()
    {
        UIManager.OnClickBattle -= CloseButton;
    }
}
