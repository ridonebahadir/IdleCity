using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgradeUIManager : MonoBehaviour
{
    [SerializeField] private Button meleeButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button diggerButton;

    [SerializeField] private List<GameObject> characterUpgradePanels;
    
    private void Awake()
    {
        meleeButton.onClick.AddListener(Melee);
        archerButton.onClick.AddListener(Archer);
        diggerButton.onClick.AddListener(Digger);
        ClosePanel();
    }

    private void Melee()
    {
        ClosePanel();
        OpenPanel(0);
    }
    private void Archer()
    {
        ClosePanel();
        OpenPanel(1);
    }

    private void Digger()
    {
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
