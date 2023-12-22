using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterUpgradePanel : MonoBehaviour
{
   
    public SOAgent soAgent;
    public SOAgentUpgrade soAgentUpgrade;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image sliderFilled;
    [SerializeField] private Image nextIcon;
    [SerializeField] private Image currentIcon;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button button;

    
    
    
    
    private void Start()
    {
        SetCost();
        SetHealth(0);
        SetDamage(0);
        SetSlider();
        levelText.SetText("Level = "+soAgentUpgrade.levelCount);
        characterName.SetText(soAgentUpgrade.name);
        currentIcon.sprite = soAgentUpgrade.icon;
        nextIcon.sprite = soAgentUpgrade.nextIcon;
        button.onClick.AddListener(Clicked);
    }

    private void SetCost()
    {
        soAgentUpgrade.cost += ((soAgentUpgrade.levelCount+1)*(soAgentUpgrade.multipher+1));
        costText.SetText(soAgentUpgrade.cost.ToString());
    }

    private void SetSlider()
    {
        progressText.SetText(soAgentUpgrade.levelCount.ToString()+" / "+soAgentUpgrade.levelBorders[soAgentUpgrade.multipher]);
       
        if (soAgentUpgrade.levelCount==0)
        {
            sliderFilled.fillAmount= 0;
        }
        else
        {
            float a= (float)(soAgentUpgrade.levelCount)/(float)soAgentUpgrade.levelBorders[soAgentUpgrade.multipher];
            sliderFilled.fillAmount= a;
            Debug.Log(a);
           
        }
       
    }

    private void SetHealth(int value)
    {
        soAgent.health += value;
        healthText.SetText("Health = "+soAgent.health.ToString());
    }

    private void SetDamage(int value)
    {
        soAgent.damage += value;
        damageText.SetText("Damage = "+soAgent.damage.ToString());
    }

    private void SetLevel()
    {
        //soAgent.level++;
        soAgentUpgrade.levelCount++;
        if (soAgentUpgrade.levelCount == soAgentUpgrade.levelBorders[soAgentUpgrade.multipher])
        {
            soAgentUpgrade.multipher++;
            soAgentUpgrade.levelCount = 0;
        }
        levelText.SetText("Level = "+soAgentUpgrade.levelCount);
    }

    private void Clicked()
    {
       SetValue();
    }

    private void SetValue()
    {
        if (soAgentUpgrade.multipher == soAgentUpgrade.levelBorders.Count - 1)
        {
            sliderFilled.fillAmount = 1;
            return;
        }
        SetLevel();
        SetCost();
        SetHealth(1);
        SetDamage(1);
        SetSlider();
        nextIcon.sprite = soAgentUpgrade.nextIcon;
    }
    
   
}
