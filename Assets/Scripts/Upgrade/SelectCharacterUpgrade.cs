using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectCharacterUpgrade : MonoBehaviour
{
   

    [SerializeField] private List<GameObject> characterUpgradePanels;

    public RectTransform currentPanel;
    public RectTransform oldPanel;
    
    public delegate void OnClickCharacter(int a);
    public static OnClickCharacter onClickMelee;
    public static OnClickCharacter onClickArcher;
    public static OnClickCharacter onClickDigger;
    
   

    public void Melee()
    {
        onClickMelee?.Invoke(0);
        ClosePanel();
        OpenPanel(0);
    }
    public void Archer()
    {
        onClickArcher?.Invoke(1);
        ClosePanel();
        OpenPanel(1);
    }

    public void Digger()
    {
        onClickDigger?.Invoke(2);
        ClosePanel();
        OpenPanel(2);
    }
    
    private void ClosePanel()
    {
        if (currentPanel==null) return;
        oldPanel = currentPanel;
        oldPanel.DOAnchorPos(new Vector2(0, -850), 0.35f).OnComplete(() =>
            {
                oldPanel.gameObject.SetActive(false);
            }

        );
    }
    private void OpenPanel(int a)
    {
        currentPanel = characterUpgradePanels[a].transform.GetComponent<RectTransform>();
        currentPanel.gameObject.SetActive(true);
        currentPanel.DOAnchorPos(new Vector2(0, 0), 0.35f);
    }
    private void OnApplicationQuit()
    {
        foreach (var item in characterUpgradePanels)
        {
            item.transform.GetComponent<CharacterUpgradePanel>().soAgent.DefaultData();
            item.transform.GetComponent<CharacterUpgradePanel>().soAgentUpgrade.DefaultData();
        }
        
    }

    private void CloseButton()
    {
        currentPanel = null;
       
    }
    private void OnEnable()
    {
       CharacterUpgradePanel.onClickClose += ClosePanel;
       CharacterUpgradePanel.onClickClose += CloseButton;
    }

    private void OnDisable()
    {
        CharacterUpgradePanel.onClickClose -= ClosePanel;
        CharacterUpgradePanel.onClickClose -= CloseButton;
    }
}