using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterUpgradeUIManager : MonoBehaviour
{
    [SerializeField] private Button meleeButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button diggerButton;

    [SerializeField] private List<GameObject> characterUpgradePanels;
    
    public delegate void OnClickCharacter(int a);
    public static OnClickCharacter onClickMelee;
    public static OnClickCharacter onClickArcher;
    public static OnClickCharacter onClickDigger;
    
    
    private void Awake()
    {
        meleeButton.onClick.AddListener(Melee);
        archerButton.onClick.AddListener(Archer);
        diggerButton.onClick.AddListener(Digger);
        ClosePanel();
    }

    private void Melee()
    {
        onClickMelee?.Invoke(0);
        ClosePanel();
        OpenPanel(0);
    }
    private void Archer()
    {
        onClickArcher?.Invoke(1);
        ClosePanel();
        OpenPanel(1);
    }

    private void Digger()
    {
        onClickDigger?.Invoke(2);
        ClosePanel();
        OpenPanel(2);
    }
    
    private void ClosePanel()
    {
        foreach (var item in characterUpgradePanels)
        {
            item.SetActive(false);
        }
    }
    private void OpenPanel(int a)
    {
        characterUpgradePanels[a].SetActive(true);
    }
    private void OnApplicationQuit()
    {
        foreach (var item in characterUpgradePanels)
        {
            item.transform.GetComponent<CharacterUpgradePanel>().soAgent.DefaultData();
            item.transform.GetComponent<CharacterUpgradePanel>().soAgentUpgrade.DefaultData();
        }
        
    }
    

}
