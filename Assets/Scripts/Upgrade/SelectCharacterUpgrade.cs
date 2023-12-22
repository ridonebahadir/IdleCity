using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectCharacterUpgrade : MonoBehaviour
{
   

    [SerializeField] private List<GameObject> characterUpgradePanels;
    
    public delegate void OnClickCharacter(int a);
    public static OnClickCharacter onClickMelee;
    public static OnClickCharacter onClickArcher;
    public static OnClickCharacter onClickDigger;
    
    
    private void Awake()
    {
       
        ClosePanel();
    }

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
